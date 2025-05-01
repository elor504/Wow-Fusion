using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] private string projectileID;
    [SerializeField] private BasicVFX hitVfxPF;
    
    private Transform _caster;
    private ITargetableEntity _casterEntity;
    private Transform _target;
    private ITargetableEntity _targetEntity;
    
    
    private bool _isActive;
    private int _damage;
    private float _speed;
    
    private float _distance;
    
    public bool IsActive => _isActive;
    public string ProjectileID => projectileID;
    
    
    private void Update()
    {
        ///Go toward Target
        Vector3 dir = (_target.position - transform.position).normalized;
        
        transform.position += dir * (_speed * Time.deltaTime);
        
        _distance = Vector3.Distance(transform.position, _target.position);
        if (_distance <= 0.1f)
        {
            _isActive = false;
            var vfx = VFXPoolSystem.Instance.GetAvailableObjectFromPool(hitVfxPF, transform.position);
            vfx.InitVFX(transform.position);
            _targetEntity.DealDamage(_casterEntity,_damage);
            gameObject.SetActive(false);
        }
    }

    public void InitProjectile(Vector3 spawnLocation,ITargetableEntity caster, ITargetableEntity target,int damage,float speed)
    {
        transform.position = spawnLocation;
        
        _casterEntity = caster;
        _caster = _casterEntity.GetEntityGO().transform;

        _targetEntity = target;
        _target = _targetEntity.GetHitPosition();
        
        _damage = damage;
        _speed = speed;
        _isActive = true;
        gameObject.SetActive(true);
    }
}

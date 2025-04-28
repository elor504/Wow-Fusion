using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    private Transform _caster;
    private ITargetableEntity _casterEntity;
    private Transform _target;
    private ITargetableEntity _targetEntity;
    
    
    private int _damage;
    private float _speed;

    private float _distance;
    
    private void Update()
    {
        ///Go toward Target
        Vector3 dir = (_target.position - transform.position).normalized;
        
        transform.position += dir * (_speed * Time.deltaTime);
        
        _distance = Vector3.Distance(transform.position, _target.position);
        if (_distance <= 0.01f)
        {
            _targetEntity.DealDamage(_casterEntity,_damage);
            Destroy(gameObject);
        }
    }

    public void InitProjectile(Vector3 spawnLocation,ITargetableEntity caster, ITargetableEntity target,int damage,float speed)
    {
        transform.position = spawnLocation;
        
        _casterEntity = caster;
        _caster = _casterEntity.GetEntityGO().transform;

        _targetEntity = target;
        _target = _targetEntity.GetEntityGO().transform;
        
        _damage = damage;
        _speed = speed;
    }
    
    
    

}

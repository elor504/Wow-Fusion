using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, ITargetableEntity
{
    [Header("References")] 
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterVFXVisual characterVFXVisual;
    
    [Header("Transform references")] 
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Transform hitPosition;
    
    
    private int _health = 10000;
    private int _mana = 50000;
    private PlayerBrain _playerBrain;


    public Animator GetAnimator => animator;
    public CharacterVFXVisual CharacterVFXVisual => characterVFXVisual;
        

    private void Awake()
    {
        InitPlayer();
    }

    public void InitPlayer()
    {
        _playerBrain = new PlayerBrain();
        _playerBrain.InitBrain(this);
    }
    
    private void Update()
    {
        _playerBrain?.UpdateState();
    }
    private void FixedUpdate()
    {
        _playerBrain?.FixedUpdateState();
    }

    public void CastSpell(BaseSpell spell, ITargetableEntity target)
    {
        if (!_playerBrain.TryToCastSpell(spell, this, target))
        {
            ///need to make some of notification :O
            Debug.Log("Cannot cast spell");
        }
    }
    


    public void OnTargeted()
    {
    }
    public void OnStopTargeting()
    {
    }
    public void OnHovering()
    {
    }
    public void OnStoppedHovering()
    {
    }
    
    public void DealDamage(ITargetableEntity caster, int damage)
    {
        
    }
    public void Heal(ITargetableEntity caster)
    {
        
    }
    
    public bool CanBeTargeted()
    {
        return true;
    }
    public bool IsEnemy()
    {
        return false;
    }
    public bool IsAlly()
    {
        return true;
    }
    public bool CanCastSpell(int amount)
    {
        if (_mana < amount)
            return false;

        _mana -= amount;
        return true;
    }
    
    public int GetHealth()
    {
        return _health;
    }
    public int GetMana()
    {
        return _mana;
    }
    
    public GameObject GetEntityGO()
    {
        return gameObject;
    }

    public Transform GetProjectileSpawnPosition()
    {
        return projectileSpawnPoint;
    }
    public Transform GetHitPosition()
    {
        return hitPosition;
    }
}
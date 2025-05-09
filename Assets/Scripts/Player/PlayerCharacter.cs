using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerCharacter : MonoBehaviour, ITargetableEntity
{
    [Header("References")] 
    [SerializeField] private PlayerAnimator animator;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private CharacterVFXVisual characterVFXVisual;
    [SerializeField] private CharacterClass characterClass;
    [SerializeField] private EntityStat characterStat;
    
    [Header("Transform references")] 
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Transform hitPosition;
    
    [Header("Temp references")]
    [SerializeField] private BaseClassData baseClassData;
    
    
    private PlayerBrain _playerBrain;


    public PlayerAnimator GetAnimator => animator;
    public PlayerMovement GetMovement => movement;
    public CharacterVFXVisual CharacterVFXVisual => characterVFXVisual;
    public EntityStat CharacterStat => characterStat;
    
    
    private void Awake()
    {
        InitPlayer();
    }

    public void InitPlayer()
    {
        _playerBrain = new PlayerBrain();
        _playerBrain.InitBrain(this);
        
        characterClass.Init(baseClassData, characterStat);
        characterStat.Init(baseClassData.ClassBaseStats);
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
        characterStat.DealDamage(damage);
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
        if (!characterStat.CanUseMana(amount))
            return false;
        
       // characterStat.UseMana(amount);
        return true;
    }
    
    public int GetHealth()
    {
        return 0;
    }
    public int GetMana()
    {
        return 0;
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
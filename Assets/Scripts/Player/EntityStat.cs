using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStat : MonoBehaviour
{
    private ITargetableEntity _entity;
    [SerializeField] private StatContainer entityBaseStat;

    private int _currentLevel = 1;
    [SerializeField] private int currentHealth;
    [SerializeField] private int currentMana;
    
    [SerializeField] private StatusEffectHolder statusEffectHolder;
    private StatContainer _statEffectBonuses;
    
    private int _maxHealth;
    private int _maxMana;
    
    public int CurrentHealth => currentHealth;
    public int CurrentMana => currentMana;

    public event Action <int,int> OnHealthChanged;
    public event Action <int,int> OnManaChanged;
    
    private void OnValidate()
    {
        entityBaseStat.ValidateStatList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log($"Current Intellect: {GetFullStatValue(StatType.Intellect)}");
        }
    }

    public void Init(ITargetableEntity entity,StatContainer baseStats,int level = 1)
    {
        _entity = entity;
        entityBaseStat = baseStats;
        
        _currentLevel = level;
        
        _maxHealth = MaxHealth(_currentLevel);
        currentHealth = _maxHealth;
        
        _maxMana = MaxMana(_currentLevel);
        currentMana = _maxMana;

        statusEffectHolder = new StatusEffectHolder(this, _entity);

        _statEffectBonuses = new();
        _statEffectBonuses.ValidateStatList();
        
        
        OnHealthChanged?.Invoke(currentHealth, _maxHealth);
        OnManaChanged?.Invoke(currentMana, _maxMana);
        
    
    }
    public void Init(ITargetableEntity entity, CharacterData data)
    {
        _entity = entity;
        entityBaseStat = data.CharacterBaseStat;

        _currentLevel = data.CharacterLevel;

        _maxHealth = MaxHealth(_currentLevel);
        currentHealth = _maxHealth;

        _maxMana = MaxMana(_currentLevel);
        currentMana = _maxMana;

        statusEffectHolder = new StatusEffectHolder(this, _entity);

        _statEffectBonuses = new();
        _statEffectBonuses.ValidateStatList();


        OnHealthChanged?.Invoke(currentHealth, _maxHealth);
        OnManaChanged?.Invoke(currentMana, _maxMana);


    }
    public void DealDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            ///death
        }
        OnHealthChanged?.Invoke(currentHealth, _maxHealth);
    }
    public void UseMana(int amount)
    {
        currentMana -= amount;
        if (currentMana <= 0)
        {
            currentMana = 0;
        }
        OnManaChanged?.Invoke(currentMana, _maxMana);
    }

    public bool TryToAddStatusEffect(IStatusEffect effect)
    {
        statusEffectHolder.AddEffect(effect);
        return true;
    }
    
    public bool CanUseMana(int amount)
    {
        return currentMana >= amount;
    }
    
    public int MaxHealth(int currentLevel = 1)
    {
      StatType stamina = StatType.Stamina;
      int levelBonus = ((entityBaseStat.GetStatAmount(stamina) * 10) * currentLevel);
      int statBonus = Mathf.RoundToInt(entityBaseStat.GetStatAmount(stamina) * 0.6f);
        
      return levelBonus + statBonus;
    }
    public int MaxMana(int currentLevel = 1)
    {
        StatType intellect = StatType.Intellect;
        int levelBonus = ((entityBaseStat.GetStatAmount(intellect) * 5) * currentLevel);
        int statBonus = Mathf.RoundToInt((entityBaseStat.GetStatAmount(intellect) * 0.3f));
        return levelBonus + statBonus;
    }

    public int GetFullStatValue(StatType type)
    {
        var baseStats = entityBaseStat.GetStatAmount(type);
        var statusEffectVal = _statEffectBonuses.GetStatAmount(type);
        
        return baseStats + statusEffectVal;
    }
    public void ApplyStatusEffectStatbonuses(StatType type,int amount)
    {
        _statEffectBonuses.AddAmount(type,amount);
    }
    
}



[Serializable]
public class StatusEffectHolder
{
    private ITargetableEntity _entity;
    private List<IStatusEffect> _statusEffects;
    private EntityStat _entityStat;
    private List<IStatusEffect> _finishedStatusEffects;
    
    ///effect ID, effect description, icon and starting time
    private event Action<StatEffectData> OnAddedStatusEffect;
    
    public event Action<string> OnRemovedStatusEffect;
    
    public StatusEffectHolder(EntityStat stat,ITargetableEntity entity)
    {
        _entityStat = stat;
        _entity = entity;
        _statusEffects = new();
    }
    public void AddEffect(IStatusEffect effect)
    {
        Debug.Log("Added new effect");
        effect.OnEnterStatus(_entity);
        _statusEffects.Add(effect);
        OnAddedStatusEffect?.Invoke(effect.GetStatusEffectUIInformation());
        if (_statusEffects.Count == 1)
        {
            _entityStat.StartCoroutine(UpdateStatusEffectsRoutine());
        }
        
    }
    public IEnumerator UpdateStatusEffectsRoutine()
    {
        while (_statusEffects.Count > 0)
        {
            foreach (var statusEffect in _statusEffects)
            {
                statusEffect.UpdateStatusEffect(Time.deltaTime);
            }
            RemoveFinishedStatusEffects();
            yield return null;
        }
        
    }
    private void RemoveFinishedStatusEffects()
    {
        _finishedStatusEffects = _statusEffects.FindAll(s => s.IsFinished());
        foreach (var finishedStatus in _finishedStatusEffects)
        {
            _statusEffects.Remove(finishedStatus);
            finishedStatus.OnExitStatus();
        }
        _finishedStatusEffects.Clear();
    }
    
}
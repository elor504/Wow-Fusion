using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityStat : MonoBehaviour
{
    
    [SerializeField] private StatContainer statContainer;

    private int _currentLevel = 1;
    private int _maxHealth;
    private int _maxMana;
    
    [SerializeField] private int currentHealth;
    [SerializeField] private int currentMana;
    
    public int CurrentHealth => currentHealth;
    public int CurrentMana => currentMana;

    public event Action <int,int> OnHealthChanged;
    public event Action <int,int> OnManaChanged;
    
    
    private void OnValidate()
    {
        statContainer.ValidateStatList();
    }
    
    public void Init(StatContainer newStatContainer,int level = 1)
    {
        statContainer = newStatContainer;
        
        _currentLevel = level;
        
        _maxHealth = MaxHealth(_currentLevel);
        currentHealth = _maxHealth;
        
        _maxMana = MaxMana(_currentLevel);
        currentMana = _maxMana;
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

    public bool CanUseMana(int amount)
    {
        return currentMana >= amount;
    }
    
    public int MaxHealth(int currentLevel = 1)
    {
      StatType stamina = StatType.Stamina;
      int levelBonus = ((statContainer.GetStatAmount(stamina) * 10) * currentLevel);
      int statBonus = Mathf.RoundToInt(statContainer.GetStatAmount(stamina) * 0.6f);
        
      return levelBonus + statBonus;
    }
    public int MaxMana(int currentLevel = 1)
    {
        StatType intellect = StatType.Intellect;
        int levelBonus = ((statContainer.GetStatAmount(intellect) * 5) * currentLevel);
        int statBonus = Mathf.RoundToInt((statContainer.GetStatAmount(intellect) * 0.3f));
        return levelBonus + statBonus;
    }
}

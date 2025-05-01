using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class BasicEnemy : MonoBehaviour, ITargetableEntity,IPointerEnterHandler,IPointerExitHandler  
{

    [Header("References")] 
    [SerializeField] private GameObject hoveringVisual;
    [SerializeField] private GameObject beingTargetedVisual;
    [SerializeField] private Transform projectileSpawn;
    [SerializeField] private Transform hitPosition;
    
    //Hold ref to base stats
    [Header("Temp Base Stats")]
    [SerializeField]private int baseHealth;
    [SerializeField]private int baseMana;
    
    
    //hold runtime stat variables
    private int _currentHealth;
    
 
    public void DealDamage(ITargetableEntity caster, int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            ///Death
        }
    }


    public void Heal(ITargetableEntity caster)
    {
        _currentHealth += 0;
        if (_currentHealth > baseHealth)
        {
            _currentHealth = baseHealth;
        }
    }

    public int GetHealth()
    {
        return baseHealth;
    }

    public int GetMana()
    {
        return baseMana;
    }

    public bool CanCastSpell(int amount)
    {
        if (baseMana >= amount)
        {
            baseMana -= amount;
            
            if(baseMana < 0)
                baseMana = 0;

            return true;
        }


        return false;
    }

    public GameObject GetEntityGO()
    {
        return gameObject;
    }

    public Transform GetProjectileSpawnPosition()
    {
        return projectileSpawn;
    }

    public Transform GetHitPosition()
    {
        if (hitPosition)
            return hitPosition;

        return transform;
    }


    public bool CanBeTargeted()
    {
        return true;
    }

    public void OnTargeted()
    {
        beingTargetedVisual.SetActive(true);
    }

    public void OnStopTargeting()
    {
        beingTargetedVisual.SetActive(false);
    }

    public bool IsEnemy()
    {
        return true;
    }
    public bool IsAlly()
    {
        return false;
    }



    public void OnHovering()
    {
        hoveringVisual.SetActive(true);
    }

    public void OnStoppedHovering()
    {
        hoveringVisual.SetActive(false);
    }
    
    
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        TargetManager.SetCurrentHoveredEntity(this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        TargetManager.SetCurrentHoveredEntity(null);
    }
}
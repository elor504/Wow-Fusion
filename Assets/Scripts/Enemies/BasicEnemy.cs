using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class BasicEnemy : MonoBehaviour, ITargetableEntity,IPointerEnterHandler,IPointerExitHandler
{
    [FormerlySerializedAs("targetVisual")]
    [Header("References")] 
    [SerializeField] private GameObject hoveringVisual;
    [SerializeField] private GameObject beingTargetedVisual;
    
    
    //Hold ref to base stats
    [Header("Temp Base Stats")]
    [SerializeField]private int baseHealth;
    
    
    //hold runtime stat variables
    private int _currentHealth;
    
    public void DealDamage(GameObject caster)
    {
        _currentHealth -= 0;
        if (_currentHealth <= 0)
        {
            ///Death
        }
    }

    public void Heal(GameObject caster)
    {
        _currentHealth += 0;
        if (_currentHealth > baseHealth)
        {
            _currentHealth = baseHealth;
        }
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
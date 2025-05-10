using System;
using UnityEngine;

public abstract class StatusEffectDataSO : ScriptableObject
{
   public abstract IStatusEffect GetStatusEffect();
}

[Serializable]
public abstract class StatusEffectData
{
   [SerializeField] private string statusEffectName;
   [SerializeField] private string statusEffectDescription;
   [SerializeField] private Sprite statusEffectIcon;
   [SerializeField] private protected float statusEffectDuration;
   
   public string StatusEffectName => statusEffectName;
   public string StatusEffectDescription => statusEffectDescription;
   public Sprite StatusEffectIcon => statusEffectIcon;
   public float StatusEffectDuration => statusEffectDuration;
}
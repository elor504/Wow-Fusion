using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stat Effect Data", menuName = "Status Effects/Stat Effect")]
[Serializable]
public class StatEffectDataSO : StatusEffectDataSO
{
    [SerializeField] private StatEffectData statEffectData;
    public StatEffectData StatEffectData => statEffectData;
    
    
    public override IStatusEffect GetStatusEffect()
    {
        return new StatBonusEffect(statEffectData, statEffectData.StatusEffectDuration);
    }
}
[Serializable]
public class StatEffectData : StatusEffectData
{
    
    [SerializeField] private StatType statToEffect;
    [SerializeField] private int statBonus;
    
    public StatType StatToEffect => statToEffect;
    public int StatBonus => statBonus;
}
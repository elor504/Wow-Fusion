using System;
using UnityEngine;

[Serializable]
public class Stat
{
    [HideInInspector]
    [SerializeField] private string statName;
    public StatType StatID;
    public int BaseValue;
    

    public Stat(StatType statID,int baseValue)
    {
        StatID = statID;
        BaseValue = baseValue;
        statName = statID.ToString();
    }
    
    
}

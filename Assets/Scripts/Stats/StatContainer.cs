using System;
using System.Collections.Generic;
using UnityEngine;

public class StatContainer
{
    ///Credit to chatgpt i don't want to take credit :D
    private Dictionary<PrimaryStatType,Stat> _primaryStats = new Dictionary<PrimaryStatType, Stat>();
    private Dictionary<SecondaryStatType,Stat> _secondaryStats = new Dictionary<SecondaryStatType, Stat>();

    public StatContainer(Dictionary<PrimaryStatType, Stat> primary, Dictionary<SecondaryStatType, Stat> secondary)
    {
        LoadStats(primary, secondary);
    }
    
    public void LoadStats(Dictionary<PrimaryStatType,Stat>  primary, Dictionary<SecondaryStatType,Stat> secondary)
    {
        _primaryStats = primary;
        _secondaryStats = secondary;
    }
    
    public float GetPrimaryStat(PrimaryStatType type) => _primaryStats[type].Value;
    public float GetSecondaryStat(SecondaryStatType type) => _secondaryStats[type].Value;
    
    public void AddBonus(PrimaryStatType type, float bonus)
    {
        _primaryStats[type].BonusValue += bonus;
    }
    public void AddBonus(SecondaryStatType type, float bonus)
    {
        _secondaryStats[type].BonusValue += bonus;
    }
}


public enum PrimaryStatType { Strength, Agility, Intellect, Stamina }
public enum SecondaryStatType { Crit, Haste, Mastery }
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatContainer
{   
    [SerializeField] private List<Stat> stats = new List<Stat>();

    public StatContainer()
    {
        ValidateStatList();
    }
    
    public void ValidateStatList()
    {
        foreach (StatType type in Enum.GetValues(typeof(StatType)))
        {
            int statID = (int) type;
            Stat stat = stats.Find(x => x.StatID == type);
            if (stat == null)
            {
                int indexToInsert = (int) type;
                if (CheckIfIndexToInsertExitsFromMainStat(indexToInsert))
                {
//                    Debug.Log($"[StatContainer] Adding stat to list {NormalizeIndex(indexToInsert)}");
                    indexToInsert = NormalizeIndex(indexToInsert);
                }

                if ((stats.Count - 1) >= indexToInsert)
                {
                    stats.Add(new Stat(type, 0));
                }
                else
                {
                    stats.Insert(indexToInsert, new Stat(type, 0));
                }
            }
        }
    }

    public void AddAmount(StatType type, int amount)
    {
        Stat matchedStat = stats.Find(s => s.StatID == type);
        if (matchedStat != null)
        {
            matchedStat.BaseValue += amount;
        }
    }
    public int GetStatAmount(StatType type)
    {
        Stat stat = stats.Find(x => x.StatID == type);
        if (stat == null)
        {
            return 0;
        }

        return stat.BaseValue;
    }
    ///Find good math to make it more flexible 
    private bool CheckIfIndexToInsertExitsFromMainStat(int index)
    {
        return index > 3;
    }
    
    
    ///I don't know if this is good, but if i do continue to work on the stats it would be ready for more complicated stuff I think :P
    private int NormalizeIndex(int indexToInsert)
    {
        int index = indexToInsert - StartingIDOfSecondaryStats() + AmountOfMainStats();
        return index;
    }
    private int AmountOfMainStats()
    {
        return 4;
    }
    private int StartingIDOfSecondaryStats()
    {
        return 100;
    }
}

public enum StatType
{
    ///Main stats
    Strength = 0, 
    Agility = 1, 
    Intellect = 2, 
    Stamina = 3,
    ///Secondaries stats
    Crit = 100,
    Haste = 101, 
    Speed = 102
}

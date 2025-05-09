using System;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;

public class CharacterClass : MonoBehaviour
{
    private StatContainer _classBaseStats;
    private EntityStat _characterStat;


    public void Init(BaseClassData classData, EntityStat characterStat)
    {
        LoadClassData(classData);
        _characterStat = characterStat;

    }

    public void LoadClassData(BaseClassData classDataSO)
    {
        _classBaseStats = classDataSO.ClassBaseStats;
    }
}
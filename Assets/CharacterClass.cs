using System;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;

public class CharacterClass : MonoBehaviour
{
    
    ///TEMP
    [SerializeField] private BaseClassData baseClassData;
    
    private PlayerClassData _playerClassData;

    private void Awake()
    {
        LoadClassData(baseClassData);
    }


    public void LoadClassData(PlayerClassData loadedData)
    {
        _playerClassData = loadedData;
    }

    public void LoadClassData(BaseClassData classDataSO)
    {
        _playerClassData = new PlayerClassData(classDataSO);
    }
}

/// <summary>
/// This is a Runtime class data to seperate pure data which can be also be saved in a save file 
/// </summary>
[Serializable]
public class PlayerClassData
{

    [SerializeField] private readonly string _classID;
    [SerializeField] private readonly string _className;
    [SerializeField] private readonly string _classDescription;

    [SerializeField] private readonly Sprite _classIcon;

    [SerializeField] private readonly PrimaryStatType _classMainStatType;
    [SerializeField] private readonly List<StartingClassPrimaryStat> _primaryStats;
    [SerializeField] private readonly List<StartingClassSecondaryStat> _secondaryStats;

    [SerializeField] private readonly StatContainer _statContainer;
    
    public string ClassID => _classID;
    public string ClassName => _className;
    public string ClassDescription => _classDescription;

    public Sprite ClassIcon => _classIcon;

    public PrimaryStatType ClassMainStatType => _classMainStatType;
    public List<StartingClassPrimaryStat> PrimaryStats => _primaryStats;
    public List<StartingClassSecondaryStat> SecondaryStats => _secondaryStats;
    
    public StatContainer StatContainer => _statContainer;
    
    public PlayerClassData(BaseClassData classData)
    {
        _classID = classData.ClassID;
        _className = classData.ClassName;
        _classDescription = classData.ClassDescription;
        _classIcon = classData.ClassIcon;
        _classMainStatType = classData.ClassMainStatType;
        _primaryStats = classData.StartingClassPrimaryStats;
        _secondaryStats = classData.StartingClassSecondaryStats;
        _statContainer = new StatContainer(classData.GetBasePrimaryStats(), classData.GetBaseSecondaryStats());
    }

}
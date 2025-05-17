using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Class Data", menuName = "ScriptableObjects/Classes/base Class Data", order = 1)]

///NOTES
///If I do add levels, how exactly do i update the stats in the player?,
/// also if I do ever change it that it would update for other players as well who had the previous
/// stat bonuses

public class BaseClassData : ScriptableObject
{
  [SerializeField] private ClassData classData;

  [SerializeField] private List<SkillDataSO> classSkills;
  
  public string ClassName => classData.ClassName;
  public string ClassDescription => classData.ClassDescription;
  
  public Sprite ClassIcon => classData.ClassIcon;
  
  public StatType ClassMainStatType => classData.ClassMainStatType;
  public StatContainer ClassBaseStats => classData.ClassBaseStats;
  
  public List<SkillDataSO> ClassSkills => classSkills;
    public ClassData GetClassData => classData;
  
  private void OnValidate()
  {
    classData.ClassBaseStats.ValidateStatList();
  }
}

[Serializable]
public struct ClassData
{
  [Header("Data")]
  [SerializeField] private ClassType classID;
  [SerializeField] private string className;
  [SerializeField] private string classDescription;
  
  [SerializeField] private Sprite classIcon;
  [Header("Primary Stats")]
  [SerializeField] private StatType classMainStatType;
  [SerializeField] private StatContainer classBaseStats;
  
  public string ClassName => className;
  public string ClassDescription => classDescription;
  
  public Sprite ClassIcon => classIcon;
    public ClassType GetClassID => classID;
  public StatType ClassMainStatType => classMainStatType;
  public StatContainer ClassBaseStats => classBaseStats;
  
  
}

public enum ClassType
{
    Warrior = 100,
    Mage = 200,
    Ranger = 300
}

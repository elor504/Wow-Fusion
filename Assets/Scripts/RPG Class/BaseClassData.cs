using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Class Data", menuName = "ScriptableObjects/Classes/base Class Data", order = 1)]
public class BaseClassData : ScriptableObject
{
  [Header("Data")]
  [SerializeField] private string classID;
  [SerializeField] private string className;
  [SerializeField] private string classDescription;
  
  [SerializeField] private Sprite classIcon;
  [Header("Primary Stats")]
  [SerializeField] private PrimaryStatType classMainStatType;
  [SerializeField] private List<StartingClassPrimaryStat> startingClassPrimaryStats;
  
  [Header(("Secondary Stats"))]
  [SerializeField] private List<StartingClassSecondaryStat> startingClassSecondaryStats;

  
  public string ClassID => classID;
  public string ClassName => className;
  public string ClassDescription => classDescription;
  
  public Sprite ClassIcon => classIcon;
  
  public PrimaryStatType ClassMainStatType => classMainStatType;
  
  public List<StartingClassPrimaryStat> StartingClassPrimaryStats => startingClassPrimaryStats;
  public List<StartingClassSecondaryStat> StartingClassSecondaryStats => startingClassSecondaryStats;
  

  public Dictionary<PrimaryStatType, Stat> GetBasePrimaryStats()
  {
      var statDict = new Dictionary<PrimaryStatType, Stat>();
      foreach (var primaryStat in startingClassPrimaryStats)
      {
          statDict[primaryStat.GetPrimaryStatType] = new Stat(primaryStat.PrimaryStatValue);
      }

      return statDict;
  }
  public Dictionary<SecondaryStatType, Stat> GetBaseSecondaryStats()
  {
      var statDict = new Dictionary<SecondaryStatType, Stat>();
      foreach (var secondaryStat in startingClassSecondaryStats)
      {
          statDict[secondaryStat.GetSecondaryStat] = new Stat(secondaryStat.PrimaryStatValue);
      }

      return statDict;
  }

  private void OnValidate()
  {
      foreach (PrimaryStatType type in Enum.GetValues(typeof(PrimaryStatType)))
      {
          StartingClassPrimaryStat stat = startingClassPrimaryStats.Find(x => x.GetPrimaryStatType == type);
          if (stat == null)
          {
              startingClassPrimaryStats.Insert((int)type,new StartingClassPrimaryStat(type));
          }
      }
      
      foreach (SecondaryStatType type in Enum.GetValues(typeof(SecondaryStatType)))
      {
          StartingClassSecondaryStat stat = startingClassSecondaryStats.Find(x => x.GetSecondaryStat == type);
          if (stat == null)
          {
              startingClassSecondaryStats.Insert((int)type,new StartingClassSecondaryStat(type));
          }
      }
  }
}

[Serializable]
public class StartingClassPrimaryStat
{
    [SerializeField] private PrimaryStatType PrimaryStatType;
    [SerializeField] private float primaryStatStartValue;

    public StartingClassPrimaryStat(PrimaryStatType primaryStatType)
    {
        PrimaryStatType = primaryStatType;
    }
    
    public PrimaryStatType GetPrimaryStatType => PrimaryStatType;
    public float PrimaryStatValue => primaryStatStartValue;
}
[Serializable]
public class StartingClassSecondaryStat
{
    [SerializeField] private SecondaryStatType secondaryStat;
    [SerializeField] private float primaryStatStartValue;
    public StartingClassSecondaryStat(SecondaryStatType secondaryStatType)
    {
        secondaryStat = secondaryStatType;
    }
    public SecondaryStatType GetSecondaryStat => secondaryStat;
    public float PrimaryStatValue => primaryStatStartValue;
}
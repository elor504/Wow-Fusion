

using System;
using UnityEngine;

[Serializable]
public class CharacterData
{
    [SerializeField] private int characterLevel;
    [SerializeField] private int classType;
    [SerializeField] private string characterName;
    [SerializeField] private StatContainer characterBaseStat;
    [SerializeField] private CharacterVisualData characterVisualData;


    public int CharacterLevel => characterLevel;
    public int ClassType => classType;
    public string CharacterName => characterName;
    public StatContainer CharacterBaseStat => characterBaseStat;
    public CharacterVisualData CharacterVisualData => characterVisualData;

    ///skills

    ///Inventory?

    public CharacterData (string characterName ,int characterLevel, ClassType classType, StatContainer characterBaseStat, CharacterVisualData characterVisualData)
    {
        this.characterName = characterName;
        this.characterLevel = characterLevel;
        this.classType = (int)classType;
        this.characterBaseStat = characterBaseStat;
        this.characterVisualData = characterVisualData;
    }
}


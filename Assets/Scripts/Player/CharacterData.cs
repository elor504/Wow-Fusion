

using System;

[Serializable]
public class CharacterData
{
    public int CharacterLevel { private set; get; }
    public int ClassType { private set; get; }
    public StatContainer CharacterBaseStat { private set; get; }

    ///skills

    ///Inventory?

    public CharacterData (int characterLevel, ClassType classType, StatContainer characterBaseStat)
    {
        CharacterLevel = characterLevel;
        CharacterBaseStat = characterBaseStat;
        ClassType = (int)classType;
    }
}


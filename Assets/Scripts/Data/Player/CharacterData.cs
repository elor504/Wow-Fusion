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
    [SerializeField] private CharacterEquipmentData characterEquipmentData;

    public int CharacterLevel => characterLevel;
    public int ClassType => classType;
    public string CharacterName => characterName;
    public StatContainer CharacterBaseStat => characterBaseStat;
    public CharacterVisualData CharacterVisualData => characterVisualData;
    public CharacterEquipmentData CharacterEquipmentData => characterEquipmentData;
    ///skills

    ///Inventory?

    public CharacterData(string characterName, int characterLevel, ClassType classType,
        StatContainer characterBaseStat, CharacterVisualData characterVisualData, CharacterEquipmentData characterEquipmentData)
    {
        this.characterName = characterName;
        this.characterLevel = characterLevel;
        this.classType = (int)classType;
        this.characterBaseStat = characterBaseStat;
        this.characterVisualData = characterVisualData;
        this.characterEquipmentData = characterEquipmentData;
    }
    public CharacterData()
    {

    }

    public void DeserializeEquipment(string serializedEquipmentData)
    {
        characterEquipmentData = JsonUtility.FromJson<CharacterEquipmentData>(serializedEquipmentData);
    }
    public string SerializeEquipment()
    {
        return JsonUtility.ToJson(characterEquipmentData);
    }

    public void DeserializeVisual(string serializedVisualData)
    {
        characterVisualData = JsonUtility.FromJson<CharacterVisualData>(serializedVisualData);
    }
    public string SerializeVisual()
    {
        return JsonUtility.ToJson(characterVisualData);
    }

    public string Serialize()
    {
        return JsonUtility.ToJson(this);
    }

}
public static class CharacterDataExtention
{
    public static void Deserialize(this CharacterData data, string serializedData)
    {
        data = JsonUtility.FromJson<CharacterData>(serializedData);
    }


}
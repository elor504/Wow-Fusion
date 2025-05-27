using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class CharacterEquipmentData
{
    [SerializeField] private EquipableItemData helmetEquipmentData;
    [SerializeField] private EquipableItemData chestplateEquipmentData;
    [SerializeField] private EquipableItemData pantsEquipmentData;
    [SerializeField] private EquipableItemData glovesEquipmentData;
    [SerializeField] private EquipableItemData shoesEquipmentData;

    public EquipableItemData GetEquipableDataByType(EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.Helmet:
                return helmetEquipmentData;
            case EquipmentType.Chestplate:
                return chestplateEquipmentData;
            case EquipmentType.Pants:
                return pantsEquipmentData;
            case EquipmentType.gloves:
                return glovesEquipmentData;
            case EquipmentType.Shoes:
                return shoesEquipmentData;
        }

        return null;
    }
    
    
    public bool TryToEquip(EquipableItemData equipableItemData,out EquipableItemData previousEquipped)
    {
        previousEquipped = null;
       
        previousEquipped = UpdateEquipment(equipableItemData);
        return true;
        return false;
    }

    private EquipableItemData UpdateEquipment(EquipableItemData equipableItemData)
    {
        EquipableItemData previousEquipment = null;
        switch (equipableItemData.EquipmentType)
        {
            case EquipmentType.Helmet:
                previousEquipment = helmetEquipmentData;
                helmetEquipmentData = equipableItemData;
                break;
            case EquipmentType.Chestplate:
                previousEquipment = chestplateEquipmentData;
                chestplateEquipmentData = equipableItemData;
                break;
            case EquipmentType.Pants:
                previousEquipment = pantsEquipmentData;
                pantsEquipmentData = equipableItemData;
                break;
            case EquipmentType.gloves:
                previousEquipment = glovesEquipmentData;
                glovesEquipmentData = equipableItemData;
                break;
            case EquipmentType.Shoes:
                previousEquipment = shoesEquipmentData;
                shoesEquipmentData = equipableItemData;
                break;
        }
        return previousEquipment;
    }
    
}
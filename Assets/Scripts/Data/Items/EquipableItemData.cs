using System;
using UnityEngine;
[Serializable]
public class EquipableItemData : BaseItemData
{
    [SerializeField] private int levelReq;
    [SerializeField] private EquipmentType equipmentType;
    [SerializeField] private StatContainer stats;
    
    public int LevelReq => levelReq;
    public EquipmentType EquipmentType => equipmentType;
    public StatContainer Stats => stats;
    
    public EquipableItemData(string itemName, string itemDescription, int levelReq,EquipmentType equipmentType,StatContainer statContainer,int itemAmount = 1) : base(itemName,
        itemDescription, itemAmount)
    {
        stats = statContainer;
        this.levelReq = levelReq;
        this.equipmentType = equipmentType;
    }

    public bool IsEmpty()
    {
        return string.IsNullOrEmpty(ItemName);
    }
}
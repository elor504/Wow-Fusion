using System;
using UnityEngine;

[Serializable]
public class BaseItemData
{
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private int itemAmount;

    public string ItemName => itemName;
         public string ItemDescription => itemDescription;
    public int ItemAmount => itemAmount;


    public BaseItemData(string itemName,string itemDescription,int itemAmount = 1)
    {
        this.itemName = itemName;
        this.itemDescription = itemDescription;
        this.itemAmount = itemAmount;
    }
}

public class EquipableItemData : BaseItemData
{
    [SerializeField] private int levelReq;
    [SerializeField] private EquipmentType equipmentType;
    [SerializeField] private StatContainer stats;
    
    public int LevelReq => levelReq;
    public EquipmentType EquipmentType => equipmentType;
    public StatContainer Stats => stats;
    
    public EquipableItemData(string itemName, string itemDescription, int levelReq,EquipmentType equipmentType,int itemAmount = 1) : base(itemName,
        itemDescription, itemAmount)
    {
        
        this.levelReq = levelReq;
        this.equipmentType = equipmentType;
    }

}
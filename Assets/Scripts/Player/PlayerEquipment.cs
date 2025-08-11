using Fusion;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PlayerEquipment : NetworkBehaviour
{
    [SerializeField] private Transform root;
    [SerializeField] private Transform[] bones;
    [SerializeField] private Transform armorParent;
    [SerializeField] private CharacterHairMeshes characterHairMeshes;

    private List<EquipmentObject> _equipmentObjects = new List<EquipmentObject>();
    [SerializeField] private List<EquipmentObject> defaultEquipment = new List<EquipmentObject>();


    [Networked]
    public int HairColor { get; set; }

    [Networked]
    public string CurrentEquippedPantsID { set; get; }
    [Networked]
    public string CurrentEquippedChestplateID { set; get; }
    [Networked]
    public string CurrentEquippedShoesID { set; get; }
    [Networked]
    public string CurrentEquippedHelmetID { set; get; }
    [Networked]
    public string CurrentEquippedGlovesID { set; get; }


    public override void Spawned()
    {
        base.Spawned();

        foreach (var item in defaultEquipment)
        {
            item.Init(item.EquipmentType.ToString());
        }
        _equipmentObjects.AddRange(defaultEquipment);



        Color hairColor = DataBankSO.Instance.CharacterVisual.GetHairColorByType((HairColorType)HairColor);
        characterHairMeshes.ChangeHairMeshesColor(hairColor);
    }
    public void Init(CharacterData data)
    {
        HairColor = (int)data.CharacterVisualData.HairColor;

        Color hairColor = DataBankSO.Instance.CharacterVisual.GetHairColorByType(data.CharacterVisualData.HairColor);
        characterHairMeshes.ChangeHairMeshesColor(hairColor);
        EquipmentType type = EquipmentType.Helmet;
        var equipmentData = data.CharacterEquipmentData.GetEquipableDataByType(type);
        int equipmentTypeLength = Enum.GetNames(typeof(EquipmentType)).Length;

        if (data.CharacterEquipmentData != null)
          {
              for (int i = 0; i < equipmentTypeLength; i++)
              {
                  type = (EquipmentType)i;
                  equipmentData = data.CharacterEquipmentData.GetEquipableDataByType(type);

                  UpdateVisual(type, equipmentData);
              }
          }
          else
          {
              Debug.Log("[Player Equipment] Character EquipmentData is null");
          }
    }


    public void UpdateVisual(EquipmentType type, EquipableItemData equipment)
    {
        UpdateEquipmentVisual(type, equipment);
    }
    private void UpdateEquipmentVisual(EquipmentType type, EquipableItemData equipment)
    {
        var previousEquippedID = GetCurrentEquippedItemByType(type);
        SetCurrentEquippedItemByType(type, equipment.ItemName);
        var currentEquipmentObject = _equipmentObjects.Find(equipment => equipment.EquipmentID == GetCurrentEquippedItemByType(type));
        if (!currentEquipmentObject)
        {
            var armorPF = DataBankSO.Instance.GetEquipmentDataByID(equipment.ItemName).EquipmentObject;
            var spawnedArmor = Instantiate(armorPF, armorParent);
            spawnedArmor.Init(equipment.ItemName, root, bones);
            _equipmentObjects.Add(spawnedArmor);
        }
        SetEquipmentVisualActive(previousEquippedID, false);
        SetEquipmentVisualActive(GetCurrentEquippedItemByType(type), true);

    }
    private string GetCurrentEquippedItemByType(EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.Helmet:
                return CurrentEquippedHelmetID;
            case EquipmentType.Chestplate:
                return CurrentEquippedChestplateID;
            case EquipmentType.Pants:
                return CurrentEquippedPantsID;
            case EquipmentType.Shoes:
                return CurrentEquippedShoesID;
            case EquipmentType.gloves:
                return CurrentEquippedGlovesID;
        }

        return "";
    }
    private void SetCurrentEquippedItemByType(EquipmentType type, string id)
    {
        switch (type)
        {
            case EquipmentType.Helmet:
                CurrentEquippedHelmetID = id;
                break;
            case EquipmentType.Chestplate:
                CurrentEquippedChestplateID = id;
                break;
            case EquipmentType.Pants:
                CurrentEquippedPantsID = id;
                break;
            case EquipmentType.Shoes:
                CurrentEquippedShoesID = id;
                break;
            case EquipmentType.gloves:
                CurrentEquippedGlovesID = id;
                break;
        }
    }
    private void SetEquipmentVisualActive(string equipmentID, bool isActive)
    {
        var equipmentObj = _equipmentObjects.Find(equipment => equipment.EquipmentID == CurrentEquippedPantsID);
        equipmentObj.gameObject.SetActive(isActive);
    }
   




  


}



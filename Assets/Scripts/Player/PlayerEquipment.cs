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

    //TODO: Add Id hold for each equipment for getting the visuals

    public string CurrentEquippedPantsID { set; get; }


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

 

    public void UpdateVisual(EquipmentType type, EquipableItemData equipment)
    {
        UpdateEquipmentVisual(type, equipment);
    }
    private void UpdateEquipmentVisual(EquipmentType type, EquipableItemData equipment)
    {
        switch (type)
        {
            case EquipmentType.Helmet:
                break;
            case EquipmentType.Chestplate:
                break;
            case EquipmentType.Pants:
                var previousEquippedID = CurrentEquippedPantsID;
                CurrentEquippedPantsID = equipment.ItemName;
                Debug.Log($"New equipment: {CurrentEquippedPantsID}, Previous: {previousEquippedID}");
                var currentEquipmentObject = _equipmentObjects.Find(equipment => equipment.EquipmentID == CurrentEquippedPantsID);
                if (!currentEquipmentObject)
                {
                    var armorPF = DataBankSO.Instance.GetEquipmentDataByID(equipment.ItemName).EquipmentObject;
                    var spawnedArmor = Instantiate(armorPF, armorParent);
                    spawnedArmor.Init(equipment.ItemName,root, bones);
                    _equipmentObjects.Add(spawnedArmor);
                }
                SetEquipmentVisualActive(previousEquippedID, false);
                SetEquipmentVisualActive(CurrentEquippedPantsID, true);
                break;
            case EquipmentType.Shoes:
                break;
            case EquipmentType.gloves:
                break;
            default:
                break;
        }


    }


    private void SetEquipmentVisualActive(string equipmentID, bool isActive)
    {
        var equipmentObj = _equipmentObjects.Find(equipment => equipment.EquipmentID == CurrentEquippedPantsID);
        equipmentObj.gameObject.SetActive(isActive);
    }
   
    public void Init(CharacterData data)
    {
        HairColor = (int)data.CharacterVisualData.HairColor;

        Color hairColor = DataBankSO.Instance.CharacterVisual.GetHairColorByType(data.CharacterVisualData.HairColor);
        characterHairMeshes.ChangeHairMeshesColor(hairColor);
        EquipmentType type = EquipmentType.Helmet;
        string equipmentName = data.CharacterEquipmentData.GetEquipableDataByType(type).ItemName;

      /*  if (data.CharacterEquipmentData != null)
        {
            for (int i = 0; i < equipmentVisuals.Count; i++)
            {
                type = (EquipmentType)i;
                equipmentName = data.CharacterEquipmentData.GetEquipableDataByType(type).ItemName;

                UpdateVisual(type, equipmentName);
            }
        }
        else
        {
            Debug.Log("[Player Equipment] Character EquipmentData is null");
        }*/
    }



  


}



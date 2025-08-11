using Fusion;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterVis : MonoBehaviour
{
    [SerializeField] private Transform root;
    [SerializeField] private Transform[] bones;
    [SerializeField] private Transform armorParent;
    [SerializeField] private CharacterHairMeshes characterHairMeshes;

    private List<EquipmentObject> _equipmentObjects = new List<EquipmentObject>();
    [SerializeField] private List<EquipmentObject> defaultEquipment = new List<EquipmentObject>();


    public int HairColor { get; set; }

    //TODO: Add Id hold for each equipment for getting the visuals

    public string CurrentEquippedPantsID { set; get; }
    public string CurrentEquippedChestplateID { set; get; }
    public string CurrentEquippedShoesID { set; get; }
    public string CurrentEquippedHelmetID { set; get; }
    public string CurrentEquippedGlovesID { set; get; }

    private void Awake()
    {
        
    }

    public void Init()
    {
        foreach (var item in defaultEquipment)
        {
            item.Init();
        }
        CurrentEquippedPantsID = EquipmentType.Pants.ToString();
        CurrentEquippedChestplateID = EquipmentType.Chestplate.ToString();
        CurrentEquippedShoesID = EquipmentType.Shoes.ToString();
        CurrentEquippedGlovesID = EquipmentType.gloves.ToString();
        CurrentEquippedHelmetID = EquipmentType.Helmet.ToString();
        _equipmentObjects.Clear();
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
        var equipmentObj = _equipmentObjects.Find(equipment => equipment.EquipmentID == equipmentID);
        equipmentObj?.gameObject.SetActive(isActive);
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

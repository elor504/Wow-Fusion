using Fusion;
using System;
using System.Collections.Generic;
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

    [Networked, OnChangedRender(nameof(ChangedPantsHandler))]
    public string CurrentEquippedPantsID { set; get; }
    [Networked, OnChangedRender(nameof(ChangedChestPlateHandler))]
    public string CurrentEquippedChestplateID { set; get; }
    [Networked, OnChangedRender(nameof(ChangedShoesHandler))]
    public string CurrentEquippedShoesID { set; get; }
    [Networked]
    public string CurrentEquippedHelmetID { set; get; }
    [Networked]
    public string CurrentEquippedGlovesID { set; get; }


    public override void Spawned()
    {
        base.Spawned();

        if (!Object.HasInputAuthority && !Object.HasStateAuthority)
        {
            foreach (var item in defaultEquipment)
            {
                item.Init();
            }
            _equipmentObjects.Clear();
            _equipmentObjects.AddRange(defaultEquipment);

            //ChangedChestPlateHandler();
            ChangedPantsHandler();
            ChangedShoesHandler();

            Color hairColor = DataBankSO.Instance.CharacterVisual.GetHairColorByType((HairColorType)HairColor);
            characterHairMeshes.ChangeHairMeshesColor(hairColor);
        }


    }
    public void InitVisual(CharacterVisualData data)
    {
        HairColor = (int)data.HairColor;

        Color hairColor = DataBankSO.Instance.CharacterVisual.GetHairColorByType(data.HairColor);
        characterHairMeshes.ChangeHairMeshesColor(hairColor);


    }
    public void InitEquipment(string[] data)
    {

        foreach (var item in defaultEquipment)
        {
            item.Init();
        }
        _equipmentObjects.Clear();
        _equipmentObjects.AddRange(defaultEquipment);
        Debug.Log("[PlayerEquipment]Init equipments");

        EquipmentType type = EquipmentType.Helmet;
        if (data != null)
        {
            for (int i = 0; i < data.Length; i++)
            {
                type = (EquipmentType)i;
                SetCurrentEquippedItemByType(type, data[i]);
            }
        }
    }
    public void InitEquipment(CharacterEquipmentData data)
    {
        EquipmentType type = EquipmentType.Helmet;
        var equipmentData = data.GetEquipableDataByType(type);
        int equipmentTypeLength = Enum.GetNames(typeof(EquipmentType)).Length;

        foreach (var item in defaultEquipment)
        {
            item.Init();
        }
        _equipmentObjects.Clear();
        _equipmentObjects.AddRange(defaultEquipment);
        Debug.Log("[PlayerEquipment]Init equipments");

        if (data != null)
        {
            for (int i = 0; i < equipmentTypeLength; i++)
            {
                type = (EquipmentType)i;
                equipmentData = data.GetEquipableDataByType(type);
                if (equipmentData != null)
                {
                    SetCurrentEquippedItemByType(type, equipmentData.ItemName);
                }
                else
                {
                    SetCurrentEquippedItemByType(type, type.ToString());
                }
            }
        }
        else
        {
            Debug.Log("[Player Equipment] Character EquipmentData is null");
        }

        #region old
        //if (data != null)
        //{
        //    for (int i = 0; i < equipmentTypeLength; i++)
        //    {
        //        type = (EquipmentType)i;
        //        equipmentData = data.GetEquipableDataByType(type);

        //        if (equipmentData != null)
        //            UpdateEquipmentVisual(type, equipmentData);
        //    }
        //}
        //else
        //{
        //    Debug.Log("[Player Equipment] Character EquipmentData is null");
        //}
        #endregion
    }
    private void ChangedChestPlateHandler(NetworkBehaviour network,string old,string newValue)
    {
      
        Debug.Log($"Changed Chestplate, oldID {old}, new id: {newValue}");
        EquipmentDataSO equipmentData = DataBankSO.Instance.GetEquipmentDataByID(newValue);
        if (equipmentData != null)
        {
            UpdateEquipmentVisual(EquipmentType.Chestplate, equipmentData.EquipmentName);
        }
    }
    private void ChangedPantsHandler()
    {
        EquipmentDataSO equipmentData = DataBankSO.Instance.GetEquipmentDataByID(CurrentEquippedPantsID);
        if (equipmentData != null)
        {
            UpdateEquipmentVisual(EquipmentType.Pants, equipmentData.EquipmentName);
        }

    }
    private void ChangedShoesHandler()
    {
        EquipmentDataSO equipmentData = DataBankSO.Instance.GetEquipmentDataByID(CurrentEquippedShoesID);
        if (equipmentData != null)
        {
            UpdateEquipmentVisual(EquipmentType.Shoes, equipmentData.EquipmentName);
        }
    }

    private void UpdateEquipmentVisual(EquipmentType type, string equipmentID)
    {
        var previousEquippedID = GetCurrentEquippedItemByType(type);
        SetCurrentEquippedItemByType(type, equipmentID);
        var currentEquipmentObject = _equipmentObjects.Find(equipment => equipment.EquipmentID == GetCurrentEquippedItemByType(type));
        if (!currentEquipmentObject)
        {
            var armorPF = DataBankSO.Instance.GetEquipmentDataByID(equipmentID).EquipmentObject;
            if (armorPF != null)
            {
                var spawnedArmor = Instantiate(armorPF, armorParent);
                spawnedArmor.Init(equipmentID, root, bones);
                _equipmentObjects.Add(spawnedArmor);
            }
            else
            {
                return;
            }
        }
        Debug.Log($"Changed equipment Previous: {previousEquippedID}, new {GetCurrentEquippedItemByType(type)}");
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
        Debug.Log($"Set current equipped item by type: {type} id: {id}");
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








}



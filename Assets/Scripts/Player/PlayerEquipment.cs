using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] private List<EquipmentVisual> equipmentVisuals = new List<EquipmentVisual>();
    private void OnValidate()
    {
        if (equipmentVisuals != null && equipmentVisuals.Count == 0)
        {
            int enumLength = Enum.GetNames(typeof(EquipmentType)).Length;
            for (int i = 0; i < enumLength; i++)
            {
                EquipmentVisual visual = new EquipmentVisual(null,(EquipmentType)i);
                equipmentVisuals.Add(visual);
            }
        }
    }

    public void UpdateVisual(EquipmentType type,Mesh[] meshes)
    {
        EquipmentVisual equipment = equipmentVisuals.Find((e => e.EquipmentType == type));

        var i = 0;
        foreach (var mesh in equipment.MeshRenderer)
        {
            mesh.sharedMesh = meshes[i];
            i++;
        }
    }
}

[Serializable]
public struct EquipmentVisual
{
    [HideInInspector] public string EquipmentTypeName;
    public EquipmentType EquipmentType;
    public SkinnedMeshRenderer[] MeshRenderer;
    
    public EquipmentVisual(SkinnedMeshRenderer[] renderer,EquipmentType type)
    {
        EquipmentTypeName = type.ToString();
        MeshRenderer = renderer;
        EquipmentType = type;
    }
}
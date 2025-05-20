using UnityEngine;
[CreateAssetMenu(fileName = "Equipment data", menuName = "ScriptableObjects/Equipment/Equipment Data", order = 1)]
public class EquipmentDataSO : ScriptableObject
{
    [Header("Settings")]
    [SerializeField] private string equipmentName;
    [SerializeField] private string equipmentDescription;
    [SerializeField] private EquipmentType equipmentType;
    [SerializeField] private int levelRequirement;
    [SerializeField] private StatContainer statContainer;

    [Header("Visual")]
    [SerializeField] private Mesh[] equipmentMeshes;
    

    public string EquipmentName => equipmentName;
    public string EquipmentDescription => equipmentDescription;
    public EquipmentType EquipmentType => equipmentType;
    public int LevelRequirement => levelRequirement;
    public StatContainer StatContainer => statContainer;
    public Mesh[] EquipmentMeshes => equipmentMeshes;
}

public enum EquipmentType
{
    Helmet,
    Chestplate,
    Pants,
    Shoes,
    gloves
}
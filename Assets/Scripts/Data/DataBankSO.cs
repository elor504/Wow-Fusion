using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DataBankSO", menuName = "Singleton/Bank")]
public class DataBankSO : ScriptableObject
{
    private static DataBankSO _instance;
    public static DataBankSO Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<DataBankSO>("Data");
            }
            return _instance;
        }
    }

    [Header("Equipments")]
    [SerializeField] private List<EquipmentDataSO> _equipmentData;
    public EquipmentDataSO GetEquipmentDataByID(string id)
    {
        return _equipmentData.Find(equipment => equipment.EquipmentName == id);
    }


}

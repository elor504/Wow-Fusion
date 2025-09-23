using UnityEngine;

[CreateAssetMenu(fileName = "Global_Settings_SO", menuName = "Singleton/GlobalSettings")]
public class GlobalSettingsSO : ScriptableObject
{
    private static GlobalSettingsSO _instance;
    public static GlobalSettingsSO Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<GlobalSettingsSO>("Global_Settings_SO");
            }
            return _instance;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenUI : MonoBehaviour
{
    [SerializeField] private Image loadingImage;
    [SerializeField] private Image loadingBar;


    public void ShowLoadingScreenUI()
    {
        gameObject.SetActive(true);
    }
    public void HideLoadingScreenUI()
    {
        gameObject.SetActive(true);
    }
    public void UpdateLoadingBar(float value,float min,float max)
    {
        loadingBar.fillAmount = UtilityMath.Map(value, min, max, 0, 1);  
    }
}

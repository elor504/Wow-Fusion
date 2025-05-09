using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDetector : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{



    public void OnPointerEnter(PointerEventData eventData)
    {
        InputManager.GetInstance.OnOnMouseOnUI(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InputManager.GetInstance.OnOnMouseOnUI(false);
    }
}

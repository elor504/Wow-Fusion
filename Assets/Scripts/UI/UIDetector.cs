using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDetector : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{



    public void OnPointerEnter(PointerEventData eventData)
    {
       GameManager.Instance.InputManager.OnOnMouseOnUI(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.InputManager.OnOnMouseOnUI(false);
    }
}

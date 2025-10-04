using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDetector : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{



    public void OnPointerEnter(PointerEventData eventData)
    {
       RuntimeSessionManager.LocalCharacter.InputManager.OnOnMouseOnUI(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RuntimeSessionManager.LocalCharacter.InputManager.OnOnMouseOnUI(false);
    }
}

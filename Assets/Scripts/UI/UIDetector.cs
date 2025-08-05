using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDetector : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{



    public void OnPointerEnter(PointerEventData eventData)
    {
       GameTest.LocalCharacter.InputManager.OnOnMouseOnUI(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameTest.LocalCharacter.InputManager.OnOnMouseOnUI(false);
    }
}

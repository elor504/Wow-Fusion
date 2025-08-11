using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HairColorSelector : MonoBehaviour
{
    [SerializeField] private List<Button> hairColorsButtons;

    public event Action<HairColorType> HairColorSelected;


    private void OnEnable()
    {
        SetButtonsColors();
    }
    private void OnDisable()
    {
        foreach (var hairColorButton in hairColorsButtons)
        {
            hairColorButton.onClick.RemoveAllListeners();
        }
    }

    private void SetButtonsColors()
    {
        int hairColorEnumLength = Enum.GetValues(typeof(HairColorType)).Length;
        for (int i = 0; i < hairColorEnumLength; i++)
        {
            HairColorType colorType = (HairColorType)i;
            Color color = DataBankSO.Instance.CharacterVisual.GetHairColorByType(colorType);
            color.a = 1;
            hairColorsButtons[i].image.color = color;
            hairColorsButtons[i].onClick.AddListener(() => OnClickHairButton(colorType));
        }
    }



    public void OnClickHairButton(HairColorType hairColorType)
    {
        HairColorSelected?.Invoke(hairColorType);
    }

}

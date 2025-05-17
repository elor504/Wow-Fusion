using System;
using UnityEngine;

[Serializable]
public class CharacterVisualData
{
    [SerializeField]private HairColorType hairColor;
    public HairColorType HairColor => hairColor;

    public CharacterVisualData(HairColorType color)
    {
        hairColor = color;
    }



}


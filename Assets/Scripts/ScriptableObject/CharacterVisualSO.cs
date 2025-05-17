
using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Visual Data", menuName = "ScriptableObjects/Visual/Color Data", order = 1)]
public class CharacterVisualSO : ScriptableObject
{
    [SerializeField] private List<HairVisualData> hairData;



    public Color GetHairColorByType(HairColorType type)
    {
        foreach (var data in hairData)
        {
            if (data.HairColor == type)
                return data.Color;
        }

        return Color.black;
    }

}

[Serializable]
public struct HairVisualData
{
    [SerializeField] private HairColorType hairColor;
    [SerializeField] private Color color;

    public HairColorType HairColor => hairColor;
    public Color Color => color;
}

public enum HairColorType
{
    Black,
    Brown,
    Blonde,
    Ginger,
    White
}
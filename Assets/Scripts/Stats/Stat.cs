using System;
using UnityEngine;

[Serializable]
public class Stat
{
    public float BaseValue;
    public float BonusValue;

    public float Value => BaseValue + BonusValue;

    public Stat(float baseValue)
    {
        BaseValue = baseValue;
        BonusValue = 0f;
    }
}

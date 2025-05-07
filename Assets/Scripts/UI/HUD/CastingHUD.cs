
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CastingHUD : MonoBehaviour
{
    [SerializeField] private GameObject castingHud;
    [SerializeField] private TextMeshProUGUI castingText;
    [SerializeField] private Image castingImage;


    private void Awake()
    {
        OnStoppedCasting();
    }

    private void OnEnable()
    {
        PlayerCastState.UpdateCastingHandler +=  UpdateCastHud;
        PlayerCastState.StartCastHandler +=  OnStartCasting;
        PlayerCastState.CastedHandler +=  OnStoppedCasting;
    }

    private void OnDisable()
    {
        PlayerCastState.UpdateCastingHandler -=  UpdateCastHud;
        PlayerCastState.StartCastHandler -=  OnStartCasting;
        PlayerCastState.CastedHandler -=  OnStoppedCasting;
    }

    private void UpdateCastHud(float castTimeLeft,float startingCastTime)
    {
        castingText.text = "Casting: " + castTimeLeft.ToString("0.00");
        var normalizedTime = UtilityMath.Map(castTimeLeft, 0, startingCastTime, 0, 1);
        castingImage.fillAmount = normalizedTime;
    }
    
    private void OnStartCasting(float castTimeLeft,float startingCastTime)
    {
        UpdateCastHud(castTimeLeft,startingCastTime);
        castingHud.SetActive(true);
    }

    private void OnStoppedCasting()
    {
        castingHud.SetActive(false);
    }
}


public static class UtilityMath
{
    
    public static float Map(float value, float inMin, float inMax, float OutMin, float outMax)
    {
        return (value - inMin) * (outMax - OutMin) / (inMax - inMin) + OutMin;
    }

    
}
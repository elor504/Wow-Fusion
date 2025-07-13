using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Homework 
{
    public class CharacterHPBar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private Image barImage;

        public void UpdateBar(int current,int max)   
        {
            amountText.text = current.ToString() + " / " + max.ToString();
            barImage.fillAmount = Map(current, 0, max, 0, 1);
        }
        public float Map(float value, float inMin, float inMax, float OutMin, float outMax)
        {
            return (value - inMin) * (outMax - OutMin) / (inMax - inMin) + OutMin;
        }

    }
}
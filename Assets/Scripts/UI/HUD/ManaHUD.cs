using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class ManaHUD : MonoBehaviour
{
    [SerializeField] private Image manaBar;
    [FormerlySerializedAs("healthText")] [SerializeField] private TextMeshProUGUI manaText;
   
    public void UpdateMana(int currentMana,int maxMana)
    {
        var normalizedHealth = UtilityMath.Map(currentMana, 0, maxMana, 0, 1);
        manaBar.fillAmount = normalizedHealth;
        manaText.text = "Mana: " + currentMana;
    }

    private void OnEnable()
    {
        if (GameTest.LocalCharacter)
            GameTest.LocalCharacter.CharacterStat.OnManaChanged += UpdateMana;
    }
    private void OnDisable()
    {
        if (GameTest.LocalCharacter)
            GameTest.LocalCharacter.CharacterStat.OnManaChanged -= UpdateMana;
    }
}

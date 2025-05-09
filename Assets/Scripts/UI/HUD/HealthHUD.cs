using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
   [SerializeField] private Image healthBar;
   [SerializeField] private TextMeshProUGUI healthText;
   
   public void UpdateHealth(int currentHealth,int maxHealth)
   {
      var normalizedHealth = UtilityMath.Map(currentHealth, 0, maxHealth, 0, 1);
      healthBar.fillAmount = normalizedHealth;
      healthText.text = "Health: " + currentHealth;
   }

   private void OnEnable()
   {
      GameManager.Instance.ClientPlayer.CharacterStat.OnHealthChanged += UpdateHealth;
   }
   private void OnDisable()
   {
      GameManager.Instance.ClientPlayer.CharacterStat.OnHealthChanged -= UpdateHealth;
   }
}

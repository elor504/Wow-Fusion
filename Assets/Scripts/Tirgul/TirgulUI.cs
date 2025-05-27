using TMPro;
using UnityEngine;

public class TirgulUI : MonoBehaviour
{
	private static TirgulUI instance;
	public static TirgulUI Instance => instance;
	[SerializeField] private TextMeshProUGUI playerAmountText;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}


	public void OnPlayerAmountChanged(int amount,int maxAmount)
	{
		playerAmountText.text = $"{amount}/{maxAmount}";
	}
}

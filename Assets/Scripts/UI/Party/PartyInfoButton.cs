using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyInfoButton : MonoBehaviour
{
	[SerializeField] private Button partyButton;
	[SerializeField] private TextMeshProUGUI partyLeaderNameText;
	[SerializeField] private TextMeshProUGUI partyAmountText;

	private string _partyName;
	public string PartyName => _partyName;

	public void UpdateInfo(string partyName, string amount)
	{
		_partyName = partyName;
		partyLeaderNameText.text = _partyName;
		partyAmountText.text = amount;
	}

	public void OnClickButton()
	{
		GameManager.Instance.PartyManager.RPC_AskToJoinParty(_partyName);
		OnClickedRequestToJoinParty();
	}

	public void OpenButton()
	{
		gameObject.SetActive(true);
		if (!string.IsNullOrEmpty(_partyName))
		{
			partyButton.onClick.AddListener(OnClickButton);
			partyButton.interactable = true;
		}
	}
	public void CloseButton()
	{
		_partyName = string.Empty;
		gameObject.SetActive(false);
		partyButton.onClick.RemoveListener(OnClickButton);
		partyButton.interactable = false;
	}


	private void OnClickedRequestToJoinParty()
	{
		partyButton.onClick.RemoveListener(OnClickedRequestToJoinParty);
		partyButton.interactable = false;
	}
}

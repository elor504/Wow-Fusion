using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyUI : MonoBehaviour
{
	private enum PartyUIState
	{
		CreateOrPartyList,
		InsideParty
	}

	private PartyUIState _currentUIState;

	[Header("References")]
	[SerializeField] private GameObject partyWindow;

	[Header("Create party References")]
	[SerializeField] private GameObject createPartyWindow;
	[SerializeField] private Button createPartyButton;

	[Header("Player party References")]
	[SerializeField] private GameObject playerPartyWindow;
	[SerializeField] private List<PartyMemberInfo> partyMembersInfoUI;

	[Header("Party Search References")]
	[SerializeField] private GameObject partyInfoWindow;
	[SerializeField] private Transform partyInfoContent;
	[SerializeField] private PartyInfoButton partyInfoButtonPF;
	[SerializeField] private List<PartyInfoButton> partyInfoButtons;


	private bool _isWindowOpen;

	private void OnEnable()
	{
		createPartyButton.onClick.AddListener(OnClickCreateParty);
		createPartyButton.interactable = true;
	}
	private void OnDisable()
	{
		createPartyButton.onClick.RemoveListener(OnClickCreateParty);
		createPartyButton.interactable = false;
	}

	private void Awake()
	{
		CloseAllEmptyPartyButtons();
	}

	#region state handler

	public void OnClickPartyList()
	{
		ChangeState(PartyUIState.CreateOrPartyList);
	}

	private void ChangeState(PartyUIState state)
	{
		ExitCurrentState(state);
		_currentUIState = state;
		EnterCurrentState(state);
	}

	private void EnterCurrentState(PartyUIState state)
	{
		switch (state)
		{
			case PartyUIState.CreateOrPartyList:
				HandleCreateOrPartyWindow();
				break;
			case PartyUIState.InsideParty:
				break;
		}
	}

	private void HandleCreateOrPartyWindow()
	{
		if (IsLocalCharacterIsInAParty())
		{
			EnterParty();
		}
		else
		{
			EnterCreate();
		}
	}

	private void ExitCurrentState(PartyUIState state)
	{
		switch (state)
		{
			case PartyUIState.CreateOrPartyList:
				ExitCreate();
				break;
			case PartyUIState.InsideParty:
				ExitParty();
				break;
		}
	}

	private void EnterParty()
	{
		//Update The party members info
		Party myParty = GameTest.LocalParty;
		foreach (var item in myParty.PartyMember)
		{
			//PlayerCharacter memberCharacter = 
		}

		playerPartyWindow.SetActive(true);
	}
	private void ExitParty()
	{
		playerPartyWindow.SetActive(false);
		partyInfoWindow.SetActive(false);
	}

	private void EnterCreate()
	{
		partyInfoWindow.SetActive(true);
		createPartyWindow.SetActive(true);
	}
	private void ExitCreate()
	{
		createPartyWindow.SetActive(false);
	}


	#endregion
	#region Open new party
	public void OnClickCreateParty()
	{
		createPartyButton.interactable = false;
		PartyManager.Instance.RPC_RequestToOpenNewParty(GameTest.LocalCharacter.CharacterName);
	}
	#endregion
	#region Party Search

	private void CloseAllEmptyPartyButtons()
	{
		foreach (var partyInfo in partyInfoButtons)
		{
			if (string.IsNullOrEmpty(partyInfo.PartyName))
			{
				partyInfo.CloseButton();
			}
		}
	}

	public void AddPartyInfo(string partyLeader, int currentPartyAmount, int maxPartyAmount)
	{
		string amount = $"{currentPartyAmount}/{maxPartyAmount}";
		if (partyInfoButtons.Count == 0)
		{
			PartyInfoButton newPartyInfo = Instantiate(partyInfoButtonPF, partyInfoContent);
			partyInfoButtons.Add(newPartyInfo);
			newPartyInfo.UpdateInfo(partyLeader, amount);
			newPartyInfo.OpenButton();
			return;
		}

		PartyInfoButton existedButton = GetPartyInfoByPartyLeaderName(partyLeader);
		if (existedButton)
		{
			existedButton.UpdateInfo(partyLeader, amount);
			return;
		}

		PartyInfoButton newParty = Instantiate(partyInfoButtonPF, partyInfoContent);
		partyInfoButtons.Add(newParty);
		newParty.UpdateInfo(partyLeader, amount);
		newParty.OpenButton();
	}
	public void ClosePartyInfo(string partyLeader)
	{
		GetPartyInfoByPartyLeaderName(partyLeader).CloseButton();
	}

	private PartyInfoButton GetPartyInfoByPartyLeaderName(string partyLeaderName)
	{
		return partyInfoButtons.Find(p => p.PartyName == partyLeaderName);
	}
	#endregion
	#region base
	public void ToggleWindow()
	{
		if (_isWindowOpen)
			CloseWindow();
		else
			OpenWindow();
	}
	public void OpenWindow()
	{
		_isWindowOpen = true;
		partyWindow.gameObject.SetActive(_isWindowOpen);
		UpdateState();
	}
	public void UpdateState()
	{
		if (IsLocalCharacterIsInAParty())
		{
			EnterCurrentState(PartyUIState.InsideParty);
		}
		else
		{
			EnterCurrentState(PartyUIState.CreateOrPartyList);
		}
	}
	public void CloseWindow()
	{
		_isWindowOpen = false;
		partyWindow.gameObject.SetActive(_isWindowOpen);
	}
	#endregion

	private bool IsLocalCharacterIsInAParty()
	{
		return PartyManager.Instance.IsPlayerInsideAParty(GameTest.LocalCharacter.CharacterName);
	}


	public void OnEnteredParty(PlayerRef playerRef)
	{
		if (!GameTest.ComparePlayerRef(playerRef))
			return;

		Debug.Log("[PartyUI] update enteredParty");
		UpdateState();
	}
	public void OnExitedParty(PlayerRef playerRef)
	{
		if (!GameTest.ComparePlayerRef(playerRef))
			return;

		Debug.Log("[PartyUI] update enteredParty");
		UpdateState();
	}
}

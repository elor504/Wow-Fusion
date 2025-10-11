using Fusion;
using System;
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
	[SerializeField] private Button abandonPartyButton;
	[Header("Party Search References")]
	[SerializeField] private GameObject partyInfoWindow;
	[SerializeField] private Transform partyInfoContent;
	[SerializeField] private PartyInfoButton partyInfoButtonPF;
	[SerializeField] private List<PartyInfoButton> partyInfoButtons;


	private bool _isWindowOpen;

	private void OnEnable()
	{
		createPartyButton.onClick.AddListener(OnClickCreateParty);
		abandonPartyButton.onClick.AddListener(OnClickAbandonParty);
		createPartyButton.interactable = true;
		Debug.Log("[PartyUI] Enabled");
	}



	private void OnDisable()
	{
		createPartyButton.onClick.RemoveListener(OnClickCreateParty);
		abandonPartyButton.onClick.RemoveListener(OnClickAbandonParty);
		createPartyButton.interactable = false;

		Debug.Log("[PartyUI] Disabled");
	}

	private void Awake()
	{
		CloseWindow();
		CloseAllEmptyPartyButtons();
		//need to check before we load but for now we will leave the party
		_currentUIState = PartyUIState.CreateOrPartyList;
		ChangeState(_currentUIState);
	}

	#region state handler
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
				EnterCreate();
				break;
			case PartyUIState.InsideParty:
				EnterParty();
				break;
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
		ExitCreate();
		RefreshPartyMembers();
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


	private void RefreshPartyMembers()
	{
		var partyMembers = RuntimeSessionManager.LocalParty.GetPartyCharacters();
		for (int i = 0; i < partyMembersInfoUI.Count; i++)
		{
			if (partyMembers.Count > i)
			{
				partyMembersInfoUI[i].UpdateMemberInfo(partyMembers[i].CharacterName, "ClassNameTEMP", i == 0);
				partyMembersInfoUI[i].ShowInfo();
			}
			else
			{
				partyMembersInfoUI[i].HideInfo();
			}
		}
	}


	#endregion
	#region Open new party
	public void OnClickCreateParty()
	{
		createPartyButton.interactable = false;
		PartyManager.Instance.RPC_RequestToOpenNewParty();
	}
	#endregion

	private void OnClickAbandonParty()
	{
		Debug.Log("Request to abandon party");
		abandonPartyButton.interactable = false;
		PartyManager.Instance.RPC_RequestToAbandonParty();
	}

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
	
	[ContextMenu("Toggle party window")]
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
			ChangeState(PartyUIState.InsideParty);
		}
		else
		{
			ChangeState(PartyUIState.CreateOrPartyList);
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
		return PartyManager.Instance.IsPlayerInsideAParty(RuntimeSessionManager.LocalCharacter);
	}


	public void OnEnteredParty(PlayerRef playerRef)
	{

		if (!RuntimeSessionManager.CompareLocalPlayerRef(playerRef))
			return;

		Debug.Log("[PartyUI] update entered Party");
		UpdateState();
	}
	public void OnExitedParty(PlayerRef playerRef)
	{
		if (!RuntimeSessionManager.CompareLocalPlayerRef(playerRef))
			return;

		Debug.Log("[PartyUI] update exited Party");
		UpdateState();
	}
}

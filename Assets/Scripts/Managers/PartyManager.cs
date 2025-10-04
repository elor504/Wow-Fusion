using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : NetworkBehaviour
{
	public const int PARTY_MAX_MEMBERS = 5;


	private static PartyManager _instance;
	public static PartyManager Instance => _instance;
	private NetworkRunner _serverRunner;


	[SerializeField] private PartyUI partyUI;
	[SerializeField] private DungeonsManager dungeonManager;
	[SerializeField] private List<Party> partyList;


	public override void Spawned()
	{
		base.Spawned();
		if (Object.HasStateAuthority)
		{
			_serverRunner = Object.Runner;
		}
		else
		{
			//Request Fully info Party list
		}
		if (_instance == null)
		{
			_instance = this;
		}
		else if (_instance != this)
		{
			Destroy(gameObject);
		}
	}
	public override void Despawned(NetworkRunner runner, bool hasState)
	{
		base.Despawned(runner, hasState);
	}

	[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
	public void RPC_RequestAllPartyInfo(string partyInfo, RpcInfo rpcInfo = default)
	{

	}



	[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
	public void RPC_RequestToOpenNewParty(string leaderName, RpcInfo rpcInfo = default)
	{
		if (IsPlayerInsideAParty(leaderName))
		{
			//How the fuck did he even succeded in doing that?
			RPC_PartyMessage(rpcInfo.Source, "[PartyManager] HOW THE FUCK DID YOU ASK TO OPEN NEW PARTY WHILE YOU ARE IN A PARTY?! CHEATER! (or bugged, i just love to blame people)");
			return;
		}
		Debug.Log($"[Server] Accepted to create new party for {leaderName} with the playerRef: {rpcInfo.Source}");
		RPC_AcceptRequestToCreateNewParty(leaderName, rpcInfo.Source);
	}
	[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
	public void RPC_AcceptRequestToCreateNewParty(string leaderName, PlayerRef playerRef)
	{
		Party newParty = new Party();
		if (RuntimeSessionManager.CharactersList.TryGetCharacterByPlayerRef(playerRef, out PlayerCharacter player))
		{
			newParty.OpenParty(player, leaderName);
			RuntimeSessionManager.LocalParty = newParty;
			partyList.Add(newParty);
			partyUI.AddPartyInfo(leaderName, newParty.GetPartyCharacters().Count, PARTY_MAX_MEMBERS);
			partyUI.OnEnteredParty(playerRef);
		}
	}
	#region requests
	[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
	public void RPC_AskToJoinParty(string partyName, RpcInfo rpcSource = default)
	{
		//Server check if can join the party if its available to join
		Party partyToJoin = GetPartyByLeaderName(partyName);


		if (partyToJoin != null && !partyToJoin.IsPartyFull())
		{
			//Send request to the party (in ui only the leader should be able to interact
			RPC_SendLeaderPartyRequest(partyName, rpcSource.Source);
		}
		else
		{
			//Send message to the player for the reason that he cannot join (mostly will be because the party is full if coded correctly)
			RPC_PartyMessage(rpcSource.Source, "[PartyManager] The party you attempted to join are full");
		}
	}
	//Hate this i should somehow only make the party themselves to be able to see it, it will be easier if i make only the party leader to see thou...
	[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
	private void RPC_SendLeaderPartyRequest(string partyName, PlayerRef playerWhoRequested)
	{
		Party partyToJoin = GetPartyByLeaderName(partyName);
		partyToJoin.JoinRequests.Add(playerWhoRequested);

		//ui update
		//UIManager.PartyUI.OnReceivedRequest(playerWhoRequested);
		if (RuntimeSessionManager.ComparePlayerRef(playerWhoRequested))
		{
			RuntimeSessionManager.LocalParty = partyToJoin;
		}

		partyUI.OnEnteredParty(playerWhoRequested);
	}
	#endregion

	[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
	private void RPC_PartyMessage([RpcTarget] PlayerRef playerWhoRequested, string exception)
	{
		//MessageManager.Instance.SendServerMessage(exception);
		Debug.Log(exception, this);
	}

	public void StartDungeon(string leaderNickname)
	{
		Party dungeonParty = GetPartyByLeaderName(leaderNickname);
		if (dungeonParty == null)
		{
			Debug.LogError($"[PartyManager] Failed to start a dungeon because there is no party existed with a leader called: {leaderNickname}");
			return;
		}
		if (dungeonParty.IsEveryMemberOnline())
		{
			Debug.LogError($"[PartyManager] Failed to start a dungeon because there are members that are not online, Party: {leaderNickname}");
			return;
		}
		dungeonManager.CreateNewDungeon(dungeonParty);
	}
	public bool IsPlayerInsideAParty(string playerName)
	{
		foreach (var party in partyList)
		{
			foreach (var character in party.GetPartyCharacters())
			{
				if (character.CharacterName == playerName)
					return true;
			}
		}

		return false;
	}
	public Party GetPartyByLeaderName(string leaderName)
	{
		return partyList.Find(p => p.LeaderName == leaderName);
	}
}
[Serializable]
public class Party
{
	public CharactersList partyMembers = new CharactersList();
	public string LeaderName;

	public List<PlayerRef> JoinRequests = new List<PlayerRef>();

	public void OpenParty(PlayerCharacter member, string leaderCharacterName)
	{
		LeaderName = leaderCharacterName;
		AddNewMember(member);
	}
	public void AddNewMember(PlayerCharacter member)
	{
		partyMembers.AddCharacterToList(member);
	}
	public void AbandonParty(PlayerCharacter playerWhoLeft, string memberName)
	{
		partyMembers.RemoveCharacterFromList(playerWhoLeft);
	}

	public bool IsPartyFull() => partyMembers.GetPlayerCharacters.Count == PartyManager.PARTY_MAX_MEMBERS;
	public PlayerCharacter GetLeaderCharacter(NetworkRunner serverRunner)
	{
		return null;
	}
	public List<PlayerCharacter> GetPartyCharacters()
	{		
		return partyMembers.GetPlayerCharacters;
	}
	public bool IsEveryMemberOnline()
	{
		List<PlayerCharacter> partyCharacters = GetPartyCharacters();

		foreach (var character in partyCharacters)
		{
			if (character == null)
				return false;
		}

		return true;
	}
}

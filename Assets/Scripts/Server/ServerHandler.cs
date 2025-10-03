using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.Unicode;

public class ServerHandler : MonoBehaviour, INetworkRunnerCallbacks
{
	private static ServerHandler _instance;
	public static ServerHandler Instance => _instance;

	[SerializeField] private DungeonsManager dungeonsHandler;
	[SerializeField] private PlayerCharacter characterPF;


	private static int CHANNEL_AMOUNT = 1;
	private static int PLAYER_AMOUNT = 20;
	public static string CUSTOM_LOBBY_NAME = "MAIN_LOBBY";
	[SerializeField]
	private Dictionary<string, SessionServerInfo> _sessionList = new Dictionary<string, SessionServerInfo>();



	private async void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(this);
		}
		else if (_instance != this)
		{
			Destroy(gameObject);
			return;
		}

		await CreateSessions();

	}

	public void AddEnemyNetworkID(NetworkId networkID, DragonEnemy enemy, NetworkRunner serverRunner)
	{
		GetServerInfo(serverRunner).dragonEntity[networkID] = enemy;
		Debug.Log($"[Server Handler] Registered enemy entity: {enemy.transform.name} into the server enemy dictionary, Server name: {serverRunner.SessionInfo.Name}");
	}
	public void RemoveEnemyNetworkID(NetworkId networkID, NetworkRunner serverRunner)
	{
		GetServerInfo(serverRunner).dragonEntity.Remove(networkID);
	}

	public DragonEnemy GetEnemyByNetworkID(NetworkId networkID, NetworkRunner serverRunner)
	{
		return GetServerInfo(serverRunner).dragonEntity[networkID];
	}

	public PlayerCharacter GetPlayerCharacterByNickname(NetworkRunner serverRunner, string nickname)
	{
		Dictionary<PlayerRef, PlayerCharacter> playersCharacters = GetServerInfo(serverRunner).playersCharacters;

		foreach (var characterValue in playersCharacters.Values)
		{
			if(characterValue.CharacterName == nickname)
			{
				return characterValue;
			}
		}

		return null;
	}
	private async Task CreateSessions()
	{
		for (int i = 0; i < CHANNEL_AMOUNT; i++)
		{
			await CreateNewSession($"Lobby_{i}");
			Debug.Log($"Created new session: {"Lobby_" + i}");
		}
	}
	private async Task CreateNewSession(string sessionName)
	{
		var newGO = new GameObject(sessionName + " Session");
		var networkRunner = newGO.AddComponent<NetworkRunner>();

        AddNewSession(sessionName, networkRunner);

        await OpenNewSession(networkRunner, sessionName);
	}
	private async Task OpenNewSession(NetworkRunner runner, string sessionName)
	{
		//runner.AddCallbacks(this);
		int sceneIndex = SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/Main_City.unity");
		var gameArg = new StartGameArgs
		{
			CustomLobbyName = CUSTOM_LOBBY_NAME,
			SessionName = sessionName,
			GameMode = GameMode.Server,
			PlayerCount = PLAYER_AMOUNT,
			Scene = SceneRef.FromIndex(sceneIndex),
			SceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>()
    };
		await runner.StartGame(gameArg);
	}

	public void AddNewSession(string sessionName, NetworkRunner serverRunner)
	{
		_sessionList[sessionName] = new SessionServerInfo
		{
			sessionRunner = serverRunner,
			playersRefs = new List<PlayerRef>(),
			playersCharacters = new Dictionary<PlayerRef, PlayerCharacter>(),
			dragonEntity = new Dictionary<NetworkId, DragonEnemy>()

		};
	}
	public void RemoveSession(string sessionName,NetworkRunner serverRunner)
	{
        _sessionList.Remove(sessionName);
    }


    public void OnConnectedToServer(NetworkRunner runner)
	{
		Debug.Log($"[Client/Server] connected to server: {runner.name}");
	}
	#region Player
	public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
	{
		Debug.Log($"A player joined a session: {runner.IsServer} player: {player.PlayerId}");
		GetServerInfo(runner).playersRefs.Add(player);
		SpawnCharacter(runner, player);
	}

	private async void SpawnCharacter(NetworkRunner runner, PlayerRef player)
	{
		var spawnResult = await runner.SpawnAsync(characterPF, Vector3.zero, Quaternion.identity, player);
		var character = spawnResult.gameObject.GetComponent<PlayerCharacter>();
		GetServerInfo(runner).playersCharacters.Add(player, character);
		GetServerInfo(runner).playersCharacters[player].InitPlayer();
	}

	public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
	{
		GetServerInfo(runner).playersRefs.Remove(player);
		Debug.Log($"A player left on the session: {runner.name} player: {player.PlayerId}");

		Debug.Log($"[Server] Attempting to destroy character: {GetServerInfo(runner).playersCharacters[player]}");
		var character = GetServerInfo(runner).playersCharacters[player];
		try
		{
			runner.Despawn(character.NetworkObject);
		}
		catch (Exception e)
		{
			Debug.Log($"[Depsawn failed: ] {e.Message}");
		}
		GetServerInfo(runner).playersCharacters.Remove(player);
	}
	#endregion
	private SessionServerInfo GetServerInfo(NetworkRunner runner)
	{
		return _sessionList[runner.SessionInfo.Name];
	}
	public void UpdateGameManager(NetworkRunner serverRunner,GameManager manager)
	{
		var server = GetServerInfo(serverRunner);
		manager.OnPlayerJoinedSession += OnPlayerJoined;
		manager.OnPlayerLeftSession += OnPlayerLeft;
        server.gameManager = manager;
    }



	#region unused
	public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
	{

	}

	public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
	{

	}

	public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
	{

	}

	public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
	{

	}

	public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
	{

	}

	public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
	{

	}

	public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
	{

	}

	public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
	{

	}

	public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
	{

	}

	public void OnInput(NetworkRunner runner, NetworkInput input)
	{

	}

	public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
	{

	}


	public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
	{

	}

	public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
	{

	}

	public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
	{

	}

	public void OnSceneLoadDone(NetworkRunner runner)
	{

	}

	public void OnSceneLoadStart(NetworkRunner runner)
	{

	}




	#endregion


	[Serializable]
	private class SessionServerInfo
	{
		public NetworkRunner sessionRunner;
		public GameManager gameManager;
		public List<PlayerRef> playersRefs;
		public Dictionary<PlayerRef, PlayerCharacter> playersCharacters;
		public Dictionary<NetworkId, DragonEnemy> dragonEntity;


		public bool TryGetPlayerRef(PlayerCharacter character,out PlayerRef playerRef)
		{
			playerRef = default;

			if(playersCharacters.ContainsValue(character))
			{
				foreach (var charac in playersCharacters)
				{
					if(charac.Value == character)
					{
						playerRef = charac.Key;
						return true;
					}
				}
			}
			return false;
		}
		public bool TryGetPlayerRef(string characterName, out PlayerRef playerRef)
		{
			playerRef = default;

			List<PlayerCharacter> playerCharacters = playersCharacters.Values.ToList();
			foreach (var character in playerCharacters)
			{
				if(character.CharacterName == characterName)
				{
					return TryGetPlayerRef(character,out playerRef);
				}
			}

			return false;
		}
	}
}

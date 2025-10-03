using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerHandler : MonoBehaviour, INetworkRunnerCallbacks
{
	private static ServerHandler _instance;
	public static ServerHandler Instance => _instance;

	[SerializeField] private DungeonsManager dungeonsHandler;
	[SerializeField] private PlayerCharacter characterPF;

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
			if (characterValue.CharacterName == nickname)
			{
				return characterValue;
			}
		}

		return null;
	}
	private async Task CreateSessions()
	{
		await CreateNewSession($"Lobby_0");
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
	public void RemoveSession(string sessionName, NetworkRunner serverRunner)
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
		SpawnCharacter(runner, player);
	}

	private async void SpawnCharacter(NetworkRunner runner, PlayerRef player)
	{
		var spawnResult = await runner.SpawnAsync(characterPF, Vector3.zero, Quaternion.identity, player);
		var character = spawnResult.gameObject.GetComponent<PlayerCharacter>();
	}

	public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
	{

		Debug.Log($"A player left on the session: {runner.name} player: {player.PlayerId}");
		GameManager.Instance.TryGetCharacterByPlayerRef(player, out var character);
		if (character)
			runner.Despawn(character.NetworkObject);

	}
	#endregion

	public void UpdateGameManager(NetworkRunner serverRunner, GameManager manager)
	{
		manager.OnPlayerJoinedSession += OnPlayerJoined;
		manager.OnPlayerLeftSession += OnPlayerLeft;
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


}

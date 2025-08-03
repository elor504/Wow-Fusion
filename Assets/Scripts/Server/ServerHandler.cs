using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ServerHandler : MonoBehaviour, INetworkRunnerCallbacks
{
    private static int CHANNEL_AMOUNT = 5;
    private static int PLAYER_AMOUNT = 20;
    public static string CUSTOM_LOBBY_NAME = "MAIN_LOBBY";
    private Dictionary<string, SessionServerInfo> _sessionList = new Dictionary<string, SessionServerInfo>();

    [SerializeField] private PlayerCharacter characterPF;

    private async void Awake()
    {
#if UNITY_SERVER
        await CreateSessions();
#else
        
#endif
    }
    private async Task CreateSessions()
    {
        for (int i = 0; i < CHANNEL_AMOUNT; i++)
        {
            await CreateNewSession($"Lobby_{i}");
        }
    }

    private async Task CreateNewSession(string sessionName)
    {
        var newGO = new GameObject(sessionName + " Session");
        var networkRunner = newGO.AddComponent<NetworkRunner>();
        _sessionList[sessionName] = new SessionServerInfo
        {
            sessionRunner = networkRunner,
            playersRefs = new List<PlayerRef>(),
            playersCharacters = new Dictionary<PlayerRef, PlayerCharacter>()

        };
        await OpenNewSession(networkRunner, sessionName);
        Debug.Log($"Created new session: {newGO.name}");
    }
    private async Task OpenNewSession(NetworkRunner runner, string sessionName)
    {
        runner.AddCallbacks(this);
        var gameArg = new StartGameArgs
        {
            CustomLobbyName = CUSTOM_LOBBY_NAME,
            SessionName = sessionName,
            GameMode = GameMode.Server,
            PlayerCount = PLAYER_AMOUNT
        };
        var result = await runner.StartGame(gameArg);
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log($"[Client/Server] connected to server: {runner.name}");
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"A player joined a session: {runner.IsServer} player: {player.PlayerId}");
        GetServerInfo(runner).playersRefs.Add(player);
        SpawnCharacter(runner, player);
        // FusionManager.RPC_LoadMainCity(player,runner);
    }

    private async void SpawnCharacter(NetworkRunner runner, PlayerRef player)
    {
        var spawnResult = await runner.SpawnAsync(characterPF, Vector3.zero, Quaternion.identity, player);
        GetServerInfo(runner).playersCharacters.Add(player, spawnResult.gameObject.GetComponent<PlayerCharacter>());
        spawnResult.gameObject.SetActive(false);
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

    private SessionServerInfo GetServerInfo(NetworkRunner runner)
    {
        return _sessionList[runner.SessionInfo.Name];
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



    private struct SessionServerInfo
    {
        public NetworkRunner sessionRunner;
        public List<PlayerRef> playersRefs;
        public Dictionary<PlayerRef, PlayerCharacter> playersCharacters;
    }


}

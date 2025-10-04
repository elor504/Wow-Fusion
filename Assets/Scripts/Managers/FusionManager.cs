using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FusionManager : MonoBehaviour, INetworkRunnerCallbacks
{
    private const string MainCityID = "Lobby_0";

    [SerializeField] private CharacterSpawnManager characterSpawnManager;

    private CharacterData _selectedCharacterData;
    public CharacterData SelectedCharacterData => _selectedCharacterData;
 
    public CharacterSpawnManager CharacterSpawnManager => characterSpawnManager;

    public void SwitchSession(string sessionName)
    {
        StartCoroutine(SwitchSessionCouru(sessionName));
    }
    private IEnumerator SwitchSessionCouru(string sessionName)
    {
        // First leave current session
        if (RuntimeSessionManager.GetMyRunner() != null && RuntimeSessionManager.GetMyRunner())
        {
           yield return RuntimeSessionManager.GetMyRunner().Shutdown();
        }
        yield return new WaitForSeconds(5f);
        RuntimeSessionManager.RefreshNetworkRunner();


        // Then join the new session
        var args = new StartGameArgs()
        {
            CustomLobbyName = ServerHandler.CUSTOM_LOBBY_NAME,
            GameMode = GameMode.Client,
            SessionName = sessionName
        };

        yield return RuntimeSessionManager.GetMyRunner().StartGame(args);
    }


    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (RuntimeSessionManager.FusionManager == null)
        {
            RuntimeSessionManager.FusionManager = this;
        }
        else if (RuntimeSessionManager.FusionManager != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
        RuntimeSessionManager.AddCallBacks(this);
    }

    public void ConnectToMainCity()
    {
        LoadingScreen.Instance.LoadIntoOpenWorld();
    }
  
    public void SetSelectedCharacterData(CharacterData characterData) 
    {
        _selectedCharacterData = characterData;
    }

   
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        RuntimeSessionManager.ReturnToLoginMenu();
        Debug.Log($"Shutdown: {shutdownReason}");
    }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        RuntimeSessionManager.ReturnToLoginMenu();
        Debug.LogError($"[Fusion Manager] failed to connect : {reason}");
    }


    #region unused
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    public void OnConnectedToServer(NetworkRunner runner)
    {

    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {

    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }
    public void OnSceneLoadDone(NetworkRunner runner)
    {

    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }


    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        RuntimeSessionManager.ReturnToLoginMenu();
        Debug.Log($"Shutdown: {reason}");
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }
    #endregion
}

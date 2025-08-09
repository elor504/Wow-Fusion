using Fusion;
using Fusion.Sockets;
using Homework;
using PlayFab.MultiplayerModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Unity.Collections.Unicode;

public class FusionManager : MonoBehaviour, INetworkRunnerCallbacks
{
    private const string MainCityID = "Lobby_0";

    [SerializeField] private CharacterSpawnManager characterSpawnManager;


    public CharacterSpawnManager CharacterSpawnManager => characterSpawnManager;
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (GameTest.FusionManager == null)
        {
            GameTest.FusionManager = this;
        }
        else if (GameTest.FusionManager != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
        GameTest.AddCallBacks(this);
    }

    public void ConnectToServer()
    {
        ConnectToMainCity();
    }


	public async void ConnectToMainCity()
	{
        Debug.Log("[Client] start connection to main city async");
        //loading screen
        //var result = await GameTest.GetMyRunner().StartGame(new StartGameArgs
        //{
        //    CustomLobbyName = ServerHandler.CUSTOM_LOBBY_NAME,
        //    SessionName = MainCityID,
        //    GameMode = GameMode.Client,
        //    PlayerCount = 20
        //}) ;

		//if (result.Ok)
		//{
		//	//Debug.Log($"Joined lobby: {GameTest.GetMyRunner().UserId}");
          LoadingScreen.Instance.LoadIntoOpenWorld();
  //      }
		//else
		//{
		//	Debug.LogError($"Failed to join session lobby{result.ShutdownReason}");
		//}
		//await Task.Run(() => JoinLobby(GameTest.GetMyRunner(), MainCityID));


		//remove screens
	}
    public async Task JoinLobby(NetworkRunner runner, string lobbyID)
    {
        try
        {
            // var result = await runner.JoinSessionLobby(SessionLobby.Shared, MainCityID);
            var result = await runner.StartGame(new StartGameArgs
            {
                SessionName = lobbyID,
                GameMode = GameMode.Shared,
                PlayerCount = 20
            });

            if (result.Ok)
            {
                Debug.Log($"Joined lobby: {runner.UserId}");
            }
            else
            {
                Debug.LogError($"Failed to join session lobby{result.ShutdownReason}");
            }

        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception during JoinLobby: {ex.Message}\n{ex.StackTrace}");
        }

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

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        GameTest.ReturnToLoginMenu();
        Debug.Log($"Shutdown: {shutdownReason}");
    }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        GameTest.ReturnToLoginMenu();
        Debug.LogError($"[Fusion Manager] failed to connect : {reason}");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    #region unused
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        GameTest.ReturnToLoginMenu();
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

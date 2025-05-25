using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using PlayFab.MultiplayerModels;
using UnityEngine;

//https://doc-api.photonengine.com/en/fusion/current/interface_fusion_1_1_i_network_runner_callbacks.html
public class LobbyManager : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner _networkRunner;
    private bool _isLocalPlayer;
    private List<PlayerRef> playerRefs = new List<PlayerRef>();




    public static event Action<List<PlayerRef>> OnPlayerConnection; 

    
    public void Init(NetworkRunner networkRunner)
    {
        _networkRunner = networkRunner;
        _networkRunner.AddCallbacks(this);
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log($"Connected to server {runner.UserId}");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log($"Failed to Connected to server {runner.UserId}");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log($"Connect request {runner.UserId}");
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (_networkRunner.LocalPlayer == player)
            _isLocalPlayer = true;

        playerRefs.Add(player);
        OnPlayerConnection?.Invoke(playerRefs);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        playerRefs.Remove(player);
        OnPlayerConnection?.Invoke(playerRefs);
    }


    #region Not used


    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {

    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }


    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {

    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {

    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }
    #endregion
}

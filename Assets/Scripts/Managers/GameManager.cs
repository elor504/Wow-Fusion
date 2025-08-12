using Fusion;
using Fusion.Sockets;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour, INetworkRunnerCallbacks
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private NetworkRunner _serverRunner;

    [Header("Managers")]

    [SerializeField] private TargetManager targetManager;
    [SerializeField] private ClassSkillManager skillManager;
   

    [Header("UI")]
    [SerializeField] private GameObject playerHUD;

    [Header("Data")]
    [SerializeField] private CharacterVisualSO equipmentVisualData;

    [Header("Prefabs")]
    [SerializeField] private PlayerCharacter playerCharacter;

    public TargetManager TargetManager => targetManager;
    public ClassSkillManager ClassSkillManager => skillManager;


    public CharacterVisualSO EquipmentVisualData => equipmentVisualData;


    public static event Action<GameManager> OnGameManagerSpawned;
    public static event Action<GameManager> OnGameManagerDespawned;
    public event Action<NetworkRunner, PlayerRef> OnPlayerJoinedSession;
    public event Action<NetworkRunner, PlayerRef> OnPlayerLeftSession;

    public override void Spawned()
    {
        Init();
        base.Spawned();
        if (!Object.HasStateAuthority)//No need for the server to see the player hud
        {
            playerHUD.SetActive(true);
        }
        if(Object.HasStateAuthority)
        {
            _serverRunner = Object.Runner;
            _serverRunner.AddCallbacks(this);
            ServerHandler.Instance.UpdateGameManager(_serverRunner, this);
        }

    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        base.Despawned(runner, hasState);
        if (Object.HasStateAuthority)
        {
            //_serverRunner.RemoveCallbacks(this);
           // OnGameManagerDespawned?.Invoke(this);
        }
    }
    public void Init()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this);
        }

        //targetManager.Init();
        skillManager.Init();
        GameTest.AddCallBacks(this);
    }



    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        OnPlayerJoinedSession?.Invoke(runner, player);
    }


    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        OnPlayerLeftSession?.Invoke(runner, player);
    }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {

    }



    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
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

    public void OnConnectedToServer(NetworkRunner runner)
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
}

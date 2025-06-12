using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Homework
{
    //https://doc-api.photonengine.com/en/fusion/current/interface_fusion_1_1_i_network_runner_callbacks.html
    //https://doc.photonengine.com/fusion/current/manual/connection-and-matchmaking/matchmaking
    public class LobbyManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        private static LobbyManager _instance;
        public static LobbyManager Instance => _instance;

        public CharacterSelectionManager characterSelectionManagerPF;

        [Header("Reference")]
        [SerializeField] private NetworkRunner networkRunner;
        [SerializeField] private UIManager uiManager;
        [Header("Lobby Settings")]
        [SerializeField] private SessionLobby sessionLobby = SessionLobby.Shared;
        [Header("Session settings")]
        [SerializeField] private GameMode gamemode = GameMode.Shared;

        private CharacterSelectionManager characterSelectionManager;
        public CharacterSelectionManager GetCharacterSelectionManager => characterSelectionManagerPF;

        private string _lobbyID;
        private bool _isLocalPlayer;
        private List<PlayerRef> playerRefs = new List<PlayerRef>();

        public string LobbyID => _lobbyID;


        public static readonly string MainLobbyID = "EasterEgg";

        public static event Action<List<PlayerRef>> OnPlayerConnection;//need to think of a better name because it also handles the disconnection
        public static event Action<NetworkRunner, List<SessionInfo>> OnSessionUpdated;//need to think of a better name because it also handles the disconnection
        public static Action OnGameStarted;
        public static Action OnStartLoadingLobby;
        public static Action<string> OnFinishedLoadingLobby;
        public static Action<string, string> EnterLobby;

        public static Action<string, int> EnterSession;

        private void Awake()
        {

            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(_instance);
            }
            networkRunner.AddCallbacks(this);
            uiManager.ChangeToLobbySelection();
        }
        private void OnEnable()
        {
            EnterLobby += EnterLobbyHandler;
            EnterSession += EnterSessionHandler;
        }
        private void OnDisable()
        {
            EnterLobby -= EnterLobbyHandler;
            EnterSession -= EnterSessionHandler;
        }



        public async void EnterLobbyHandler(string lobbyID, string nickname)//DIDN'T KNEW IT POSSIBLE AAAAAAAAAAAAAAAAAAAAAAH
        {
            _lobbyID = lobbyID;
            OnStartLoadingLobby.Invoke();
            await Task.Run(() => JoinLobby(networkRunner, _lobbyID));  
            OnFinishedLoadingLobby?.Invoke(_lobbyID);
        }
        private async Task JoinLobby(NetworkRunner runner, string lobbyID)
        {
            try
            {
                var result = await networkRunner.JoinSessionLobby(sessionLobby, lobbyID);

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

        [ContextMenu("Start Game")]
        public void EnterSessionHandler(string sessionName, int maxPlayers)
        {
            networkRunner.StartGame(new StartGameArgs
            {
                GameMode = gamemode,
                SessionName = sessionName,
                PlayerCount = maxPlayers,
                OnGameStarted = GameStarted

            });
        }

        private void GameStarted(NetworkRunner obj)
        {
            Debug.Log("Game Started!");
            OnGameStarted?.Invoke();
            if (obj.IsSharedModeMasterClient)
                characterSelectionManager = obj.Spawn(characterSelectionManagerPF);
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
            Debug.Log($"Player joined {runner.UserId}");
            if (networkRunner.LocalPlayer == player)
                _isLocalPlayer = true;
            playerRefs.Add(player);
            OnPlayerConnection?.Invoke(playerRefs);
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            playerRefs.Remove(player);
            OnPlayerConnection?.Invoke(playerRefs);
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            Debug.Log($"Session list updated. current sessions that are active are: {sessionList.Count}");
            OnSessionUpdated?.Invoke(runner, sessionList);
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

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {

        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {

        }
        #endregion
    }
}
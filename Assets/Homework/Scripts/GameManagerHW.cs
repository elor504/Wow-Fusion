using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Homework
{
    public class GameManagerHW : NetworkBehaviour, INetworkRunnerRequired, INetworkRunnerCallbacks
    {
        public static readonly string LOBBY_SCENE_NAME = "LobbyScene_HW";
        private static GameManagerHW instance;
        public static GameManagerHW Instance => instance;

        public CharacterSelectionManager characterSelectionManagerPF;
        [HideInInspector]
        public static CharacterSelectionManager CharacterSelectionManager;

        public static ChatManager ChatManager;

        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private NetworkRunner runner;
        [SerializeField] private PlayerSpawnManager spawnManager;


        public NetworkRunner GetRunner => runner;
        private void Awake()
        {

        }
        public override void Spawned()
        {
            base.Spawned();
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(instance);
                return;
            }
            NetworkRunnerInjector.Instance.AddInjector(this);
            runner.AddCallbacks(this);
        }
        public void CloseGame()
        {
            if (runner.IsSharedModeMasterClient)
            {
                RPC_CloseSessionToAll();
                CloseHost();
            }
        }
        private async void CloseHost()
        {
            await Task.Delay(5000);
            await runner.Shutdown(true, ShutdownReason.Ok);
        }
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_CloseSessionToAll()
        {
            if (!runner.IsSharedModeMasterClient)
            {
                Debug.Log("[Client] requesting shutdown");
                runner.Shutdown();
            }
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_RequestSpawn(RpcInfo info = default)
        {
            if (spawnManager.TryGetSpawnPosition(info.Source, out var position))
            {
				Debug.Log($"[Server] Requesting to spawn player: {info.Source.PlayerId}");
				RPC_SetSpawn(info.Source, position);
            }
            else
            {
                Debug.LogError("Attempting to get a spawn position but failed");
            }
        }
        [Rpc(RpcSources.StateAuthority,RpcTargets.All)]
        private void RPC_SetSpawn([RpcTarget] PlayerRef playerRef, Vector3 spawnPosition)
        {
            Debug.Log($"[Client] Attempting to spawn player: {playerRef.PlayerId}");
            runner.SpawnAsync(playerPrefab, spawnPosition, Quaternion.identity,playerRef);
        }

        public void InjectRunner(NetworkRunner runner)
        {
            this.runner = runner;
            if (this.runner.IsSharedModeMasterClient)
            {
                CharacterSelectionManager = runner.Spawn(characterSelectionManagerPF);
            }
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_SendPrivateMessage(string SenderName, string Message, Color TextColor, string targetName)
        {
            var target = PlayerList.Instance.GetPlayerRefByName(targetName);
            if (target == default)
            {
                Debug.Log($"[Server] ChatManager: there is no player with the name {targetName}");
                //send a message locally to the player that there is no player with this name existed
                return;
            }
            RPC_ReceivePrivateMessage(target, SenderName, Message, TextColor);
        }
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_ReceivePrivateMessage([RpcTarget] PlayerRef playerRef, string SenderName, string Message, Color TextColor)
        {
            MessageInfo info;
            info.SenderName = SenderName;
            info.Message = Message;
            info.TextColor = TextColor;
            ChatManager.AddMessage(info);
        }


        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_SendMessageToAll(string SenderName, string Message, Color TextColor)
        {
            RPC_ReceiveMessageToAll(SenderName, Message, TextColor);
            Debug.Log($"[Server] sending a message to everyone from {SenderName} message: {Message}");
        }
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_ReceiveMessageToAll(string SenderName, string Message, Color TextColor)
        {
            MessageInfo info;
            info.SenderName = SenderName;
            info.Message = Message;
            info.TextColor = TextColor;
            ChatManager.AddMessage(info);
            Debug.Log($"[Client] Receiving a message from {SenderName} message: {Message}");
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            Debug.Log("[Shutdown] attempting to change scene");
            SceneManager.LoadScene(LOBBY_SCENE_NAME);
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
            runner.LoadScene(LOBBY_SCENE_NAME);
        }

        #region unused


        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {

        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {

        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {

        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
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
        #endregion
    }
}
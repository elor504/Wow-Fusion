using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Homework
{
    public class PlayerList : NetworkBehaviour
    {
        private static PlayerList instance;
        public static PlayerList Instance => instance;


        public Dictionary<PlayerRef, string> playerNames = new Dictionary<PlayerRef, string>();
        public PlayerLobbyCheck LobbyCheck;

        public static event Action<PlayerRef> OnPlayerJoined;
        public static event Action<RpcInfo> OnPlayerLeave;

        private Dictionary<PlayerRef, bool> playerLobbyReady = new Dictionary<PlayerRef, bool>();


        [Header("Debug")]
        [SerializeField] private bool[] readyTest;



        public override void Spawned()
        {
            base.Spawned();
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            LobbyManager.Instance.PlayerListInstance = this;
            OnPlayerJoined += RPC_AddPlayer;
            LobbyManager.Instance.AddNickname();
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            base.Despawned(runner, hasState);
            OnPlayerJoined -= RPC_AddPlayer;
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPCRegisterNickname(string nickName, RpcInfo info = default)
        {
            Debug.Log($"[Server] added nickname: {nickName} from: {info.Source}");
            playerNames[info.Source] = nickName;

            string[] names = new string[playerNames.Count];
            PlayerRef[] refs = new PlayerRef[playerNames.Count];
            int i = 0;
            foreach (var playerInfo in playerNames)
            {
                refs[i] = playerInfo.Key;
                names[i] = playerInfo.Value;
                i++;
            }
            RPCUpdateAddedNicknames(names, refs);
            OnPlayerJoined.Invoke(info.Source);
        }
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPCUpdateAddedNicknames(string[] names, PlayerRef[] refs, RpcInfo info = default)
        {
            Dictionary<PlayerRef, string> playersUpdatedInfo = new Dictionary<PlayerRef, string>();
            for (int i = 0; i < names.Length; i++)
            {
                playersUpdatedInfo[refs[i]] = names[i];
            }
            playerNames = playersUpdatedInfo;
            LobbyManager.Instance.PlayersInSessionChanged(playerNames);
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPCRemoveNickname(RpcInfo info = default)
        {
            Debug.Log($"[Server] removed nickname: {info.Source}");
            RPCUpdateRemovedNicknames(info.Source);
        }
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPCUpdateRemovedNicknames(PlayerRef player, RpcInfo info = default)
        {
            Debug.Log($"[Client] removed nickname: {player}");
            playerNames.Remove(player);
            LobbyManager.Instance.PlayersInSessionChanged(playerNames);

        }

        public string GetPlayerName(PlayerRef player)
        {
            return playerNames[player];
        }
        public string GetLocalPlayerName()
        {
            return playerNames[GameManagerHW.Instance.GetRunner.LocalPlayer];
        }
        public PlayerRef GetPlayerRefByName(string name)
        {
            foreach (var player in playerNames)
            {
                if (player.Value == name)
                {
                    return player.Key;
                }
            }
            return default;
        }


        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_AddPlayer(PlayerRef info)
        {
            if (info.IsMasterClient)
                return;
            Debug.Log($"[Server] lobby check player joined: {info.PlayerId} {LobbyManager.Instance.PlayerListInstance.GetPlayerName(info)}");
            playerLobbyReady[info] = false;
            var checkArrays = ChangeDictionaryToArrays();
            RPC_UpdatePlayer(checkArrays.playersReferences, checkArrays.lobbyCondition);
        }
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_UpdatePlayer(PlayerRef[] playersRef, bool[] conditions, RpcInfo info = default)
        {
            Dictionary<PlayerRef, bool> newDict = new Dictionary<PlayerRef, bool>();
            for (int i = 0; i < playersRef.Length; i++)
            {
                newDict[playersRef[i]] = conditions[i];
            }
            playerLobbyReady = newDict;
            readyTest = conditions;
            var newPlayerRef = playersRef[playersRef.Length - 1];
            Debug.Log($"[Client] lobby check player joined: {newPlayerRef.PlayerId} {LobbyManager.Instance.PlayerListInstance.GetPlayerName(newPlayerRef)}");
            SessionUI.UpdateSessionInfo.Invoke(playerLobbyReady);
        }


        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPCToggleReady(RpcInfo info = default)
        {
            playerLobbyReady[info.Source] = !playerLobbyReady[info.Source];
            var checkArrays = ChangeDictionaryToArrays();
            RPC_UpdatePlayer(checkArrays.playersReferences, checkArrays.lobbyCondition);
        }

        private (PlayerRef[] playersReferences, bool[] lobbyCondition) ChangeDictionaryToArrays()
        {
            PlayerRef[] playersReferences = new PlayerRef[playerLobbyReady.Count];
            bool[] lobbyCondition = new bool[playerLobbyReady.Count];
            int i = 0;
            foreach (var player in playerLobbyReady)
            {
                playersReferences[i] = player.Key;
                lobbyCondition[i] = player.Value;
                i++;
            }

            return (playersReferences, lobbyCondition);
        }
    }
}
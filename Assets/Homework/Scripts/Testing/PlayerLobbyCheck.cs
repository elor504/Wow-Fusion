using Fusion;
using System.Collections.Generic;
using UnityEngine;
namespace Homework
{
    public class PlayerLobbyCheck : NetworkBehaviour
    {
        private Dictionary<PlayerRef, bool> playerLobbyReady = new Dictionary<PlayerRef, bool>();

        [Header("Debug")]
        [SerializeField]private bool[] readyTest;

        public override void Spawned()
        {
            base.Spawned();
        }


        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_AddPlayer(PlayerRef info)
        {
            Debug.Log($"[Server] lobby check player joined: {info.PlayerId} {LobbyManager.Instance.PlayerListInstance.GetPlayerName(info)}");
            playerLobbyReady[info] = false;
            var checkArrays = ChangeDictionaryToArrays();      
            RPC_UpdatePlayer(checkArrays.playersReferences, checkArrays.lobbyCondition);
        }
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_UpdatePlayer(PlayerRef[] playersRef,bool[] conditions,RpcInfo info = default)
        {
            Dictionary<PlayerRef,bool> newDict = new Dictionary<PlayerRef,bool>();
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


        [Rpc(RpcSources.All,RpcTargets.StateAuthority)]
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
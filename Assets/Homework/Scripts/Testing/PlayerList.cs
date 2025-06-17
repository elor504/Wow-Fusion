using Fusion;
using Homework;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Homework
{
    public class PlayerList : NetworkBehaviour
    {
        public Dictionary<PlayerRef, string> playerNames = new Dictionary<PlayerRef, string>();
        public PlayerLobbyCheck LobbyCheck;

        public static event Action<PlayerRef> OnPlayerJoined;
        public static event Action<RpcInfo> OnPlayerLeave;

        public override void Spawned()
        {
            base.Spawned();

            LobbyManager.Instance.PlayerListInstance = this;
            OnPlayerJoined += LobbyCheck.RPC_AddPlayer;
            LobbyManager.Instance.AddNickname();
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
    }
}
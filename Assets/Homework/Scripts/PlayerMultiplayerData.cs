using Fusion;
using UnityEngine;

namespace Homework
{
    public class PlayerMultiplayerData : NetworkBehaviour
    {
        public PlayerRef playerRef;
        public string playerNickName;

        public override void Spawned()
        {
            base.Spawned();


        }


        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        public void RPC_SetNickname(string nickname, RpcInfo info = default)
        {
            Debug.Log("Nickname test: " + nickname);
            playerNickName = nickname;
            playerRef = info.Source;
        }

    }
}
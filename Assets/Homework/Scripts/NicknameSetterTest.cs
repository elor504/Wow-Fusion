using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class NicknameSetterTest : NetworkBehaviour
{
    private static NicknameSetterTest instance;
    public static NicknameSetterTest Instance => instance;


    public override void Spawned()
    {
        base.Spawned();
        if (instance == null)
        {
            instance = this;
        }
    }


    [SerializeField] private List<string> nicknames;
    public void SetNickname(string nickname)
    {
        RPC_AddNickname(nickname);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_AddNickname(string nickname,RpcInfo info = default)
    {
        nicknames.Add(nickname);
        Debug.Log(" Added nickname to list: " + nickname);
    }

}

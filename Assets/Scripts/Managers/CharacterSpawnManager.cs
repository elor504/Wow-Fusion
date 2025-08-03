using Fusion;
using Unity.Mathematics;
using UnityEngine;

public class CharacterSpawnManager : MonoBehaviour
{
    [SerializeField] private PlayerCharacter playerPF;

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_RequestSpawnCharacter(Vector3 spawnPos, RpcInfo info = default)
    {
        Debug.Log("[Server] Attempting to spawn character");
        //GameTest.GetMyRunner().play
        //RPC_SpawnCharacter(info.Source, spawnPos);
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SpawnCharacter([RpcTarget] PlayerRef playerref, Vector3 spawnPos)
    {
        GameTest.GetMyRunner().Spawn(playerPF, spawnPos, quaternion.identity, playerref);
    }

}

using Fusion;
using UnityEngine;
namespace Homework
{
    public class GameManagerHW : NetworkBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private NetworkRunner runner;
        [SerializeField] private PlayerSpawnManager spawnManager;
        public override void Spawned()
        {
            base.Spawned();
            RPC_RequestSpawn();
        }
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_RequestSpawn(RpcInfo info = default)
        {
            if (spawnManager.TryGetSpawnPosition(info.Source, out var position))
            {
                RPC_SetSpawn(info.Source,position);
            }
            else
            {
                Debug.LogError("Attempting to get a spawn position but failed");
            }
        }

        private void RPC_SetSpawn([RpcTarget] PlayerRef playerRef, Vector3 spawnPosition)
        {
            runner.SpawnAsync(playerPrefab, spawnPosition);
        }
    }


    public struct PlayerMultiplayerData
    {
        public PlayerRef playerRef;
        public string playerNickName;
    }
}
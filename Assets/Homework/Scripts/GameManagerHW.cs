using Fusion;
using UnityEngine;
namespace Homework
{
    public class GameManagerHW : NetworkBehaviour, INetworkRunnerRequired
    {
        private static GameManagerHW instance;
        public static GameManagerHW Instance => instance;

        public CharacterSelectionManager characterSelectionManagerPF;
        [HideInInspector]
        public static CharacterSelectionManager CharacterSelectionManager;

        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private NetworkRunner runner;
        [SerializeField] private PlayerSpawnManager spawnManager;

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
        }




        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_RequestSpawn(RpcInfo info = default)
        {
            if (spawnManager.TryGetSpawnPosition(info.Source, out var position))
            {
                RPC_SetSpawn(info.Source, position);
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

        public void InjectRunner(NetworkRunner runner)
        {
            this.runner = runner;
            if (this.runner.IsSharedModeMasterClient)
            {
                CharacterSelectionManager = runner.Spawn(characterSelectionManagerPF);
            }
        }
    }
}
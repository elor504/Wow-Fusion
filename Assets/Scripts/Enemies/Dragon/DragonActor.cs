using Fusion;
using PlayFab.MultiplayerModels;
using StateMachine.Dragon;
using UnityEngine;

public class DragonActor : NetworkBehaviour
{
    [SerializeField] private DragonBrain dragonBrain;

    private NetworkRunner _serverRunner;

    public override void Spawned()
    {
        base.Spawned();
        if (Object.HasStateAuthority)
        {
            _serverRunner = Object.Runner;
            dragonBrain.Init();
            Debug.Log("[DragonActor] has been initialized at the server");
        }
    }


    public void Update()
    {
        
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority) return;
        base.FixedUpdateNetwork();
        dragonBrain.FixedUpdateState(_serverRunner.DeltaTime);
    }
}

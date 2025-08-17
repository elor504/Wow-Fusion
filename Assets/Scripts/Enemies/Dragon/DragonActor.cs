using Fusion;
using PlayFab.MultiplayerModels;
using StateMachine.Dragon;
using UnityEngine;

public class DragonActor : NetworkBehaviour
{
    [SerializeField] private DragonEnemy enemy;
    [Header("Brain")]
    [SerializeField] private DragonBrain dragonBrain;
    [Header("Roaming Settings")]
    [SerializeField] private Transform roamingTrans;
    [SerializeField] private float roamingRadius;

    private NetworkRunner _serverRunner;

    public override void Spawned()
    {
        base.Spawned();
        if (Object.HasStateAuthority)
        {
            _serverRunner = Object.Runner;
            dragonBrain.Init();
            ServerHandler.Instance.AddEnemyNetworkID(enemy.GetNetworkId(),enemy,_serverRunner);
        }
    }
    
    public override void FixedUpdateNetwork()
    {
        /*if (!Object.HasStateAuthority) return;
        base.FixedUpdateNetwork();
        dragonBrain.FixedUpdateState(_serverRunner.DeltaTime);*/
    }

    public void UpdateActor()
    {
        if (!Object.HasStateAuthority) return;
        dragonBrain.FixedUpdateState(_serverRunner.DeltaTime);
    }
    
    public Vector3 GetWalkTargetPosition()
    {
        var position = roamingTrans.position + Random.onUnitSphere * roamingRadius;
        position.y = 0;
        return position;
    }


    private void OnDrawGizmosSelected()
    {
        if (!roamingTrans) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(roamingTrans.position, roamingRadius);
    }
}

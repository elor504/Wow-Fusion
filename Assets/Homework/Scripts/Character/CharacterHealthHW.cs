using Fusion;
using UnityEngine;

public class CharacterHealthHW : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(HPValueChangedHandler))]
    public int HP { get; set; }

    [SerializeField] private int maxHP;

    public override void Spawned()
    {
        base.Spawned();
        HP = maxHP;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_DealDamage(int dmg)
    {
        HP -= dmg;
        if(HP < 0)
        {
            HP = 0;
        }
        Debug.Log($"Got damaged, HP left: {HP}");
    }



    public void HPValueChangedHandler()
    {
        //UI
        Debug.Log("HP value changed");
    }
}

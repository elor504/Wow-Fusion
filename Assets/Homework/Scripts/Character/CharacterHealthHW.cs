using Fusion;
using Homework;
using UnityEngine;

public class CharacterHealthHW : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(HPValueChangedHandler))]
    public int HP { get; set; }

    [Networked]
    public int CharacterIndex { get; set; }
    [SerializeField] private int maxHP;

    public override void Spawned()
    {
        base.Spawned();
        HP = maxHP;
        CharacterIndex = GameManagerHW.Instance.UIManager.CurrentLocalSelectedIndex + 1;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_DealDamage(int dmg,int senderCharacterIndex)
    {
        Debug.Log($"Can be damage?: {CanBeDamaged(senderCharacterIndex)} my index: {CharacterIndex}, sender index: {senderCharacterIndex}");
        if (!CanBeDamaged(senderCharacterIndex + 1))
            return;
     

        HP -= dmg;
        if(HP < 0)
        {
            HP = 0;
        }
    }

    private bool CanBeDamaged(int senderCharacterIndex)
    {
        return !(CharacterIndex % 2 == 0 && senderCharacterIndex % 2 == 0 || CharacterIndex % 2 == 1 && senderCharacterIndex % 2 == 1);
    }

    public void HPValueChangedHandler()
    {
        if (HasStateAuthority)
        {
            GameManagerHW.Instance.UIManager.HPBar.UpdateBar(HP, maxHP);
        }
    }
}

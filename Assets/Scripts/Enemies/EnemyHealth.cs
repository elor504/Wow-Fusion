using Fusion;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] private int maxHealth;
    [Networked] public int CurrentHP { get; private set; }

    public bool IsDead => CurrentHP <= 0;



    public void Init(int maxHP)
    {
        maxHealth = maxHP;
        CurrentHP = maxHealth;
    }

    [Rpc(RpcSources.All,RpcTargets.StateAuthority)]
    public void DealDamage(int damage,RpcInfo source = default)
    {
        //TODO: Add the damage dealth by the player to this enemy into some kind of a recorder so show who dealth the most damage in the dungeon

        CurrentHP -= damage;
        if (CurrentHP < 0)
        {
            CurrentHP = 0;
            //Death
        }
        Debug.Log($"[EnemyHealth] Dealing damage to enemy, damage: {damage}, hp left: {CurrentHP}");

    }

}

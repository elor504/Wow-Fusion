using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetableEntity : ITargetable
{
    public bool IsEnemy();
    public bool IsAlly();

    ///Will ask for the spell data
    public void DealDamage(ITargetableEntity caster,int damage);
    public void Heal(ITargetableEntity caster);
    
    
    public int GetHealth();
    public int GetMana();


    public bool CanCastSpell(int amount);

    public bool TryGetEntityStat(out EntityStat entityStat);
    public bool TryGetEntityVisualPosition(out CharacterVFXVisual vfxVisual);
    public GameObject GetEntityGO();
    public Transform GetProjectileSpawnPosition();
    public Transform GetHitPosition();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetableEntity : ITargetable
{
    public bool IsEnemy();
    public bool IsAlly();

    ///Will ask for the spell data
    public void DealDamage(GameObject caster);
    public void Heal(GameObject caster);
    
    
}

using System.Collections.Generic;
using UnityEngine;

public class SelfBuffSpell : BaseSpell
{
    private IStatusEffect _statusEffect;
    public SelfBuffSpell(StatBuffData data) : base(data)
    {
        _statusEffect = data.StatusEffect.GetStatusEffect();
    }
    
    public override void CastSkill(ITargetableEntity caster, ITargetableEntity target)
    {
        if (caster.TryGetEntityStat(out var entityStat))
        {
            if (entityStat.TryToAddStatusEffect(_statusEffect))
            {
                
            }
        }
    }

    public override bool CanStartCasting(ITargetableEntity caster)
    {
      return caster.CanCastSpell(_manaCost);
    }

    public override bool CanCast(ITargetableEntity caster, ITargetableEntity target)
    {
        return caster.CanCastSpell(_manaCost);
    }

    public override bool CompareID(string id)
    {
       return _spellID == id;
    }

}

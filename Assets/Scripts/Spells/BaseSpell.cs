using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class BaseSpell
{
   [SerializeField] private protected string spellName;
   [SerializeField] private protected string description;
   [SerializeField] private protected float timeToCast;

   public BaseSpell(SkillDataSO spellData)
   {
      spellName = spellData.name;
      description = spellData.Description;
      timeToCast = spellData.TimeToCast;

   }

   protected BaseSpell()
   {
      
   }

   public abstract void CastSkill(ITargetableEntity caster, ITargetableEntity target);
   public abstract bool CanCast(ITargetableEntity caster);
   public abstract bool OnCast(ITargetableEntity caster);
}


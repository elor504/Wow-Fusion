using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class BaseSpell
{
   private protected string _spellID;
   [SerializeField] private protected string spellName;
   [SerializeField] private protected string description;
   [SerializeField] private protected float timeToCast;
   private protected BasicVFX _handsSpellVFX;

   
   public float TimeToCast => timeToCast;
   public string SpellID => _spellID;
   public BasicVFX HandsSpellVFX => _handsSpellVFX;
   public BaseSpell(SkillDataSO spellData)
   {
      _spellID = spellData.ID;
      spellName = spellData.name;
      description = spellData.Description;
      timeToCast = spellData.TimeToCast;
      _handsSpellVFX = spellData.HandsSpellVFX;
   }

   protected BaseSpell()
   {
      
   }

   public abstract void CastSkill(ITargetableEntity caster, ITargetableEntity target);
   public abstract bool CanStartCasting(ITargetableEntity caster);
   public abstract bool CanCast(ITargetableEntity caster, ITargetableEntity target);
   public abstract bool CompareID(string id);
}


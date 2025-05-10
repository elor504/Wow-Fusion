using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Buff Data", menuName = "Data/Stat Buff Skill")]
public class StatBuffData : SkillDataSO
{
   [SerializeField] private StatEffectDataSO statusEffect;
   public StatEffectDataSO StatusEffect => statusEffect;


   public override BaseSpell GetSpell()
   {
      return new SelfBuffSpell(this);
   }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassSkillManager : MonoBehaviour
{
    [SerializeField] private ProjectileSpellData spellData;
    [SerializeReference] private BaseSpell baseSpell;

    [SerializeField] private BasicEnemy caster;
    [SerializeField] private BasicEnemy target;
        
    private void Awake()
    {
        baseSpell = (ProjectileSpell)spellData.GetSpell();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            baseSpell.CastSkill(caster,target);
        }
    }
}

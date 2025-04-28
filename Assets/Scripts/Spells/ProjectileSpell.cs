using System;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

[Serializable]
public class ProjectileSpell : BaseSpell
{
    [SerializeField] private protected int _damage;
    [SerializeField] private protected int _manaCost;
    [SerializeField] private protected float _speed;
    [SerializeField] private protected BaseProjectile _projectile;
    public ProjectileSpell(ProjectileSpellData spellData) : base(spellData)
    {
        _damage = spellData.Damage;
        _manaCost = spellData.ManaCost;
        _speed = spellData.Speed;
        _projectile = spellData.Projectile;
    }
    
    public override void CastSkill(ITargetableEntity caster, ITargetableEntity target)
    {
        if (OnCast(caster))
        {
            var proj = Object.Instantiate(_projectile);
            
            proj.InitProjectile(caster.GetProjectileSpawnPosition().position,caster, target, _damage, _speed);
        }
    }

    public override bool CanCast(ITargetableEntity caster)
    {
       return true;
    }

    public override bool OnCast(ITargetableEntity caster)
    {
        return caster.CanCastSpell(_manaCost);
    }
}


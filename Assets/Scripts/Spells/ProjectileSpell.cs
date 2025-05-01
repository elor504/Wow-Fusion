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
        if (CanCast(caster,target))
        {
            var spawnPos = caster.GetProjectileSpawnPosition().position;
            var proj = ProjectilePoolSystem.Instance.GetAvailableObjectFromPool(_projectile,spawnPos);
            
            proj.InitProjectile(spawnPos,caster, target, _damage, _speed);
        }
    }

    public override bool CanStartCasting(ITargetableEntity caster)
    {
       return true;
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


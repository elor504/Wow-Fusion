using UnityEngine;

[CreateAssetMenu(fileName = "SpellData", menuName = "Data/SpellData")]
public class ProjectileSpellData : SkillDataSO
{
    [SerializeField] private int manaCost;
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private protected BaseProjectile _projectile;

    public int ManaCost => manaCost;    
    public int Damage => damage;
    public float Speed => speed;
    public BaseProjectile Projectile => _projectile;

    
    public override BaseSpell GetSpell()
    {
        return new ProjectileSpell(this);
    }
}
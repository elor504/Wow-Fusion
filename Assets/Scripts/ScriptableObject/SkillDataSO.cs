using UnityEngine;

public abstract class SkillDataSO : ScriptableObject
{
    [SerializeField] private protected string id;
    [SerializeField] private protected string name;
    [SerializeField] private protected string description;
    [SerializeField] private protected float timeToCast;
    [SerializeField] private protected BasicVFX _handsSpellVFX;
    public string ID => id;
    public string Name => name;
    public string Description => description;
    public float TimeToCast => timeToCast;
    public BasicVFX HandsSpellVFX => _handsSpellVFX;
    
    public virtual BaseSpell GetSpell()
    {
        Debug.LogError(("Attempting To Create a spell with an abstract class!!!"));
        return null;
    }
}
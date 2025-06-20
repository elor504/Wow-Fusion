using UnityEngine;
using UnityEngine.Serialization;

public abstract class SkillDataSO : ScriptableObject
{
    [SerializeField] private protected string id;
    [SerializeField] private protected string name;
    [SerializeField] private protected string description;
    [SerializeField] private int manaCost;
    [SerializeField] private protected float timeToCast;
    [SerializeField] private protected BasicVFX handsSpellVFX;
    [SerializeField] private protected BasicVFX bodyVFX;
    public string ID => id;
    public string Name => name;
    public string Description => description;
    public float TimeToCast => timeToCast;
    public int ManaCost => manaCost;
    public BasicVFX HandsSpellVFX => handsSpellVFX;
    public BasicVFX BodyVFX => bodyVFX;
    public virtual BaseSpell GetSpell()
    {
        Debug.LogError(("Attempting To Create a spell with an abstract class!!!"));
        return null;
    }
}
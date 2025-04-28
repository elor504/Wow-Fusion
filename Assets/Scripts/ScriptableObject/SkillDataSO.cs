using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillDataSO : ScriptableObject
{

    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private float timeToCast;



    public string Name => name;
    public string Description => description;


    public abstract void CastSkill(GameObject caster, GameObject target);

    public abstract bool CanCast();
    public abstract bool OnCast();



}

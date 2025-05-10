using System;
using System.Collections.Generic;
using UnityEngine;

public class StatBonusEffect : IStatusEffect
{
    private float _timer;
    private Dictionary<StatType, int> _statsToApply = new Dictionary<StatType, int>();
    private ITargetableEntity _entity;
    private StatEffectData _data;
    
    public StatEffectData Data => _data;
    public StatBonusEffect(StatEffectData data, float time)
    {
        _data = data;
        _statsToApply = new Dictionary<StatType,int>();
        _statsToApply[_data.StatToEffect] = _data.StatBonus;
        _timer = time;
    }
    
    
    public void OnEnterStatus(ITargetableEntity entity)
    {
        _entity = entity;
        if(_entity.TryGetEntityStat(out var stat))
        {
            foreach (var statToApply in _statsToApply)
            {
                stat.ApplyStatusEffectStatbonuses(statToApply.Key, statToApply.Value);
            }
        }
    }


    public void UpdateStatusEffect(float deltaTime)
    {
        _timer -= deltaTime;
        
        //Debug.Log("Update Stat bonus effect: " + _timer);
    }
    public void OnExitStatus()
    {
        Debug.Log("Finished stat bonus");
        if(_entity.TryGetEntityStat(out var stat))
        {
            foreach (var statToApply in _statsToApply)
            {
                stat.ApplyStatusEffectStatbonuses(statToApply.Key, -statToApply.Value);
            }
        }
    }

    public StatEffectData GetStatusEffectUIInformation()
    {
        return _data;
    }

    public bool IsFinished()
    {
        return _timer <= 0;
    }
}


public interface IStatusEffect
{
    public void OnEnterStatus(ITargetableEntity entity);
    public void UpdateStatusEffect(float deltaTime);
    public void OnExitStatus();

    public StatEffectData GetStatusEffectUIInformation();
    
    public bool IsFinished();
}
using System;
using System.Collections.Generic;
using UnityEngine;

public class StatBonusEffect : IStatusEffect
{
    private float _timer;
    private Dictionary<StatType, int> _statsToApply = new Dictionary<StatType, int>();
    private ITargetableEntity _entity;
    private StatEffectData _data;
    private BasicVFX _effectVFX;
    private const string StatusEffectID = "StatusEffect";
    
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
        if (entity.TryGetEntityVisualPosition(out var characterVFX) && characterVFX.TryGetVisualPositionParent(StatusEffectID,out var parentVFX))
        {
            _effectVFX = VFXPoolSystem.Instance.GetAvailableObjectFromPool(_data.StatusEffectVfxPF,parentVFX.position);
            _effectVFX.SetParent(parentVFX);
            _effectVFX.InitVFX(parentVFX.position);
            _effectVFX.gameObject.SetActive(true);
        }
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
        
        _effectVFX.StopParticleSystem();
        _effectVFX = null;
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
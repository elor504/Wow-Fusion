using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBrain : BaseBrain
{
    private PlayerCharacter _playerCharacter;
    private List<BaseState> _playerStates = new List<BaseState>();
    private BaseState _currentState;

    
    public PlayerCharacter PlayerCharacter => _playerCharacter;

    public void InitBrain(PlayerCharacter player)
    {
        _playerCharacter = player;
        _playerStates.Add(new PlayerIdleState((int) PlayerStates.Idle, this)); 
        _playerStates.Add(new PlayerWalkState((int) PlayerStates.Walk, this));
        _playerStates.Add(new PlayerCastState((int) PlayerStates.Cast, this));

        ChangeState((int) PlayerStates.Idle);
    }

    public override void ChangeState(int state)
    {
        _currentState?.ExitState();
        _currentState = GetStateByID(state);
        _currentState?.EnterState();
    }

    public override void UpdateState()
    {
        _currentState?.UpdateState(Time.deltaTime);
    }

    public override void FixedUpdateState()
    {
        _currentState?.FixedUpdateState(Time.fixedDeltaTime);
    }

    public override void OnAnimationCallFunction(int eventID)
    {
    }

    public bool TryToCastSpell(BaseSpell spell, ITargetableEntity caster, ITargetableEntity target)
    {
        var castStateIndex = (int) PlayerStates.Cast;
        var castState = GetStateByID(castStateIndex);
        if (castState is PlayerCastState playerCastState && spell.CanCast(caster, target))
        {
            playerCastState.SetSpellToCast(spell,caster,target);
            ChangeState(castStateIndex);

            return true;
        }
        
        return false;
    }
    
    
    private BaseState GetStateByID(int id)
    {
        foreach (var state in _playerStates)
        {
            if (state.CompareID(id))
            {
                return state;
            }
        }

        return null;
    }
}

public enum PlayerStates
{
    Idle,
    Walk,
    Jump,
    Cast
}
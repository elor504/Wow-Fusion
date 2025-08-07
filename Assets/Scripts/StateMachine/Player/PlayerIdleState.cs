using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : BaseState
{
    private int _id;
    private PlayerBrain _brain;
    private PlayerAnimator _animator;
    private PlayerMovement _movement;
    
    private static readonly int Idle = Animator.StringToHash("Idle");
    public PlayerIdleState(int id, PlayerBrain brain)
    {
        _brain = brain;
        _id = id;

        _animator = _brain.PlayerCharacter.GetAnimator;
        _movement = _brain.PlayerCharacter.GetMovement;
    }
    
    public override void EnterState()
    {
        _animator.SetBool(Idle,true);
    }

    public override void ExitState()
    {
        _animator.SetBool(Idle,false);
    }

    public override void UpdateState(float deltaTime)
    {
        if (_movement.IsPressingMovement)
        {
            if (TryToChangeToMovementState())
            {
                _brain.ChangeState((int)PlayerStates.Walk);
                return;
            }
        }
    }

    private bool TryToChangeToMovementState()
    {
        //Check if grounded ETC


        return true;
    }
    public override void FixedUpdateState(float fixedDeltaTime)
    {
       
    }

    public override bool CompareID(int id)
    {
        return _id == id;
    }
}

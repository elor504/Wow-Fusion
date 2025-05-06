using UnityEngine;

public class PlayerWalkState : BaseState
{
    private int _id;
    private PlayerBrain _brain;
    
    private PlayerAnimator _animator;
    private PlayerMovement _movement;
    private static readonly int Walk = Animator.StringToHash("Walk");
    public PlayerWalkState(int id, PlayerBrain brain)
    {
        _brain = brain;
        _id = id;
        
        _animator = _brain.PlayerCharacter.GetAnimator;
        _movement = _brain.PlayerCharacter.GetMovement;
    }

    public override void EnterState()
    {
        _brain.PlayerCharacter.GetAnimator.SetBool(Walk,true);
    }

    public override void ExitState()
    {
        _brain.PlayerCharacter.GetAnimator.SetBool(Walk,false);
    }

    public override void UpdateState(float deltaTime)
    {
        if (!_movement.IsPressingMovement)
        {
            if (TryToChangeToIdleState())
            {
                _brain.ChangeState((int)PlayerStates.Idle);
                return;
            }
        }
        
        
        _brain.PlayerCharacter.GetMovement.Move();
        _brain.PlayerCharacter.GetAnimator.UpdateMovementOnAnimator();
    }

    private bool TryToChangeToIdleState()
    {
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
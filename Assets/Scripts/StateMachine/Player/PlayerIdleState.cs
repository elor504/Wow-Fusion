using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : BaseState
{
    private static readonly int Idle = Animator.StringToHash("Idle");
    private int _id;
    private PlayerBrain _brain;

    public PlayerIdleState(int id, PlayerBrain brain)
    {
        _brain = brain;
        _id = id;
    }
    
    public override void EnterState()
    {
        _brain.PlayerCharacter.GetAnimator.SetBool(Idle,true);
    }

    public override void ExitState()
    {
        _brain.PlayerCharacter.GetAnimator.SetBool(Idle,false);
    }

    public override void UpdateState(float deltaTime)
    {
        
    }

    public override void FixedUpdateState(float fixedDeltaTime)
    {
       
    }

    public override bool CompareID(int id)
    {
        return _id == id;
    }
}

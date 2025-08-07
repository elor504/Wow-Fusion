using System;
using UnityEngine;
namespace StateMachine.Dragon
{
    public enum DragonState
    {
        idle,
        Walk,
        Atk1,
        Atk2,
        Atk3,
        Death
    }
    [Serializable]
    public class DragonBrain : BaseBrain
    {
        [SerializeField] private DragonIdleState idleState;

        private BaseState _currentState;
        //TODO: make the dragon start the timer in the idle state only after the players aggro him (distance or damaging)
        public void Init()
        {
            idleState.InitState(this);


            ChangeState((int)DragonState.idle);
        }

        public override void ChangeState(int state)
        {
            _currentState?.ExitState();
            _currentState = GetStateByID((DragonState)state);
            _currentState.EnterState();
        }
        public void ChooseRandomBehaviour()
        {
            //TODO: Small random for the attacks (less hp maybe more aggro)
            ChangeState((int)DragonState.idle);
        }
        public override void FixedUpdateState(float fixedDeltaTime)
        {
            _currentState?.FixedUpdateState(fixedDeltaTime);
        }    
        public override void UpdateState(float time)
        {
            _currentState?.UpdateState(time);
        }
        public override void OnAnimationCallFunction(int eventID)
        {

        }

        private BaseState GetStateByID(DragonState state)
        {
            switch (state)
            {
                case DragonState.idle:
                    return idleState;
            }
            return null;
        }

        
    }
}

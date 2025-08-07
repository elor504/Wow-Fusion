using System;
using UnityEngine;
using Random = UnityEngine.Random;

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
        [Header("References")]
        [SerializeField] private DragonActor actor;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform bodyTrans;
        [SerializeField] private Animator animator;

        [Header("States")]
        [SerializeField] private DragonIdleState idleState;
        [SerializeField] private DragonWalkState walkState;

        private BaseState _currentState;
        //TODO: make the dragon start the timer in the idle state only after the players aggro him (distance or damaging)
        public void Init()
        {
            idleState.InitState(this,animator);
            walkState.Init(this, rb, bodyTrans,animator);

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
            ChangeState( Random.Range(0,2));
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

        public Vector3 GetRandomWalkPosition()
        {
            return actor.GetWalkTargetPosition();
        }

        private BaseState GetStateByID(DragonState state)
        {
            switch (state)
            {
                case DragonState.idle:
                    return idleState;
                case DragonState.Walk:
                    walkState.SetTargetPosition(GetRandomWalkPosition());
                    return walkState;
            }
            return null;
        }


    }
}

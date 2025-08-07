using System;
using UnityEngine;
using Random = UnityEngine.Random;
namespace StateMachine.Dragon
{
    [Serializable]
    public class DragonIdleState : BaseState
    {
        //References
        private DragonBrain _brain;
        private Animator _animator;

        [SerializeField] private int stateID;
        [SerializeField] private float maxRandomActionTime = 10f;
        [SerializeField] private float minRandomActionTime = 6f;

        private float _actionCounter;

        private static int IdleStateVariable = Animator.StringToHash("Idle");


        public void InitState(DragonBrain brain, Animator animator)
        {
            _brain = brain;
            _animator = animator;
        }

        public override void EnterState()
        {
            _actionCounter = Random.Range(minRandomActionTime, maxRandomActionTime);
            _animator.SetBool(IdleStateVariable, true);
            Debug.Log("[DragonIdleState] Enter state");
        }
        public override void ExitState()
        {
            _animator.SetBool(IdleStateVariable, false);
            Debug.Log("[DragonIdleState] Exit state");
        }

        public override void FixedUpdateState(float fixedDeltaTime)
        {
            _actionCounter -= fixedDeltaTime;
            if (_actionCounter <= 0)
            {
                _brain.ChooseRandomBehaviour();
                Debug.Log("[DragonIdleState]Change state");
            }


        }
        public override void UpdateState(float deltaTime)
        {

        }


        public override bool CompareID(int id)
        {
            return stateID == id;
        }
    }
}
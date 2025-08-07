using System;
using UnityEngine;
using Random = UnityEngine.Random;
namespace StateMachine.Dragon
{
    [Serializable]
    public class DragonIdleState : BaseState
    {
        [SerializeField] private int stateID;

        [SerializeField] private float maxRandomActionTime = 10f;
        [SerializeField] private float minRandomActionTime = 6f;

        private DragonBrain _brain;



        private float actionCounter;

        public void InitState(DragonBrain brain)
        {
            _brain = brain;
        }

        public override void EnterState()
        {
            actionCounter = Random.Range(minRandomActionTime, maxRandomActionTime);
            Debug.Log("[DragonIdleState] Enter state");
            //Set Animator 
        }
        public override void ExitState()
        {
            Debug.Log("[DragonIdleState] Exit state");
            //Set Animator 
        }

        public override void FixedUpdateState(float fixedDeltaTime)
        {
            actionCounter -= fixedDeltaTime;
            if (actionCounter <= 0)
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
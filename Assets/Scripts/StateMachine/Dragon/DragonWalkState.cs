using System;
using UnityEngine;

namespace StateMachine.Dragon
{
    [Serializable]
    public class DragonWalkState : BaseState
    {
        private static int WalkStateVariable = Animator.StringToHash("Walk");

        //References
        private DragonBrain _brain;
        private Rigidbody _rb;
        private Transform _dragonTrans;
        private Animator _animator;

        [Header("State Settings")]
        [SerializeField] private int stateID;

        [SerializeField] private float movementSpeed;

        private Vector2 _direction;
        private Vector3 _targetPosition;

        public void Init(DragonBrain brain, Rigidbody rb, Transform dragonTrans, Animator animator)
        {
            _brain = brain;
            _rb = rb;
            _dragonTrans = dragonTrans;
            _animator = animator;
        }

        public override bool CompareID(int id)
        {
            return stateID == id;
        }

        public override void EnterState()
        {
            _animator.SetBool(WalkStateVariable, true);
        }

        public override void ExitState()
        {
            _animator.SetBool(WalkStateVariable, false);
        }
        public void SetTargetPosition(Vector3 targetPosition)
        {
            _targetPosition = targetPosition;
            _targetPosition.y = 0;
        }

        public override void FixedUpdateState(float fixedDeltaTime)
        {
            _direction = (_targetPosition - _dragonTrans.position).normalized;
            _rb.MovePosition(_direction * (movementSpeed * fixedDeltaTime));

            if (Vector3.Distance(_dragonTrans.position, _targetPosition) < 0.01f)
            {
                _brain.ChooseRandomBehaviour();
            }
        }

        public override void UpdateState(float deltaTime)
        {

        }
    }
}
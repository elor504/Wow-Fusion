using StateMachine.Dragon;
using UnityEngine;

public class DragonWalkState : BaseState
{
    //References
    private DragonBrain _brain;
    private Rigidbody _rb;
    private Transform _dragonTrans;


    [Header("State Settings")]
    [SerializeField] private int stateID;

    [SerializeField] private float movementSpeed;

    private Vector2 _direction;
    private Vector3 _targetPosition;

    public void Init(DragonBrain brain, Rigidbody rb,Transform dragonTrans)
    {
        _brain = brain;
        _rb = rb;
        _dragonTrans = dragonTrans;
    }

    public override bool CompareID(int id)
    {
        return stateID == id;
    }

    public override void EnterState()
    {
      
    }

    public override void ExitState()
    {
        
    }
    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
        _targetPosition.y = 0;
    }

    public override void FixedUpdateState(float fixedDeltaTime)
    {
        _direction = (_dragonTrans.position - _targetPosition).normalized;
        _rb.MovePosition(_direction * (movementSpeed * fixedDeltaTime));

        if(Vector3.Distance(_dragonTrans.position, _targetPosition) < 0.01f)
        {
            _brain.ChooseRandomBehaviour();
        }
    }

    public override void UpdateState(float deltaTime)
    {
       
    }
}

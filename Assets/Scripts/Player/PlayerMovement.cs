using Fusion;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private NetworkCharacterController controller;
    [SerializeField] private float moveSpeed = 10f;

    private Vector2 _movementInput;
    public bool IsPressingMovement => _movementInput != Vector2.zero;

    public void Move(float networkedTime)
    {
        Vector3 direction = new Vector3(_movementInput.x, 0f, _movementInput.y);
        direction.Normalize();
        var velocity = direction * (moveSpeed * networkedTime);
        Debug.Log($"[PlayerMovement, Move Function] Move character: {velocity}");
        controller.Move(velocity);
    }
    public void Rotate(Vector3 foward)
    {
        transform.forward = foward;
    }

    public void ListenToMovementInput(Vector2 movement)
    {
        _movementInput = movement;
    }
    private void OnEnable()
    {
        InputManager.OnMovementInput += ListenToMovementInput;
    }
    private void OnDisable()
    {
        InputManager.OnMovementInput -= ListenToMovementInput;
    }

}

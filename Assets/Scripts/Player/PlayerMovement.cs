using Fusion;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private NetworkCharacterController controller;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed;

    private Vector3 _movementDirection;
    public bool IsPressingMovement => _movementDirection != Vector3.zero;

    public void Move(float networkedTime)
    {
        Vector3 direction = _movementDirection;
        direction.Normalize();
        var velocity = direction * (moveSpeed * networkedTime);
        Debug.Log($"[PlayerMovement, Move Function] Move character: {velocity}");
        controller.Move(velocity);
    }
    public void Rotate(Vector3 foward)
    {
        transform.forward = foward;
    }

    public void RotateByInput(int direction,float networkedTime)
    {
        var currentRotation = transform.rotation;
        currentRotation.y += direction * (rotationSpeed * networkedTime);
        transform.Rotate(Vector3.up, currentRotation.y + direction * (rotationSpeed * networkedTime));
    }
    public void ListenToMovementInput(Vector3 movement)
    {
        _movementDirection = movement;
    }
    private void OnEnable()
    {
        InputManager.OnMovementDirection += ListenToMovementInput;
        InputManager.OnRotateCharacterInput += RotateByInput;
    }
    private void OnDisable()
    {
        InputManager.OnMovementDirection -= ListenToMovementInput;
        InputManager.OnRotateCharacterInput -= RotateByInput;
    }

}

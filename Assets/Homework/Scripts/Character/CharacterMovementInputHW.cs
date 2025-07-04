using Fusion;
using Homework;
using System.Collections;
using UnityEngine;
public class CharacterMovementInputHW : NetworkBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float movementSpeed;

    private Vector2 _movementInput;
    private Vector3 _movement;
    private bool _initialized;

  
    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            //For some reason if i try to move my character even after Spawned() being called it resets its position
            //So i added Delay
            StartCoroutine(DelayedInitialization());
            GameManagerHW.Instance.PlayerCamera.SetCameraOnObject(transform);
        }
    }

    private IEnumerator DelayedInitialization()
    {
        yield return new WaitForSeconds(1f);
        _initialized = true;
  
    }

    public override void FixedUpdateNetwork()
    {
        if (!_initialized) return;

        _movementInput.x = Input.GetAxis("Horizontal");
        _movementInput.y = Input.GetAxis("Vertical");
        if (_movementInput.magnitude > float.Epsilon)
        {
            _movement.x = _movementInput.x * movementSpeed * Time.fixedDeltaTime;
            _movement.z = _movementInput.y * movementSpeed * Time.fixedDeltaTime;
            characterController.Move(_movement);
        }
    }

}

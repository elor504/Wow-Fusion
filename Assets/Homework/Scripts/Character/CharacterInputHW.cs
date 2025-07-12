using Fusion;
using Homework;
using System;
using System.Collections;
using UnityEngine;
public class CharacterInputHW : NetworkBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed;
    [Header("Shooting Settings")]
    [SerializeField] private float shootingCD;

    private bool _initialized;

    private Vector2 _movementInput;
    private Vector3 _movement;

    private float _shootCounter;


    public event Action<PlayerRef> ShootAction;



    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            GameManagerHW.Instance.PlayerCamera.SetCameraOnObject(transform);
            //For some reason if i try to move my character even after Spawned() being called it resets its position
            //So i added Delay
            StartCoroutine(DelayedInitialization());
        }
    }
    private IEnumerator DelayedInitialization()
    {
        yield return new WaitForSeconds(1f);
        _initialized = true;
    }

    //https://doc.photonengine.com/fusion/v1/manual/network-object/fixed-update-network
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
        if (_shootCounter > 0)
        {
            //https://doc.photonengine.com/fusion/v1/tutorials/host-mode-basics/3-prediction
            _shootCounter -= Runner.DeltaTime;
        }

        if (_shootCounter <= 0 && Input.GetMouseButton(0))
        {
            _shootCounter = shootingCD;
            ShootAction?.Invoke(Object.Runner.LocalPlayer);
        }

        animator.SetFloat("MovementX", _movementInput.x);
        animator.SetFloat("MovementY", _movementInput.y);
        animator.SetBool("Walk", _movementInput.magnitude > float.Epsilon);

    }

}

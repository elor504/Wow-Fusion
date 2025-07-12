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
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpHeight;

    [Header("Shooting Settings")]
    [SerializeField] private float shootingCD;

    private bool _initialized;
    private float _gravity = -9.81f;
    private bool _isGrounded;
    private float _yVelocity;
    private Vector2 _movementInput;
    private Vector3 _movement;
    private Vector3 _rotation;
    private Vector3 _finalMovement;
    private float _shootCounter;
    private NetworkRunner _myRunner;

    public event Action<PlayerRef> ShootAction;



    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            GameManagerHW.Instance.PlayerCamera.SetCameraOnObject(transform);
            _rotation = transform.localRotation.eulerAngles;
            //For some reason if i try to move my character even after Spawned() being called it resets its position
            //So i added Delay
            StartCoroutine(DelayedInitialization());
            _myRunner = GameTest.GetMyRunner();
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
        _isGrounded = characterController.isGrounded;

        _movementInput.x = Input.GetAxisRaw("Horizontal");
        _movementInput.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.E))
        {
            _rotation.y += rotationSpeed * _myRunner.DeltaTime;
            transform.localRotation = Quaternion.Euler(_rotation);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            _rotation.y -= rotationSpeed * _myRunner.DeltaTime;
            transform.localRotation = Quaternion.Euler(_rotation);
        }

        if (_isGrounded && _yVelocity < 0)
        {
            _yVelocity = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _yVelocity = Mathf.Sqrt(jumpHeight * -2f * _gravity);
            Debug.Log($"Jump: {_yVelocity}");
        }

        _movement = (transform.forward * _movementInput.y) + (transform.right * _movementInput.x);
        _finalMovement = _movement + (transform.up * _yVelocity);
        characterController.Move(_finalMovement * _myRunner.DeltaTime);




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
        animator.SetBool("Jump", _yVelocity > float.Epsilon);
        animator.SetBool("Idle", _movementInput.magnitude < float.Epsilon && _yVelocity == 0);
        _yVelocity += _gravity * _myRunner.DeltaTime;
    }

}

using System;
using System.Collections;
using UnityEngine;


public class PlayerCamera : MonoBehaviour
{
    private static PlayerCamera _instance;
    public static PlayerCamera Instance => _instance;


    [Header("References")] 
    [SerializeField] private Transform playerTrans;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform cameraTransform;


    [Header("Camera offset")] 
    [SerializeField] private Vector3 cameraOffsetToPlayer;

    [Header("Camera Rotation settings")] 
    [SerializeField] private bool invertYAxis;

    [SerializeField] private float cameraRotationXSpeed;
    [SerializeField] private float cameraRotationYSpeed;
    [SerializeField] private Vector2 yCameraRange;


    [Header(("Camera Zoom settings"))] 
    [SerializeField] private Vector2 zoomRange;

    [SerializeField] private float zoomSpeed;
    [SerializeField] private float smoothTime;

    public Camera GetCamera => camera;
    public Vector3 Foward => camera.transform.forward;
    public Vector3 Right => camera.transform.right;
    private Coroutine zoomCoroutine;
    
    private Vector3 _cameraRotation;
    private Vector3 _mouseAxis;

    private Vector3 _cameraOffset;
    private Vector3 _cameraLocalPosition;
    private Vector3 _cameraSmoothedLocalPosition;

    private float _currentZoomVelocity;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }
    }


    private void OnEnable()
    {
        _cameraOffset = cameraTransform.localPosition;
        _cameraRotation = cameraTransform.localEulerAngles;
        _cameraLocalPosition = cameraTransform.localPosition;
        _cameraLocalPosition.z = Mathf.Clamp(_cameraLocalPosition.z, zoomRange.x, zoomRange.y);

        InputManager.OnScroll += HandleMouseWheel;
        InputManager.OnHoldingRightMouse += HandleCameraRotation;
        InputManager.OnHoldingLeftMouseOnEmpty += HandleCameraRotation;


    }

    private void OnDisable()
    {
        InputManager.OnScroll -= HandleMouseWheel;
        InputManager.OnHoldingRightMouse -= HandleCameraRotation;
        InputManager.OnHoldingLeftMouseOnEmpty -= HandleCameraRotation;
    }


    private void LateUpdate()
    {
        if (!playerTrans)
            return;

        UpdateCamera();
    }
    public void InitCamera(Transform playerToFollow)
    {
        playerTrans = playerToFollow;
    }


    private void UpdateCamera()
    {
        transform.position = playerTrans.transform.position + cameraOffsetToPlayer;
    }

    private void HandleCameraRotation()
    {
        int multiply = invertYAxis ? -1 : 1;
        _mouseAxis.x = Input.GetAxis("Mouse Y") * cameraRotationXSpeed * multiply;
        _mouseAxis.y = Input.GetAxis("Mouse X") * cameraRotationYSpeed;
        _cameraRotation += _mouseAxis * Time.deltaTime;
        _cameraRotation.x = Mathf.Clamp(_cameraRotation.x, yCameraRange.x, yCameraRange.y);
        transform.localEulerAngles = _cameraRotation;
        Debug.Log($"[Player Camera] camera's foward: {transform.forward}");
    }

    
    ///Zoom
    private void HandleMouseWheel(float scroll)
    {
        if (Mathf.Abs(scroll) > Mathf.Epsilon)
        {
            float targetZ = _cameraLocalPosition.z - scroll * zoomSpeed;
            targetZ = Mathf.Clamp(targetZ, zoomRange.x, zoomRange.y);
            
            if (zoomCoroutine != null)
                StopCoroutine(zoomCoroutine);

            zoomCoroutine = StartCoroutine(SmoothZoomTo(targetZ));
        }
    }
    private IEnumerator SmoothZoomTo(float targetZ)///Chatgpt
    {
        float duration = smoothTime; // e.g. 0.1f
        float elapsed = 0f;
        float startZ = cameraTransform.localPosition.z;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // Optional: ease in/out with smoother interpolation
            t = Mathf.SmoothStep(0f, 1f, t);
        
            float newZ = Mathf.Lerp(startZ, targetZ, t);
            _cameraLocalPosition.z = newZ;
            cameraTransform.localPosition = _cameraLocalPosition;

            yield return null;
        }

        // Snap to target just in case
        _cameraLocalPosition.z = targetZ;
        cameraTransform.localPosition = _cameraLocalPosition;
    }
    
}
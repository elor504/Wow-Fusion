using System;
using System.Collections;
using UnityEngine;


public class PlayerCamera : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Transform playerTrans;
    [SerializeField] private Transform camera;


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

    private Coroutine zoomCoroutine;
    
    private Vector3 _cameraRotation;
    private Vector3 _mouseAxis;

    private Vector3 _cameraOffset;
    private Vector3 _cameraLocalPosition;
    private Vector3 _cameraSmoothedLocalPosition;

    private float _currentZoomVelocity;

    private void Awake()
    {
        _cameraOffset = camera.localPosition;
        _cameraRotation = camera.localEulerAngles;
        _cameraLocalPosition = camera.localPosition;
        _cameraLocalPosition.z = Mathf.Clamp(_cameraLocalPosition.z, zoomRange.x, zoomRange.y);
    }

    private void OnEnable()
    {
        InputManager.OnScroll += HandleMouseWheel;
        InputManager.OnHoldingRightMouse += HandleCameraRotation;
    }

    private void OnDisable()
    {
        InputManager.OnScroll -= HandleMouseWheel;
        InputManager.OnHoldingRightMouse -= HandleCameraRotation;
    }


    private void Update()
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
        ///Detect if mouse is on any ui
        
        int multiply = invertYAxis ? -1 : 1;
        _mouseAxis.x = Input.GetAxis("Mouse Y") * cameraRotationXSpeed * multiply;
        _mouseAxis.y = Input.GetAxis("Mouse X") * cameraRotationYSpeed;
        _cameraRotation += _mouseAxis * Time.deltaTime;
        _cameraRotation.x = Mathf.Clamp(_cameraRotation.x, yCameraRange.x, yCameraRange.y);
        transform.localEulerAngles = _cameraRotation;
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
        float startZ = camera.localPosition.z;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // Optional: ease in/out with smoother interpolation
            t = Mathf.SmoothStep(0f, 1f, t);
        
            float newZ = Mathf.Lerp(startZ, targetZ, t);
            _cameraLocalPosition.z = newZ;
            camera.localPosition = _cameraLocalPosition;

            yield return null;
        }

        // Snap to target just in case
        _cameraLocalPosition.z = targetZ;
        camera.localPosition = _cameraLocalPosition;
    }
    
}
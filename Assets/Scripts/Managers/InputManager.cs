using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager GetInstance => instance;

    public PlayerControls playerControls;
    
    public static event Action OnHoldingRightMouse;
    public static event Action OnClickLeftMouse;
    public static event Action<float> OnScroll;
    
    private bool _isHoldingRightMouseDown;
    private float _scroll;
    
    public InputAction _mouseWheel;
    
    
    private bool _isMouseOverUI;

    public bool IsMouseOverUI => _isMouseOverUI;
    
    
    private void Awake()
    {
        playerControls = new PlayerControls();
        Init();
    }
    public void Init()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
            return;
        }
    }
    
    ///Try to place it on the new unity input system
    private void Update()
    {
        _isHoldingRightMouseDown = Input.GetMouseButton(1);
        
        if (_isHoldingRightMouseDown)
            OnHoldingRightMouse?.Invoke();

        HandleMouseLeftClick();
    }


    private void HandleMouseLeftClick()
    {
        ///Check only for targetable for now
        if (Input.GetMouseButtonDown(0))
        {
            OnClickLeftMouse?.Invoke();
        }
        
        ///need to test with ui
        
    }
    
    
    
    private void OnEnable()
    {
        _mouseWheel = playerControls.Player.MouseWheel;
        
        playerControls.Enable();
        _mouseWheel.Enable();

        _mouseWheel.performed += OnMouseWheelScroll;
    }
    private void OnMouseWheelScroll(InputAction.CallbackContext context)
    {
        Vector2 scroll = context.action.ReadValue<Vector2>();
        OnScroll?.Invoke(scroll.y);
    }
    private void OnDisable()
    {
        playerControls.Disable();
        _mouseWheel.Disable();
    }

}
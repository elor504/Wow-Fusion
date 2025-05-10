using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager GetInstance => instance;

    public PlayerControls playerControls;
    
    public static event Action OnHoldingRightMouse;
    public static event Action OnClickLeftMouse;
    public static event Action<float> OnScroll;
    
    public static event Action<Vector2> OnMovementInput;
    public static event Action<Vector2> OnStartedMovingInput;
    
    
    
    private bool _isHoldingRightMouseDown;
    private float _scroll;
    
    private List<HotKey> _hotKeysList = new List<HotKey>();


    public InputAction Movement;
    public InputAction MouseWheel;
    public InputAction HotKeys;
    
    
    private bool _isMouseOverUI;
    public bool IsMouseOverUI => _isMouseOverUI;



    [Header("Testing")] 
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private BasicEnemy enemy;
    [SerializeField] private ProjectileSpellData spellToTest;
    private ProjectileSpell projectileToTest;
    [SerializeField] private StatBuffData selfBuffDataToTest;
    [SerializeField] private SelfBuffSpell selfBuffToTest;
    
    private void Awake()
    {
        playerControls = new PlayerControls();
        Init();

        projectileToTest = spellToTest.GetSpell() as ProjectileSpell;
        selfBuffToTest = selfBuffDataToTest.GetSpell() as SelfBuffSpell;
        
        _hotKeysList.Add(new HotKey("1"));
        _hotKeysList[0].AddHotkeyable(projectileToTest.SpellID,Attack);
        _hotKeysList.Add(new HotKey("2"));
        _hotKeysList[1].AddHotkeyable(projectileToTest.SpellID,SelfCast);
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
        OnMovementInput?.Invoke( Movement.ReadValue<Vector2>());
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
    
    private void OnClickedHotKey(InputAction.CallbackContext context)
    {
        var key = context.control.name;
        switch (key)
        {
            case "1":
                GetHotKey("1")?.Press();
                break;
            case "2":
                GetHotKey("2")?.Press();
                break;
            case "3":
                break;
            case "4":
                break;
        }
    }

    public void Attack()
    {
        if (TargetManager.Instance.CurrentTarget != null)
        {
            playerCharacter.CastSpell(projectileToTest, TargetManager.Instance.CurrentTarget);
        }
        else
        {
            Debug.Log("No target");
        }
    }

    public void SelfCast()
    {
        playerCharacter.CastSpell(selfBuffToTest, null);
    }
    
    private HotKey GetHotKey(string key)
    {
        return _hotKeysList.Find(hotKey => hotKey.HotKeyID == key);
    }
    
    private void OnMouseWheelScroll(InputAction.CallbackContext context)
    {
        Vector2 scroll = context.action.ReadValue<Vector2>();
        OnScroll?.Invoke(scroll.y);
    }

    private void MovementInput(InputAction.CallbackContext context)
    {
        Vector2 movementInput = context.ReadValue<Vector2>();
        OnMovementInput.Invoke(movementInput);
    }

    private void DetectMovementInput(InputAction.CallbackContext context)
    {
        Vector2 movementInput = context.ReadValue<Vector2>();
        OnStartedMovingInput?.Invoke(movementInput);
    }
    private void OnEnable()
    {
        Movement = playerControls.Player.Move;
        MouseWheel = playerControls.Player.MouseWheel;
        HotKeys = playerControls.Player.NumKeys;

        playerControls.Enable();
        Movement.Enable();
        MouseWheel.Enable();

        Movement.performed += DetectMovementInput;
        MouseWheel.performed += OnMouseWheelScroll;
        HotKeys.performed += OnClickedHotKey;

    }
    private void OnDisable()
    {
        playerControls.Disable();
        MouseWheel.Disable();
        HotKeys.Disable();
        
        Movement.performed -= DetectMovementInput;
        MouseWheel.performed -= OnMouseWheelScroll;
        HotKeys.performed -= OnClickedHotKey;
    }

    public void OnOnMouseOnUI(bool value)
    {
        _isMouseOverUI = value;
    }
}


public class HotKey
{
    private string _hotKeyID;
    public string HotKeyID => _hotKeyID;
    public event Action OnPressed;

    private string _hotKeyableID;
    
    
    public HotKey(string hotKeyID)
    {
        _hotKeyID = hotKeyID;
    }
    
    public void Press()
    {
        OnPressed?.Invoke();
    }


    public void AddHotkeyable(string hotKeyableID,Action action)
    {
        _hotKeyableID = hotKeyableID;
        OnPressed += action;
    }

    public void ClearHotkeyable()
    {
        _hotKeyableID = "";
        OnPressed = null;
    }

}
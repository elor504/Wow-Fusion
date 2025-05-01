using System;
using System.Collections.Generic;
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
    
    private List<HotKey> _hotKeysList = new List<HotKey>();
    
    
    public InputAction _mouseWheel;
    public InputAction _hotKeys;
    
    
    private bool _isMouseOverUI;

    public bool IsMouseOverUI => _isMouseOverUI;



    [Header("Testing")] 
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private BasicEnemy enemy;
    [SerializeField] private ProjectileSpellData spellToTest;
    private ProjectileSpell projectileToTest;
    
    private void Awake()
    {
        playerControls = new PlayerControls();
        Init();

        projectileToTest = spellToTest.GetSpell() as ProjectileSpell;
        
        _hotKeysList.Add(new HotKey("1"));
        _hotKeysList[0].AddHotkeyable(projectileToTest.SpellID,Attack);
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
    
    private void OnClickedHotKey(InputAction.CallbackContext context)
    {
        var key = context.control.name;
        switch (key)
        {
            case "1":
                GetHotKey("1")?.Press();
                break;
            case "2":
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
    
    private HotKey GetHotKey(string key)
    {
        return _hotKeysList.Find(hotKey => hotKey.HotKeyID == key);
    }
    
    private void OnMouseWheelScroll(InputAction.CallbackContext context)
    {
        Vector2 scroll = context.action.ReadValue<Vector2>();
        OnScroll?.Invoke(scroll.y);
    }
    
    private void OnEnable()
    {
        _mouseWheel = playerControls.Player.MouseWheel;
        _hotKeys = playerControls.Player.NumKeys;
        
        playerControls.Enable();
        _mouseWheel.Enable();

        _mouseWheel.performed += OnMouseWheelScroll;
        _hotKeys.performed += OnClickedHotKey;
        
    }
    private void OnDisable()
    {
        playerControls.Disable();
        _mouseWheel.Disable();
        _hotKeys.Disable();
        
        _mouseWheel.performed -= OnMouseWheelScroll;
        _hotKeys.performed -= OnClickedHotKey;
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
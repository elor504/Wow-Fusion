using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : NetworkBehaviour, INetworkRunnerCallbacks
{
    public PlayerMovement playerMovement;
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

    private bool _denyInput;


    [Header("Testing")]
    [SerializeField] private ProjectileSpellData spellToTest;
    private ProjectileSpell projectileToTest;
    [SerializeField] private StatBuffData selfBuffDataToTest;
    [SerializeField] private SelfBuffSpell selfBuffToTest;


    ///Try to place it on the new unity input system
    private void Update()
    {
        if (GameTest.LocalCharacter == null || !GameTest.LocalCharacter.HasInputAuthority || _denyInput)
            return;

        _isHoldingRightMouseDown = Input.GetMouseButton(1);

        if (_isHoldingRightMouseDown)
        {
            OnHoldingRightMouse?.Invoke();
        }

        HandleMouseLeftClick();
    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out PlayerInputStruct input))
        {
            _isHoldingRightMouseDown = input.MouseRightClick;
            OnMovementInput?.Invoke(input.MovementInput);


            if (input.MovementInput != Vector2.zero)
                Debug.Log($"[InputManager] Movement Input: {input.MovementInput}");
        }

    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var clientInput = new PlayerInputStruct();

        Vector2 movementInput = _isHoldingRightMouseDown? GetMovementRelativeToCamera(Movement.ReadValue<Vector2>()) : Movement.ReadValue<Vector2>();

        clientInput.MovementInput = movementInput;
        clientInput.MouseRightClick = _isHoldingRightMouseDown;
        input.Set(clientInput);

        _isHoldingRightMouseDown = false;

        Debug.Log("[Client] Input sent");
    }

    private Vector2 GetMovementRelativeToCamera(Vector2 movementInput)
    {
        Vector3 Foward = PlayerCamera.Instance.Foward;
        Foward.y = 0;
        Foward.Normalize();
        Vector3 Right = PlayerCamera.Instance.Right;
        Right.y = 0;
        Right.Normalize();

        Vector3 fowardRelativeVerticalInput = movementInput.y * Foward;
        Vector3 rightRelativeVerticalInput = movementInput.x * Right;
        Vector3 cameraRelativeMovement = fowardRelativeVerticalInput + rightRelativeVerticalInput;
        return cameraRelativeMovement;
    }

    public override void Spawned()
    {
        base.Spawned();
        playerControls = new PlayerControls();
        projectileToTest = spellToTest.GetSpell() as ProjectileSpell;
        selfBuffToTest = selfBuffDataToTest.GetSpell() as SelfBuffSpell;

        _hotKeysList.Add(new HotKey("1"));
        _hotKeysList[0].AddHotkeyable(projectileToTest.SpellID, Attack);
        _hotKeysList.Add(new HotKey("2"));
        _hotKeysList[1].AddHotkeyable(projectileToTest.SpellID, SelfCast);
        Debug.Log("Spawned input manager");
        GameTest.AddCallBacks(this);
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
        if (GameManager.Instance.TargetManager.CurrentTarget != null)
        {
            GameTest.LocalCharacter.CastSpell(projectileToTest, GameManager.Instance.TargetManager.CurrentTarget);
        }
        else
        {
            Debug.Log("No target");
        }
    }

    public void SelfCast()
    {
        GameTest.LocalCharacter.CastSpell(selfBuffToTest, null);
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
        if (playerControls == null)
            playerControls = new PlayerControls();
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
    /// <summary>
    /// Incase we want to disable the player moving in the map and doing stuff example for loading scenes and ETC
    /// </summary>
    public void EnableDenyInput()
    {
        _denyInput = true;
    }
    public void DisableDenyInput()
    {
        _denyInput = false;
    }






    #region unused
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {

    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {

    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {

    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {

    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnConnectedToServer(NetworkRunner runner)
    {

    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {

    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }
    #endregion

}
public struct PlayerInputStruct : INetworkInput
{
    public Vector2 MovementInput;
    public float RotationInput;
    public bool MouseRightClick;
    public Vector3 CharacterFoward;
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


    public void AddHotkeyable(string hotKeyableID, Action action)
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
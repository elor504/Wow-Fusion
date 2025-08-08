using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : NetworkBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private PlayerCharacter character;
    public PlayerMovement playerMovement;
    public PlayerControls playerControls;

    public static event Action OnHoldingRightMouse;
    public static event Action OnHoldingLeftMouseOnEmpty;
    public static event Action OnClickLeftMouse;
    public static event Action<float> OnScroll;

    public static event Action<int, float> OnRotateCharacterInput;
    public static event Action<Vector2> OnMovementInput;
    public static event Action<Vector3> OnMovementDirection;
    public static event Action<Vector2> OnStartedMovingInput;

    private bool _pressedHotKeyOne;
    private bool _pressedHotKeyTwo;

    private bool _isHoldingRightMouseDown;
    private bool _isHoldingLeftMouseDown;
    private float _scroll;
    private int _characterRotationInput;



    private List<HotKey> _hotKeysList = new List<HotKey>();

    public InputAction Movement;
    public InputAction MouseWheel;
    public InputAction HotKeys;

    private bool _isMouseOverUI;
    public bool IsMouseOverUI => _isMouseOverUI;
    public bool PressedHotKeyOne => _pressedHotKeyOne;
    public bool PressedHotKeyTwo => _pressedHotKeyTwo;

    private bool _denyInput;

    [Header("Testing")]
    [SerializeField] private ProjectileSpellData spellToTest;
    //TODO: move into the combat class
    private ProjectileSpell projectileToTest;
    public ProjectileSpell ProjectileToTest => projectileToTest;
    [SerializeField] private StatBuffData selfBuffDataToTest;
    [SerializeField] private SelfBuffSpell selfBuffToTest;

    public override void FixedUpdateNetwork()
    {

        if (GetInput(out PlayerInputStruct input) && Object.HasStateAuthority)
        {
            _isHoldingRightMouseDown = input.MouseRightClick;
            Debug.Log($"[InputManager] Movement Input: {input.MovementInput}");
            OnMovementInput?.Invoke(input.MovementInput);
            OnMovementDirection?.Invoke(input.MovementDirection);
            if (input.RotationInput != 0)
                OnRotateCharacterInput?.Invoke(input.RotationInput, Object.Runner.DeltaTime);

            if (_isHoldingRightMouseDown)
                playerMovement.Rotate(input.CharacterFoward);

            if (input.PressedHotKeyOne)
            {
                //GetHotKey("1")?.Press();
            }
            if (input.PressedHotKeyTwo)
            {
                GetHotKey("2")?.Press();
            }
        }
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {


        var clientInput = new PlayerInputStruct();
        Vector3 movementInput = Vector3.zero;

        movementInput = TranslatePlayerInputRelatedToPlayer(Movement.ReadValue<Vector2>());

        clientInput.MovementInput = Movement.ReadValue<Vector2>();
        clientInput.MovementDirection = movementInput;

        clientInput.MouseRightClick = _isHoldingRightMouseDown;
        var foward = PlayerCamera.Instance.Foward;
        foward.y = 0;
        clientInput.CharacterFoward = foward;

        clientInput.RotationInput = _characterRotationInput;

        clientInput.PressedHotKeyOne = _pressedHotKeyOne;
        clientInput.PressedHotKeyTwo = _pressedHotKeyTwo;



        input.Set(clientInput);

        _isHoldingRightMouseDown = false;
        _pressedHotKeyOne = false;
        _pressedHotKeyTwo = false;
    }

    private void Update()
    {
        if (GameTest.LocalCharacter == null || !GameTest.LocalCharacter.HasInputAuthority || _denyInput)
            return;

        HandleMouseRightClick();
        HandleMouseLeftClick();
        HandleCharacterRotationInput();
    }

    private void HandleCharacterRotationInput()
    {
        //Think of a better way ><
        if (Input.GetKey(KeyCode.E))
        {
            _characterRotationInput = 1;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            _characterRotationInput = -1;
        }
        else if (_characterRotationInput != 0)
        {
            _characterRotationInput = 0;
        }
    }

    public Vector3 TranslatePlayerInputRelatedToPlayer(Vector2 playerInput)
    {
        float verticalInput = playerInput.y;
        float horizontalInput = playerInput.x;

        Vector3 fwd = transform.forward;
        Vector3 right = transform.right;
        Vector3 move = fwd * verticalInput + right * horizontalInput;

        return move.normalized;
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
        //TODO: make a way to skip a frame check to prevent holding if i only want to click
        _isHoldingLeftMouseDown = Input.GetMouseButton(0);
        if (_isHoldingLeftMouseDown && !_isMouseOverUI)
        {
            OnHoldingLeftMouseOnEmpty.Invoke();
        }

        if (Input.GetMouseButtonDown(0))
        {
            OnClickLeftMouse?.Invoke();
        }
    }
    private void HandleMouseRightClick()
    {
        _isHoldingRightMouseDown = Input.GetMouseButton(1);
        if (_isHoldingRightMouseDown)
        {
            OnHoldingRightMouse?.Invoke();
        }
    }


    private void OnClickedHotKey(InputAction.CallbackContext context)
    {
        var key = context.control.name;
        //_targetObjectNetworkID = GameManager.Instance.TargetManager.CurrentTarget.GetNetworkID();
        switch (key)
        {
            case "1":
                _pressedHotKeyOne = true;
                //GetHotKey("1")?.Press();
                break;
            case "2":
                _pressedHotKeyTwo = true;
                //GetHotKey("2")?.Press();
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
            RPC_Attack(GameManager.Instance.TargetManager.CurrentTarget.GetNetworkId());
            //character.CastSpell(projectileToTest, GameManager.Instance.TargetManager.CurrentTarget);
        }
        else
        {
            Debug.Log("No target");
        }
    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_Attack(NetworkId networkID, RpcSources source = default)
    {
        var target = ServerHandler.Instance.GetEnemyByNetworkID(networkID, Object.Runner);
        character.CastSpell(projectileToTest, target);
    }

    public void SelfCast()
    {
        GameTest.LocalCharacter.CastSpell(selfBuffToTest, null);
    }
    public HotKey GetHotKey(string key)
    {
        return _hotKeysList.Find(hotKey => hotKey.HotKeyID == key);
    }

    private void OnMouseWheelScroll(InputAction.CallbackContext context)
    {
        Vector2 scroll = context.action.ReadValue<Vector2>();
        OnScroll?.Invoke(scroll.y);
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
    public Vector3 MovementDirection;
    public int RotationInput;
    public bool MouseRightClick;
    public Vector3 CharacterFoward;

    public bool PressedHotKeyOne;
    public bool PressedHotKeyTwo;

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
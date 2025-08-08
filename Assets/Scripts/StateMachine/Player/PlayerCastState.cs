using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCastState : BaseState
{
    private PlayerBrain _playerBrain;
    private EntityStat _characterStat;
    private InputManager _inputManager;
    private PlayerAnimator _playerAnimator;
    private bool _finishedCasting;
    private bool _pressedMovement;
    private int _id;
    private float _castTime;


    private BaseSpell _spellToCast;
    private BasicVFX _handVFX;
    private ITargetableEntity _caster;
    private ITargetableEntity _target;

    private static readonly int Cast = Animator.StringToHash("Cast");
    private static readonly int StartCasting = Animator.StringToHash("StartCasting");

    private const string leftHandID = "LeftHand";
    private const string rightHandID = "RightHand";


    private List<Transform> _handVFXParents = new List<Transform>();
    private List<BasicVFX> _handVFXs = new List<BasicVFX>();

    public static event Action<float, float> StartCastHandler;
    public static event Action CastedHandler;
    public static event Action<float, float> UpdateCastingHandler;


    public PlayerCastState(int id, PlayerBrain playerBrain)
    {
        _playerBrain = playerBrain;
        _inputManager = _playerBrain.PlayerCharacter.InputManager;
        _playerAnimator = _playerBrain.PlayerCharacter.GetAnimator;
        _id = id;
        _characterStat = _playerBrain.PlayerCharacter.CharacterStat;
        AttemptToAddHandsToVFXList();
    }

    public void SetSpellToCast(BaseSpell spellToCast, ITargetableEntity caster, ITargetableEntity target)
    {
        _spellToCast = spellToCast;
        _caster = caster;
        _target = target;
        _handVFX = _spellToCast.HandsSpellVFX;
        _castTime = _spellToCast.TimeToCast;
    }

    public override void EnterState()
    {
        _inputManager.OnStartedMovingInput += ListenToMovementInput;

        _finishedCasting = false;
        Debug.Log("Entering Cast State");
        TryToAddHandVFX();

        _playerAnimator.SetBool(Cast, true);
        _playerAnimator.SetTrigger(StartCasting);

        StartCastHandler?.Invoke(_castTime, _castTime);
    }

    public override void ExitState()
    {
        _inputManager.OnStartedMovingInput -= ListenToMovementInput;
        if (_finishedCasting)
        {
            //Cast spell
            _spellToCast.CastSkill(_caster, _target);
            var manaCost = _spellToCast.ManaCost;
            _characterStat.UseMana(manaCost);
            _spellToCast = null;
            Debug.Log("Casted Spell!");
        }

        foreach (var handVFX in _handVFXs)
        {
            handVFX.StopParticleSystem();
        }
        _handVFXs.Clear();
        _playerAnimator.SetBool(Cast, false);

        _pressedMovement = false;

        CastedHandler?.Invoke();
        Debug.Log("Exiting Cast State");
    }

    public override void UpdateState(float deltaTime)
    {

    }



    public override void FixedUpdateState(float fixedDeltaTime)
    {
        if (_pressedMovement)
        {
            _finishedCasting = false;
            _playerBrain.PlayerCharacter.CurrentState = (int)PlayerStates.Idle;
            return;
        }


        _castTime -= fixedDeltaTime;
        UpdateCastingHandler?.Invoke(_castTime, _spellToCast.TimeToCast);
        if (_castTime <= 0)
        {
            _castTime = 0;

            _finishedCasting = true;
            _playerBrain.PlayerCharacter.CurrentState = (int)PlayerStates.Idle;
            return;
        }
    }

    public override bool CompareID(int id)
    {
        return _id == id;
    }

    private void ListenToMovementInput(Vector2 input)
    {
        if (input != Vector2.zero)
            _pressedMovement = true;
    }

    private void AttemptToAddHandsToVFXList()
    {
        if (_playerBrain.PlayerCharacter.CharacterVFXVisual.TryGetVisualPositionParent(leftHandID, out var leftHand))
        {
            _handVFXParents.Add(leftHand);
        }
        if (_playerBrain.PlayerCharacter.CharacterVFXVisual.TryGetVisualPositionParent(rightHandID, out var rightHand))
        {
            _handVFXParents.Add(rightHand);
        }
    }

    private bool TryToAddHandVFX()
    {
        //TODO Server spawn vfx locally
        _handVFXs.Clear();

        if (_handVFX == null)
        {
            if (_spellToCast != null)
                Debug.LogError($"Hand VFX is null with the spell: {_spellToCast.SpellID}");
            return false;
        }

        foreach (var hand in _handVFXParents)
        {
            Debug.Log("Hand " + hand);
            var vfx = VFXPoolSystem.Instance.GetAvailableObjectFromPool(_handVFX, hand.position);
            vfx.SetParent(hand);
            vfx.InitVFX(hand.position);
            vfx.gameObject.SetActive(true);
            _handVFXs.Add(vfx);
        }

        return true;
    }

}
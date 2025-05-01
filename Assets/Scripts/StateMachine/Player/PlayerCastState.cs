using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerCastState : BaseState
{
    private PlayerBrain _playerBrain;
    private int _id;
    private bool _finishedCasting;
    private float _castTime;


    private BaseSpell _spellToCast;
    private BasicVFX _handVFX;
    private ITargetableEntity _caster;
    private ITargetableEntity _target;
    
    private static readonly int Cast = Animator.StringToHash("Cast");
    private static readonly int StartCasting = Animator.StringToHash("StartCasting");

    private const string leftHandID = "LeftHand";
    private const string rightHandID = "RightHand";
    

    private List<Transform> _handVFXParents = new List<Transform>() ;
    private List<BasicVFX> _handVFXs = new List<BasicVFX>();
    
    public PlayerCastState(int id, PlayerBrain playerBrain)
    {
        _playerBrain = playerBrain;
        _id = id;
        AttemptToAddHandsToVFXList();
    }

    public void SetSpellToCast(BaseSpell spellToCast,ITargetableEntity caster,ITargetableEntity target)
    {
        _spellToCast = spellToCast;
        _caster = caster;
        _target = target;
        _handVFX = _spellToCast.HandsSpellVFX;
        _castTime = _spellToCast.TimeToCast;
    }
    
    public override void EnterState()
    {
        _finishedCasting = false;
        Debug.Log("Entering Cast State");
        TryToAddHandVFX();
        
        _playerBrain.PlayerCharacter.GetAnimator.SetBool(Cast,true);
        _playerBrain.PlayerCharacter.GetAnimator.SetTrigger(StartCasting);
    }

    public override void ExitState()
    {
        if (_finishedCasting)
        {
            //Cast spell
            _spellToCast.CastSkill(_caster,_target);
            _spellToCast = null;
            Debug.Log("Casted Spell!");
        }

        foreach (var handVFX in _handVFXs)
        {
            handVFX.StopParticleSystem();
        }
        _handVFXs.Clear();
        _playerBrain.PlayerCharacter.GetAnimator.SetBool(Cast,false);
        Debug.Log("Exiting Cast State");
    }

    public override void UpdateState(float deltaTime)
    {
        _castTime -= Time.deltaTime;
        if (_castTime <= 0)
        {
            _finishedCasting = true;
            _playerBrain.ChangeState((int) PlayerStates.Idle);
            return;
        }
    }

    public override void FixedUpdateState(float fixedDeltaTime)
    {
    }

    public override bool CompareID(int id)
    {
        return _id == id;
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
        _handVFXs.Clear();
        
        if (_handVFX == null)
        {
            Debug.LogError($"Hand VFX is null with the spell: {_spellToCast.SpellID}");
            return false;
        }

        foreach (var hand in _handVFXParents)
        {
            Debug.Log("Hand " + hand);
            var vfx = VFXPoolSystem.Instance.GetAvailableObjectFromPool(_handVFX,hand.position);
            vfx.SetParent(hand);
            vfx.InitVFX(hand.position);
            vfx.gameObject.SetActive(true);
            _handVFXs.Add(vfx);
        }

        return true;
    }
    
}
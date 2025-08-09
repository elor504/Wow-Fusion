using Fusion;
using System;
using TMPro;
using UnityEngine;

public class PlayerCharacter : NetworkBehaviour, ITargetableEntity
{
    [Header("References")]
    [SerializeField] private PlayerAnimator animator;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private CharacterVFXVisual characterVFXVisual;
    [SerializeField] private CharacterClass characterClass;
    [SerializeField] private EntityStat characterStat;
    [SerializeField] private PlayerEquipment equipment;
    [SerializeField] private PlayerCamera characterCamera;
    [SerializeField] private NetworkObject networkObject;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private TextMeshPro nickNameText;
    [Header("Transform references")]
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Transform hitPosition;

    [Header("Temp references")]
    [SerializeField] private BaseClassData baseClassData;

    private NetworkRunner _myRunner;
    private PlayerBrain _playerBrain;
    private CharacterData _characterData;



    [Networked,OnChangedRender(nameof(CharacterNicknameChange))]
    public string CharacterName { get; set; }





    private void CharacterNicknameChange()
    {
        gameObject.name = $"Player: {CharacterName}";
    }

    public PlayerAnimator GetAnimator => animator;
    public PlayerMovement GetMovement => movement;
    public CharacterVFXVisual CharacterVFXVisual => characterVFXVisual;
    public EntityStat CharacterStat => characterStat;
    public PlayerCamera CharacterCamera => characterCamera;
    public NetworkObject NetworkObject => networkObject;
    public InputManager InputManager => inputManager;
    public PlayerBrain GetBrain => _playerBrain;


    [Networked,OnChangedRender(nameof(ChangedState))]
    public int CurrentState { get; set; }


    public override void Spawned()
    {
        base.Spawned();
        InitPlayer();
        _characterData = new CharacterData();
        nickNameText.text = CharacterName;
        if (Object.HasInputAuthority)
        {
            GameTest.LocalCharacter = this;
            gameObject.tag = TargetManager.MY_PLAYER_TAG;
            _myRunner = GameTest.GetMyRunner();
            CharacterNicknameChange();
        }
        else if(Object.HasStateAuthority)
        {

            _myRunner = Object.Runner;
        }
        else
        {
            //gameObject.tag = TargetManager.FRIENDLY_TAG;
        }
     
    }

    public void InitPlayer()
    {
        _playerBrain = new PlayerBrain();
        _playerBrain.InitBrain(this);

        characterClass.Init(baseClassData, characterStat);
        characterStat.Init(this, baseClassData.ClassBaseStats);
    }
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (!Object.HasStateAuthority)
            return;
        
          _playerBrain?.FixedUpdateState(_myRunner.DeltaTime);
        
    }
    private void Update()
    {
        _playerBrain?.UpdateState(Time.deltaTime);
    }

    private void ChangedState()
    {
        _playerBrain.ChangeState(CurrentState);
    }

    public void LoadCharacterData()
    {
        characterStat.Init(this, _characterData);
        equipment.Init(_characterData);

    }

    public void CastSpell(BaseSpell spell, ITargetableEntity target)
    {
        if (!_playerBrain.TryToCastSpell(spell, this, target))
        {
            ///need to make some of notification :O
            Debug.Log("Cannot cast spell");
        }
    }


    public void OnTargeted()
    {
    }
    public void OnStopTargeting()
    {
    }
    public void OnHovering()
    {
    }
    public void OnStoppedHovering()
    {
    }

    public void DealDamage(ITargetableEntity caster, int damage)
    {
        characterStat.DealDamage(damage);
    }
    public void Heal(ITargetableEntity caster)
    {

    }

    public bool CanBeTargeted()
    {
        return true;
    }
    public bool IsEnemy()
    {
        return false;
    }
    public bool IsAlly()
    {
        return true;
    }
    public bool CanCastSpell(int amount)
    {
        if (!characterStat.CanUseMana(amount))
            return false;

        // characterStat.UseMana(amount);
        return true;
    }

    public bool TryGetEntityStat(out EntityStat entityStat)
    {
        entityStat = null;
        if (characterStat)
        {
            entityStat = characterStat;
            return true;
        }

        return false;
    }
    public bool TryGetEntityVisualPosition(out CharacterVFXVisual vfxVisual)
    {
        vfxVisual = null;
        if (characterVFXVisual)
        {
            vfxVisual = characterVFXVisual;
            return true;
        }

        return false;
    }

    public int GetHealth()
    {
        return 0;
    }
    public int GetMana()
    {
        return 0;
    }

    public GameObject GetEntityGO()
    {
        return gameObject;
    }
    public Transform GetProjectileSpawnPosition()
    {
        return projectileSpawnPoint;
    }
    public Transform GetHitPosition()
    {
        return hitPosition;
    }

    #region nickname Update
    public void UpdateCharacterName(string nickname)
    {
        RPC_UpdateCharacterNickname(nickname);
    }
    [Rpc(RpcSources.InputAuthority,RpcTargets.StateAuthority)]
    public void RPC_UpdateCharacterNickname(string nickname)
    {
        CharacterName = nickname;
    }

    #endregion
    #region visual update
    public void UpdateCharacterVisualData(string characterData)
    {
        RPC_RequestCharacterVisualData(characterData);
    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_RequestCharacterVisualData(string serializedCharacterData)
    {
        SetCharacterVisualData(serializedCharacterData);
    }

    private void SetCharacterVisualData(string characterData)
    {
        RPC_UpdateCharacterVisualData(characterData);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_UpdateCharacterVisualData(string serializedData)
    {
        _characterData.DeserializeVisual(serializedData);
        equipment.Init(_characterData);
        //LoadCharacterData();
    }
    #endregion


    public NetworkId GetNetworkId()
    {
        return networkObject.Id;
    }

    public float ColliderSize()
    {
        //TODO: Check size with collider
        return 0.5f;
    }
}
using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [Header("Transform references")]
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Transform hitPosition;

    [Header("Temp references")]
    [SerializeField] private BaseClassData baseClassData;

    private NetworkRunner _myRunner;
    private PlayerBrain _playerBrain;


    public PlayerAnimator GetAnimator => animator;
    public PlayerMovement GetMovement => movement;
    public CharacterVFXVisual CharacterVFXVisual => characterVFXVisual;
    public EntityStat CharacterStat => characterStat;
    public PlayerCamera CharacterCamera => characterCamera;
    public NetworkObject NetworkObject => networkObject;
    public InputManager InputManager => inputManager;

    public override void Spawned()
    {
        base.Spawned();

        if (Object.HasInputAuthority)
        {
            InitPlayer();
            GameTest.LocalCharacter = this;
            gameObject.tag = TargetManager.MY_PLAYER_TAG;
            _myRunner = GameTest.GetMyRunner();
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
        if (Object.HasStateAuthority)
        {
            _playerBrain?.FixedUpdateState(_myRunner.DeltaTime);
        }
    }
    private void Update()
    {
        _playerBrain?.UpdateState(Time.deltaTime);
    }


    public void LoadCharacterData(CharacterData data)
    {
        characterStat.Init(this, data);
        equipment.Init(data);
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

    public NetworkId GetNetworkId()
    {
        return networkObject.Id;
    }
}
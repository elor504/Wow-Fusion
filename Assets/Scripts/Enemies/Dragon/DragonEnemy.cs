using Fusion;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragonEnemy : NetworkBehaviour, ITargetableEntity, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private EnemyHealth health;
    [SerializeField] private NetworkObject networkObject;
    [SerializeField] private SphereCollider sphereCollider;

    [Header("Targetable")]
    [SerializeField] private GameObject hoveringVisual;
    [SerializeField] private GameObject beingTargetedVisual;

    [Header("Visual Transform")]
    [SerializeField] private Transform hitPosition;
    [SerializeField] private Transform mouthPosition;

    public override void Spawned()
    {
        base.Spawned();
    }

    public bool CanBeTargeted()
    {
        return false;
    }

    public bool CanCastSpell(int amount)
    {
        return true;
    }

    public void DealDamage(ITargetableEntity caster, int damage)
    {
        if (Object.HasStateAuthority)
            health.RPC_DealDamage(damage);
    }

    public GameObject GetEntityGO()
    {
        return gameObject;
    }

    public int GetHealth()
    {
        throw new System.NotImplementedException();
    }

    public Transform GetHitPosition()
    {
        return hitPosition;
    }

    public int GetMana()
    {
        return 0;
    }

    public Transform GetProjectileSpawnPosition()
    {
        return mouthPosition;
    }

    public void Heal(ITargetableEntity caster)
    {

    }

    public void OnHovering()
    {
        hoveringVisual.SetActive(true);
    }
    public void OnStoppedHovering()
    {
        hoveringVisual.SetActive(false);
    }

    public void OnStopTargeting()
    {
        beingTargetedVisual.SetActive(false);
    }

    public void OnTargeted()
    {
        beingTargetedVisual.SetActive(true);
    }

    public bool IsAlly()
    {
        return false;
    }
    public bool IsEnemy()
    {
        return true;
    }

    public bool TryGetEntityStat(out EntityStat entityStat)
    {
        entityStat = null;
        return false;
    }
    public bool TryGetEntityVisualPosition(out CharacterVFXVisual vfxVisual)
    {
        vfxVisual = null;
        return false;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        TargetManager.SetCurrentHoveredEntity(this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        TargetManager.SetCurrentHoveredEntity(null);
    }

    public NetworkId GetNetworkId()
    {
        return networkObject.Id;
    }

    public float ColliderSize()
    {
        return sphereCollider.radius;
    }
}

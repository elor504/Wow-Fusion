using Fusion;
using System.Collections;
using UnityEngine;
namespace Homework
{
    public class MPProjectileHW : NetworkBehaviour
    {
        //idea for me not to forget, handle friendlies, enemies and yourself inside a layer ( at the real game not hw)
        [SerializeField] private NetworkObject obj;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private GameObject projectileVisual;

        [SerializeField] private float movementSpeed;
        [SerializeField] private float lifeTime;

        [Header("Hit PF")]
        [SerializeField] private HitVFXHW hitVFX;

        public bool IsProjectileActive { get; set; }

        public GameObject shooterGO { set; get; }

        public bool IsMoving { set; get; }
        public float LifeCounter { set; get; }

        private Vector3 _direction;
        private PlayerRef _shooter;

        private bool _initialized;


        public void Shoot(GameObject shooterGO, PlayerRef shooter, Vector3 direction)
        {
            LifeCounter = lifeTime;
            IsMoving = true;
            _shooter = shooter;
            _direction = direction;
            this.shooterGO = shooterGO;
        }

        public override void Spawned()
        {
            base.Spawned();
            if (Object.HasStateAuthority)
            {
                _initialized = true;
                IsProjectileActive = true;
            }
        }

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();

            LifeCounter -= Runner.DeltaTime;
            if (IsProjectileActive && LifeCounter <= 0)
            {
                IsProjectileActive = false;
                RPC_SpawnHitVFX();
                projectileVisual.SetActive(false);
                return;
            }

            if (!Object.HasStateAuthority)
                return;
            if (!IsProjectileActive)
                return;
            rb.position += _direction * movementSpeed * Runner.DeltaTime;
        }

        private void SpawnHitVFX()
        {
            Instantiate(hitVFX, transform.position, Quaternion.identity);
        }
        [Rpc(RpcSources.StateAuthority,RpcTargets.All)]
        private void RPC_SpawnHitVFX()
        {
            SpawnHitVFX();
            projectileVisual.SetActive(false);
            IsProjectileActive = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_initialized || !IsMoving)
                return;
            if (other.gameObject.CompareTag("Player"))
            {
                HandleHitPlayer(other);
            }
            else
            {
                Debug.Log("[Projectile] Hit but not a player");
                IsProjectileActive = false;
                if (!HasStateAuthority)
                {
                    return;
                }
                StartCoroutine(HandleHitDelay());
            }
        }
        private IEnumerator HandleHitDelay()
        {
            RPC_SpawnHitVFX();
            yield return new WaitForSeconds(3f);

            if (HasStateAuthority)
            {
                Runner.Despawn(obj);
            }
        }

        private void HandleHitPlayer(Collider other)
        {
            var characterHeatlth = other.GetComponent<CharacterHealthHW>();
            if (characterHeatlth)
            {
                if (other.gameObject != shooterGO)
                {
                    //RPC_SpawnHitVFX();
                    if (HasStateAuthority)
                    {
                        characterHeatlth.RPC_DealDamage(1, GameManagerHW.CharacterSelectionManager.GetCharacterSelectionIndexByPlayerRef(GameTest.GetMyRunner().LocalPlayer));
                        StartCoroutine(HandleHitDelay());
                       // Runner.Despawn(obj);
                    }
                }

            }
        }
    }

}
using Fusion;
using UnityEngine;
namespace Homework
{
    public class MPProjectileHW : NetworkBehaviour
    {
        //idea for me not to forget, handle friendlies, enemies and yourself inside a layer ( at the real game not hw)
        [SerializeField] private NetworkObject obj;
        [SerializeField] private Rigidbody rb;

        [SerializeField] private float movementSpeed;
        [SerializeField] private float lifeTime;


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
            }
        }

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();

            LifeCounter -= Runner.DeltaTime;
            if (LifeCounter <= 0)
            {
                IsProjectileActive = false;
                return;
            }

            if (!Object.HasStateAuthority)
                return;

            rb.position += _direction * movementSpeed * Runner.DeltaTime;
        }

        public void SetProjectileActive()
        {
            gameObject.SetActive(IsProjectileActive);

            if (!IsProjectileActive)
            {
                IsMoving = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_initialized || !IsMoving)
                return;
            if (!other.gameObject.CompareTag("Player"))
                return;


            var characterHeatlth = other.GetComponent<CharacterHealthHW>();
            if (characterHeatlth)
            {
                if (other.gameObject != shooterGO)
                {
                    if (HasStateAuthority)
                    {
                        characterHeatlth.RPC_DealDamage(1);
                        Runner.Despawn(obj);
                    }
                }

            }
            else
            {
                Debug.Log("[Projectile] Hit but not a player");
                IsProjectileActive = false;
                //Handle hitting wall, floor ETC
            }
        }
    }

}
using Fusion;
using UnityEngine;
namespace Homework
{
    public class MPProjectileHW : NetworkBehaviour
    {
        [SerializeField] private NetworkObject obj;
        [SerializeField] private Rigidbody rb;

        [SerializeField] private float movementSpeed;
        [SerializeField] private float lifeTime;
        public GameObject shooterGO { set; get; }

        [Networked]
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

            if (!_initialized)
                return;
            LifeCounter -= Runner.DeltaTime;
            if(LifeCounter <= 0)
            {
                Runner.Despawn(obj);
                return;
            }
            rb.position += _direction * movementSpeed * Runner.DeltaTime;
        }



        private void OnTriggerEnter(Collider other)
        {
            if (!_initialized || !IsMoving)
                return;

            var characterInput = other.GetComponent<CharacterInputHW>();
            if (characterInput)
            {
                var hitPlayer = characterInput.Runner.LocalPlayer;

                if (other.GetComponent<CharacterInputHW>().Runner != Object.Runner)
                {
                    Debug.Log($"[Projectile] Hit: {PlayerList.Instance.GetPlayerName(hitPlayer)}");
                    Runner.Despawn(obj);
                }
                else if (other.GetComponent<CharacterInputHW>().Runner.LocalPlayer == _shooter)
                {
                    Debug.Log("[Projectile] Trigger When entering its own shooter");
                }
            }
            else
            {
                Debug.Log("[Projectile] Hit but not a player");
                Runner.Despawn(obj);
                //Handle hitting wall, floor ETC
            }
        }
    }

}
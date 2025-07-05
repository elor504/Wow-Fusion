using Fusion;
using System.Collections.Generic;
using UnityEngine;

namespace Homework
{
    public class CharacterShootHW : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform shootPivot;
        [SerializeField] private CharacterInputHW characterInputHW;
        [Header("Shooting Settings")]
        [SerializeField] private MPProjectileHW projectilePref;





        private void Awake()
        {
            characterInputHW.ShootAction += Shoot;
        }

        private void Shoot(PlayerRef playerRef)
        {
            Vector3 directionToShoot = Camera.main.transform.forward;
            var projectile = Runner.Spawn(projectilePref, shootPivot.position, Quaternion.Euler(directionToShoot), playerRef);
            projectile.Shoot(gameObject, playerRef, directionToShoot);
        }


     
    }
}
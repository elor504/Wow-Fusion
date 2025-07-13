using System.Collections;
using UnityEngine;
namespace Homework
{
    public class HitVFXHW : MonoBehaviour
    {
        [SerializeField] private ParticleSystem ps;



        public void Init()
        {
            StartCoroutine(nameof(CheckIfParticleStopped));
        }

        private IEnumerator CheckIfParticleStopped()
        {
            while (ps.IsAlive())
            {
                yield return null;
            }
            Destroy(gameObject);
        }
    }

}
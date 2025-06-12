using Fusion;
using System.Collections.Generic;
using UnityEngine;
namespace Homework
{
    public class PlayerSpawnManager : MonoBehaviour
    {
        [SerializeField] private int spawnAmount = 10;
        [SerializeField] private float radius;

        private List<Vector3> _spawnPositions;

        //i prefered to use dictionary instead of holding bool inside another monobehaviour script so the manager will have 100% management inside of him
        private Dictionary<Vector3, PlayerRef> _playerSpawnPosition;

        private void Awake()
        {
            GenerateSpawnPositions();
        }
        private List<Vector3> GenerateSpawnPositions()//Chatgpt ><
        {
            List<Vector3> positions = new List<Vector3>();

            for (int i = 0; i < spawnAmount; i++)
            {
                float angle = i * Mathf.PI * 2f / spawnAmount;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                positions.Add(new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z));
            }

            return positions;
        }
        public List<Vector3> GenerateSpawnPositions(int amount,float radius)
        {
            List<Vector3> positions = new List<Vector3>();

            for (int i = 0; i < amount; i++)
            {
                float angle = i * Mathf.PI * 2f / amount;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                positions.Add(new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z));
            }

            return positions;
        }
        public bool TryGetSpawnPosition(PlayerRef playerRef, out Vector3 spawnPosition)
        {
            List<Vector3> positions = _spawnPositions;

            foreach (var position in positions)
            {
                if (_playerSpawnPosition.ContainsKey(position))
                {
                    positions.Remove(position);
                }
            }

            if (positions.Count == 0)
            {
                spawnPosition = Vector3.zero;
                return false;
            }
            int randomizer = Random.Range(0, positions.Count);
            spawnPosition = positions[randomizer];
            _playerSpawnPosition[spawnPosition] = playerRef;
            return true;
        }


        #region gizmos


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            var positions = GenerateSpawnPositions();
            foreach (var position in positions)
            {
                Gizmos.DrawWireSphere(position, 2f);
            }
        }
        #endregion
    }
}
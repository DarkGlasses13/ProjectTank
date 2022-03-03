using UnityEngine;

namespace Assets.Scripts
{
    public class Actor : MonoBehaviour
    {
        private Transform _spawnPoint;

        public void SetSpawnPosition(Transform spawnPoint) => _spawnPoint = spawnPoint;

        public void SetActive()
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        public void Respawn()
        {
            transform.position = _spawnPoint.position;
            transform.rotation = _spawnPoint.rotation;
            SetActive();
        }
    }
}
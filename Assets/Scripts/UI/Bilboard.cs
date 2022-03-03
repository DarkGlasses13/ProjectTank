using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class Bilboard : MonoBehaviour
    {
        private Transform _playerCamera;

        [Inject] private void Construct(PlayerCamera playerCamera)
        {
            _playerCamera = playerCamera.GetComponentInChildren<Camera>().transform;
            RotateToPlayerCamera();
        }

        private void RotateToPlayerCamera() => transform.rotation = _playerCamera.rotation;
    }
}
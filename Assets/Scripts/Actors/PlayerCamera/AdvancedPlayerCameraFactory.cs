using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class AdvancedPlayerCameraFactory : IFactory<PlayerCamera, Transform, Transform, PlayerCamera>
    {
        private DiContainer _container;

        public AdvancedPlayerCameraFactory(DiContainer container) => _container = container;

        public PlayerCamera Create(PlayerCamera prefab, Transform parent, Transform target)
        {
            PlayerCamera camera = _container
                .InstantiatePrefabForComponent<PlayerCamera>(prefab.gameObject, parent);

            CameraTargetTracker targetTracker = camera.GetComponent<CameraTargetTracker>();
            targetTracker.SetTarget(target);
            _container.Bind<PlayerCamera>().FromInstance(camera).AsSingle();
            return camera;
        }
    }
}
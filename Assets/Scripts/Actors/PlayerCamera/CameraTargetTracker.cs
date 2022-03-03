using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class CameraTargetTracker : MonoBehaviour
    {
        [SerializeField] [Range(1, 10)] float _duration;

        private Transform _target;
        private UpdateService _updateService;
        private const float _xOffset = 0;

        [Inject] private void Construct(UpdateService updateService) => _updateService = updateService;

        private void OnEnable() => _updateService.OnFixedUpdate += Track;

        private void Track()
        {
            Vector3 targetPosition = new Vector3(_xOffset, _target.position.y, _target.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, _duration * Time.fixedDeltaTime);
        }

        public void SetTarget(Transform target) => _target = target;

        private void OnDisable() => _updateService.OnFixedUpdate -= Track;
    }
}
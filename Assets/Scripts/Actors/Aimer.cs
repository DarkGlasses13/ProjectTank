using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    [RequireComponent(typeof(TargetDetector))]
    public class Aimer : MonoBehaviour
    {
        [SerializeField] private Transform _rotatableFrame;
        [SerializeField] [Range(1, 10)] private float _aimSpeed;

        private UpdateService _updateService;
        private TargetDetector _targetDetector;

        [Inject] private void Construct(UpdateService updateService)
        {
            _updateService = updateService;
            _targetDetector = GetComponent<TargetDetector>();
        }

        private void OnEnable() => _updateService.OnFixedUpdate += Aim;

        private void Aim()
        {
            if (_targetDetector.Target != null)
            {
                Quaternion direction = Quaternion.LookRotation(_targetDetector.Target.position - transform.position);
                _rotatableFrame.rotation = Quaternion.Lerp(_rotatableFrame.rotation, direction, _aimSpeed * Time.fixedDeltaTime);
                return;
            }

            ResetRotation();
        }

        private void ResetRotation()
        {
            _rotatableFrame.localRotation = Quaternion
                .Lerp(_rotatableFrame.localRotation, Quaternion.identity, _aimSpeed * Time.fixedDeltaTime);
        }

        private void OnDisable() => _updateService.OnFixedUpdate -= Aim;
    }
}
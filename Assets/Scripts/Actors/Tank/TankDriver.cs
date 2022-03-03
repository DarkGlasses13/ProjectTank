using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class TankDriver : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;

        private UpdateService _updateService;
        private Controls _controls;
        private Vector3 _direction;

        [Inject] private void Construct(UpdateService updateService, Controls controls)
        {
            
            _updateService = updateService;
            _controls = controls;
        }

        private void OnEnable()
        {
            _controls.Enable();
            _controls.Tank.Motion.performed += callbackContext => SetDirection();
            _controls.Tank.Motion.canceled += callbackContext => ResetDirection();
            _updateService.OnFixedUpdate += Move;
        }

        private void Move()
        {
            if (_direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
                transform.Translate(Vector3.forward * _moveSpeed * Time.fixedDeltaTime);
            }
        }

        private void SetDirection()
        {
            Vector2 input = _controls.Tank.Motion.ReadValue<Vector2>();
            _direction = new Vector3(input.x, transform.position.y, input.y);
        }

        private void ResetDirection() => _direction = Vector3.zero;

        private void OnDisable()
        {
            _controls.Disable();
            _updateService.OnFixedUpdate -= Move;
        }

        private void OnDestroy()
        {
            _controls.Disable();
            _updateService.OnFixedUpdate -= Move;
        }
    }
}
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    [RequireComponent(typeof(TrailRenderer))]
    public class Bullet : MonoBehaviour
    {
        public delegate void OnHitMethod(Bullet bullet);
        public event OnHitMethod OnHit;

        [SerializeField] [Range(5, 100)] float _speed;
        [SerializeField] [Range(1, 5)] float _lifetime;

        private UpdateService _updateService;
        private TrailRenderer _trailRenderer;
        private float _currentLifetime;
        
        public float Damage { get; private set; }

        [Inject] private void Construct(UpdateService updateService)
        {
            _updateService = updateService;
            _trailRenderer = GetComponent<TrailRenderer>();
        }

        private void OnEnable()
        {
            _updateService.OnFixedUpdate += Accelerate;
            _updateService.OnUpdate += Live;
        }

        private void ResetLifetime() => _currentLifetime = 0;

        private void Live()
        {
            _currentLifetime += Time.deltaTime;
            _currentLifetime = Mathf.Clamp(_currentLifetime, 0, _lifetime);

            if (_currentLifetime >= _lifetime) Hit();
        }

        private void Accelerate() => transform.Translate(Vector3.forward * _speed * Time.fixedDeltaTime);

        public void ClearTrail() => _trailRenderer.Clear();

        private void Hit() => OnHit?.Invoke(this);

        public void SetDamage(Attacker attacker) => Damage = attacker.Damage;

        private void OnTriggerEnter(Collider other)
        {
            Hit();

            if (other.TryGetComponent(out Helther helther))
                helther.TakeDamage(this);
        }

        private void OnDisable()
        {
            _updateService.OnFixedUpdate -= Accelerate;
            _updateService.OnUpdate -= Live;
            ResetLifetime();
        }
    }
}
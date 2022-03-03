using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class Helther : MonoBehaviour
    {
        public delegate void OnDeathMethod();
        public event OnDeathMethod OnDeath;
        public event Action<float> OnHelthChange;

        [SerializeField] [Range(1, 1000)] private float _maxHelth;
        [SerializeField] private bool _isRegenerable;
        [SerializeField] [Range(1, 100)] private float _regenerationSpeed = 20;

        private UIBar _helthbar;
        private float _helth;
        private float _regenerationCooldown;
        private const float _regenerationCooldownLimit = 1;
        private float _regenerationCooldownSpeed = 0.25f;
        private ParticleSystem _deathParticle;
        private UpdateService _updateService;

        public float Helth
        {
            get => _helth;

            private set
            {
                _helth = Mathf.Clamp(value, 0, _maxHelth);
                OnHelthChange?.Invoke(value / _maxHelth);

                if (value == 0) OnDeath.Invoke();
            }
        }

        [Inject] private void Construct(UpdateService updateService)
        {
            _updateService = updateService;
            _deathParticle = GetComponentInChildren<ParticleSystem>();
        }

        private void OnEnable()
        {
            if (_helthbar != null) { OnHelthChange += _helthbar.SetValue; }
            OnDeath += Death;
            _updateService.OnUpdate += Regenerate;
            Helth = _maxHelth;
            _deathParticle.transform.SetParent(transform);
            _deathParticle.transform.localPosition = Vector3.zero;
        }

        public void SetHelthbar(UIBar helthbar)
        {
            _helthbar = helthbar;
            OnHelthChange += _helthbar.SetValue;
        }

        private void Death()
        {
            _deathParticle.transform.SetParent(null);
            _deathParticle.Play();
            gameObject.SetActive(false);
        }

        private void Regenerate()
        {
            _regenerationCooldown += _regenerationCooldownSpeed * Time.deltaTime;
            _regenerationCooldown = Mathf.Clamp(_regenerationCooldown, 0, _regenerationCooldownLimit);

            if (_isRegenerable && _regenerationCooldown >= _regenerationCooldownLimit)
                Helth += _regenerationSpeed * Time.deltaTime;
        }

        public void TakeDamage(Bullet bullet)
        {
            Helth -= bullet.Damage;
            _regenerationCooldown = 0;
        }

        private void OnDisable()
        {
            OnDeath -= Death;
            OnHelthChange -= _helthbar.SetValue;
            _updateService.OnUpdate -= Regenerate;
        }
    }
}
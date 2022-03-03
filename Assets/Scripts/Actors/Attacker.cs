using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    [RequireComponent(typeof(TargetDetector))]
    public class Attacker : MonoBehaviour
    {
        [SerializeField] [Range(1, 1000)] private int _damage;
        [SerializeField] [Range(0.1f, 5)] private float _cooldownSpeed;
        private const float _cooldownLimit = 1;
        private IAttackScheme _attackScheme;
        private BulletFactory _bulletFactory;
        private List<Bullet> _bullets = new List<Bullet>();
        [SerializeField] private Transform _bulletPoint;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] [Range(10, 100)] private float _bulletPoolSize;

        public float Damage => _damage;
        public float CooldownSpeed => _cooldownSpeed;
        public float CooldownLimit => _cooldownLimit;

        [Inject] private void Construct(BulletFactory bulletFactory)
        {
            _bulletFactory = bulletFactory;
            InitPool();
        }

        private void OnEnable() => _attackScheme?.Apply(this);

        public void Fire()
        {
            foreach (Bullet bullet in _bullets)
            {
                if (bullet.gameObject.activeSelf == false)
                {
                    bullet.transform.SetParent(null);
                    bullet.gameObject.SetActive(true);
                    return;
                }
            }
        }

        public void Reload(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            bullet.transform.SetParent(_bulletPoint);
            bullet.transform.localPosition = default;
            bullet.transform.localRotation = default;
            bullet.ClearTrail();
        }

        public void InitPool()
        {
            for (int i = 0; i < _bulletPoolSize; i++)
            {
                Bullet bullet = _bulletFactory.Create(_bulletPrefab, _bulletPoint);
                bullet.gameObject.SetActive(false);
                _bullets.Add(bullet);
                bullet.OnHit += Reload;
                bullet.SetDamage(this);
            }
        }

        public void SetAttackScheme(IAttackScheme attackScheme)
        {
            _attackScheme?.Cancel(this);
            _attackScheme = attackScheme;
            _attackScheme.Apply(this);
        }

        private void OnDisable() => _attackScheme?.Cancel(this);

        private void OnDestroy()
        {
            foreach (Bullet bullet in _bullets)
                bullet.OnHit -= Reload;
        }
    }
}
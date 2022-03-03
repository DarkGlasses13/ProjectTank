using UnityEngine;

namespace Assets.Scripts
{
    public class AutomaticAttackSchene : IAttackScheme
    {
        private Attacker _attacker;
        private UpdateService _updateService;
        private TargetDetector _targetDetector;
        private TurretCooldownbar _cooldownBar;
        private float _cooldown;

        public AutomaticAttackSchene(UpdateService updateService, TargetDetector targetDetector, TurretCooldownbar cooldownBar)
        {
            _updateService = updateService;
            _targetDetector = targetDetector;
            _cooldownBar = cooldownBar;
        }

        public void Apply(Attacker attacker)
        {
            _attacker = attacker;
            _cooldown = attacker.CooldownLimit;
            _updateService.OnUpdate += Attack;
        }

        private void Attack()
        {
            _cooldown += _attacker.CooldownSpeed * Time.deltaTime;
            _cooldown = Mathf.Clamp(_cooldown, 0, _attacker.CooldownLimit);
            _cooldownBar.SetValue(_cooldown);

            if (_cooldown >= _attacker.CooldownLimit && _targetDetector.Target != null)
            {
                _attacker.Fire();
                _cooldown = 0;
            }
        }

        public void Cancel(Attacker attacker)
        {
            _updateService.OnUpdate -= Attack;
        }
    }
}
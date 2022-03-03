using UnityEngine;

namespace Assets.Scripts
{
    public class ControllableAttackScheme : IAttackScheme
    {
        private Attacker _attacker;
        private Controls _controls;
        private UpdateService _updateService;
        private TankCooldownbar _cooldownbar;
        private float _cooldown;

        public ControllableAttackScheme(Controls controls, UpdateService updateService, TankCooldownbar cooldownbar)
        {
            _controls = controls;
            _updateService = updateService;
            _cooldownbar = cooldownbar;
        }

        public void Apply(Attacker attacker)
        {
            _attacker = attacker;
            _cooldown = attacker.CooldownLimit;
            _controls.Enable();
            _updateService.OnUpdate += Cooldown;
            _controls.Tank.Attack.started += callbackContext => Attack();
        }

        private void Attack()
        {
            if (_cooldown >= _attacker.CooldownLimit)
            {
                _cooldown = 0;
                _attacker.Fire();
            }
        }

        private void Cooldown()
        {
            _cooldown += _attacker.CooldownSpeed * Time.deltaTime;
            _cooldown = Mathf.Clamp(_cooldown, 0, _attacker.CooldownLimit);
            _cooldownbar.SetValue(_cooldown);
        }

        public void Cancel(Attacker attacker)
        {
            _controls.Disable();
            _updateService.OnUpdate -= Cooldown;
        }
    }
}
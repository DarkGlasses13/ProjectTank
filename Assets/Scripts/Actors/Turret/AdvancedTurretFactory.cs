using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class AdvancedTurretFactory : IFactory<Turret, Transform, Transform, LevelStartup, Turret>
    {
        private DiContainer _container;

        public AdvancedTurretFactory(DiContainer container) => _container = container;

        public Turret Create(Turret prefab, Transform parent, Transform spawnPoint, LevelStartup levelStartup)
        {
            Turret turret = _container
                .InstantiatePrefabForComponent<Turret>(prefab.gameObject, spawnPoint.position, spawnPoint.rotation, parent);

            UpdateService updateService = _container.Resolve<UpdateService>();
            TargetDetector targetDetector = turret.GetComponent<TargetDetector>();
            Aimer aimer = turret.GetComponent<Aimer>();
            Attacker attacker = turret.GetComponent<Attacker>();
            TurretCooldownbar cooldownbar = turret.GetComponentInChildren<TurretCooldownbar>();
            Helther helther = turret.GetComponent<Helther>();

            attacker.SetAttackScheme(new AutomaticAttackSchene(updateService, targetDetector, cooldownbar));
            helther.SetHelthbar(turret.GetComponentInChildren<TurretHelthbar>());

            levelStartup.OnRestart.AddListener(turret.SetActive);

            return turret;
        }
    }
}
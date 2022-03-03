using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class AdvancedTankFactory : IFactory<Tank, Transform, Transform, LevelStartup, Tank>
    {
        private DiContainer _container;

        public AdvancedTankFactory(DiContainer container) => _container = container;

        public Tank Create(Tank prefab, Transform parent, Transform spawnPoint, LevelStartup levelStartup)
        {
            Tank tank = _container.InstantiatePrefabForComponent<Tank>(prefab, spawnPoint.position, spawnPoint.rotation, parent);
            TankDriver driver = tank.GetComponent<TankDriver>();
            TankUIPanel uIPanel = _container.Resolve<TankUIPanel>();
            TargetDetector targetDetector = tank.GetComponent<TargetDetector>();
            Aimer aimer = tank.GetComponent<Aimer>();
            Attacker attacker = tank.GetComponent<Attacker>();
            Helther helther = tank.GetComponent<Helther>();

            tank.SetSpawnPosition(spawnPoint);
            uIPanel.Enable();
            helther.SetHelthbar(_container.Resolve<TankHelthbar>());

            attacker
                .SetAttackScheme(new ControllableAttackScheme
                (
                    _container.Resolve<Controls>(),
                    _container.Resolve<UpdateService>(),
                    _container.Resolve<TankCooldownbar>())
                );

            levelStartup.OnRestart.AddListener(tank.Respawn);

            return tank;
        }
    }
}
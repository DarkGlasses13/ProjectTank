using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class ActorsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindBulletFactory();
            BindTurret();
            BindTank();
            BindPlayerCamera();
        }

        private void BindPlayerCamera()
        {
            Container
                .BindFactory<PlayerCamera, Transform, Transform, PlayerCamera, PlayerCameraFactory>()
                .FromFactory<AdvancedPlayerCameraFactory>();
        }

        private void BindTank()
        {
            Container
                .BindFactory<Tank, Transform, Transform, LevelStartup, Tank, TankFactory>()
                .FromFactory<AdvancedTankFactory>();
        }

        private void BindTurret()
        {
            Container
                .BindFactory<Turret, Transform, Transform, LevelStartup, Turret, TurretFactory>()
                .FromFactory<AdvancedTurretFactory>();
        }

        private void BindBulletFactory()
        {
            Container
                .BindFactory<Bullet, Transform, Bullet, BulletFactory>()
                .FromFactory<AdvancedBulletFactory>();
        }
    }
}
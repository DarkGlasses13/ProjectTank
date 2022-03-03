using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private RestartUIPanel _restartPanel;
        [SerializeField] private TankUIPanel _tankPanel;

        public override void InstallBindings()
        {
            BindRestartPanel();
            BindTankPanel();
        }

        private void BindTankPanel()
        {
            Container.Bind<TankUIPanel>().FromInstance(_tankPanel).AsSingle();
            Container.Bind<TankHelthbar>().FromInstance(_tankPanel.GetComponentInChildren<TankHelthbar>()).AsSingle();
            Container.Bind<TankCooldownbar>().FromInstance(_tankPanel.GetComponentInChildren<TankCooldownbar>()).AsSingle();
        }

        private void BindRestartPanel() => Container.Bind<RestartUIPanel>().FromInstance(_restartPanel).AsSingle();
    }
}
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class GeneralInstaller : MonoInstaller
    {
        [SerializeField] private UpdateService _updateService;
        [SerializeField] private CoroutineService _coroutineService;

        public override void InstallBindings()
        {
            BindUpdateService();
            BindCoroutineService();
            BindControls();
        }

        private void BindUpdateService()
        {
            Container
                .Bind<UpdateService>()
                .FromInstance(_updateService)
                .AsSingle()
                .NonLazy();
        }

        private void BindCoroutineService()
        {
            Container
                .Bind<CoroutineService>()
                .FromInstance(_coroutineService)
                .AsSingle()
                .NonLazy();
        }

        private void BindControls()
        {
            Container.Bind<Controls>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }
    }
}
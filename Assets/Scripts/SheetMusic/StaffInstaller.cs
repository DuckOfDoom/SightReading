using UnityEngine;
using Zenject;

namespace DuckOfDoom.SightReading.SheetMusic
{
    public class StaffInstaller : MonoInstaller
    {
#pragma warning disable 0649
        [SerializeField] private StaffView _staffView;
        [SerializeField] private Canvas _rootCanvas;
#pragma warning restore 0649

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<StaffConfig>()
                .FromMethod(_ => Config.Load<StaffConfig>(Consts.STAFF_CONFIG_PATH))
                .AsSingle()
                .NonLazy();
            
            Container.Bind<IStaffView>()
                .FromInstance(_staffView)
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesTo<SymbolsPool>()
                .FromMethod(_ => new SymbolsPool(_rootCanvas))
                .AsSingle()
                .NonLazy();
            
            Container.BindInterfacesTo<SymbolsFactory>()
                .FromNew()
                .AsSingle()
                .NonLazy();
            
            Container.BindInterfacesTo<StaffController>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }
    }
}

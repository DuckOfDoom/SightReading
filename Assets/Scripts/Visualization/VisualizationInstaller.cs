using DuckOfDoom.SightReading.AudioTools;
using UnityEngine;
using Zenject;

namespace DuckOfDoom.SightReading.Visualization
{
    public class VisualizationInstaller : MonoInstaller
    {
#pragma warning disable 0649
        [SerializeField] private VisualizationView _visualizationView;
#pragma warning restore 0649
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MicrophoneHandler>()
                .FromNew()
                .AsSingle()
                .NonLazy();
            
            Container.BindInterfacesTo<FrequencyDetector>()
                .FromNew()
                .AsSingle()
                .NonLazy();
            
            Container.BindInterfacesTo<FrequencyConverter>()
                .FromNew()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesTo<VisualizationView>()
                .FromInstance(_visualizationView)
                .AsSingle()
                .NonLazy();
            
            Container.BindInterfacesTo<VisualizationController>()
                .FromNew()
                .AsSingle()
                .NonLazy();

            // Container.BindInterfacesTo<VisualizationController>()
            //     .FromNewComponentOnNewGameObject()
            //     .WithGameObjectName("VisualizationController")
            //     .AsSingle()
            //     .NonLazy();
        }
    }
}

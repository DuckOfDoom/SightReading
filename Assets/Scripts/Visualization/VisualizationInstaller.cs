using DuckOfDoom.SightReading.AudioTools;
using UnityEngine;
using Zenject;

namespace DuckOfDoom.SightReading.Visualization
{
    public class VisualizationInstaller : MonoInstaller
    {
#pragma warning disable 0649
        [SerializeField] private VisualizerView _visualizerView;
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

            Container.BindInterfacesTo<VisualizerView>()
                .FromInstance(_visualizerView)
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesTo<VisualizationController>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("VisualizationController")
                .AsSingle()
                .NonLazy();
        }
    }
}

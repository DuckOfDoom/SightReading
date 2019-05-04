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
#if BASS_MIC_HANDLER
            Container.BindInterfacesTo<BassMicrophoneHandler>()
#else
            Container.BindInterfacesTo<UnityMicrophoneHandler>()
#endif
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
        }
    }
}

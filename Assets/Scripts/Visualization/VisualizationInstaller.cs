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
            Container.BindInterfacesTo<AudioProcessingConfig>()
                .FromMethod(_ => Config.Load<AudioProcessingConfig>(Consts.AUDIO_PROCESSING_CONFIG_PATH))
                .AsSingle()
                .NonLazy();
            
#if BASS_MIC_HANDLER
            Container.BindInterfacesTo<BassMicrophoneHandler>()
#else
            Container.BindInterfacesTo<UnityMicrophoneHandler>()
#endif
                .FromNew()
                .AsSingle()
                .NonLazy();
            
            
            Container.BindInterfacesTo<NoteDetector>()
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

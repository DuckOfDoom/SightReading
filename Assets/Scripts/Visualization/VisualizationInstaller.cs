using DuckOfDoom.SightReading.AudioTools;
using UnityEngine;
using Zenject;

namespace DuckOfDoom.SightReading.Visualization
{
    public class VisualizationInstaller : MonoInstaller
    {
#pragma warning disable 0649
        [SerializeField] private SampleVisualizer _visualizer;
#pragma warning restore 0649
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MicrophoneHandler>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesTo<SampleVisualizer>()
                .FromInstance(_visualizer)
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesAndSelfTo<Visualization>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("Visualization")
                .AsSingle()
                .NonLazy();
        }
    }
}

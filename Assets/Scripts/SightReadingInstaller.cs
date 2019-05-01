using DuckOfDoom.SightReading.AudioTools;
using UniRx;
using UnityEngine;
using Zenject;

namespace DuckOfDoom.SightReading
{
    public class SightReadingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MicrophoneHandler>()
                .AsSingle()
                .NonLazy();
            
            Container.BindInterfacesTo<FrequencyDetector>()
                .AsSingle()
                .NonLazy();

            var mic = Container.Resolve<IMicrophoneHandler>();
            var pDectector = Container.Resolve<IFrequencyDetector>();

            mic.SamplesStream.Subscribe(
                samples =>
                {
            var s = "[";
            foreach (var sample in samples)
            {
                s += sample + "\n";
            }
            
            s += "]";
            
            Debug.LogError(s);
                    Debug.LogError("Pitch: " + pDectector.GetFrequency(samples));
                });

            // Container.Resolve<IMicrophoneHandler>().SamplesStream.Subscribe(
            //     samples =>
            //     {
            //         var s = "[";
            //         foreach (var sample in samples)
            //         {
            //             s += sample + "\n";
            //         }
            //
            //         s += "]";
            //
            //         Debug.LogError(s);
            //     });

        }
    }
}

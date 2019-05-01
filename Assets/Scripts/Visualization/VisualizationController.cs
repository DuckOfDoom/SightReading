using System.Collections.Generic;
using DuckOfDoom.SightReading.AudioTools;
using UniRx;
using UnityEngine;
using Zenject;

namespace DuckOfDoom.SightReading.Visualization
{
    public interface IVisualizationController
    {
        
    }
    
    public class VisualizationController : MonoBehaviour, IVisualizationController
    {
        [Inject] private IMicrophoneHandler MicHandler { get; set; }
        [Inject] private IFrequencyDetector FrequencyDetector { get; set; }
        [Inject] private IVisualizerView VisualizerView { get; set; }
        
        public void Start()
        {
            MicHandler.SamplesStream.Subscribe(
                    samples =>
                    {
                        VisualizerView.VisualizeSamples(
                            // CompressSamples(samples)
                            samples
                        );

                        VisualizerView.SetFrequency(
                            FrequencyDetector.GetFrequency(samples)
                        );
                    })
                .AddTo(this);
        }

        private float[] CompressSamples(float[] samples)
        {
            // var newSamples = new float[samples.Length / 2];
            var newSamples = new List<float>();
            
            for (var i = 0; i < samples.Length; i += 2)
            {
                var s1 = samples[i];
                var s2 = samples[i+1];
                
                newSamples.Add((s1 + s2) / 2f);
            }

            return newSamples.ToArray();
        }
    }
}

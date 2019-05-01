using System.Collections.Generic;
using DuckOfDoom.SightReading.AudioTools;
using UniRx;
using UnityEngine;
using Zenject;

namespace DuckOfDoom.SightReading.Visualization
{
    public interface IVisualization
    {
        
    }
    
    public class Visualization : MonoBehaviour, IVisualization
    {
        [Inject] private IMicrophoneHandler MicHandler { get; set; }
        [Inject] private ISampleVisualizer Visualizer { get; set; }
        
        public void Start()
        {
            MicHandler.SamplesStream.Subscribe(
                    samples =>
                    {
                        Visualizer.VisualizeSamples(
                            // CompressSamples(samples)
                            samples
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

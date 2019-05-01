using UnityEngine;

namespace DuckOfDoom.SightReading.AudioTools
{
    public interface IFrequencyDetector
    {
        float GetFrequency(float[] samples);
    }
    
    // https://answers.unity.com/questions/157940/getoutputdata-and-getspectrumdata-they-represent-t.htmlv
    public class FrequencyDetector : IFrequencyDetector
    {
        private const float MIN_AMPLITUDE = 0.001f;
            
        public float GetFrequency(float[] samples)
        {
            // Min amplitude to extract pitch
            var maxSample = 0f;
            var maxSampleIndex = 0;
            
            for (var i = 0; i < samples.Length; i++)
            {
                var sample = samples[i];
                if (sample > MIN_AMPLITUDE && sample > maxSample)
                {
                    maxSample = sample;
                    maxSampleIndex = i;
                }
            }
            
            // interpolate index using neighbours
            float freq = maxSampleIndex;
            if (maxSampleIndex > 0 && maxSampleIndex < samples.Length - 1)
            {
                var dL = samples[maxSampleIndex - 1] / samples[maxSampleIndex];
                var dR = samples[maxSampleIndex + 1] / samples[maxSampleIndex];
                freq += 0.5f * (dR * dR - dL * dL);
            }

            var hertz = freq * (AudioSettings.outputSampleRate / 2f) / samples.Length;
            return hertz;
        }
    }
}

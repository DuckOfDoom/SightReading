using UnityEngine;

namespace DuckOfDoom.SightReading.AudioTools
{
    public interface IPitchDetector
    {
        float DetectPitch(float[] samples);
    }
    
    public class PitchDetector : IPitchDetector
    {
        private const float MIN_AMPLITUDE = 0.02f;
            
        public float DetectPitch(float[] samples)
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

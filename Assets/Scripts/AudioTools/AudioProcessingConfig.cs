using System;
using UnityEngine;

namespace DuckOfDoom.SightReading.AudioTools
{
    public interface IAudioProcessingConfig
    {
        int FrequencyResolution { get; }
        float MinFrequencyAmplitude { get; }
        TimeSpan NoteDetectionInterval { get; }
    }

    [CreateAssetMenu(fileName = "AudioProcessingConfig", menuName = "AudioProcessingConfig", order = 51)]
    public class AudioProcessingConfig : Config, IAudioProcessingConfig
    {
#pragma warning disable 0649
        [SerializeField] private int _frequencyResolution = 1024;
        [SerializeField] private float _minAmplitudeForFrequency = 0.01f;
        [SerializeField] private float _noteDetectionInterval = 0.1f;
#pragma warning restore 0649

        public int FrequencyResolution => _frequencyResolution;
        public float MinFrequencyAmplitude => _minAmplitudeForFrequency;
        public TimeSpan NoteDetectionInterval => TimeSpan.FromSeconds(_noteDetectionInterval);
    }
}

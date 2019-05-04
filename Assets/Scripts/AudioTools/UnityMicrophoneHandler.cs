using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace DuckOfDoom.SightReading.AudioTools
{
    public interface IAudioStreamSource 
    {
        IObservable<float[]> SamplesStream { get;  }
    }
    
    public class UnityMicrophoneHandler : IAudioStreamSource, IDisposable
    {
        private readonly AudioSource _source;
        private readonly string _primaryDevice;

        private readonly float[] _samples;

        public IObservable<float[]> SamplesStream { get;}
        
        public UnityMicrophoneHandler(IAudioProcessingConfig config)
        {
            _samples = new float[config.FrequencyResolution];
            _primaryDevice = Microphone.devices.First();
            _source = new GameObject("MicrophoneHandlerSource").AddComponent<AudioSource>();

            SamplesStream = Observable.EveryUpdate()
                .Where(_ => _source.clip != null)
                .Select(
                    _ =>
                    {
                        _source.GetSpectrumData(_samples, 0, FFTWindow.BlackmanHarris);
                        
                        return _samples;
                    }
                );
                
            StartListening();
        }

        private void StartListening()
        {
            _source.clip = Microphone.Start(_primaryDevice, true, 1, AudioSettings.outputSampleRate);
            _source.loop = true;

            while (!(Microphone.GetPosition(_primaryDevice) > 0))
            {
                
            }
            
            _source.Play();
            
            AudioSettings.GetDSPBufferSize(out var dspBufferSize, out var dspNumBuffers);
            
            _source.timeSamples = (Microphone.GetPosition(_primaryDevice) + AudioSettings.outputSampleRate - 3 * dspBufferSize * dspNumBuffers) % AudioSettings.outputSampleRate;
        }

        private void StopListening()
        {
            // _source.clip = null;
            Microphone.End(_primaryDevice);
        }

        public void Dispose()
        {
            StopListening();
            
            if (_source != null)
                UnityEngine.Object.Destroy(_source.gameObject);
        }
    }

}

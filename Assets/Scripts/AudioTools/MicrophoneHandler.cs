using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace DuckOfDoom.SightReading.AudioTools
{
    public interface IMicrophoneHandler : IDisposable
    {
        IObservable<float[]> SamplesStream { get;  }
    }
    
    public class MicrophoneHandler : IMicrophoneHandler
    {
        private const int FREQUENCY_RESOLUTION = 1024;
        
        private readonly AudioSource _source;
        private readonly string _primaryDevice;

        private readonly float[] _samples = new float[FREQUENCY_RESOLUTION];

        public IObservable<float[]> SamplesStream { get;}
        
        public MicrophoneHandler()
        {
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

        public void StartListening()
        {
            _source.clip = Microphone.Start(_primaryDevice, true, 1, AudioSettings.outputSampleRate);
            _source.loop = true;

            while (!(Microphone.GetPosition(_primaryDevice) > 0))
            {
                
            }
            
            _source.Play();
            //
            // AudioSettings.GetDSPBufferSize(out var dspBufferSize, out var dspNumBuffers);
            //
            // _source.timeSamples = (Microphone.GetPosition(_primaryDevice) + AudioSettings.outputSampleRate - 3 * dspBufferSize * dspNumBuffers) % AudioSettings.outputSampleRate;
        }

        public void StopListening()
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

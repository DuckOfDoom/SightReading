using System;
using System.Text;
using Un4seen.Bass;
using UniRx;
using UnityEngine;

namespace DuckOfDoom.SightReading.AudioTools
{
    /// <summary>
    ///     This is a wrapper that uses Bass.Net library to record sound instead of unity microphone (since it sucks)
    /// </summary>
    public class BassMicrophoneHandler : IAudioStreamSource, IDisposable
    {
        private readonly float[] _samples;
        private int _recordingHandle;
        private RECORDPROC _recordProc;
        
        public IObservable<float[]> SamplesStream { get; }
        
        public BassMicrophoneHandler(IAudioProcessingConfig config)
        {
            _samples = new float[config.FrequencyResolution];
            
            PrintDebugInfo();
            
            SamplesStream = Observable.EveryUpdate()
                .Where(_ => _recordingHandle != Bass.FALSE)
                .Select(_ => ProcessRecordedData());
            
            StartRecording();
        }

        public void Dispose()
        {
            if (_recordingHandle != Bass.FALSE)
            {
                var stop = Bass.BASS_ChannelStop(_recordingHandle);
                Debug.Log("[BassMicrophoneHandler] BASS_ChannelStop == " + stop);
            }
            
            var free = Bass.BASS_RecordFree();
            Debug.Log("[BassMicrophoneHandler] BASS_RecordFree == " + free);
        }

        private void StartRecording()
        {
            // var device = Bass.BASS_RecordGetDeviceInfos().FirstOrDefault();
            if (Bass.BASS_RecordInit(-1))
            {
                Debug.Log("[BassMicrophoneHandler] BASS_RecordInit = true");
                
                _recordProc = OnRecord;
                _recordingHandle = Bass.BASS_RecordStart(
                    AudioSettings.outputSampleRate,
                    1,
                    BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_RECORD_PAUSE,
                    _recordProc,
                    IntPtr.Zero
                );

                if (_recordingHandle == Bass.FALSE)
                {
                    Debug.LogError($"[BassMicrophoneHandler] BASS_RecordStart returned 0!");
                    return;
                }
                else
                {
                    Debug.Log($"[BassMicrophoneHandler] Recording Handle: {_recordingHandle}");
                    var play = Bass.BASS_ChannelPlay(_recordingHandle, false);
                    Debug.Log("[BassMicrophoneHandler] BASS_ChannelPlay == " + play);
                }
            }
            else
            {
                Debug.LogError("[BassMicrophoneHandler] BASS_RecordInit == False!");
            }
        }

        private void PrintDebugInfo()
        {
            var sb = new StringBuilder("BASS");
            sb.AppendLine("Device Infos:");
            var devicesInfos = Bass.BASS_RecordGetDeviceInfos();
            for (var index = 0; index < devicesInfos.Length; index++)
            {
                var di = devicesInfos[index];
                sb.AppendLine(di.ToString());
            }

            Debug.Log("[BassMicrophoneHandler]\n " + sb);
        }

        private bool OnRecord(int handle, IntPtr buffer, int length, IntPtr user) { return true; }
        
        private float[] ProcessRecordedData()
        {
            if (_recordingHandle == 0)
                return new float[] { };

            Bass.BASS_ChannelGetData(_recordingHandle, _samples, (int) BASSData.BASS_DATA_FFT2048);

            return _samples;
        }
    }
}

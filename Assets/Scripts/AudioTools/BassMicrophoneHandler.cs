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
        public IObservable<float[]> SamplesStream { get; }
        private int _recordingHandle;
        
        private readonly float[] _samples = new float[Consts.FREQUENCY_RESOLUTION];
        private RECORDPROC _recordProc;
        
        public BassMicrophoneHandler()
        {
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
                Debug.Log("BASS_ChannelStop == " + stop);
            }
            
            var free = Bass.BASS_RecordFree();
            Debug.Log("BASS_RecordFree == " + free);
        }

        private void StartRecording()
        {
            // var device = Bass.BASS_RecordGetDeviceInfos().FirstOrDefault();
            if (Bass.BASS_RecordInit(-1))
            {
                Debug.Log("BASS_RecordInit = true");
                
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
                    Debug.LogError($"BASS_RecordStart returned 0!");
                    return;
                }
                else
                {
                    Debug.Log($"Recording Handle: {_recordingHandle}");
                    var play = Bass.BASS_ChannelPlay(_recordingHandle, false);
                    Debug.Log("BASS_ChannelPlay == " + play);
                }
            }
            else
            {
                Debug.LogError("BASS_RecordInit == False!");
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
            
            Debug.Log(sb.ToString());
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

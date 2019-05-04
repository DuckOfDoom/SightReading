using System;
using System.Text;
using Un4seen.Bass;

namespace DuckOfDoom.SightReading.AudioTools
{
    public class BassWrapper
    {
        public BassWrapper()
        {
            var sb = new StringBuilder("BASS");
            sb.AppendLine("Device Infos:");
            var devicesInfos = Bass.BASS_RecordGetDeviceInfos();
            for (var index = 0; index < devicesInfos.Length; index++)
            {
                var di = devicesInfos[index];
                // sb.AppendLine($"\t {index}: {di.id} - {di.name} ({di.type}), status:{di.status}");
                sb.AppendLine(di.ToString());
            }
            
            // var x = Bass.bass_rec
            // Bass.BASS_RecordStart(44100, 0, BASSFlag.BASS_DEFAULT, OnRecord, IntPtr.Zero);
        }

        private bool OnRecord(int handle, IntPtr buffer, int length, IntPtr user)
        {
            
        }
    }
}

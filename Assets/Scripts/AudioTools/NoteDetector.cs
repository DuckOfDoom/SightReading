using System;
using System.Collections.Generic;
using System.Linq;
using Optional;
using UniRx;
using UnityEngine;

namespace DuckOfDoom.SightReading.AudioTools
{
    public interface INoteDetector
    {
        IObservable<string> DetectedNotes { get; }
        void AddSamples(float[] samples);
    }
    
    public class NoteDetector : INoteDetector
    {
        private readonly IAudioProcessingConfig _config;
        // private readonly Dictionary<string, double> _frequenciesTable = new Dictionary<string, double>();
        private readonly List<Tuple<double, string>> _frequenciesToNotes = new List<Tuple<double, string>>();
        private readonly List<string> _notes = new List<string> {"A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#"};

        // We can't detect notes constantly because they fluctuate back and forth.
        // So we gather all notes for interval of time and find the most common.
        private readonly Dictionary<string, int> _notesForInterval = new Dictionary<string, int>();
        
        public IObservable<string> DetectedNotes { get; }
            
        public NoteDetector(IAudioProcessingConfig config)
        {
            _config = config;
            
            Debug.Log($"[NoteDetector] Initializing. DetectionInterval {_config.NoteDetectionInterval}");
            
            FillNoteTable();

            DetectedNotes = Observable.Interval(_config.NoteDetectionInterval)
                .Where(_ => _notesForInterval.Any())
                .Select(_ =>
                {
                    var mostCommonNote = FindMostCommonNote();
                    _notesForInterval.Clear();
                    return mostCommonNote;
                });
        }

        public void AddSamples(float[] samples)
        {
            var frequency = GetFrequencyFromSamples(samples);
            var note = GetNoteFromFrequency(frequency);
            note.MatchSome(n =>
            {
                if (_notesForInterval.ContainsKey(n))
                    _notesForInterval[n]++;
                else
                    _notesForInterval.Add(n, 1);
            });
        }

        private float GetFrequencyFromSamples(float[] samples)
        {
            // Min amplitude to extract pitch
            var maxSample = 0f;
            var maxSampleIndex = 0;
            
            for (var i = 0; i < samples.Length; i++)
            {
                var sample = samples[i];
                if (sample > _config.MinFrequencyAmplitude && sample > maxSample)
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

        private Option<string> GetNoteFromFrequency(float freq)
        {
            var f = (double) freq;
            Tuple<double, string> prev = null;
            Tuple<double, string> next = null;

            for (var i = 0; i < _frequenciesToNotes.Count; i++)
            {
                if (f > _frequenciesToNotes[i].Item1)
                {
                    prev = _frequenciesToNotes[i];
                    
                    if (i < _frequenciesToNotes.Count - 1)
                        next = _frequenciesToNotes[i + 1];
                }
            }

            if (prev == null)
                return Option.None<string>();

            if (next == null)
                return Option.Some(prev.Item2);

            return Option.Some(
                freq - prev.Item1 < next.Item1 - freq ?
                    prev.Item2 :
                    next.Item2
            );
        }
        
        private void FillNoteTable()
        {
            // https://pages.mtu.edu/~suits/NoteFreqCalcs.html
            const int a1 = 55; 
            var a = Math.Pow(2, 1.0 / 12.0);

            var octave = 1;
            while (octave < 8)
            {
                for (var i = 0; i < _notes.Count; i++)
                {
                    var note = _notes[i] + (octave + (i < 3 ? 0 : 1));
                    var freq = a1 * Math.Pow(a, (octave - 1) * 12 + i);
                    // _frequenciesTable.Add(note, freq);
                    _frequenciesToNotes.Add(Tuple.Create(freq, note));
                }

                octave++;
            }

            // foreach (var kvp in _frequenciesTable)
            // {
            //     Debug.LogError($"{kvp.Key} = {kvp.Value}");
            // }
        }

        private string FindMostCommonNote()
        {
            if (_notesForInterval.Count == 0)
                throw new InvalidOperationException("Can't find most common note when there are none!");
            
            var mostCommon = "";
            var count = 0;
            foreach (var kvp in _notesForInterval)
            {
                if (kvp.Value > count)
                    mostCommon = kvp.Key;
            }

            return mostCommon;
        }
    }
}

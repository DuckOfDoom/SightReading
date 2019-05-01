using System;
using System.Collections.Generic;
using Optional;
using UnityEngine;

namespace DuckOfDoom.SightReading.AudioTools
{
    public interface IFrequencyConverter
    {
        Option<string> FrequencyToNote(float freq);
    }
    
    public class FrequencyConverter : IFrequencyConverter
    {
        // private readonly Dictionary<string, double> _frequenciesTable = new Dictionary<string, double>();
        private readonly List<Tuple<double, string>> _frequenciesToNotes = new List<Tuple<double, string>>();
        private readonly List<string> _notes = new List<string> {"A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#"};
            
        public FrequencyConverter()
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

        public Option<string> FrequencyToNote(float freq)
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
    }
}

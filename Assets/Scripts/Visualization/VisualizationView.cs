using System;
using System.Collections.Generic;
using System.Linq;
using Optional;
using Optional.Unsafe;
using UnityEngine;
using UnityEngine.UI;

namespace DuckOfDoom.SightReading.Visualization
{
    public interface IVisualizationView : IDisposable
    {
        void VisualizeSamples(float[] samples);
        
        void AddNote(Option<string> note);
        void SetFrequency(float frequency);
    }
    
    public class VisualizationView : MonoBehaviour, IVisualizationView
    {
#pragma warning disable 0649
        [SerializeField] private Image _sourceImage; 
        [SerializeField] private Text _frequencyText; 
        [SerializeField] private Text _noteText; 
        [SerializeField] private float _heightMultiplier = 1000; 
#pragma warning restore 0649

        private readonly List<Image> _images = new List<Image>();

        public void VisualizeSamples(float[] samples)
        {
            FixImagesCount(samples.Length);

            for (var i = 0; i < samples.Length; i++)
            {
                var rt = _images[i].transform as RectTransform;
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, samples[i] * _heightMultiplier);
            }
        }

        public void SetFrequency(float frequency) { _frequencyText.text = $"Frequency: {frequency.ToString()} Hz"; }

        private readonly Queue<string> _notes = new Queue<string>();

        public void AddNote(Option<string> note)
        {
            if (!note.HasValue)
                return;
            
            var last = _notes.LastOrDefault();
            if (last != null && last == note.ValueOrFailure())
                return;

            if (_notes.Count > 100)
                _notes.Dequeue();
            
            _notes.Enqueue(note.ValueOrFailure());
            
            _noteText.text = $"Notes: {string.Join(" - ", _notes)}";
        }

        private void FixImagesCount(int count)
        {
            if (_images.Count < count)
            {
                while (_images.Count < count)
                {
                    var i = Instantiate(_sourceImage, _sourceImage.transform.parent, false);
                    i.enabled = true;

                    _images.Add(i);
                }
            }
            else if (_images.Count > count)
            {
                for (var i = _images.Count; i < count; i--)
                    _images[i].enabled = false;
            }

            _sourceImage.enabled = false;
        }

        public void Dispose()
        {
            for (var i = 0; i < _images.Count; i++)
            {
                Destroy(_images[i]);
            }
        }
    }
}

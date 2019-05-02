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
        [SerializeField] private float _scaleFactor = 1000; 
#pragma warning restore 0649

        private readonly List<Image> _images = new List<Image>();
        private readonly List<GameObject> _cubes = new List<GameObject>();
        private readonly Queue<string> _notes = new Queue<string>();

        public void VisualizeSamples(float[] samples)
        {
            // for (int i = 1; i < samples.Length - 1; i++)
            // {
            //     Debug.DrawLine(new Vector3(i - 1, samples[i] + 10, 0), new Vector3(i, samples[i + 1] + 10, 0), Color.red);
            //     Debug.DrawLine(new Vector3(i - 1, Mathf.Log(samples[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(samples[i]) + 10, 2), Color.cyan);
            //     Debug.DrawLine(new Vector3(Mathf.Log(i - 1), samples[i - 1] - 10, 1), new Vector3(Mathf.Log(i), samples[i] - 10, 1), Color.green);
            //     Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(samples[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(samples[i]), 3), Color.blue);
            // }

            // VisualizeInUI(samples);
            VisualizeInScene(samples);
        }

        public void SetFrequency(float frequency)
        {
            _frequencyText.text = $"Frequency: {frequency.ToString()} Hz";
        }

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


        private void VisualizeInScene(float[] samples)
        {
            FixCubesCount(samples.Length);

            for (var i = 0; i < samples.Length; i++)
            {
                var go = _cubes[i];
                go.transform.localScale = new Vector3(
                    go.transform.localScale.x, 
                    samples[i] * _scaleFactor,
                    go.transform.localScale.z 
                    );
            }
        }

        private void VisualizeInUI(float[] samples)
        {
            FixImagesCount(samples.Length);
            
            for (var i = 0; i < samples.Length; i++)
            {
                var rt = _images[i].transform as RectTransform;
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, samples[i] * _heightMultiplier);
            }
        }

        private void FixCubesCount(int count)
        {
            if (_cubes.Count >= count)
                return;
            
            var startPoint = new Vector3(0, 0, 0);
            var totalSize = 100f;

            var cubeSize = totalSize / count;
            var shift = 0f;
                
            for (var i = 0; i < count; i++)
            {
                var c = GameObject.CreatePrimitive(PrimitiveType.Cube);
                c.transform.position = startPoint + new Vector3(shift, 0, 0);
                shift += cubeSize;
                c.transform.localScale = new Vector3(cubeSize, c.transform.localScale.y, c.transform.localScale.z);
                
                _cubes.Add(c);
            }
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

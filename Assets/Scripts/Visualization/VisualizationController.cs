using System;
using DuckOfDoom.SightReading.AudioTools;
using UniRx;

namespace DuckOfDoom.SightReading.Visualization
{
    public interface IVisualizationController : IDisposable
    {
        
    }
    
    public class VisualizationController : IVisualizationController
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        
        public VisualizationController(
            IAudioStreamSource micHandler,
            INoteDetector noteDetector,
            IVisualizationView view
            )
        {
            micHandler.SamplesStream.Subscribe(
                samples =>
                {
                    view.VisualizeSamples(samples);

                    // var note = noteDetector.DetectedNotes
                    //
                    // // var frequency = fDetector.GetFrequency(samples);
                    // // var note = fConverter.FrequencyToNote(frequency);
                    //
                    // view.AddNote(note);

                    noteDetector.AddSamples(samples);
                }).AddTo(_disposables);

            noteDetector.DetectedNotes.Subscribe(note => { view.AddNote(note); });
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}

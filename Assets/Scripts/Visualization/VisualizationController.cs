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
        private readonly IDisposable _sub;
        
        public VisualizationController(
            IMicrophoneHandler micHandler,
            IFrequencyDetector fDetector,
            IFrequencyConverter fConverter,
            IVisualizationView view
            )
        {
            _sub = micHandler.SamplesStream.Subscribe(
                samples =>
                {
                    view.VisualizeSamples(samples);

                    var frequency = fDetector.GetFrequency(samples);
                    var note = fConverter.FrequencyToNote(frequency);
                    
                    view.SetFrequency(frequency);
                    view.AddNote(note);
                });
        }

        public void Dispose()
        {
            _sub.Dispose();
        }
    }
}

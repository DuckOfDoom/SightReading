using System;
using System.Collections.Generic;
using System.Linq;
using Optional;
using UniRx;
using UnityEngine;

namespace DuckOfDoom.SightReading.SheetMusic
{
    public interface IStaffController
    {
        
    }
    
    public class StaffController : IStaffController, IDisposable
    {
        private readonly ISymbolsFactory _symbolsFactory;
        private readonly IStaffView _staffView;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        
        private bool _isMoving;
        
        public StaffController(ISymbolsFactory symbolsFactory, IStaffView staffView)
        {
            _symbolsFactory = symbolsFactory;
            _staffView = staffView;

            Observable.EveryUpdate()
                .Where(_ => Input.GetKey(KeyCode.M))
                .Subscribe(
                    _ =>
                    {
                        staffView.Advance(10f);
                    })
                .AddTo(_disposables);
            
            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.R))
                .Subscribe( _ => { PlaceNotes(GetRandomNotes()); })
                .AddTo(_disposables);
                
            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.L))
                .Subscribe( _ => { PlaceNotes(GetLick()); })
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void PlaceNotes(IEnumerable<Symbol> notes)
        {
            _staffView.Clear();
            foreach (var symbol in notes)
            {
                symbol.Note.Match(
                    note =>
                    {
                        var posY = MusicUtils.GetNotePosition(note.Name, note.Octave);
                        _staffView.PlaceSymbol(posY, symbol.Duration, _symbolsFactory.GetSymbol(symbol));
                    },
                    () => Debug.LogError("Not a note!"));
            }
        }
        
        private IEnumerable<Symbol> GetRandomNotes()
        {
            var rand = new System.Random();
            
            for (var i = 0; i < 5; i++)
            {
                var duration = (Duration) rand.Next(1, 6);
                var noteName = (NoteName) rand.Next(1, 7);
                var accidental = (Accidental) rand.Next(1, 6);
                var octave = rand.Next(3, 5);

                yield return
                    new Symbol
                    {
                        Duration = duration,
                        Note = new Note(noteName, accidental, octave).Some()
                    };
            }
        }

        private IEnumerable<Symbol> GetLick()
        {
            yield return new Symbol(Duration.Eighth, new Note(NoteName.D, 3).Some());
            yield return new Symbol(Duration.Eighth, new Note(NoteName.E, 3).Some());
            yield return new Symbol(Duration.Eighth, new Note(NoteName.F, 3).Some());
            yield return new Symbol(Duration.Eighth, new Note(NoteName.G, 3).Some());
            yield return new Symbol(Duration.Quarter, new Note(NoteName.E, 3).Some());
            yield return new Symbol(Duration.Eighth, new Note(NoteName.C, 3).Some());
            yield return new Symbol(Duration.Eighth, new Note(NoteName.D, 3).Some());
        }

        private void PlaceRandomNotes()
        {
            _staffView.Clear();
            
        }
    }
}

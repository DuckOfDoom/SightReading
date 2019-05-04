using System;
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
                .Subscribe( _ => { PlaceRandomNotes(); })
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void PlaceRandomNotes()
        {
            _staffView.Clear();
            
            var rand = new System.Random();
            
            for (var i = 0; i < 5; i++)
            {
                var type = SymbolType.Note;
                var duration = (Duration) rand.Next(1, 6);
                var symbol =
                    new Symbol
                    {
                        Type = type,
                        Duration = duration,
                    };
                
                var view = _symbolsFactory.GetSymbol(symbol);
                var yPos = rand.Next(-5, 5); 
                
                Debug.Log($"Placing symbol: {symbol}, {yPos}");
                
                _staffView.PlaceSymbol(yPos, duration, view);
            }
        }
    }
}

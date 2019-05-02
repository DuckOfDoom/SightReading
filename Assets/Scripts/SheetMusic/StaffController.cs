using UnityEngine;

namespace DuckOfDoom.SightReading.SheetMusic
{
    public interface IStaffController { }
    
    public class StaffController : IStaffController
    {
        public StaffController(ISymbolsFactory symbolsFactory, IStaffView staffView)
        {
            var rand = new System.Random();
            
            for (var i = 0; i < 5; i++)
            {
                var type = SymbolType.Note;
                var duration = (Duration) rand.Next(1, 6);
                var symbol = new Symbol {Type = type, Duration = duration};
                var view = symbolsFactory.GetSymbol(symbol);
                
                Debug.Log($"Placing symbol: {symbol}");
                
                staffView.PlaceSymbol(i, view);
            }
        }
    }
}

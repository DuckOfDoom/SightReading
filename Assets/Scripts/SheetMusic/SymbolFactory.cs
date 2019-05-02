using DuckOfDoom.SightReading.SheetMusic.Views;
using Zenject;

namespace DuckOfDoom.SightReading.SheetMusic
{
    public interface ISymbolsFactory
    {
        ISymbolView GetSymbol(Symbol symbol);
    }
    
    public class SymbolsFactory : ISymbolsFactory
    {
        [Inject] private ISymbolsPool Pool { get; set; }
         
        public ISymbolView GetSymbol(Symbol symbol)
        {
            var view = Pool.Pop();

            view.HasStem = view.IsFilled = symbol.Duration >= Duration.Quarter;
            
            // Add beaming options
            if (symbol.Duration == Duration.Eighth)
                view.FlagsCount = 1;
            else if (symbol.Duration == Duration.Sixteenth)
                view.FlagsCount = 2;
            else
                view.FlagsCount = 0;
                
            return view;
        }
    }

}

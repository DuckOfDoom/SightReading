using System;
using System.Collections.Generic;
using System.Linq;
using DuckOfDoom.SightReading.SheetMusic.Views;
using UnityEngine;

namespace DuckOfDoom.SightReading.SheetMusic
{
    public interface ISymbolsPool
    {
        SymbolView Pop();
        void Push(SymbolView view);
    }
    
    public class SymbolsPool : ISymbolsPool, IDisposable
    {
        private readonly Transform _heap;
        private readonly SymbolView _symbolViewPrefab;

        private const string SYMBOL_VIEW_PREFAB_PATH = "Prefabs/Symbol";

        private readonly Stack<SymbolView> _freeSymbols = new Stack<SymbolView>();

        private int PRELOAD_COUNT = 10;

        public SymbolsPool(Canvas rootCanvas)
        {
            var heapGo = new GameObject("SymbolPoolHeap").transform;
            heapGo.transform.parent = rootCanvas.transform;
            
            _heap = heapGo.transform;
            _symbolViewPrefab = Resources.Load<SymbolView>(SYMBOL_VIEW_PREFAB_PATH);
            
            Preload(PRELOAD_COUNT);
        }

        public void Dispose()
        {
            _freeSymbols.Clear();
            UnityEngine.Object.Destroy(_heap);
        }

        public SymbolView Pop()
        {
            if (!_freeSymbols.Any())
            {
                Debug.LogWarning("Popping from empty pool! Instantiating new object!");
                return InstantiateNewSymbol();
            }

            return _freeSymbols.Pop();
        }

        public void Push(SymbolView view)
        {
            view.transform.SetParent(_heap);
            _freeSymbols.Push(view);
        }

        private SymbolView InstantiateNewSymbol()
        {
            return UnityEngine.Object.Instantiate(_symbolViewPrefab, _heap, false);
        }
        
        private void Preload(int count)
        {
            for (var i = 0; i < count; i++)
                _freeSymbols.Push(InstantiateNewSymbol());
        }

    }
}

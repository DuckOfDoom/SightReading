using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using DuckOfDoom.SightReading.SheetMusic.Views;
using UnityEngine;

namespace DuckOfDoom.SightReading.SheetMusic
{
    public interface ISymbolsPool
    {
        ISymbolView Pop();
        void Push(ISymbolView view);
    }
    
    public class SymbolsPool : ISymbolsPool, IDisposable
    {
        private readonly Transform _heap;
        private readonly SymbolView _symbolViewPrefab;

        private const string SYMBOL_VIEW_PREFAB_PATH = "Prefabs/Symbol";
        private readonly Stack<ISymbolView> _freeSymbols = new Stack<ISymbolView>();
        
        private int PRELOAD_COUNT = 10;

        public SymbolsPool(Canvas rootCanvas)
        {
            var heapTransform = new GameObject("SymbolPoolHeap").transform;
            heapTransform.parent = rootCanvas.transform;
            heapTransform.gameObject.SetActive(false);

            _heap = heapTransform;
            _symbolViewPrefab = Resources.Load<SymbolView>(SYMBOL_VIEW_PREFAB_PATH);
            
            Preload(PRELOAD_COUNT);
        }

        public void Dispose()
        {
            _freeSymbols.Clear();
            UnityEngine.Object.Destroy(_heap);
        }

        public ISymbolView Pop()
        {
            if (!_freeSymbols.Any())
            {
                Debug.LogWarning("Popping from empty pool! Instantiating new object!");
                return InstantiateNewSymbol();
            }

            return _freeSymbols.Pop();
        }

        public void Push(ISymbolView view)
        {
            view.RectTransform.SetParent(_heap);
            _freeSymbols.Push(view);
        }

        private ISymbolView InstantiateNewSymbol()
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

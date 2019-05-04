using System.Collections.Generic;
using DuckOfDoom.SightReading.SheetMusic.Views;
using UnityEngine;
using Zenject;

namespace DuckOfDoom.SightReading.SheetMusic
{
    public interface IStaffView
    {
        /// <summary>
        ///     Places a symbol on a staff.
        /// </summary>
        void PlaceSymbol(int positionY, Duration symbolDuration, ISymbolView view);
        
        /// <summary>
        ///     Moves the stave horizontally
        /// </summary>
        void Advance(float distance);

        /// <summary>
        ///     Clear staff
        /// </summary>
        void Clear();
    }
    
    public class StaffView : MonoBehaviour, IStaffView
    {
#pragma warning disable 0649
        [SerializeField] private RectTransform _contents;
#pragma warning restore 0649
        
        [Inject] private ISymbolsPool SymbolsPool { get; set; }
        [Inject] private IStaffConfig Config { get; set; }
        
        private readonly List<ISymbolView> _symbolsOnTheStaff = new List<ISymbolView>();
        private float _lastSymbolPosition;
        private Duration _lastSymbolDuration;

        public void PlaceSymbol(int positionY, Duration symbolDuration, ISymbolView view)
        {
            var offsetMultiplier = 0f;
            if (_lastSymbolDuration != Duration.Undefined)
            {
                offsetMultiplier = Config.GetDistanceOffsetMultiplier(_lastSymbolDuration)
                    .Check($"[StaffView] There is no distance offset for duration '{_lastSymbolDuration}' in StaffConfig!")
                    .ValueOr(0);
            }

            var posX = _lastSymbolPosition + Config.DistanceBetweenSymbolsHorizontal * offsetMultiplier;
            var posY = positionY * Config.DistanceBetweenSymbolsVertical;
            var rt = view.RectTransform;
            
            rt.SetParent(_contents, false);
            rt.anchoredPosition = new Vector2(posX, posY);

            _symbolsOnTheStaff.Add(view);

            _lastSymbolPosition = posX;
            _lastSymbolDuration = symbolDuration;
        }

        public void Advance(float distance)
        {
            _contents.anchoredPosition -= new Vector2(distance, 0);
        }

        public void Clear()
        {
            _lastSymbolDuration = Duration.Undefined;
            _lastSymbolPosition = 0;
            
            for (var i = _symbolsOnTheStaff.Count - 1; i >= 0; i--)
            {
                SymbolsPool.Push(_symbolsOnTheStaff[i]);
                _symbolsOnTheStaff.RemoveAt(i);
            }
            
            // var keys = _symbolsOnTheStaff.Keys.ToList();
            //
            // foreach (var k in keys)
            // {
            //     SymbolsPool.Push(_symbolsOnTheStaff[k]);
            //     _symbolsOnTheStaff.Remove(k);
            // }
        }
    }
}

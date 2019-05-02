using Boo.Lang;
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
        /// <param name="positionX">Position on a horizontal axis</param>
        /// <param name="positionY">Position on a vertical axis. 0 is middle line of the staff.</param>
        /// <param name="view">Symbol to place</param>
        void PlaceSymbol(int positionX, int positionY, ISymbolView view);
        
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

        [SerializeField] private float _b1Position;
        [SerializeField] private float _distanceBetweenSymbolsVertical = 10f;
        [SerializeField] private float _distanceBetweenSymbolsHorizontal = 10f;
#pragma warning restore 0649
        
        [Inject] private ISymbolsPool SymbolsPool { get; set; }
        
        private readonly List<ISymbolView> _symbolsOnTheStaff = new List<ISymbolView>();

        public void PlaceSymbol(int positionX, int positionY, ISymbolView view)
        {
            var posX = positionX * _distanceBetweenSymbolsHorizontal;
            var posY = _b1Position + positionY * _distanceBetweenSymbolsVertical;
            var rt = view.RectTransform;
            
            rt.SetParent(_contents, false);
            rt.anchoredPosition = new Vector2(posX, posY);

            _symbolsOnTheStaff.Add(view);
        }

        public void Advance(float distance)
        {
            _contents.anchoredPosition -= new Vector2(distance, 0);
        }

        public void Clear()
        {
            for (var i = _symbolsOnTheStaff.Count - 1; i >= 0; i--)
            {
                SymbolsPool.Push(_symbolsOnTheStaff[i]);
                _symbolsOnTheStaff.RemoveAt(i);
            }
        }
    }
}

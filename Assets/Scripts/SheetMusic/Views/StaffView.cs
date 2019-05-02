using DuckOfDoom.SightReading.SheetMusic.Views;
using UnityEngine;

namespace DuckOfDoom.SightReading.SheetMusic
{
    public interface IStaffView
    {
        void PlaceSymbol(int position, ISymbolView view);
    }
    
    public class StaffView : MonoBehaviour, IStaffView
    {
#pragma warning disable 0649
        [SerializeField] private RectTransform _contents;
        [SerializeField] private float _distanceBetweenSymbols = 10f;
#pragma warning restore 0649

        public void PlaceSymbol(int position, ISymbolView view)
        {
            var posX = position * _distanceBetweenSymbols;
            var rt = view.RectTransform;
            
            rt.SetParent(_contents, false);
            rt.anchoredPosition = new Vector2(posX, rt.anchoredPosition.y);
        }
    }
}

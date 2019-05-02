using UnityEngine;
using UnityEngine.UI;

namespace DuckOfDoom.SightReading.SheetMusic.Views
{
    public interface ISymbolView
    {
        RectTransform RectTransform { get; }
        
        bool HasStem { set; }
        bool IsFilled { set; }
        int FlagsCount { set; }
    }
    
    /// <summary>
    ///      A view for a single symbol on the staff.
    /// </summary>
    public class SymbolView : MonoBehaviour, ISymbolView
    {
#pragma warning disable 0649
        [SerializeField] private Image _noteStem;
        [SerializeField] private Image _noteHeadEmpty;
        [SerializeField] private Image _noteHeadFilled;
        [SerializeField] private Image _noteFlag1;
        [SerializeField] private Image _noteFlag2;

        [SerializeField] private RectTransform _beamConnectionPoint1;
        [SerializeField] private RectTransform _beamConnectionPoint2;
#pragma warning restore 0649

        public RectTransform RectTransform => (RectTransform) transform;
        
        public bool HasStem { set => _noteStem.enabled = value; }

        public bool IsFilled
        {
            set
            {
                _noteHeadFilled.enabled = value;
                _noteHeadEmpty.enabled = !value;
            }
        }

        public int FlagsCount
        {
            set
            {
                _noteFlag1.enabled = value >= 1;
                _noteFlag2.enabled = value >= 2;
            }
        }
    }
}

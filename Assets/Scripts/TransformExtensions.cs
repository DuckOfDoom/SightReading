using UnityEngine;

namespace DuckOfDoom.SightReading
{
    public static class TransformExtensions
    {
        public static RectTransform GetRectTransform(this Transform tr) { return (RectTransform) tr; }
        
    }
}

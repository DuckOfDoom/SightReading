using Optional;
using UnityEngine;

namespace DuckOfDoom.SightReading
{
    public static class OptionExtensions
    {
        public static Option<T> Check<T>(this Option<T> opt, string errorMessage)
        {
            if (!opt.HasValue)
                Debug.LogError(errorMessage);

            return opt;
        }
    }
}

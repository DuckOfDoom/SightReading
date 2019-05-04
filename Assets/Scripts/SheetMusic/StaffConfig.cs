using System;
using System.Collections.Generic;
using System.Linq;
using Optional;
using Optional.Collections;
using UnityEngine;

namespace DuckOfDoom.SightReading.SheetMusic
{
    /// <summary>
    ///     Configuration for staff view.
    /// </summary>
    public interface IStaffConfig
    {
        Option<float> GetDistanceOffsetMultiplier(Duration symbolDuration);
        float DistanceBetweenSymbolsVertical { get; }
        float DistanceBetweenSymbolsHorizontal { get; }
    }
    
    [CreateAssetMenu(fileName = "StaffConfig", menuName = "StaffConfig", order = 51)]
    public class StaffConfig : ScriptableObject, IStaffConfig
    {
        [Serializable]
        public struct DistanceOffsetEntry
        {
#pragma warning disable 0649
            [SerializeField] private Duration _duration;
            [SerializeField] private float _distanceMultiplier;
#pragma warning restore 0649
            
            public Duration Duration => _duration;
            public float DistanceMultiplier => _distanceMultiplier;
        }
        
#pragma warning disable 0649
        [SerializeField] private float _distanceBetweenSymbolsVertical;
        [SerializeField] private float _distanceBetweenSymbolsHorizontal;
        [SerializeField] private List<DistanceOffsetEntry> _distanceMultipliersForDuration = new List<DistanceOffsetEntry>();
#pragma warning restore 0649
        
        private const string PATH = "Config/StaffConfig";
        
        private Dictionary<Duration, float> _durations = new Dictionary<Duration, float>();
        
        public float DistanceBetweenSymbolsVertical => _distanceBetweenSymbolsVertical;
        public float DistanceBetweenSymbolsHorizontal => _distanceBetweenSymbolsHorizontal;
        
        public static StaffConfig Load()
        {
            var cfg = Resources.Load<StaffConfig>(PATH);
            if (cfg == null)
                throw new NullReferenceException($"Can't load StaffConfig from path '{PATH}'!");
            
            return cfg;
        }

        public Option<float> GetDistanceOffsetMultiplier(Duration symbolDuration)
        {
            return _durations.GetValueOrNone(symbolDuration);
        }

        private void OnEnable() 
        {
            CacheDistancesForDuration();
        }

        private void CacheDistancesForDuration()
        {
            _durations = _distanceMultipliersForDuration.ToDictionary(e => e.Duration, e => e.DistanceMultiplier);
        }
    }

}

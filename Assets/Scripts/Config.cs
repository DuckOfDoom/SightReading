using System;
using UnityEngine;

namespace DuckOfDoom.SightReading
{
    public class Config : ScriptableObject
    {
        public static T Load<T>(string path) where T : Config
        {
            var cfg = Resources.Load<T>(path);
            if (cfg == null)
                throw new NullReferenceException($"Can't load Config of type {typeof(T)} from path '{path}'!");

            return cfg;
        }
    }
}
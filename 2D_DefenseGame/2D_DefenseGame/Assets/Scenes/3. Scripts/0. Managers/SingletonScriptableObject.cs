using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SingletonPattern
{
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.LoadAll<T>("").FirstOrDefault();
                    if (_instance == null) Debug.LogError($"Cannot find Singleton ScriptableObject of type {typeof(T)}");
                }

                return _instance;
            }
        }

    }
}


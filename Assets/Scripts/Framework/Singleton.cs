﻿using System;
using UnityEngine;

namespace Framework
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (applicationIsQuitting)
                    return null;

                if (instance != null) 
                    return instance;
                
                instance = FindObjectOfType<T>();
                if (instance != null) 
                    return instance;
                    
                var obj = new GameObject {name = typeof(T).Name};
                instance = obj.AddComponent<T>();

                return instance;
            }
        }

        private bool ignoreQuitting;
        private static bool applicationIsQuitting;

        public virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                ignoreQuitting = true;
                Destroy(gameObject);
            }
        }

        public virtual void OnDestroy()
        {
            if(!ignoreQuitting)
                applicationIsQuitting = true;
        }
    }
}
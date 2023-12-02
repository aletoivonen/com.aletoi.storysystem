using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StorySystem
{
    public class StorySingleton : MonoBehaviour
    {
        public static StorySingleton Instance { get { return _instance; } }

        private static StorySingleton _instance;

        [SerializeField] private StoryConfiguration _configuration;

        private ISaveGameContainer _saveGameContainer;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }

            DontDestroyOnLoad(this);
            _instance = this;

            Initialize();
        }

        private void Initialize()
        {
            if (_configuration.SlotCount == 1)
            {
                Type containerType = _configuration.SaveGameType;
                _saveGameContainer = (ISaveGameContainer)ScriptableObject.CreateInstance(containerType.Name);
                _saveGameContainer.LoadGame(0);
            }
        }

        public bool GetFlag(string flag)
        {
            return _saveGameContainer.GetFlag(flag);
        }
    }
}
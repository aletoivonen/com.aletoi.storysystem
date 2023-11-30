using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StorySystem
{
    public class StorySystem : MonoBehaviour
    {
        public static StorySystem Instance { get { return _instance; } }

        private static StorySystem _instance;

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
    }
}
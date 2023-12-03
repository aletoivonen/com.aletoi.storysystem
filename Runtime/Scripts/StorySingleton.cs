using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace StorySystem
{
    public class StorySingleton : MonoBehaviour
    {
        public static StorySingleton Instance => _instance;

        private static StorySingleton _instance;

        public static event Action<string, bool> OnFlagChanged;
        public static event Action<StoryPhase> OnPhaseChanged;

        [SerializeField] private StoryConfiguration _configuration;

        private ISaveGameContainer _saveGameContainer;

        private StoryPhase _currentPhase;
        
        [SerializeField] private StoryFlagItem _debugFlagToToggle;

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
            Type containerType = _configuration.SaveGameType;
            _saveGameContainer = (ISaveGameContainer)ScriptableObject.CreateInstance(containerType.Name);

            if (_configuration.SlotCount == 1)
            {
                LoadSlot(0);
            }
        }

        [ContextMenu("Debug Complete Flag")]
        private void DebugToggleFlag()
        {
            bool flag = GetFlag(_debugFlagToToggle.Flag);
            SetFlag(_debugFlagToToggle.Flag, !flag);
        }

        public void ActivateExit(string id)
        {
            StoryExit exit = _currentPhase.GetExit(id);

            if (exit != null && exit.GetStatus() == ExitStatus.Complete)
            {
                ChangePhase(exit.NextPhase.PhaseId);
            }
        }

        private void ChangePhase(string id)
        {
            StoryPhase phase = _configuration.GetPhase(_saveGameContainer.GetCurrentPhaseId());
            _currentPhase = phase;
            OnPhaseChanged?.Invoke(phase);
        }

        private void LoadSlot(int slot)
        {
            _saveGameContainer.LoadGame(0);
            _currentPhase = _configuration.GetPhase(_saveGameContainer.GetCurrentPhaseId());
        }

        public bool GetFlag(string flag) { return _saveGameContainer.GetFlag(flag); }

        public void SetFlag(string flag, bool value)
        {
            _saveGameContainer.SetFlag(flag, value);

            OnFlagChanged?.Invoke(flag, value);
        }

        public void GoalCompleted(StoryGoal goal)
        {
            
        }
    }
}
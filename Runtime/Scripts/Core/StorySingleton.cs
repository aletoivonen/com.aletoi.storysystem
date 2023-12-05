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
        public static event Action OnGameLoaded;

        [SerializeField] private StoryConfiguration _configuration;

        private ISaveGameContainer _saveGameContainer;

        [ReadOnly] [SerializeField] private StoryPhase _currentPhase;

        [SerializeField] private List<StoryFlagItem> _debugFlagToggle;

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

            StoryGoal.OnGoalCompleted += GoalCompleted;
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
            foreach (StoryFlagItem item in _debugFlagToggle)
            {
                bool flag = GetFlag(item.Flag);
                SetFlag(item.Flag, !flag);
            }
        }

        public void ActivateExit(string id)
        {
            StoryExit exit = _currentPhase.GetExit(id);

            if (exit != null && exit.GetStatus() == ExitStatus.Complete)
            {
                if (exit.NextPhase == null)
                {
                    Debug.Log("Game complete " + exit.ExitId);
                }
                else
                {
                    ChangePhase(exit.NextPhase.PhaseId);
                }

                _saveGameContainer.SaveGame();
            }
        }

        private void ChangePhase(string id)
        {
            Debug.Log("Change phase to " + id);
            StoryPhase phase = _configuration.GetPhase(id);
            _currentPhase = phase;
            OnPhaseChanged?.Invoke(phase);
        }

        private void LoadSlot(int slot)
        {
            _saveGameContainer.LoadGame(slot);

            string currentPhase = _saveGameContainer.GetCurrentPhaseId();

            if (string.IsNullOrEmpty(currentPhase))
            {
                _currentPhase = _configuration.DefaultPhase();
                _saveGameContainer.SetCurrentPhaseId(_currentPhase.PhaseId);
            }
            else
            {
                _configuration.GetPhase(currentPhase);
            }

            OnGameLoaded?.Invoke();
        }

        public void CheckProgression()
        {
            foreach (StoryExit exit in _currentPhase.Exits)
            {
                if (exit.GetStatus() == ExitStatus.Complete && exit.AutoActivate)
                {
                    ActivateExit(exit.NextPhase.PhaseId);
                }
            }
        }

        public bool GetFlag(string flag) { return _saveGameContainer.GetFlag(flag); }

        public void SetFlag(string flag, bool value)
        {
            Debug.Log("Set flag " + flag + " to " + value);
            _saveGameContainer.SetFlag(flag, value);

            OnFlagChanged?.Invoke(flag, value);

            CheckProgression();
        }

        public GoalStatus GetGoalFinished(string id) { return _saveGameContainer.GetGoalFinishStatus(id); }

        public void GoalCompleted(StoryGoal goal)
        {
            foreach (StoryFlagItem flag in goal.RewardFlags)
            {
                SetFlag(flag.Flag, true);
            }
        }
    }
}
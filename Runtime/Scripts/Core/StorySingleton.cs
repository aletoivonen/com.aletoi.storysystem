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
        public static event Action<StoryGoal> OnGoalCompleted;
        public static event Action<StoryGoal> OnGoalFailed;
        public static event Action<StoryExit> OnExitOpened;
        public static event Action<StoryExit> OnExitFailed;
        public static event Action<StoryExit> OnExitActivated;

        public StoryConfiguration Configuration => _configuration;

        [SerializeField] private StoryConfiguration _configuration;

        private ISaveGameContainer _saveGameContainer;

        private StoryState _storyState;
        private bool _checkingProgression;

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

            StoryGoal.InternalGoalCompleted += InternalGoalCompleted;
            StoryGoal.InternalGoalFailed += InternalGoalFailed;

            StoryExit.InternalExitOpened += InternalExitOpened;
            StoryExit.InternalExitFailed += InternalExitFailed;
        }

        private void OnDestroy()
        {
            StoryGoal.InternalGoalCompleted -= InternalGoalCompleted;
            StoryGoal.InternalGoalFailed -= InternalGoalFailed;

            StoryExit.InternalExitOpened -= InternalExitOpened;
            StoryExit.InternalExitFailed -= InternalExitFailed;
        }

        private void Initialize()
        {
            Type containerType = _configuration.SaveGameType;
            _saveGameContainer = (ISaveGameContainer)ScriptableObject.CreateInstance(containerType.Name);

            _storyState = new StoryState(_configuration);

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
            Debug.Log("Activate exit: " + id);
            StoryExit exit = _storyState.CurrentPhase.GetExit(id);

            if (exit.NextPhase == null)
            {
                Debug.Log("Game complete " + exit.ExitId);
            }
            else
            {
                Debug.Log("Change phase on activate exit");
                ChangePhase(exit.NextPhase.PhaseId);
            }

            SaveSlot();

            OnExitActivated?.Invoke(exit);
        }

        private void ChangePhase(string id)
        {
            Debug.Log("Change phase to " + id);
            StoryPhase phase = _configuration.GetPhase(id);
            _storyState.SetCurrentPhase(phase);
            OnPhaseChanged?.Invoke(phase);
        }

        [ContextMenu("Load game")]
        public void LoadSlot() => LoadSlot(-1);

        public void LoadSlot(int slot)
        {
            _saveGameContainer.LoadGame(slot);

            _storyState.LoadFromSave(ref _saveGameContainer);

            OnGameLoaded?.Invoke();
        }

        [ContextMenu("Save game")]
        public void SaveSlot() => SaveSlot(-1);

        public void SaveSlot(int slot = -1)
        {
            _storyState.WriteToSave(ref _saveGameContainer);
            _saveGameContainer.SaveGame(slot);
        }

        public void CheckProgression()
        {
            if (_checkingProgression) { return; }

            _checkingProgression = true;

            foreach (StoryExit exit in _storyState.CurrentPhase.Exits)
            {
                if (exit.GetStatus() == ExitStatus.Opened && exit.AutoActivate)
                {
                    ActivateExit(exit.ExitId);
                    break;
                }
            }

            _checkingProgression = false;
        }

        public Dictionary<string, bool> GetAllFlags()
        {
            return _storyState.Flags;
        }

        public StoryPhase GetCurrentPhase()
        {
            return _storyState.CurrentPhase;
        }

        public bool GetFlag(string flag)
        {
            return _storyState.GetFlag(flag);
        }

        public void SetFlag(string flag, bool value)
        {
            Debug.Log("Set flag " + flag + " to " + value);
            _storyState.SetFlag(flag, value);

            OnFlagChanged?.Invoke(flag, value);

            CheckProgression();
        }

        public GoalStatus GetGoalFinishStatus(string id)
        {
            return _storyState.GetGoalFinishStatus(id);
        }

        public void SetGoalFinished(string goalId, GoalStatus status)
        {
            _storyState.SetGoalFinishStatus(goalId, status);
        }

        public ExitStatus GetExitStatus(string id)
        {
            return _storyState.GetExitStatus(id);
        }

        public void SetExitStatus(string exitId, ExitStatus status)
        {
            _storyState.SetExitStatus(exitId, status);
        }

        public void InternalGoalCompleted(StoryGoal goal)
        {
            foreach (StoryFlagItem flag in goal.RewardFlags)
            {
                SetFlag(flag.Flag, true);
            }

            SetGoalFinished(goal.GoalId, GoalStatus.Complete);

            OnGoalCompleted?.Invoke(goal);
        }

        public void InternalGoalFailed(StoryGoal goal)
        {
            SetGoalFinished(goal.GoalId, GoalStatus.Failed);

            OnGoalFailed?.Invoke(goal);
        }

        public void InternalExitOpened(StoryExit exit)
        {
            SetExitStatus(exit.ExitId, ExitStatus.Opened);

            OnExitOpened?.Invoke(exit);
        }

        public void InternalExitFailed(StoryExit exit)
        {
            SetExitStatus(exit.ExitId, ExitStatus.Failed);

            OnExitFailed?.Invoke(exit);
        }
    }
}

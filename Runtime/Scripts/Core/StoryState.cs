using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StorySystem
{
    public class StoryState : IStoryState
    {
        public Dictionary<string, bool> Flags { get; private set; } = new Dictionary<string, bool>();
        public Dictionary<string, GoalStatus> GoalStatuses { get; private set; } = new Dictionary<string, GoalStatus>();

        public StoryPhase CurrentPhase { get; private set; }

        private StoryConfiguration _configuration;

        public StoryState(StoryConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void LoadFromSave(ref ISaveGameContainer saveGameContainer)
        {
            string currentPhaseId = saveGameContainer.GetCurrentPhaseId();

            if (string.IsNullOrEmpty(currentPhaseId))
            {
                StoryPhase phase = _configuration.DefaultPhase();

                CurrentPhase = phase;

                saveGameContainer.SetCurrentPhaseId(phase.PhaseId);
            }
            else
            {
                CurrentPhase = _configuration.GetPhase(currentPhaseId);
            }

            List<string> flags = _configuration.FlagDefinitions.Flags;

            foreach (string flag in flags)
            {
                Flags[flag] = saveGameContainer.GetFlag(flag);
            }

            foreach (StoryGoal goal in _configuration.GetAllGoals())
            {
                GoalStatuses[goal.GoalId] = saveGameContainer.GetGoalFinishStatus(goal.GoalId);
            }
        }

        public void WriteToSave(ref ISaveGameContainer saveGameContainer)
        {
            saveGameContainer.SetCurrentPhaseId(CurrentPhase.PhaseId);

            List<string> flags = _configuration.FlagDefinitions.Flags;

            foreach (string flag in flags)
            {
                saveGameContainer.SetFlag(flag, Flags[flag]);
            }

            foreach (StoryGoal goal in _configuration.GetAllGoals())
            {
                saveGameContainer.SetGoalFinishStatus(goal.GoalId, GoalStatuses[goal.GoalId]);
            }
        }

        public void SetCurrentPhase(StoryPhase phase)
        {
            CurrentPhase = phase;
        }

        public bool GetFlag(string id)
        {
            return Flags[id];
        }

        public void SetFlag(string id, bool value)
        {
            Flags[id] = value;
        }

        public GoalStatus GetGoalFinishStatus(string id)
        {
            return GoalStatuses[id];
        }

        public void SetGoalFinishStatus(string id, GoalStatus status)
        {
            GoalStatuses[id] = status;
        }
    }
}
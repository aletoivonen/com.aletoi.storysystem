using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace StorySystem
{
    [CreateAssetMenu(fileName = "StoryGoal", menuName = "StorySystem/StoryGoal", order = 2)]
    public class StoryGoal : ScriptableObject, IStoryGoal
    {
        public static event Action<StoryGoal> OnGoalCompleted;
        public static event Action<StoryGoal> OnGoalFailed;

        [SerializeField] private string _goalId;

        [SerializeField] private List<StoryCondition> _prerequisites;
        [SerializeField] private List<StoryCondition> _completeConditions;
        [SerializeField] private List<StoryCondition> _failConditions;

        [SerializeField] private List<StoryFlagItem> _rewardFlags;
        public string GoalId => _goalId;

        private bool _unlocked;
        private bool _init;

        public List<StoryFlagItem> RewardFlags => _rewardFlags;

        private GoalStatus _finishStatus = GoalStatus.InProgress;

        public virtual GoalStatus GetStatus()
        {
            if (!_init) { Initialize(); }

            if (_finishStatus != GoalStatus.InProgress)
            {
                Debug.Log("goal cached: " + _goalId + " " + _finishStatus);
                return _finishStatus;
            }

            if (!_unlocked && !_prerequisites.All(condition => condition.IsFulfilled()))
            {
                Debug.Log("goal locked: " + _goalId);
                return GoalStatus.Locked;
            }

            _unlocked = true;

            if (_failConditions.Any(condition => condition.IsFulfilled()))
            {
                Debug.Log("goal failed: " + _goalId);
                OnGoalFailed?.Invoke(this);
                _finishStatus = GoalStatus.Failed;

                return GoalStatus.Failed;
            }

            if (_completeConditions.All(condition => condition.IsFulfilled()))
            {
                Debug.Log("goal completed: " + _goalId);
                OnGoalCompleted?.Invoke(this);
                _finishStatus = GoalStatus.Complete;

                return GoalStatus.Complete;
            }

            Debug.Log("goal in progress: " + _goalId);
            return GoalStatus.InProgress;
        }

        private void Initialize()
        {
            _finishStatus = StorySingleton.Instance.GetGoalFinished(_goalId);
            Debug.Log("init goal " + _goalId + " to status " + _finishStatus);
            _init = true;
        }
    }

    public interface IStoryGoal
    {
        public GoalStatus GetStatus();
    }
}
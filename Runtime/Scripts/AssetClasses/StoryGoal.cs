using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace StorySystem
{
    [CreateAssetMenu(fileName = "StoryGoal", menuName = "StorySystem/StoryGoal", order = 2)]
    public class StoryGoal : ScriptableObject, IStoryGoal
    {
        public static event Action<StoryGoal> InternalGoalCompleted;
        public static event Action<StoryGoal> InternalGoalFailed;

        [SerializeField] private string _goalId;

        [SerializeField] private List<StoryCondition> _prerequisites = new List<StoryCondition>();
        [SerializeField] private List<StoryCondition> _completeConditions = new List<StoryCondition>();
        [SerializeField] private List<StoryCondition> _failConditions = new List<StoryCondition>();

        public List<StoryCondition> Prerequisites => _prerequisites;
        public List<StoryCondition> CompleteConditions => _completeConditions;
        public List<StoryCondition> FailConditions => _failConditions;

        [SerializeField] private List<StoryFlagItem> _rewardFlags;
        public string GoalId => _goalId;

        public List<StoryFlagItem> RewardFlags => _rewardFlags;

        public GoalStatus GetStatus(bool skipInvoke = false)
        {
            GoalStatus finishStatus = StorySingleton.Instance.GetGoalFinishStatus(_goalId);

            if (finishStatus != GoalStatus.InProgress && finishStatus != GoalStatus.Locked)
            {
                Debug.Log("goal cached: " + _goalId + " " + finishStatus);
                return finishStatus;
            }

            if (!_prerequisites.All(condition => condition != null && condition.IsFulfilled()))
            {
                Debug.Log("goal locked: " + _goalId);
                return GoalStatus.Locked;
            }

            if (_failConditions.Any(condition => condition != null && condition.IsFulfilled()))
            {
                Debug.Log("goal failed: " + _goalId);

                if (!skipInvoke)
                {
                    InternalGoalFailed?.Invoke(this);
                }

                return GoalStatus.Failed;
            }

            if (_completeConditions.All(condition => condition != null && condition.IsFulfilled()))
            {
                Debug.Log("goal completed: " + _goalId);
                if (!skipInvoke)
                {
                    InternalGoalCompleted?.Invoke(this);
                }

                return GoalStatus.Complete;
            }

            Debug.Log("goal in progress: " + _goalId);
            return GoalStatus.InProgress;
        }
    }
}
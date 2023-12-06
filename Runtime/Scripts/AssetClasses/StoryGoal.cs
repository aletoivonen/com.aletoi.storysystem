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

        public List<StoryFlagItem> RewardFlags => _rewardFlags;

        public virtual GoalStatus GetStatus()
        {
            GoalStatus finishStatus = StorySingleton.Instance.GetGoalFinishStatus(_goalId);
            
            if (finishStatus != GoalStatus.InProgress && finishStatus != GoalStatus.Locked)
            {
                Debug.Log("goal cached: " + _goalId + " " + finishStatus);
                return finishStatus;
            }

            if (!_prerequisites.All(condition => condition.IsFulfilled()))
            {
                Debug.Log("goal locked: " + _goalId);
                return GoalStatus.Locked;
            }

            if (_failConditions.Any(condition => condition.IsFulfilled()))
            {
                Debug.Log("goal failed: " + _goalId);
                OnGoalFailed?.Invoke(this);

                return GoalStatus.Failed;
            }

            if (_completeConditions.All(condition => condition.IsFulfilled()))
            {
                Debug.Log("goal completed: " + _goalId);
                OnGoalCompleted?.Invoke(this);

                return GoalStatus.Complete;
            }

            Debug.Log("goal in progress: " + _goalId);
            return GoalStatus.InProgress;
        }
    }
}
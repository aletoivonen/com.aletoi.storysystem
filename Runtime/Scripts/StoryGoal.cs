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
        
        [SerializeField] private List<StoryCondition> _prerequisites;
        [SerializeField] private List<StoryCondition> _conditions;
        [SerializeField] private string _goalId;
        [SerializeField] private List<StoryFlagItem> _rewardFlags;
        public string GoalId => _goalId;

        public virtual GoalStatus GetStatus()
        {
            if (!_prerequisites.All(condition => condition.GetStatus()))
            {
                return GoalStatus.Locked;
            }

            GoalStatus status = GoalStatus.Locked;

            if (_conditions.All(condition => condition.GetStatus()))
            {
                status = GoalStatus.Complete;
                
                OnGoalCompleted?.Invoke(this);
            }

            return status;
        }
    }

    public interface IStoryGoal
    {
        public GoalStatus GetStatus();
    }
}
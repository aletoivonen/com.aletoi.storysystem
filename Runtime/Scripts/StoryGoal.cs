using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace StorySystem
{
    [CreateAssetMenu(fileName = "StoryGoal", menuName = "StorySystem/StoryGoal", order = 2)]
    public class StoryGoal : ScriptableObject, IStoryGoal
    {
        [SerializeField] private List<StoryCondition> _prerequisites;
        [SerializeField] private List<StoryCondition> _conditions;
        [SerializeField] private string _goalId;
        [SerializeField] private List<StoryFlagItem> _rewardFlags;
        public string GoalId => _goalId;
        
        public virtual GoalStatus GetStatus()
        {
            GoalStatus status = GoalStatus.InProgress;

            if (_conditions.All(condition => condition.GetStatus()))
            {
                status = GoalStatus.Complete;
            }

            return status;
        }
    }

    public interface IStoryGoal
    {
        public GoalStatus GetStatus();
    }
}
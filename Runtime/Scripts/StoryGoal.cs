using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StorySystem
{
    [CreateAssetMenu(fileName = "StoryGoal", menuName = "StorySystem/StoryGoal", order = 2)]
    public class StoryGoal : ScriptableObject, IStoryGoal
    {
        [SerializeField] private List<StoryCondition> _conditions;
        
        public virtual GoalStatus GetStatus()
        {
            GoalStatus status = GoalStatus.Open;

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
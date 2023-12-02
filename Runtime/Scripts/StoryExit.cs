using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace StorySystem
{
    [CreateAssetMenu(fileName = "StoryExit", menuName = "StorySystem/StoryExit", order = 3)]
    public class StoryExit : ScriptableObject, IStoryExit
    {
        public virtual ExitStatus GetStatus()
        {
            ExitStatus status = ExitStatus.Possible;

            if (_requiredGoals.All(goal => goal.GetStatus() == GoalStatus.Complete))
            {
                status = ExitStatus.Complete;
            }

            return status;
        }

        [SerializeField] private List<StoryGoal> _requiredGoals;

        public List<StoryGoal> RequiredGoals => _requiredGoals;
    }

    public interface IStoryExit
    {
        public ExitStatus GetStatus();
    }
}
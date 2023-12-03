using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;


namespace StorySystem
{
    [CreateAssetMenu(fileName = "StoryExit", menuName = "StorySystem/StoryExit", order = 3)]
    public class StoryExit : ScriptableObject, IStoryExit
    {
        [SerializeField] private StoryPhase _nextPhase;
        [SerializeField] private List<StoryGoal> _requiredGoals;
        [SerializeField] private string _exitId;
        public string ExitId => _exitId;
        public StoryPhase NextPhase => _nextPhase;

        [SerializeField] private bool _autoActivate;
        
        public ExitStatus GetStatus()
        {
            ExitStatus status = ExitStatus.Complete;
            
            foreach (StoryGoal goal in _requiredGoals)
            {
                if (goal.GetStatus() == GoalStatus.Failed)
                {
                    return ExitStatus.Failed;
                }
                if (goal.GetStatus() == GoalStatus.Locked)
                {
                    return ExitStatus.Locked;
                }
                if (goal.GetStatus() == GoalStatus.InProgress)
                {
                    status = ExitStatus.InProgress;
                }
            }

            return status;
        }
    }

    public interface IStoryExit
    {
        public ExitStatus GetStatus();
    }
}
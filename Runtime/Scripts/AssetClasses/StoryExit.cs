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
        [SerializeField] private List<StoryGoal> _goals;
        [SerializeField] private string _exitId;
        public string ExitId => _exitId;
        public StoryPhase NextPhase => _nextPhase;

        public List<StoryGoal> Goals => _goals;

        public bool AutoActivate;
        
        public ExitStatus GetStatus(bool skipInvoke = false)
        {
            // TODO dont have default return as complete
            
            ExitStatus status = ExitStatus.Complete;
            
            foreach (StoryGoal goal in _goals)
            {
                GoalStatus goalStatus = goal.GetStatus(skipInvoke);
                
                if (goalStatus == GoalStatus.Failed)
                {
                    Debug.Log("exit failed: " + _exitId);
                    return ExitStatus.Failed;
                }
                if (goalStatus == GoalStatus.Locked)
                {
                    Debug.Log("exit locked: " + _exitId);
                    return ExitStatus.Locked;
                }
                if (goalStatus == GoalStatus.InProgress)
                {
                    status = ExitStatus.InProgress;
                }
            }

            Debug.Log("exit " + _exitId + " " + status.ToString());
            return status;
        }

        public List<StoryGoal> GetOpenGoals()
        {
            return _goals.Where(goal => goal.GetStatus(true) == GoalStatus.InProgress).ToList();
        }
    }

    public interface IStoryExit
    {
        public ExitStatus GetStatus(bool skipInvoke = false);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace StorySystem
{
    [CreateAssetMenu(fileName = "StoryExit", menuName = "StorySystem/StoryExit", order = 3)]
    public class StoryExit : ScriptableObject, IStoryExit
    {
        public static event Action<StoryExit> InternalExitFailed;
        public static event Action<StoryExit> InternalExitOpened;

        [SerializeField] private StoryPhase _nextPhase;
        [SerializeField] private List<StoryGoal> _goals;
        [SerializeField] private string _exitId;
        [SerializeField] private List<IStoryActivationAction> _activationActions = new List<IStoryActivationAction>();
        
        public string ExitId => _exitId;
        public StoryPhase NextPhase => _nextPhase;

        public List<StoryGoal> Goals => _goals;

        public bool AutoActivate;

        public ExitStatus GetStatus(bool skipInvoke = false)
        {
            ExitStatus status = StorySingleton.Instance.GetExitStatus(_exitId);

            if (status != ExitStatus.Locked && status != ExitStatus.InProgress)
            {
                return status;
            }

            if (_goals.Any(goal => goal.GetStatus(skipInvoke) == GoalStatus.Locked))
            {
                return ExitStatus.Locked;
            }

            if (_goals.Any(goal => goal.GetStatus(skipInvoke) == GoalStatus.Failed))
            {
                if (!skipInvoke)
                {
                    InternalExitFailed?.Invoke(this);
                }

                return ExitStatus.Failed;
            }

            if (_goals.All(goal => goal.GetStatus(skipInvoke) == GoalStatus.Complete))
            {
                if (!skipInvoke)
                {
                    InternalExitOpened?.Invoke(this);
                }

                return ExitStatus.Opened;
            }

            return ExitStatus.InProgress;

            Debug.Log("exit " + _exitId + " " + status.ToString());
            return status;
        }

        public void OnActivate()
        {
            foreach (IStoryActivationAction action in _activationActions)
            {
                action.OnActivate();
            }
        }

        public List<StoryGoal> GetOpenGoals()
        {
            return _goals.Where(goal => goal.GetStatus(true) == GoalStatus.InProgress).ToList();
        }
    }

    public interface IStoryExit
    {
        public ExitStatus GetStatus(bool skipInvoke = false);

        public void OnActivate();
    }

    public interface IStoryActivationAction
    {
        public void OnActivate();
    }
}

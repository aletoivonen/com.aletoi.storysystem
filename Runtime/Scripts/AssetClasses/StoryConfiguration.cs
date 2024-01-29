using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StorySystem
{
    [CreateAssetMenu(fileName = "StoryConfiguration", menuName = "StorySystem/StoryConfiguration", order = 1)]
    public class StoryConfiguration : ScriptableObject
    {
        public FlagDefinitions FlagDefinitions;
        [Range(1, 10)] public int SlotCount = 1;

        [SerializeField] private ScriptableObject SaveGameContainer;

        [SerializeField] private List<StoryPhase> _phases;

        public List<StoryPhase> Phases => _phases;

        public System.Type SaveGameType => SaveGameContainer.GetType();

        public StoryPhase GetPhase(string id)
        {
            return _phases.Find(phase => phase.PhaseId == id);
        }

        public StoryPhase DefaultPhase()
        {
            return _phases[0];
        }

        public List<StoryGoal> GetAllGoals()
        {
            List<StoryGoal> allGoals = new List<StoryGoal>();

            foreach (StoryExit exit in GetAllExits())
            {
                foreach (StoryGoal goal in exit.Goals)
                {
                    allGoals.Add(goal);
                }
            }

            return allGoals;
        }

        public List<StoryExit> GetAllExits()
        {
            List<StoryExit> allExits = new List<StoryExit>();

            foreach (StoryPhase phase in _phases)
            {
                foreach (StoryExit exit in phase.Exits)
                {
                    allExits.Add(exit);
                }
            }

            return allExits;
        }
    }
}

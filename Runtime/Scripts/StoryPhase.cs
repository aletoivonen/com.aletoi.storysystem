using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StorySystem
{
    [CreateAssetMenu(fileName = "StoryPhase", menuName = "StorySystem/StoryPhase", order = 7)]
    public class StoryPhase : ScriptableObject
    {
        [SerializeField] private List<StoryExit> _exits;
        [SerializeField] private string _phaseId;

        public string PhaseId => _phaseId;

        public StoryExit GetExit(string id)
        {
            return _exits.Find(exit => exit.ExitId == id);
        }
    }
    
}


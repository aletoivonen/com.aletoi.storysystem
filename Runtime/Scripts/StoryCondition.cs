using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StorySystem
{
    public abstract class StoryCondition : ScriptableObject, IStoryCondition
    {
        private List<string> _requiredFlags = new List<string>();
        private List<string> _blockingFlags = new List<string>();

        public bool GetStatus()
        {
            return _requiredFlags.All(flag => StorySingleton.Instance.GetFlag(flag)) &&
                   _blockingFlags.All(flag => !StorySingleton.Instance.GetFlag(flag));
        }
    }

    public interface IStoryCondition
    {
        public bool GetStatus();
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace StorySystem
{
    [CreateAssetMenu(fileName = "StoryCondition", menuName = "StorySystem/StoryCondition", order = 5)]
    public class StoryCondition : ScriptableObject, IStoryCondition
    {
        public List<StoryFlagItem> RequiredFlags = new List<StoryFlagItem>();
        public List<StoryFlagItem> BlockingFlags = new List<StoryFlagItem>();

        public bool IsFulfilled()
        {
            return RequiredFlags.All(flag => StorySingleton.Instance.GetFlag(flag.Flag))
                   && BlockingFlags.All(flag => !StorySingleton.Instance.GetFlag(flag.Flag));
        }
    }

    public class StoryFlagAttribute : PropertyAttribute { }

    [System.Serializable]
    public class StoryFlagItem
    {
        public static implicit operator string(StoryFlagItem i) => i.Flag;
        
        [StoryFlag]
        public string Flag;
    }

    public interface IStoryCondition
    {
        public bool IsFulfilled();
    }
}

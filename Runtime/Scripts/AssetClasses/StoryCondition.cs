using System;
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

    public class StoryFlagAttribute : PropertyAttribute
    {
    }

    [System.Serializable]
    public class StoryFlagItem : IEquatable<StoryFlagItem>
    {
        public static implicit operator string(StoryFlagItem i) => i.Flag;

        [StoryFlag]
        public string Flag;

        public bool Equals(StoryFlagItem other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Flag == other.Flag;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StoryFlagItem)obj);
        }

        public override int GetHashCode()
        {
            return (Flag != null ? Flag.GetHashCode() : 0);
        }

        public static bool operator ==(StoryFlagItem left, StoryFlagItem right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(StoryFlagItem left, StoryFlagItem right)
        {
            return !Equals(left, right);
        }

        public bool IsTrue()
        {
            return StorySingleton.Instance.GetFlag(Flag);
        }
    }

    public interface IStoryCondition
    {
        public bool IsFulfilled();
    }
}
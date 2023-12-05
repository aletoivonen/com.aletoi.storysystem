using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StorySystem
{
    [CreateAssetMenu(fileName = "FlagDefinitions", menuName = "StorySystem/FlagDefinitions", order = 1)]
    public class FlagDefinitions : ScriptableObject
    {
        [SerializeField] private List<string> _flags = new List<string>();

#if UNITY_EDITOR
        [HideInInspector] public string EnumPath;
#endif

        public List<string> Flags => _flags;
    }
}
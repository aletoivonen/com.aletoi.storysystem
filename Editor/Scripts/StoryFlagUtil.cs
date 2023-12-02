using System.Collections.Generic;
using StorySystem;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class StoryFlagUtil
{
    private static List<string> _flags = null;

    public static List<string> Flags
    {
        get
        {
            if (_flags == null)
            {
                var config = Resources.FindObjectsOfTypeAll<FlagDefinitions>();
                if (config == null || config.Length < 1)
                {
                    Debug.LogError("No FlagDefinitions found, please create the asset!");
                    return null;
                }

                _flags = config[0].Flags;
            }

            return _flags;
        }
        set => _flags = value;
    }
}
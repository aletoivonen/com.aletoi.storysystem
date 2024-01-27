using System;
using UnityEditor;
using System.IO;
using StorySystem;
using UnityEngine;
using System.Linq;

[CustomEditor(typeof(FlagDefinitions))]
[CanEditMultipleObjects]
public class FlagDefinitionsEditor : Editor
{
    private FlagDefinitions _defs;
    private readonly string _defaultPath = "Assets/Scripts/StorySystem/";

    public override void OnInspectorGUI()
    {
        _defs = (FlagDefinitions)target;

        DrawDefaultInspector();
        
        StoryFlagUtil.ClearCache();
    }

    private void Generate(string path)
    {
        if (path.ToCharArray().Last() != Char.Parse("/") || path.ToCharArray().Last() != Char.Parse("\\"))
        {
            path += "/";
        }

        string enumName = "StoryFlags";
        string[] enumEntries = _defs.Flags.ToArray();
        string filePathAndName = path + enumName + ".cs"; //The folder Scripts/Enums/ is expected to exist

        using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
        {
            streamWriter.WriteLine("namespace StorySystem {");
            streamWriter.WriteLine("public enum " + enumName);
            streamWriter.WriteLine("{");
            for (int i = 0; i < enumEntries.Length; i++)
            {
                streamWriter.WriteLine("\t" + enumEntries[i] + ",");
            }

            streamWriter.WriteLine("}");
            streamWriter.WriteLine("}");
        }

        AssetDatabase.Refresh();
    }
}
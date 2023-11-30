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

        string path = GUILayout.TextField(string.IsNullOrEmpty(_defs.EnumPath) ? _defaultPath : _defs.EnumPath);
        if (path != _defaultPath)
        {
            _defs.EnumPath = path;
        }
        
        if (GUILayout.Button("Generate enum"))
        {
            Generate(path);
        }
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
 
        using ( StreamWriter streamWriter = new StreamWriter( filePathAndName ) )
        {
            streamWriter.WriteLine( "public enum " + enumName );
            streamWriter.WriteLine( "{" );
            for( int i = 0; i < enumEntries.Length; i++ )
            {
                streamWriter.WriteLine( "\t" + enumEntries[i] + "," );
            }
            streamWriter.WriteLine( "}" );
        }
        AssetDatabase.Refresh();
    }
}
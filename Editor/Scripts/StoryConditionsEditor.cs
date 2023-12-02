using System.Collections;
using System.Collections.Generic;
using StorySystem;
using UnityEngine;
using UnityEditor;


namespace StorySystem
{
    [CustomEditor(typeof(StoryCondition))]
    public class StoryConditionsEditor : Editor
    {
        private List<string> choices => StoryFlagUtil.Flags;

        public override void OnInspectorGUI()
        {
           // StoryCondition condition = (StoryCondition)target;

            DrawDefaultInspector();

           // serializedObject.Update();
            
            /*int newMask = EditorGUILayout.MaskField(
                "Story Flags", condition.RequiredMask,
                choices.ToArray());*/

            /*if (newMask != condition.RequiredMask)
            {
                condition.RequiredFlags.Clear();
            
                for (int i = 0; i < sizeof(int) * 8; i++)
                {
                    int bitValue = (newMask >> i) & 1;

                    if (bitValue == 1 && i < choices.Count)
                    {
                        condition.RequiredFlags.Add(choices[i]);
                    }
                }

                foreach (var VARIABLE in condition.RequiredFlags)
                {
                    Debug.Log(VARIABLE);
                }

                condition.RequiredMask = newMask;
                serializedObject.ApplyModifiedProperties();
            }*/
        }
    }
}
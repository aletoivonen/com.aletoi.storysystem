using UnityEditor;
using UnityEngine;

namespace StorySystem
{
    [CustomPropertyDrawer(typeof(StoryFlagAttribute))]
    public class StoryFlagAttr : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            var flags = StoryFlagUtil.Flags;
            var oldIndex = flags.IndexOf(property.stringValue);
            int index = EditorGUI.Popup(position, oldIndex >= 0 ? oldIndex : 0,
                StoryFlagUtil.Flags.ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                property.stringValue = StoryFlagUtil.Flags[index];
            }
        }
    }

// Create a custom attribute for read-only fields
    public class StoryFlagAttribute : PropertyAttribute
    {
    }
}
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

            if (flags == null)
            {
                EditorGUI.HelpBox(position, "Flags not found, story not set up?", MessageType.Error);
                return;
            }

            var oldIndex = flags.IndexOf(property.stringValue);

            if (oldIndex < 0 && property.stringValue != "")
            {
                EditorGUI.HelpBox(position, "Invalid flag: " + property.stringValue, MessageType.Error);
            }

            int index = EditorGUI.Popup(position, oldIndex >= 0 ? oldIndex : -1, StoryFlagUtil.Flags.ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                property.stringValue = StoryFlagUtil.Flags[index];
            }
        }
    }
}

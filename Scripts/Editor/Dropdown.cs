using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace MyTools.UI
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class Dropdown : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // Get the target object
            MonoBehaviour script = (MonoBehaviour)target;

            // Check if the target object has DropdownIndex, DropdownLabel, and DropdownItems
            FieldInfo dropdownIndexField = script.GetType().GetField("DropdownIndex");
            FieldInfo dropdownLabelField = script.GetType().GetField("DropdownLabel");
            FieldInfo dropdownItemsField = script.GetType().GetField("DropdownItems");

            if (dropdownIndexField != null && dropdownLabelField != null && dropdownItemsField != null)
            {
                int dropdownIndex = (int)dropdownIndexField.GetValue(script);
                string dropdownLabel = (string)dropdownLabelField.GetValue(script);
                string[] dropdownItems = (string[])dropdownItemsField.GetValue(script);

                GUIContent label = new GUIContent(dropdownLabel);
                dropdownIndex = EditorGUILayout.Popup(label, dropdownIndex, dropdownItems);
                dropdownIndexField.SetValue(script, dropdownIndex);
            }
            else
            {
                return;
            }
        }
    }
}
    

using UnityEditor;
using UnityEngine;

namespace MyTools.SceneViewTools
{
    [InitializeOnLoad]
    static class SceneViewToggleResetAll
    {
        private const string MENU_NAME = "My Tools/Scene View Toolset/Reset All Views On Launch";

        public static bool _enabled;

        /// Called on load thanks to the InitializeOnLoad attribute
        static SceneViewToggleResetAll()
        {
            _enabled = EditorPrefs.GetBool(MENU_NAME, false);

            // Delaying until first editor tick so that the menu
            // will be populated before setting check state, and
            // re-apply correct action
            EditorApplication.delayCall += () => { PerformAction(_enabled); };
        }

        [MenuItem(MENU_NAME)]
        private static void ToggleAction()
        {
            // Toggling action
            PerformAction(!_enabled);
            Debug.Log($"MyTools: Reset All Views On Launch is {(_enabled ? "Enabled" : "Disabled")}");
        }

        private static void PerformAction(bool enabled)
        {
            // Set checkmark on menu item
            Menu.SetChecked(MENU_NAME, enabled);
            // Saving editor state
            EditorPrefs.SetBool(MENU_NAME, enabled);

            _enabled = enabled;
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace MyTools.FastPlay
{
    [InitializeOnLoad]
    static class ToggleFastPlay
    {
        private const string MENU_NAME = "My Tools/Fast Play Mode &f8";

        internal static bool _enabled;

        /// Called on load thanks to the InitializeOnLoad attribute
        static ToggleFastPlay()
        {
            _enabled = EditorPrefs.GetBool(MENU_NAME, true);

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
        }

        private static void PerformAction(bool enabled)
        {
            // Set checkmark on menu item
            Menu.SetChecked(MENU_NAME, enabled);
            // Saving editor state
            EditorPrefs.SetBool(MENU_NAME, enabled);

            _enabled = enabled;

            ToggleFastPlayMode(_enabled);
        }

        private static void ToggleFastPlayMode(bool enabled)
        {
            EditorSettings.enterPlayModeOptionsEnabled = enabled;
            AssetDatabase.Refresh();
            bool playModeState = EditorSettings.enterPlayModeOptionsEnabled;
            if (playModeState)
            {
                Debug.Log($"MyTools: Fast Play Mode is Enabled");
            }
            else
            {
                Debug.Log($"MyTools: Fast Play Mode is Disabled");
            }
        }
    }
}
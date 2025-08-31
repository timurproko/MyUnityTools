#if UNITY_EDITOR
using UnityEditor;

namespace MyTools
{
    [InitializeOnLoad]
    internal static class FastPlay
    {
        private const string MENU_NAME = Menus.MY_TOOLS_MENU + "Fast Play Mode";
        private const int ITEM_INDEX = Menus.MODES_INDEX + 301;
        private static bool _enabled;

        static FastPlay()
        {
            _enabled = EditorPrefs.GetBool(MENU_NAME, true);
            EditorApplication.delayCall += () => { PerformAction(_enabled); };
        }

        [MenuItem(MENU_NAME, priority = ITEM_INDEX)]
        private static void ToggleAction()
        {
            if (State.disabled) return;
            PerformAction(!_enabled);
        }

        [MenuItem(MENU_NAME, validate = true, priority = ITEM_INDEX)]
        private static bool ValidateToggleAction()
        {
            return !State.disabled;
        }

        private static void PerformAction(bool enabled)
        {
            if (State.disabled) return;

            Menu.SetChecked(MENU_NAME, enabled);
            EditorPrefs.SetBool(MENU_NAME, enabled);

            _enabled = enabled;
            ToggleFastPlayMode(_enabled);
        }

        private static void ToggleFastPlayMode(bool enabled)
        {
            if (State.disabled) return;

            EditorSettings.enterPlayModeOptionsEnabled = enabled;
            AssetDatabase.Refresh();
            var playModeState = EditorSettings.enterPlayModeOptionsEnabled;

            Utils.Log(playModeState ? "Fast Play Mode is Enabled" : "Fast Play Mode is Disabled");
        }
    }
}
#endif
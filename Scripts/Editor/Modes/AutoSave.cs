#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

namespace MyTools
{
    [InitializeOnLoad]
    internal static class AutoSave
    {
        private const string MENU_NAME = Menus.MY_TOOLS_MENU + "Auto Save on Play";
        private const int ITEM_INDEX = Menus.MODES_INDEX + 100;
        internal static bool _enabled;

        static AutoSave()
        {
            _enabled = EditorPrefs.GetBool(MENU_NAME, true);
            EditorApplication.delayCall += () => { PerformAction(_enabled); };
        }

        [MenuItem(MENU_NAME, priority = ITEM_INDEX)]
        private static void ToggleAction()
        {
            if (State.disabled) return;

            PerformAction(!_enabled);
            Debug.Log(Debug.DefaultPrefix, $"Auto Save on Play is {(_enabled ? "Enabled" : "Disabled")}");
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
        }
    }

    [InitializeOnLoad]
    static class AutoSaveExtension
    {
        static AutoSaveExtension()
        {
            EditorApplication.playModeStateChanged -= AutoSaveWhenPlaymodeStarts;
            EditorApplication.playModeStateChanged += AutoSaveWhenPlaymodeStarts;
        }

        private static void AutoSaveWhenPlaymodeStarts(PlayModeStateChange playModeStateChange)
        {
            if (State.disabled) return;

            if (playModeStateChange == PlayModeStateChange.ExitingEditMode && AutoSave._enabled)
            {
                EditorSceneManager.SaveOpenScenes();
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif
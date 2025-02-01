using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MyTools
{
    [InitializeOnLoad]
    internal static class AutoSave
    {
        private const string MENU_NAME = Menu.MY_TOOLS_MENU + "Auto Save on Play";
        internal static bool _enabled;

        static AutoSave()
        {
            _enabled = EditorPrefs.GetBool(MENU_NAME, true);

            EditorApplication.delayCall += () => { PerformAction(_enabled); };
        }

        [MenuItem(MENU_NAME)]
        private static void ToggleAction()
        {
            PerformAction(!_enabled);
            Debug.Log($"MyTools: Auto Save on Play is {(_enabled ? "Enabled" : "Disabled")}");
        }

        private static void PerformAction(bool enabled)
        {
            UnityEditor.Menu.SetChecked(MENU_NAME, enabled);
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
            if (playModeStateChange == PlayModeStateChange.ExitingEditMode && AutoSave._enabled)
            {
                EditorSceneManager.SaveOpenScenes();
                AssetDatabase.SaveAssets();
            }
        }
    }
}
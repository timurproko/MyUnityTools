using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MyTools.AutoSave
{
    [InitializeOnLoad]
    static class ToggleAutoSave
    {
        private const string MENU_NAME = "My Tools/Auto Save on Play";

        internal static bool _enabled;

        /// Called on load thanks to the InitializeOnLoad attribute
        static ToggleAutoSave()
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
            Debug.Log($"MyTools: Auto Save on Play is {(_enabled ? "Enabled" : "Disabled")}");
        }

        private static void PerformAction(bool enabled)
        {
            // Set checkmark on menu item
            Menu.SetChecked(MENU_NAME, enabled);
            // Saving editor state
            EditorPrefs.SetBool(MENU_NAME, enabled);

            _enabled = enabled;

            // Perform your logic here...
        }
    }

    /// <summary>
    /// This static class registers the autosave methods when playmode state changes
    /// in the editor.
    /// </summary>
    [InitializeOnLoad]
    static class AutoSaveExtension
    {
        /// <summary>
        /// Static constructor that gets called when Unity fires up or recompiles the scripts.
        /// (triggered by the InitializeOnLoad attribute above)
        /// </summary>
        static AutoSaveExtension()
        {
            // Normally I'm against defensive programming, and this is probably
            // not necessary. The intent is to make sure we don't accidentally
            // subscribe to the playModeStateChanged event more than once.
            EditorApplication.playModeStateChanged -= AutoSaveWhenPlaymodeStarts;
            EditorApplication.playModeStateChanged += AutoSaveWhenPlaymodeStarts;
        }

        /// <summary>
        /// This method saves open scenes and other assets when entering playmode. 
        /// </summary>
        /// <param name="playModeStateChange">The enum that specifies how the playmode is changing in the editor.</param>
        private static void AutoSaveWhenPlaymodeStarts(PlayModeStateChange playModeStateChange)
        {
            // If we're exiting edit mode (entering play mode)
            if (playModeStateChange == PlayModeStateChange.ExitingEditMode && ToggleAutoSave._enabled)
            {
                Debug.Log("MyTools: Saving Scenes and Assets");

                // Save the open scenes and any assets.
                EditorSceneManager.SaveOpenScenes();
                AssetDatabase.SaveAssets();
            }
        }
    }
}
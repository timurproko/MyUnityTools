using UnityEditor;
using UnityEngine;

namespace MyTools.MaximizeOnPlay
{
    [InitializeOnLoad]
    static class ToggleMaximizeOnPlay
    {
        private const string MENU_NAME = "My Tools/Maximize Game View on Play &f11";

        internal static bool _enabled;

        /// Called on load thanks to the InitializeOnLoad attribute
        static ToggleMaximizeOnPlay()
        {
            _enabled = EditorPrefs.GetBool(MENU_NAME, true);

            // Delaying until first editor tick so that the menu
            // will be populated before setting check state, and
            // re-apply correct action
            EditorApplication.delayCall += () => { PerformAction(_enabled); };

            // Subscribe to play mode state change
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        [MenuItem(MENU_NAME)]
        private static void ToggleAction()
        {
            // Toggling action
            PerformAction(!_enabled);
            Debug.Log($"MyTools: Maximize Game View on Play is {(_enabled ? "Enabled" : "Disabled")}");
        }

        private static void PerformAction(bool enabled)
        {
            // Set checkmark on menu item
            Menu.SetChecked(MENU_NAME, enabled);
            // Saving editor state
            EditorPrefs.SetBool(MENU_NAME, enabled);

            _enabled = enabled;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (_enabled && state == PlayModeStateChange.EnteredPlayMode)
            {
                // Delay the action to ensure the Game view is properly initialized
                EditorApplication.delayCall += () =>
                {
                    // Focus the Game view
                    EditorWindow gameView = GetGameView();
                    if (gameView != null)
                    {
                        gameView.maximized = true;
                    }
                };
            }
        }

        private static EditorWindow GetGameView()
        {
            System.Type gameViewType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
            return EditorWindow.GetWindow(gameViewType);
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace MyTools.MaximizeOnPlay
{
    [InitializeOnLoad]
    static class ToggleMaximizeOnPlay
    {
        private const string MENU_NAME = "My Tools/Maximize Game View on Play &f11";

        private static bool _enabled;
        private static bool _isOtherView;

        // Called on load thanks to the InitializeOnLoad attribute
        static ToggleMaximizeOnPlay()
        {
            _enabled = EditorPrefs.GetBool(MENU_NAME, true);

            // Delaying until first editor tick so that the menu
            // will be populated before setting check state, and
            // re-apply correct action
            EditorApplication.delayCall += () => { PerformAction(_enabled); };

            // Subscribe to play mode state change
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            // Subscribe to pause state change
            EditorApplication.pauseStateChanged += OnPauseStateChanged;
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
                if (SceneView.lastActiveSceneView.hasFocus)
                {
                    _isOtherView = true;
                }
                else
                {
                    _isOtherView = false;
                }

                // Delay the action to ensure the Game view is properly initialized
                EditorApplication.delayCall += () =>
                {
                    {
                        MaximizeGameView(true);
                    }
                };
            }
            else if (_enabled && state == PlayModeStateChange.ExitingPlayMode)
            {
                EditorApplication.delayCall += () =>
                {
                    {
                        MaximizeGameView(false);
                        if (_isOtherView)
                        {
                            GetView("UnityEditor.SceneView");
                        }
                    }
                };
                
            }
        }

        private static void OnPauseStateChanged(PauseState state)
        {
            if (_enabled)
            {
                if (state == PauseState.Paused)
                {
                    MaximizeGameView(false);
                }
                else if (state == PauseState.Unpaused)
                {
                    MaximizeGameView(true);
                }
            }
        }

        private static void MaximizeGameView(bool maximize)
        {
            // Focus the Game view and set maximized state
            EditorWindow gameView = GetView("UnityEditor.GameView");
            if (gameView != null)
            {
                gameView.maximized = maximize;
            }
        }

        private static EditorWindow GetView(string name)
        {
            System.Type viewType = typeof(Editor).Assembly.GetType(name);
            return EditorWindow.GetWindow(viewType);
        }
    }
}
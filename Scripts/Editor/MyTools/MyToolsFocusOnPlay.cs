using UnityEditor;
using UnityEngine;

namespace MyTools
{
    [InitializeOnLoad]
    static class FocusOnPlay
    {
        private const string MENU_NAME = "My Tools/Focus Game View on Play";
        private static bool _enabled;
        private static bool _isOtherView;

        // Called on load thanks to the InitializeOnLoad attribute
        static FocusOnPlay()
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
            Debug.Log($"MyTools: Focus GameView on Play is {(_enabled ? "Enabled" : "Disabled")}");
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
                if (SceneView.lastActiveSceneView != null)
                {
                    if (SceneView.lastActiveSceneView.hasFocus)
                    {
                        _isOtherView = true;
                    }
                    else
                    {
                        _isOtherView = false;
                    }
                }

                // Delay the action to ensure the Game view is properly initialized
                EditorApplication.delayCall += () =>
                {
                    {
                        if (SceneView.lastActiveSceneView != null)
                        {
                            if (!SceneView.lastActiveSceneView.maximized)
                            {
                                FocusView("GameView");
                            }
                        }
                        else if (SceneView.lastActiveSceneView == null)
                        {
                            FocusView("GameView");
                        }
                    }
                };
            }
            else if (_enabled && state == PlayModeStateChange.ExitingPlayMode)
            {
                EditorApplication.delayCall += () =>
                {
                    {
                        if (SceneView.lastActiveSceneView != null)
                        {
                            if (!SceneView.lastActiveSceneView.maximized)
                            {
                                if (_isOtherView && !MyTools.GetView("UnityEditor.GameView").maximized)
                                {
                                    FocusView("SceneView");
                                }
                            }
                        }
                    }
                };
            }
        }

        private static void OnPauseStateChanged(PauseState state)
        {
            if (_enabled)
            {
                if (SceneView.lastActiveSceneView != null)
                {
                    if (!SceneView.lastActiveSceneView.maximized)
                    {
                        if (state == PauseState.Unpaused)
                        {
                            FocusView("GameView");
                        }
                    }
                }
                else if (SceneView.lastActiveSceneView == null)
                {
                    if (state == PauseState.Unpaused)
                    {
                        FocusView("GameView");
                    }
                }
            }
        }

        private static void FocusView(string name)
        {
            EditorApplication.delayCall += () => { MyTools.GetView("UnityEditor." + name).Focus(); };
        }
    }
}
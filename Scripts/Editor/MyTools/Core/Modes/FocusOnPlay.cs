using UnityEditor;
using UnityEngine;

namespace MyTools
{
    [InitializeOnLoad]
    internal static class FocusOnPlay
    {
        private const string MENU_NAME = Menu.MY_TOOLS_MENU + "Focus Game View on Play &f8"; // Alt+F8
        private static bool _enabled;
        private static bool _isOtherView;

        static FocusOnPlay()
        {
            _enabled = EditorPrefs.GetBool(MENU_NAME, true);

            EditorApplication.delayCall += () => { PerformAction(_enabled); };
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            EditorApplication.pauseStateChanged += OnPauseStateChanged;
        }

        [MenuItem(MENU_NAME)]
        private static void ToggleAction()
        {
            PerformAction(!_enabled);
            Debug.Log($"MyTools: Focus GameView on Play is {(_enabled ? "Enabled" : "Disabled")}");
        }

        private static void PerformAction(bool enabled)
        {
            UnityEditor.Menu.SetChecked(MENU_NAME, enabled);
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
                                if (_isOtherView && !Functions.GetView("UnityEditor.GameView").maximized)
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
            EditorApplication.delayCall += () => { Functions.GetView("UnityEditor." + name).Focus(); };
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace MyTools
{
    [InitializeOnLoad]
    internal static class MaximizeOnPlay
    {
        private const string MENU_NAME = Menu.MY_TOOLS_MENU + "Maximize Game View on Play &f11";
        private static bool _enabled;

        static MaximizeOnPlay()
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
            Debug.Log($"MyTools: Maximize GameView on Play is {(_enabled ? "Enabled" : "Disabled")}");
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
            if (SceneView.lastActiveSceneView != null)
            {
                if (!SceneView.lastActiveSceneView.maximized)
                {
                    EditorWindow gameView = Functions.GetView("UnityEditor.GameView");
                    if (gameView != null)
                    {
                        gameView.maximized = maximize;
                    }
                }
            }

            else if (SceneView.lastActiveSceneView == null)
            {
                EditorWindow gameView = Functions.GetView("UnityEditor.GameView");
                if (gameView != null)
                {
                    gameView.maximized = maximize;
                }
            }
        }
    }
}
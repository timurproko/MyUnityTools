using UnityEditor;
using UnityEngine;

namespace MyTools
{
    [InitializeOnLoad]
    internal static class ChangeViewOnPlay
    {
        private const int FOCUS_INDEX = Menus.MODES_INDEX + 102;
        private const int MAXIMIZE_INDEX = Menus.MODES_INDEX + 103;
        private const string FOCUS_MENU = Menus.MY_TOOLS_MENU + "Focus Game View on Play";
        private const string MAXIMIZE_MENU = Menus.MY_TOOLS_MENU + "Maximize Active View on Play";

        private static bool _maximizeEnabled;
        private static bool _focusEnabled;
        private static bool _isMaximized;
        private static bool _wasSceneViewActive;
        private static bool _wasGameViewActive;

        static ChangeViewOnPlay()
        {
            _maximizeEnabled = EditorPrefs.GetBool(MAXIMIZE_MENU, true);
            _focusEnabled = EditorPrefs.GetBool(FOCUS_MENU, true);

            EditorApplication.delayCall += () =>
            {
                SetMenuChecked(MAXIMIZE_MENU, _maximizeEnabled);
                SetMenuChecked(FOCUS_MENU, _focusEnabled);
            };

            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            EditorApplication.pauseStateChanged += OnPauseStateChanged;
        }

        [MenuItem(MAXIMIZE_MENU, priority = MAXIMIZE_INDEX)]
        private static void ToggleMaximize()
        {
            _maximizeEnabled = !_maximizeEnabled;
            SetMenuChecked(MAXIMIZE_MENU, _maximizeEnabled);
            Debug.Log($"MyTools: Maximize Active View on Play is {(_maximizeEnabled ? "Enabled" : "Disabled")}");
        }

        [MenuItem(FOCUS_MENU, priority = FOCUS_INDEX)]
        private static void ToggleFocus()
        {
            _focusEnabled = !_focusEnabled;
            SetMenuChecked(FOCUS_MENU, _focusEnabled);
            Debug.Log($"MyTools: Focus GameView on Play is {(_focusEnabled ? "Enabled" : "Disabled")}");
        }

        private static void SetMenuChecked(string menuName, bool enabled)
        {
            UnityEditor.Menu.SetChecked(menuName, enabled);
            EditorPrefs.SetBool(menuName, enabled);
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                _wasSceneViewActive = UnityEditor.SceneView.lastActiveSceneView != null && UnityEditor.SceneView.lastActiveSceneView.hasFocus;
                _wasGameViewActive = GetActiveView()?.GetType().Name == "GameView";

                if (_focusEnabled)
                {
                    EditorApplication.update += ExecuteFocusAndMaximize;
                }
                else
                {
                    EditorApplication.update += ExecuteFocusLastActiveView;
                }
            }
            else if (state == PlayModeStateChange.ExitingPlayMode)
            {
                if (_focusEnabled)
                {
                    EditorApplication.delayCall += RestoreSceneView;
                }
            }
        }

        private static void ExecuteFocusAndMaximize()
        {
            EditorApplication.update -= ExecuteFocusAndMaximize;

            FocusView("GameView");

            EditorApplication.delayCall += () => EditorApplication.update += ExecuteMaximize;
        }

        private static void ExecuteFocusLastActiveView()
        {
            EditorApplication.update -= ExecuteFocusLastActiveView;

            if (_wasSceneViewActive)
            {
                FocusView("SceneView");
            }
            else if (_wasGameViewActive)
            {
                FocusView("GameView");
            }
            else
            {
                Debug.LogWarning("MyTools: No active SceneView or GameView found to focus.");
            }

            EditorApplication.delayCall += () => EditorApplication.update += ExecuteMaximize;
        }

        private static void ExecuteMaximize()
        {
            EditorApplication.update -= ExecuteMaximize;

            if (_maximizeEnabled)
            {
                MaximizeActiveView();
            }
        }

        private static void OnPauseStateChanged(PauseState state)
        {
            if (_maximizeEnabled)
            {
                MaximizeActiveView(state == PauseState.Unpaused);
            }
        }

        private static void MaximizeActiveView(bool maximize = true)
        {
            var activeView = GetActiveView();
            if (activeView != null)
            {
                if (maximize)
                {
                    _isMaximized = activeView.maximized;
                    activeView.maximized = true;
                }
                else
                {
                    activeView.maximized = _isMaximized;
                }
            }
        }

        private static EditorWindow GetActiveView()
        {
            var allWindows = Resources.FindObjectsOfTypeAll<EditorWindow>();

            foreach (var window in allWindows)
            {
                if (window.GetType().Name == "GameView" || window.GetType().Name == "SceneView")
                {
                    if (window.hasFocus || window == EditorWindow.focusedWindow)
                    {
                        return window;
                    }
                }
            }

            Debug.LogWarning("MyTools: No active GameView or SceneView found to maximize.");
            return null;
        }

        private static void FocusView(string name)
        {
            EditorApplication.delayCall += () =>
            {
                var view = Functions.GetView("UnityEditor." + name);
                if (view != null)
                {
                    view.Focus();
                }
            };
        }

        private static void RestoreSceneView()
        {
            if (_wasSceneViewActive)
            {
                FocusView("SceneView");
            }
        }
    }
}
// using UnityEditor;
// using UnityEngine;
//
// namespace MyTools
// {
//     [InitializeOnLoad]
//     internal static class MaximizeOnPlay
//     {
//         private const string MENU_NAME = Menus.MY_TOOLS_MENU + "Maximize Active View on Play &f11"; // Alt+F11
//         private const int ITEM_INDEX = Menus.MODES_INDEX + 103;
//         private static bool _enabled;
//         private static bool _isMaximized;
//
//         static MaximizeOnPlay()
//         {
//             _enabled = EditorPrefs.GetBool(MENU_NAME, true);
//
//             EditorApplication.delayCall += () => { PerformAction(_enabled); };
//             EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
//             EditorApplication.pauseStateChanged += OnPauseStateChanged;
//         }
//
//         [MenuItem(MENU_NAME, priority = ITEM_INDEX)]
//         private static void ToggleAction()
//         {
//             PerformAction(!_enabled);
//             Debug.Log($"MyTools: Maximize Active View on Play is {(_enabled ? "Enabled" : "Disabled")}");
//         }
//
//         private static void PerformAction(bool enabled)
//         {
//             UnityEditor.Menu.SetChecked(MENU_NAME, enabled);
//             EditorPrefs.SetBool(MENU_NAME, enabled);
//
//             _enabled = enabled;
//         }
//
//         private static void OnPlayModeStateChanged(PlayModeStateChange state)
//         {
//             if (_enabled)
//             {
//                 if (state == PlayModeStateChange.EnteredPlayMode)
//                 {
//                     EditorApplication.delayCall += () =>
//                     {
//                         MaximizeActiveView(true);
//                     };
//                 }
//                 else if (state == PlayModeStateChange.ExitingPlayMode)
//                 {
//                     EditorApplication.delayCall += () =>
//                     {
//                         MaximizeActiveView(false);
//                     };
//                 }
//             }
//         }
//
//         private static void OnPauseStateChanged(PauseState state)
//         {
//             if (_enabled)
//             {
//                 if (state == PauseState.Paused)
//                 {
//                     MaximizeActiveView(false);
//                 }
//                 else if (state == PauseState.Unpaused)
//                 {
//                     MaximizeActiveView(true);
//                 }
//             }
//         }
//
//         private static void MaximizeActiveView(bool maximize)
//         {
//             var allWindows = Resources.FindObjectsOfTypeAll<EditorWindow>();
//
//             EditorWindow activeView = null;
//             foreach (var window in allWindows)
//             {
//                 if (window.GetType().Name == "GameView" || window.GetType().Name == "SceneView")
//                 {
//                     if (window.hasFocus || window == EditorWindow.focusedWindow)
//                     {
//                         activeView = window;
//                         break;
//                     }
//                 }
//             }
//
//             if (activeView != null)
//             {
//                 if (maximize)
//                 {
//                     _isMaximized = activeView.maximized;
//                     activeView.maximized = true;
//                 }
//                 else
//                 {
//                     activeView.maximized = _isMaximized;
//                 }
//             }
//             else
//             {
//                 Debug.LogWarning("MyTools: No active GameView or SceneView found to maximize.");
//             }
//         }
//     }
// }
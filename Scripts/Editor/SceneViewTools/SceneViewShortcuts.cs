using UnityEditor;
using UnityEngine;

namespace MyTools.SceneViewTools
{
    class SceneViewShortcuts
    {
        [MenuItem("My Tools/Scene View Toolset/Reset Current View &h", priority = 100)] //  Alt+H
        public static void ResetSceneViewCamera()
        {
            Tools.ActivateWindowUnderCursor();
            if (SceneViewRef.sceneView != null)
            {
                SceneViewResetAll.ResetView(SceneViewSaveData.GetLastSavedSceneViewType());

                SceneViewResetAll.RedrawLastSavedSceneView();
            }
        }

        [MenuItem("My Tools/Scene View Toolset/Reset All Views %&h", priority = 101)] // Ctrl+Alt+H
        public static void ResetAllViews()
        {
            SceneViewResetAll.ResetAllSceneViews();
        }

        [MenuItem("My Tools/Scene View Toolset/Log Camera Data &l", priority = 102)] //  Alt+L
        static void LogSceneViewCameraData()
        {
            SceneViewRef.sceneView = SceneView.lastActiveSceneView;
            Debug.Log(
                $"X:{SceneViewRef.sceneView.rotation.eulerAngles.x}," +
                $"Y:{SceneViewRef.sceneView.rotation.eulerAngles.y}," +
                $"Z:{SceneViewRef.sceneView.rotation.eulerAngles.z}");
        }

        [MenuItem("My Tools/Scene View Toolset/Frame Selected &f", priority = 103)] //  Alt+F
        static void FrameSelected()
        {
            Tools.ActivateWindowUnderCursor();
            SceneView.FrameLastActiveSceneView();
        }

        [MenuItem("My Tools/Scene View Toolset/Toggle Skybox &s", priority = 104)] //  Alt+S
        static void ToggleSkybox()
        {
            Tools.ActivateWindowUnderCursor();
            SceneViewRef.sceneView = SceneView.lastActiveSceneView;
            if (SceneViewRef.sceneView.sceneViewState.skyboxEnabled)
            {
                DisableSkybox();
            }
            else
            {
                EnableSkybox();
            }
        }

        [MenuItem("My Tools/Scene View Toolset/Toggle All Gizmos &g", priority = 105)] // Alt+G
        public static void ToggleSceneViewGizmos()
        {
            Tools.ActivateWindowUnderCursor();
            var currentValue = GetSceneViewGizmosEnabled();
            SetSceneViewGizmos(!currentValue);
        }

        public static void SetSceneViewGizmos(bool gizmosOn)
        {
#if UNITY_EDITOR
            SceneView sv = EditorWindow.GetWindow<SceneView>(null, false);
            sv.drawGizmos = gizmosOn;
#endif
        }

        public static bool GetSceneViewGizmosEnabled()
        {
#if UNITY_EDITOR
            SceneView sv = EditorWindow.GetWindow<SceneView>(null, false);
            return sv.drawGizmos;
#else
            return false;
#endif
        }

        public static void DisableSkybox()
        {
            SceneViewRef.sceneView = SceneView.lastActiveSceneView;
            SceneViewRef.sceneView.sceneViewState.showSkybox = false;
        }

        public static void EnableSkybox()
        {
            SceneViewRef.sceneView = SceneView.lastActiveSceneView;
            SceneViewRef.sceneView.sceneViewState.showSkybox = true;
        }

        [MenuItem("My Tools/Scene View Toolset/Toggle Projection _o", priority = 106)] // O
        public static void ToggleProjection()
        {
            SceneViewRef.sceneView = SceneView.lastActiveSceneView;
            if (SceneViewRef.sceneView == null)
            {
                return;
            }

            SceneViewRef.sceneView.orthographic = !SceneViewRef.sceneView.orthographic;
            SceneViewRef.sceneView.Repaint();
        }

        [MenuItem("My Tools/Scene View Toolset/Toggle 2D View &o", priority = 107)] // Alt+O
        public static void Toggle2DView()
        {
            SceneViewRef.sceneView = SceneView.lastActiveSceneView;
            if (SceneViewRef.sceneView != null)
            {
                SceneViewRef.sceneView.in2DMode = !SceneViewRef.sceneView.in2DMode;

                if (SceneViewRef.sceneView.in2DMode && SceneViewRef.SceneViewType == SceneViewType.Perspective)
                {
                    DisableSkybox();
                }
                else if (!SceneViewRef.sceneView.in2DMode &&
                         SceneViewRef.SceneViewType == SceneViewType.Perspective)
                {
                    EnableSkybox();
                }

                SceneViewRef.sceneView.Repaint();
            }
        }
    }
}
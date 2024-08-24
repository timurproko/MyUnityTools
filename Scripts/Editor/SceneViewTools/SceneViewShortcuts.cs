using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace MyTools.SceneViewTools
{
    class SceneViewShortcuts
    {
        private static SceneView sceneView;
        public static bool wasIn2DMode;

        [MenuItem("My Tools/Scene View Toolset/Log Camera Data &l", priority = 100)] //  Alt+L
        static void LogSceneViewCameraData()
        {
            sceneView = SceneViewNavigation.GetLastActiveSceneView();
            Debug.Log(
                $"X:{sceneView.rotation.eulerAngles.x}," +
                $"Y:{sceneView.rotation.eulerAngles.y}," +
                $"Z:{sceneView.rotation.eulerAngles.z}");
        }

        [MenuItem("My Tools/Scene View Toolset/Frame Selected &f", priority = 101)] //  Alt+F
        static void FrameSelected()
        {
            Tools.ActivateWindowUnderCursor();
            SceneView.FrameLastActiveSceneView();
        }

        [MenuItem("My Tools/Scene View Toolset/Toggle Skybox &s", priority = 102)] //  Alt+S
        static void ToggleSkybox()
        {
            Tools.ActivateWindowUnderCursor();
            sceneView = SceneViewNavigation.GetLastActiveSceneView();
            if (sceneView.sceneViewState.skyboxEnabled)
            {
                DisableSkybox();
            }
            else
            {
                EnableSkybox();
            }
        }

        [MenuItem("My Tools/Scene View Toolset/Toggle All Gizmos &g", priority = 103)] // Alt+G
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
            sceneView = SceneViewNavigation.GetLastActiveSceneView();
            sceneView.sceneViewState.showSkybox = false;
        }

        public static void EnableSkybox()
        {
            sceneView = SceneViewNavigation.GetLastActiveSceneView();
            sceneView.sceneViewState.showSkybox = true;
        }

        [MenuItem("My Tools/Scene View Toolset/Toggle Projection _o", priority = 104)] // O
        public static void ToggleProjection()
        {
            sceneView = SceneView.lastActiveSceneView;
            if (sceneView == null)
            {
                return;
            }

            sceneView.orthographic = !sceneView.orthographic;
            sceneView.Repaint();

            if (SceneViewNavigation.sceneViewType != SceneViewType.Perspective)
            {
                SceneViewNavigation.sceneViewType = SceneViewType.Perspective;
            }
        }

        [MenuItem("My Tools/Scene View Toolset/Toggle 2D View &o", priority = 105)] // Alt+O
        public static void Toggle2DView()
        {
            sceneView = SceneView.lastActiveSceneView;
            if (sceneView != null)
            {
                sceneView.in2DMode = !sceneView.in2DMode;
                if (sceneView.in2DMode && SceneViewNavigation.sceneViewType == SceneViewType.Perspective)
                {
                    DisableSkybox();
                }
                else if (!sceneView.in2DMode && SceneViewNavigation.sceneViewType == SceneViewType.Perspective)
                {
                    EnableSkybox();
                }
                else if (!sceneView.in2DMode)
                {
                    wasIn2DMode = true;
                }

                sceneView.Repaint();
            }
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace SceneViewNavigation
{
    public static class SceneViewNavigationMenus
    {
        [MenuItem("My Tools/Scene View Navigation/Perspective &1", priority = 200)]
        static void PerspectiveView()
        {
            SetSceneView(SceneViewType.Perspective);
        }

        [MenuItem("My Tools/Scene View Navigation/Toggle Top-Bottom &2", priority = 201)]
        static void ToggleTopBottomView()
        {
            if (ActiveSceneView.SceneViewType == SceneViewType.Top)
            {
                SetSceneView(SceneViewType.Bottom);
            }
            else
            {
                SetSceneView(SceneViewType.Top);
            }
        }

        [MenuItem("My Tools/Scene View Navigation/Toggle Front-Back &3", priority = 202)]
        static void ToggleFrontBackView()
        {
            if (ActiveSceneView.SceneViewType == SceneViewType.Front)
            {
                SetSceneView(SceneViewType.Back);
            }
            else
            {
                SetSceneView(SceneViewType.Front);
            }
        }

        [MenuItem("My Tools/Scene View Navigation/Toggle Right-Left &4", priority = 203)]
        static void ToggleRightLeftView()
        {
            if (ActiveSceneView.SceneViewType == SceneViewType.Right)
            {
                SetSceneView(SceneViewType.Left);
            }
            else
            {
                SetSceneView(SceneViewType.Right);
            }
        }

        [MenuItem("My Tools/Scene View Navigation/Top", priority = 300)]
        static void TopView()
        {
            SetSceneView(SceneViewType.Top);
        }

        [MenuItem("My Tools/Scene View Navigation/Bottom", priority = 301)]
        static void BottomView()
        {
            SetSceneView(SceneViewType.Bottom);
        }

        [MenuItem("My Tools/Scene View Navigation/Front", priority = 302)]
        static void FrontView()
        {
            SetSceneView(SceneViewType.Front);
        }

        [MenuItem("My Tools/Scene View Navigation/Back", priority = 303)]
        static void BackView()
        {
            SetSceneView(SceneViewType.Back);
        }

        [MenuItem("My Tools/Scene View Navigation/Left", priority = 305)]
        static void LeftView()
        {
            SetSceneView(SceneViewType.Left);
        }

        [MenuItem("My Tools/Scene View Navigation/Right", priority = 304)]
        static void RightView()
        {
            SetSceneView(SceneViewType.Right);
        }

        [MenuItem("My Tools/Scene View Navigation/Reset Current View &h", priority = 100)] //  Alt+H
        public static void ResetSceneViewCamera()
        {
            SceneViewNavigationManager.RedrawLastSavedSceneView();
        }

        [MenuItem("My Tools/Scene View Navigation/Reset All Views %&h", priority = 101)] // Ctrl+Alt+H
        public static void ResetAllViews()
        {
            SceneViewNavigationManager.ResetAllSceneViews();
        }

        [MenuItem("My Tools/Scene View Navigation/Frame Selected &f", priority = 102)] //  Alt+F
        static void FrameSelected()
        {
            SceneView.FrameLastActiveSceneView();
        }

        [MenuItem("My Tools/Scene View Navigation/Toggle Skybox &s", priority = 103)] //  Alt+S
        static void ToggleSkybox()
        {
            ActiveSceneView.sceneView = SceneView.lastActiveSceneView;
            if (ActiveSceneView.sceneView.sceneViewState.skyboxEnabled)
            {
                SceneViewNavigationManager.DisableSkybox();
            }
            else
            {
                SceneViewNavigationManager.EnableSkybox();
            }
        }

        [MenuItem("My Tools/Scene View Navigation/Toggle All Gizmos &g", priority = 104)] // Alt+G
        public static void ToggleSceneViewGizmos()
        {
            var currentValue = SceneViewNavigationManager.GetSceneViewGizmosEnabled();
            SceneViewNavigationManager.SetSceneViewGizmos(!currentValue);
        }

        [MenuItem("My Tools/Scene View Navigation/Toggle Projection _o", priority = 105)] // O
        public static void ToggleProjection()
        {
            ActiveSceneView.sceneView = SceneView.lastActiveSceneView;
            if (ActiveSceneView.sceneView == null)
            {
                return;
            }

            ActiveSceneView.sceneView.orthographic = !ActiveSceneView.sceneView.orthographic;
            ActiveSceneView.sceneView.Repaint();
        }

        [MenuItem("My Tools/Scene View Navigation/Toggle 2D View &o", priority = 106)] // Alt+O
        public static void Toggle2DView()
        {
            ActiveSceneView.sceneView = SceneView.lastActiveSceneView;
            if (ActiveSceneView.sceneView != null)
            {
                ActiveSceneView.sceneView.in2DMode = !ActiveSceneView.sceneView.in2DMode;

                if (ActiveSceneView.sceneView.in2DMode && ActiveSceneView.SceneViewType == SceneViewType.Perspective)
                {
                    SceneViewNavigationManager.DisableSkybox();
                }
                else if (!ActiveSceneView.sceneView.in2DMode &&
                         ActiveSceneView.SceneViewType == SceneViewType.Perspective)
                {
                    SceneViewNavigationManager.EnableSkybox();
                }

                ActiveSceneView.sceneView.Repaint();
            }
        }

        [MenuItem("My Tools/Scene View Navigation/Log Camera Data &l", priority = 400)] //  Alt+L
        static void LogSceneViewCameraData()
        {
            ActiveSceneView.sceneView = SceneView.lastActiveSceneView;
            Debug.Log("");
        }

        private static void SetSceneView(SceneViewType sceneViewType)
        {
            SceneViewNavigationManager.SaveSceneView(sceneViewType);
            ActiveSceneView.SceneViewType = sceneViewType;
            SceneViewNavigationManager.SetView(sceneViewType);
            SceneViewNavigationManager.SetView(sceneViewType);
        }
    }
}
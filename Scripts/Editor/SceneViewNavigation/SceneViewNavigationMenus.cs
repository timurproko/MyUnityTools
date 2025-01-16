using UnityEditor;
using UnityEngine;

namespace SceneViewNavigation
{
    public static class SceneViewNavigationMenus
    {
        private const string MY_TOOLS_MENU = "My Tools/";
        private const string SCENE_VIEW_MENU = MY_TOOLS_MENU + "Scene View Navigation/";
        
        [MenuItem(SCENE_VIEW_MENU + "Reset Current View &h", priority = 100)] //  Alt+H
        public static void ResetSceneViewCamera()
        {
            SceneViewNavigationManager.RedrawLastSavedSceneView();
        }

        [MenuItem(SCENE_VIEW_MENU + "Reset All Views %&h", priority = 101)] // Ctrl+Alt+H
        public static void ResetAllViews()
        {
            SceneViewNavigationManager.ResetAllSceneViews();
        }

        [MenuItem(SCENE_VIEW_MENU + "Frame Selected &f", priority = 102)] //  Alt+F
        static void FrameSelected()
        {
            SceneView.FrameLastActiveSceneView();
        }

        [MenuItem(SCENE_VIEW_MENU + "Toggle Skybox &s", priority = 103)] //  Alt+S
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

        [MenuItem(SCENE_VIEW_MENU + "Toggle All Gizmos &g", priority = 104)] // Alt+G
        public static void ToggleSceneViewGizmos()
        {
            var currentValue = SceneViewNavigationManager.GetSceneViewGizmosEnabled();
            SceneViewNavigationManager.SetSceneViewGizmos(!currentValue);
        }

        [MenuItem(SCENE_VIEW_MENU + "Toggle Projection _o", priority = 105)] // O
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

        [MenuItem(SCENE_VIEW_MENU + "Toggle 2D View &o", priority = 106)] // Alt+O
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

        
        [MenuItem(SCENE_VIEW_MENU + "Perspective &1", priority = 200)]
        static void PerspectiveView()
        {
            SetSceneView(SceneViewType.Perspective);
        }

        [MenuItem(SCENE_VIEW_MENU + "Toggle Top-Bottom &2", priority = 201)]
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

        [MenuItem(SCENE_VIEW_MENU + "Toggle Front-Back &3", priority = 202)]
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

        [MenuItem(SCENE_VIEW_MENU + "Toggle Right-Left &4", priority = 203)]
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

        
        [MenuItem(SCENE_VIEW_MENU + "Top", priority = 300)]
        static void TopView()
        {
            SetSceneView(SceneViewType.Top);
        }

        [MenuItem(SCENE_VIEW_MENU + "Bottom", priority = 301)]
        static void BottomView()
        {
            SetSceneView(SceneViewType.Bottom);
        }

        [MenuItem(SCENE_VIEW_MENU + "Front", priority = 302)]
        static void FrontView()
        {
            SetSceneView(SceneViewType.Front);
        }

        [MenuItem(SCENE_VIEW_MENU + "Back", priority = 303)]
        static void BackView()
        {
            SetSceneView(SceneViewType.Back);
        }

        [MenuItem(SCENE_VIEW_MENU + "Left", priority = 305)]
        static void LeftView()
        {
            SetSceneView(SceneViewType.Left);
        }

        [MenuItem(SCENE_VIEW_MENU + "Right", priority = 304)]
        static void RightView()
        {
            SetSceneView(SceneViewType.Right);
        }

        
        [MenuItem(SCENE_VIEW_MENU + "Log Camera Data &l", priority = 400)] //  Alt+L
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
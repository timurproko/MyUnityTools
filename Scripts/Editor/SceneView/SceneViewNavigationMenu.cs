using UnityEditor;

namespace SceneViewTools
{
    public static class SceneViewNavigationMenu
    {
        [MenuItem(SceneViewMenus.SCENE_VIEW_NAVIGATION_MENU + "Perspective &1", priority = 100)]
        static void PerspectiveView()
        {
            SetSceneView(SceneViewType.Perspective);
        }

        [MenuItem(SceneViewMenus.SCENE_VIEW_NAVIGATION_MENU + "Toggle Top-Bottom &2", priority = 101)]
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

        [MenuItem(SceneViewMenus.SCENE_VIEW_NAVIGATION_MENU + "Toggle Front-Back &3", priority = 102)]
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

        [MenuItem(SceneViewMenus.SCENE_VIEW_NAVIGATION_MENU + "Toggle Right-Left &4", priority = 103)]
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

        
        [MenuItem(SceneViewMenus.SCENE_VIEW_NAVIGATION_MENU + "Top", priority = 200)]
        static void TopView()
        {
            SetSceneView(SceneViewType.Top);
        }

        [MenuItem(SceneViewMenus.SCENE_VIEW_NAVIGATION_MENU + "Bottom", priority = 201)]
        static void BottomView()
        {
            SetSceneView(SceneViewType.Bottom);
        }

        [MenuItem(SceneViewMenus.SCENE_VIEW_NAVIGATION_MENU + "Front", priority = 202)]
        static void FrontView()
        {
            SetSceneView(SceneViewType.Front);
        }

        [MenuItem(SceneViewMenus.SCENE_VIEW_NAVIGATION_MENU + "Back", priority = 203)]
        static void BackView()
        {
            SetSceneView(SceneViewType.Back);
        }

        [MenuItem(SceneViewMenus.SCENE_VIEW_NAVIGATION_MENU + "Left", priority = 204)]
        static void LeftView()
        {
            SetSceneView(SceneViewType.Left);
        }

        [MenuItem(SceneViewMenus.SCENE_VIEW_NAVIGATION_MENU + "Right", priority = 205)]
        static void RightView()
        {
            SetSceneView(SceneViewType.Right);
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
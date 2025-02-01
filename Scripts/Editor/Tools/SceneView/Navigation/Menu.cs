using UnityEditor;

namespace SceneViewTools
{
    public static class SceneViewNavigationMenu
    {
        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Perspective &1", priority = MyTools.Menus.SCENE_VIEW_INDEX + 100)]
        static void PerspectiveView()
        {
            SetSceneView(SceneViewType.Perspective);
        }

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Toggle Top-Bottom &2", priority = MyTools.Menus.SCENE_VIEW_INDEX + 101)]
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

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Toggle Front-Back &3", priority = MyTools.Menus.SCENE_VIEW_INDEX + 102)]
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

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Toggle Right-Left &4", priority = MyTools.Menus.SCENE_VIEW_INDEX + 103)]
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

        
        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Top", priority = MyTools.Menus.SCENE_VIEW_INDEX + 200)]
        static void TopView()
        {
            SetSceneView(SceneViewType.Top);
        }

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Bottom", priority = MyTools.Menus.SCENE_VIEW_INDEX + 201)]
        static void BottomView()
        {
            SetSceneView(SceneViewType.Bottom);
        }

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Front", priority = MyTools.Menus.SCENE_VIEW_INDEX + 202)]
        static void FrontView()
        {
            SetSceneView(SceneViewType.Front);
        }

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Back", priority = MyTools.Menus.SCENE_VIEW_INDEX + 203)]
        static void BackView()
        {
            SetSceneView(SceneViewType.Back);
        }

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Left", priority = MyTools.Menus.SCENE_VIEW_INDEX + 204)]
        static void LeftView()
        {
            SetSceneView(SceneViewType.Left);
        }

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Right", priority = MyTools.Menus.SCENE_VIEW_INDEX + 205)]
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
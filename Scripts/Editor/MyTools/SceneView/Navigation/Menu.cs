using UnityEditor;

namespace SceneViewTools
{
    public static class SceneViewNavigationMenu
    {
        [MenuItem(MyTools.Menu.NAVIGATION_MENU + "Perspective &1", priority = MyTools.Menu.NAVIGATION_MENU_INDEX + 100)]
        static void PerspectiveView()
        {
            SetSceneView(SceneViewType.Perspective);
        }

        [MenuItem(MyTools.Menu.NAVIGATION_MENU + "Toggle Top-Bottom &2", priority = MyTools.Menu.NAVIGATION_MENU_INDEX + 101)]
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

        [MenuItem(MyTools.Menu.NAVIGATION_MENU + "Toggle Front-Back &3", priority = MyTools.Menu.NAVIGATION_MENU_INDEX + 102)]
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

        [MenuItem(MyTools.Menu.NAVIGATION_MENU + "Toggle Right-Left &4", priority = MyTools.Menu.NAVIGATION_MENU_INDEX + 103)]
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

        
        [MenuItem(MyTools.Menu.NAVIGATION_MENU + "Top", priority = MyTools.Menu.NAVIGATION_MENU_INDEX + 200)]
        static void TopView()
        {
            SetSceneView(SceneViewType.Top);
        }

        [MenuItem(MyTools.Menu.NAVIGATION_MENU + "Bottom", priority = MyTools.Menu.NAVIGATION_MENU_INDEX + 201)]
        static void BottomView()
        {
            SetSceneView(SceneViewType.Bottom);
        }

        [MenuItem(MyTools.Menu.NAVIGATION_MENU + "Front", priority = MyTools.Menu.NAVIGATION_MENU_INDEX + 202)]
        static void FrontView()
        {
            SetSceneView(SceneViewType.Front);
        }

        [MenuItem(MyTools.Menu.NAVIGATION_MENU + "Back", priority = MyTools.Menu.NAVIGATION_MENU_INDEX + 203)]
        static void BackView()
        {
            SetSceneView(SceneViewType.Back);
        }

        [MenuItem(MyTools.Menu.NAVIGATION_MENU + "Left", priority = MyTools.Menu.NAVIGATION_MENU_INDEX + 204)]
        static void LeftView()
        {
            SetSceneView(SceneViewType.Left);
        }

        [MenuItem(MyTools.Menu.NAVIGATION_MENU + "Right", priority = MyTools.Menu.NAVIGATION_MENU_INDEX + 205)]
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
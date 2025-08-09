#if UNITY_EDITOR
using UnityEditor;

namespace SceneViewTools
{
    public static class SceneViewNavigationMenu
    {
        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Perspective &1", priority = MyTools.Menus.SCENE_VIEW_INDEX + 100)]
        static void PerspectiveView()
        {
            if (MyTools.State.disabled) return;
            SetSceneView(SceneViewType.Perspective);
        }

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Perspective &1", validate = true, priority = MyTools.Menus.SCENE_VIEW_INDEX + 100)]
        static bool ValidatePerspectiveView() => !MyTools.State.disabled;

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Toggle Top-Bottom &2", priority = MyTools.Menus.SCENE_VIEW_INDEX + 101)]
        static void ToggleTopBottomView()
        {
            if (MyTools.State.disabled) return;

            if (ActiveSceneView.SceneViewType == SceneViewType.Top)
                SetSceneView(SceneViewType.Bottom);
            else
                SetSceneView(SceneViewType.Top);
        }

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Toggle Top-Bottom &2", validate = true, priority = MyTools.Menus.SCENE_VIEW_INDEX + 101)]
        static bool ValidateToggleTopBottomView() => !MyTools.State.disabled;

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Toggle Front-Back &3", priority = MyTools.Menus.SCENE_VIEW_INDEX + 102)]
        static void ToggleFrontBackView()
        {
            if (MyTools.State.disabled) return;

            if (ActiveSceneView.SceneViewType == SceneViewType.Front)
                SetSceneView(SceneViewType.Back);
            else
                SetSceneView(SceneViewType.Front);
        }

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Toggle Front-Back &3", validate = true, priority = MyTools.Menus.SCENE_VIEW_INDEX + 102)]
        static bool ValidateToggleFrontBackView() => !MyTools.State.disabled;

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Toggle Right-Left &4", priority = MyTools.Menus.SCENE_VIEW_INDEX + 103)]
        static void ToggleRightLeftView()
        {
            if (MyTools.State.disabled) return;

            if (ActiveSceneView.SceneViewType == SceneViewType.Right)
                SetSceneView(SceneViewType.Left);
            else
                SetSceneView(SceneViewType.Right);
        }

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Toggle Right-Left &4", validate = true, priority = MyTools.Menus.SCENE_VIEW_INDEX + 103)]
        static bool ValidateToggleRightLeftView() => !MyTools.State.disabled;

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Top", priority = MyTools.Menus.SCENE_VIEW_INDEX + 200)]
        static void TopView()
        {
            if (MyTools.State.disabled) return;
            SetSceneView(SceneViewType.Top);
        }

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Top", validate = true, priority = MyTools.Menus.SCENE_VIEW_INDEX + 200)]
        static bool ValidateTopView() => !MyTools.State.disabled;

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Bottom", priority = MyTools.Menus.SCENE_VIEW_INDEX + 201)]
        static void BottomView()
        {
            if (MyTools.State.disabled) return;
            SetSceneView(SceneViewType.Bottom);
        }

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Bottom", validate = true, priority = MyTools.Menus.SCENE_VIEW_INDEX + 201)]
        static bool ValidateBottomView() => !MyTools.State.disabled;

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Front", priority = MyTools.Menus.SCENE_VIEW_INDEX + 202)]
        static void FrontView()
        {
            if (MyTools.State.disabled) return;
            SetSceneView(SceneViewType.Front);
        }

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Front", validate = true, priority = MyTools.Menus.SCENE_VIEW_INDEX + 202)]
        static bool ValidateFrontView() => !MyTools.State.disabled;

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Back", priority = MyTools.Menus.SCENE_VIEW_INDEX + 203)]
        static void BackView()
        {
            if (MyTools.State.disabled) return;
            SetSceneView(SceneViewType.Back);
        }

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Back", validate = true, priority = MyTools.Menus.SCENE_VIEW_INDEX + 203)]
        static bool ValidateBackView() => !MyTools.State.disabled;

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Left", priority = MyTools.Menus.SCENE_VIEW_INDEX + 204)]
        static void LeftView()
        {
            if (MyTools.State.disabled) return;
            SetSceneView(SceneViewType.Left);
        }

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Left", validate = true, priority = MyTools.Menus.SCENE_VIEW_INDEX + 204)]
        static bool ValidateLeftView() => !MyTools.State.disabled;

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Right", priority = MyTools.Menus.SCENE_VIEW_INDEX + 205)]
        static void RightView()
        {
            if (MyTools.State.disabled) return;
            SetSceneView(SceneViewType.Right);
        }

        [MenuItem(MyTools.Menus.NAVIGATION_MENU + "Right", validate = true, priority = MyTools.Menus.SCENE_VIEW_INDEX + 205)]
        static bool ValidateRightView() => !MyTools.State.disabled;

        internal static void SetSceneView(SceneViewType sceneViewType)
        {
            if (MyTools.State.disabled) return;

            SceneViewNavigationManager.SaveSceneView(sceneViewType);
            ActiveSceneView.SceneViewType = sceneViewType;
            SceneViewNavigationManager.SetView(sceneViewType);
            SceneViewNavigationManager.SetView(sceneViewType);
        }
    }
}
#endif

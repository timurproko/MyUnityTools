#if UNITY_EDITOR
using MyTools;
using UnityEditor;

namespace SceneViewTools
{
    public static class SceneViewNavigationMenu
    {
        private static SceneViewType _lastTopBottomView = SceneViewType.Top;
        private static SceneViewType _lastFrontBackView = SceneViewType.Front;
        private static SceneViewType _lastRightLeftView = SceneViewType.Right;

        [MenuItem(Menus.NAVIGATION_MENU + "Perspective &1", priority = Menus.SCENE_VIEW_INDEX + 100)]
        static void PerspectiveView()
        {
            if (State.disabled) return;
            SetSceneView(SceneViewType.Perspective);
        }

        [MenuItem(Menus.NAVIGATION_MENU + "Perspective &1", validate = true, priority = Menus.SCENE_VIEW_INDEX + 100)]
        static bool ValidatePerspectiveView() => !State.disabled;

        [MenuItem(Menus.NAVIGATION_MENU + "Toggle Top-Bottom &2", priority = Menus.SCENE_VIEW_INDEX + 101)]
        static void ToggleTopBottomView()
        {
            if (State.disabled) return;

            var currentType = ActiveSceneView.SceneViewType;

            if (currentType == SceneViewType.Top || currentType == SceneViewType.Bottom)
            {
                var newView = (currentType == SceneViewType.Top) ? SceneViewType.Bottom : SceneViewType.Top;
                _lastTopBottomView = newView;
                SetSceneView(newView);
            }
            else
            {
                SetSceneView(_lastTopBottomView);
            }
        }

        [MenuItem(Menus.NAVIGATION_MENU + "Toggle Top-Bottom &2", validate = true,
            priority = Menus.SCENE_VIEW_INDEX + 101)]
        static bool ValidateToggleTopBottomView() => !State.disabled;

        [MenuItem(Menus.NAVIGATION_MENU + "Toggle Front-Back &3", priority = Menus.SCENE_VIEW_INDEX + 102)]
        static void ToggleFrontBackView()
        {
            if (State.disabled) return;

            var currentType = ActiveSceneView.SceneViewType;

            if (currentType == SceneViewType.Front || currentType == SceneViewType.Back)
            {
                var newView = (currentType == SceneViewType.Front) ? SceneViewType.Back : SceneViewType.Front;
                _lastFrontBackView = newView;
                SetSceneView(newView);
            }
            else
            {
                SetSceneView(_lastFrontBackView);
            }
        }

        [MenuItem(Menus.NAVIGATION_MENU + "Toggle Front-Back &3", validate = true,
            priority = Menus.SCENE_VIEW_INDEX + 102)]
        static bool ValidateToggleFrontBackView() => !State.disabled;

        [MenuItem(Menus.NAVIGATION_MENU + "Toggle Right-Left &4", priority = Menus.SCENE_VIEW_INDEX + 103)]
        static void ToggleRightLeftView()
        {
            if (State.disabled) return;

            var currentType = ActiveSceneView.SceneViewType;

            if (currentType == SceneViewType.Right || currentType == SceneViewType.Left)
            {
                var newView = (currentType == SceneViewType.Right) ? SceneViewType.Left : SceneViewType.Right;
                _lastRightLeftView = newView;
                SetSceneView(newView);
            }
            else
            {
                SetSceneView(_lastRightLeftView);
            }
        }

        [MenuItem(Menus.NAVIGATION_MENU + "Toggle Right-Left &4", validate = true,
            priority = Menus.SCENE_VIEW_INDEX + 103)]
        static bool ValidateToggleRightLeftView() => !State.disabled;

        [MenuItem(Menus.NAVIGATION_MENU + "Top", priority = Menus.SCENE_VIEW_INDEX + 200)]
        static void TopView()
        {
            if (State.disabled) return;
            SetSceneView(SceneViewType.Top);
        }

        [MenuItem(Menus.NAVIGATION_MENU + "Top", validate = true, priority = Menus.SCENE_VIEW_INDEX + 200)]
        static bool ValidateTopView() => !State.disabled;

        [MenuItem(Menus.NAVIGATION_MENU + "Bottom", priority = Menus.SCENE_VIEW_INDEX + 201)]
        static void BottomView()
        {
            if (State.disabled) return;
            SetSceneView(SceneViewType.Bottom);
        }

        [MenuItem(Menus.NAVIGATION_MENU + "Bottom", validate = true, priority = Menus.SCENE_VIEW_INDEX + 201)]
        static bool ValidateBottomView() => !State.disabled;

        [MenuItem(Menus.NAVIGATION_MENU + "Front", priority = Menus.SCENE_VIEW_INDEX + 202)]
        static void FrontView()
        {
            if (State.disabled) return;
            SetSceneView(SceneViewType.Front);
        }

        [MenuItem(Menus.NAVIGATION_MENU + "Front", validate = true, priority = Menus.SCENE_VIEW_INDEX + 202)]
        static bool ValidateFrontView() => !State.disabled;

        [MenuItem(Menus.NAVIGATION_MENU + "Back", priority = Menus.SCENE_VIEW_INDEX + 203)]
        static void BackView()
        {
            if (State.disabled) return;
            SetSceneView(SceneViewType.Back);
        }

        [MenuItem(Menus.NAVIGATION_MENU + "Back", validate = true, priority = Menus.SCENE_VIEW_INDEX + 203)]
        static bool ValidateBackView() => !State.disabled;

        [MenuItem(Menus.NAVIGATION_MENU + "Left", priority = Menus.SCENE_VIEW_INDEX + 204)]
        static void LeftView()
        {
            if (State.disabled) return;
            SetSceneView(SceneViewType.Left);
        }

        [MenuItem(Menus.NAVIGATION_MENU + "Left", validate = true, priority = Menus.SCENE_VIEW_INDEX + 204)]
        static bool ValidateLeftView() => !State.disabled;

        [MenuItem(Menus.NAVIGATION_MENU + "Right", priority = Menus.SCENE_VIEW_INDEX + 205)]
        static void RightView()
        {
            if (State.disabled) return;
            SetSceneView(SceneViewType.Right);
        }

        [MenuItem(Menus.NAVIGATION_MENU + "Right", validate = true, priority = Menus.SCENE_VIEW_INDEX + 205)]
        static bool ValidateRightView() => !State.disabled;

        internal static void SetSceneView(SceneViewType sceneViewType)
        {
            if (State.disabled) return;

            SceneViewNavigationManager.SaveSceneView(sceneViewType);
            ActiveSceneView.SceneViewType = sceneViewType;
            SceneViewNavigationManager.SetView(sceneViewType);
            SceneViewNavigationManager.SetView(sceneViewType);
        }
    }
}
#endif
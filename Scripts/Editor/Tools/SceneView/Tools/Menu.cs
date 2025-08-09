#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SceneViewTools
{
    public static class SceneViewToolsMenu
    {
        private static GameObject lastSelectedObject;
        private static bool toggleState;
        private static HashSet<GameObject> hiddenObjects = new();

        [MenuItem(MyTools.Menus.TOOLS_MENU + "Reset Current View &h",
            priority = MyTools.Menus.SCENE_VIEW_INDEX + 100)] //  Alt+H
        public static void ResetSceneViewCamera()
        {
            if (MyTools.State.disabled) return;
            SceneViewNavigationManager.RedrawLastSavedSceneView();
        }

        [MenuItem(MyTools.Menus.TOOLS_MENU + "Reset Current View &h", validate = true,
            priority = MyTools.Menus.SCENE_VIEW_INDEX + 100)]
        private static bool ValidateResetSceneViewCamera() => !MyTools.State.disabled;

        [MenuItem(MyTools.Menus.TOOLS_MENU + "Reset All Views %&h",
            priority = MyTools.Menus.SCENE_VIEW_INDEX + 101)] // Ctrl+Alt+H
        public static void ResetAllViews()
        {
            if (MyTools.State.disabled) return;
            SceneViewNavigationManager.ResetAllSceneViews();
        }

        [MenuItem(MyTools.Menus.TOOLS_MENU + "Reset All Views %&h", validate = true,
            priority = MyTools.Menus.SCENE_VIEW_INDEX + 101)]
        private static bool ValidateResetAllViews() => !MyTools.State.disabled;

        [MenuItem(MyTools.Menus.TOOLS_MENU + "Toggle Projection _o",
            priority = MyTools.Menus.SCENE_VIEW_INDEX + 200)] // O
        public static void ToggleProjection()
        {
            if (MyTools.State.disabled) return;

            ActiveSceneView.sceneView = UnityEditor.SceneView.lastActiveSceneView;
            if (ActiveSceneView.sceneView == null || ActiveSceneView.sceneView.in2DMode)
            {
                return;
            }

            ActiveSceneView.sceneView.orthographic = !ActiveSceneView.sceneView.orthographic;
            ActiveSceneView.sceneView.Repaint();
        }

        [MenuItem(MyTools.Menus.TOOLS_MENU + "Toggle Projection _o", validate = true,
            priority = MyTools.Menus.SCENE_VIEW_INDEX + 200)]
        private static bool ValidateToggleProjection() => !MyTools.State.disabled;

        [MenuItem(MyTools.Menus.TOOLS_MENU + "Toggle 2D View &o",
            priority = MyTools.Menus.SCENE_VIEW_INDEX + 201)] // Alt+O
        public static void Toggle2DView()
        {
            if (MyTools.State.disabled) return;

            ActiveSceneView.sceneView = UnityEditor.SceneView.lastActiveSceneView;
            if (ActiveSceneView.sceneView != null)
            {
                ActiveSceneView.sceneView.in2DMode = !ActiveSceneView.sceneView.in2DMode;
                ActiveSceneView.sceneView.Repaint();
            }
        }

        [MenuItem(MyTools.Menus.TOOLS_MENU + "Toggle 2D View &o", validate = true,
            priority = MyTools.Menus.SCENE_VIEW_INDEX + 201)]
        private static bool ValidateToggle2DView() => !MyTools.State.disabled;

        [MenuItem(MyTools.Menus.TOOLS_MENU + "Toggle Skybox &s",
            priority = MyTools.Menus.SCENE_VIEW_INDEX + 202)] //  Alt+S
        static void ToggleSkybox()
        {
            if (MyTools.State.disabled) return;

            ActiveSceneView.sceneView = UnityEditor.SceneView.lastActiveSceneView;
            if (ActiveSceneView.sceneView.sceneViewState.skyboxEnabled)
            {
                SceneViewNavigationManager.DisableSkybox();
            }
            else
            {
                SceneViewNavigationManager.EnableSkybox();
            }
        }

        [MenuItem(MyTools.Menus.TOOLS_MENU + "Toggle Skybox &s", validate = true,
            priority = MyTools.Menus.SCENE_VIEW_INDEX + 202)]
        private static bool ValidateToggleSkybox() => !MyTools.State.disabled;

        [MenuItem(MyTools.Menus.TOOLS_MENU + "Toggle All Gizmos &g",
            priority = MyTools.Menus.SCENE_VIEW_INDEX + 203)] // Alt+G
        public static void ToggleSceneViewGizmos()
        {
            if (MyTools.State.disabled) return;

            var currentValue = SceneViewNavigationManager.GetSceneViewGizmosEnabled();
            SceneViewNavigationManager.SetSceneViewGizmos(!currentValue);
        }

        [MenuItem(MyTools.Menus.TOOLS_MENU + "Toggle All Gizmos &g", validate = true,
            priority = MyTools.Menus.SCENE_VIEW_INDEX + 203)]
        private static bool ValidateToggleSceneViewGizmos() => !MyTools.State.disabled;

        [MenuItem(MyTools.Menus.TOOLS_MENU + "Toggle Grid Snapping &j",
            priority = MyTools.Menus.SCENE_VIEW_INDEX + 204)] // Alt+J
        public static void ToggleGridSnapping()
        {
            if (MyTools.State.disabled) return;

#if UNITY_6000
            EditorSnapSettings.snapEnabled = !EditorSnapSettings.snapEnabled;
#else
            Debug.Log("MyTools: Snapping shortcut is not supported in this version.");
#endif
        }

        [MenuItem(MyTools.Menus.TOOLS_MENU + "Toggle Grid Snapping &j", true)]
        public static bool ValidateToggleGridSnapping()
        {
            if (MyTools.State.disabled) return false;
#if UNITY_6000
            return true;
#else
            return false;
#endif
        }

        [MenuItem(MyTools.Menus.TOOLS_MENU + "Toggle Grid %&#g",
            priority = MyTools.Menus.SCENE_VIEW_INDEX + 205)] // Ctrl+Alt+Shift+G
        private static void ToggleGridVisibility()
        {
            if (MyTools.State.disabled) return;

            foreach (var sceneView in UnityEditor.SceneView.sceneViews)
            {
                if (sceneView is UnityEditor.SceneView view)
                {
                    view.showGrid = !view.showGrid;
                }
            }
        }

        [MenuItem(MyTools.Menus.TOOLS_MENU + "Toggle Grid %&#g", validate = true,
            priority = MyTools.Menus.SCENE_VIEW_INDEX + 205)]
        private static bool ValidateToggleGridVisibility() => !MyTools.State.disabled;

        [MenuItem(MyTools.Menus.TOOLS_MENU + "Toggle Isolation on Selection #\\", false,
            MyTools.Menus.SCENE_VIEW_INDEX + 206)] // Shift+\
        private static void ToggleObjectVisibility()
        {
            if (MyTools.State.disabled) return;

            GameObject selectedObject = Selection.activeGameObject;

            if (selectedObject == null)
            {
                RestoreVisibility();
                toggleState = false;
                lastSelectedObject = null;
                return;
            }

            if (selectedObject != lastSelectedObject && toggleState)
            {
                RestoreVisibility();
                toggleState = false;
            }

            if (selectedObject == lastSelectedObject && toggleState)
            {
                RestoreVisibility();
            }
            else
            {
                HideAllExceptSelected(selectedObject);
            }

            toggleState = !toggleState;
            lastSelectedObject = selectedObject;
        }

        [MenuItem(MyTools.Menus.TOOLS_MENU + "Toggle Isolation on Selection #\\", true,
            MyTools.Menus.SCENE_VIEW_INDEX + 206)]
        private static bool ValidateToggleObjectVisibility() => !MyTools.State.disabled;

        private static void HideAllExceptSelected(GameObject selectedObject)
        {
            GameObject[] rootObjects = selectedObject.scene.GetRootGameObjects();

            foreach (GameObject obj in rootObjects)
            {
                if (obj != selectedObject)
                {
                    SetSceneVisibility(obj, false);
                }
            }

            SetSceneVisibility(selectedObject, true);
        }

        private static void SetSceneVisibility(GameObject obj, bool visible)
        {
            if (visible)
            {
                SceneVisibilityManager.instance.Show(obj, true);
                hiddenObjects.Remove(obj);
            }
            else
            {
                SceneVisibilityManager.instance.Hide(obj, true);
                hiddenObjects.Add(obj);
            }

            foreach (Transform child in obj.transform)
            {
                SetSceneVisibility(child.gameObject, visible);
            }
        }

        private static void RestoreVisibility()
        {
            foreach (GameObject obj in hiddenObjects)
            {
                SceneVisibilityManager.instance.Show(obj, true);
            }

            hiddenObjects.Clear();
        }

        [MenuItem(MyTools.Menus.TOOLS_MENU + "Frame Selected &f",
            priority = MyTools.Menus.SCENE_VIEW_INDEX + 300)] //  Alt+F
        static void FrameSelected()
        {
            if (MyTools.State.disabled) return;
            UnityEditor.SceneView.FrameLastActiveSceneView();
        }

        [MenuItem(MyTools.Menus.TOOLS_MENU + "Frame Selected &f", validate = true,
            priority = MyTools.Menus.SCENE_VIEW_INDEX + 300)]
        private static bool ValidateFrameSelected() => !MyTools.State.disabled;
    }
}
#endif

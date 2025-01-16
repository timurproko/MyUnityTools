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
        
        [MenuItem(SceneViewMenus.SCENE_VIEW_TOOLS_MENU + "Toggle Grid %&#g", priority = 100)] // Ctrl+Alt+Shift+G
        private static void ToggleGridVisibility()
        {
            // Iterate through all open SceneViews
            foreach (var sceneView in SceneView.sceneViews)
            {
                if (sceneView is SceneView view)
                {
                    // Toggle the grid visibility based on its current state
                    view.showGrid = !view.showGrid;
                }
            }
        }

        [MenuItem(SceneViewMenus.SCENE_VIEW_TOOLS_MENU + "Toggle Grid Snapping &j", priority = 100)] // Alt+J
        public static void ToggleGridSnapping()
        {
#if UNITY_6000
            EditorSnapSettings.snapEnabled = !EditorSnapSettings.snapEnabled;
#else
            Debug.Log("MyTools: Snapping shortcut is not supported in this version.");
#endif
        }

        [MenuItem(SceneViewMenus.SCENE_VIEW_TOOLS_MENU + "Toggle Grid Snapping &j", true)]
        public static bool ValidateToggleGridSnapping()
        {
#if UNITY_6000
            return true;
#else
            return false;
#endif
        }

        [MenuItem(SceneViewMenus.SCENE_VIEW_TOOLS_MENU + "Toggle Isolation on Selection #\\", false, 100)]
        private static void ToggleObjectVisibility()
        {
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

        private static void HideAllExceptSelected(GameObject selectedObject)
        {
            GameObject[] rootObjects = selectedObject.scene.GetRootGameObjects();

            foreach (GameObject obj in rootObjects)
            {
                if (obj != selectedObject)
                {
                    SetSceneVisibility(obj, false); // Hide the object
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
            // Show all previously hidden objects
            foreach (GameObject obj in hiddenObjects)
            {
                SceneVisibilityManager.instance.Show(obj, true);
            }

            hiddenObjects.Clear();
        }

        [MenuItem(SceneViewMenus.SCENE_VIEW_TOOLS_MENU + "Reset Current View &h", priority = 100)] //  Alt+H
        public static void ResetSceneViewCamera()
        {
            SceneViewNavigationManager.RedrawLastSavedSceneView();
        }

        [MenuItem(SceneViewMenus.SCENE_VIEW_TOOLS_MENU + "Reset All Views %&h", priority = 101)] // Ctrl+Alt+H
        public static void ResetAllViews()
        {
            SceneViewNavigationManager.ResetAllSceneViews();
        }

        [MenuItem(SceneViewMenus.SCENE_VIEW_TOOLS_MENU + "Frame Selected &f", priority = 102)] //  Alt+F
        static void FrameSelected()
        {
            SceneView.FrameLastActiveSceneView();
        }

        [MenuItem(SceneViewMenus.SCENE_VIEW_TOOLS_MENU + "Toggle Skybox &s", priority = 103)] //  Alt+S
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

        [MenuItem(SceneViewMenus.SCENE_VIEW_TOOLS_MENU + "Toggle All Gizmos &g", priority = 104)] // Alt+G
        public static void ToggleSceneViewGizmos()
        {
            var currentValue = SceneViewNavigationManager.GetSceneViewGizmosEnabled();
            SceneViewNavigationManager.SetSceneViewGizmos(!currentValue);
        }

        [MenuItem(SceneViewMenus.SCENE_VIEW_TOOLS_MENU + "Toggle Projection _o", priority = 105)] // O
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

        [MenuItem(SceneViewMenus.SCENE_VIEW_TOOLS_MENU + "Toggle 2D View &o", priority = 106)] // Alt+O
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
    }
}
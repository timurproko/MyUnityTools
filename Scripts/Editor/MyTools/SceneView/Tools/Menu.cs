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

        [MenuItem(MyTools.Menu.TOOLS_MENU + "Reset Current View &h",
            priority = MyTools.Menu.TOOLS_MENU_INDEX + 100)] //  Alt+H
        public static void ResetSceneViewCamera()
        {
            SceneViewNavigationManager.RedrawLastSavedSceneView();
        }

        [MenuItem(MyTools.Menu.TOOLS_MENU + "Reset All Views %&h",
            priority = MyTools.Menu.TOOLS_MENU_INDEX + 101)] // Ctrl+Alt+H
        public static void ResetAllViews()
        {
            SceneViewNavigationManager.ResetAllSceneViews();
        }

        [MenuItem(MyTools.Menu.TOOLS_MENU + "Toggle Projection _o",
            priority = MyTools.Menu.TOOLS_MENU_INDEX + 200)] // O
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

        [MenuItem(MyTools.Menu.TOOLS_MENU + "Toggle 2D View &o",
            priority = MyTools.Menu.TOOLS_MENU_INDEX + 201)] // Alt+O
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

        [MenuItem(MyTools.Menu.TOOLS_MENU + "Toggle Skybox &s",
            priority = MyTools.Menu.TOOLS_MENU_INDEX + 202)] //  Alt+S
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

        [MenuItem(MyTools.Menu.TOOLS_MENU + "Toggle All Gizmos &g",
            priority = MyTools.Menu.TOOLS_MENU_INDEX + 203)] // Alt+G
        public static void ToggleSceneViewGizmos()
        {
            var currentValue = SceneViewNavigationManager.GetSceneViewGizmosEnabled();
            SceneViewNavigationManager.SetSceneViewGizmos(!currentValue);
        }

        [MenuItem(MyTools.Menu.TOOLS_MENU + "Toggle Grid Snapping &j",
            priority = MyTools.Menu.TOOLS_MENU_INDEX + 204)] // Alt+J
        public static void ToggleGridSnapping()
        {
#if UNITY_6000
            EditorSnapSettings.snapEnabled = !EditorSnapSettings.snapEnabled;
#else
            Debug.Log("MyTools: Snapping shortcut is not supported in this version.");
#endif
        }

        [MenuItem(MyTools.Menu.TOOLS_MENU + "Toggle Grid Snapping &j", true)]
        public static bool ValidateToggleGridSnapping()
        {
#if UNITY_6000
            return true;
#else
            return false;
#endif
        }

        [MenuItem(MyTools.Menu.TOOLS_MENU + "Toggle Grid %&#g",
            priority = MyTools.Menu.TOOLS_MENU_INDEX + 205)] // Ctrl+Alt+Shift+G
        private static void ToggleGridVisibility()
        {
            foreach (var sceneView in SceneView.sceneViews)
            {
                if (sceneView is SceneView view)
                {
                    view.showGrid = !view.showGrid;
                }
            }
        }

        [MenuItem(MyTools.Menu.TOOLS_MENU + "Toggle Isolation on Selection #\\", false,
            MyTools.Menu.TOOLS_MENU_INDEX + 206)]
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

        [MenuItem(MyTools.Menu.TOOLS_MENU + "Frame Selected &f",
            priority = MyTools.Menu.TOOLS_MENU_INDEX + 300)] //  Alt+F
        static void FrameSelected()
        {
            SceneView.FrameLastActiveSceneView();
        }
    }
}
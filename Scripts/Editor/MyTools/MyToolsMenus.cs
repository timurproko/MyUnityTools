using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using System.Reflection;
using UnityEngine;

namespace MyTools
{
    static class Menus
    {
        private static GameObject lastSelectedObject;
        private static bool toggleState;
        private static HashSet<GameObject> hiddenObjects = new();

        [MenuItem(MyTools.ASSETS_AND_PREFABS_MENU + "Create Prefab from Selection", priority = 300)]
        private static void CreatePrefabFromSelectedFBX()
        {
            System.Object[] selectedObjects = Selection.objects;

            foreach (var selectedObject in selectedObjects)
            {
                string path = AssetDatabase.GetAssetPath((UnityEngine.Object)selectedObject);
                if (Path.GetExtension(path).ToLower() == ".fbx")
                {
                    // Load the FBX model
                    GameObject fbxModel = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if (fbxModel == null)
                    {
                        Debug.LogError("Could not load FBX model at path: " + path);
                        return;
                    }

                    // Create a prefab from the loaded model
                    string prefabPath = Path.ChangeExtension(path, ".prefab");
                    PrefabUtility.SaveAsPrefabAsset(fbxModel, prefabPath);

                    Debug.Log("Prefab created at: " + prefabPath);
                }
                else
                {
                    Debug.LogWarning("Selected object is not an FBX file: " + path);
                }
            }
        }

        [MenuItem(MyTools.ASSETS_AND_PREFABS_MENU + "Apply Prefab Overrides &a", priority = 300)] // Alt+A
        public static void ApplySelectedPrefabOverrides()
        {
            GameObject[] selectedObjects = Selection.gameObjects;

            if (selectedObjects.Length == 0)
            {
                Debug.LogWarning("My Tools: No GameObjects selected.");
                return;
            }

            foreach (var obj in selectedObjects)
            {
                // Get the corresponding prefab root object
                GameObject prefabRoot = PrefabUtility.GetCorrespondingObjectFromSource(obj);

                if (prefabRoot != null)
                {
                    // Apply all overrides to the prefab
                    PrefabUtility.ApplyPrefabInstance(obj, InteractionMode.UserAction);
                    Debug.Log($"My Tools: Applied overrides to {prefabRoot.name}");
                }
                else
                {
                    Debug.LogWarning($"My Tools: No prefab found for {obj.name}");
                }
            }

            Selection.activeGameObject = null;
            EditorApplication.delayCall += () => Selection.objects = selectedObjects;
        }

        [MenuItem(MyTools.ASSETS_AND_PREFABS_MENU + "Create Children LOD Groups", priority = 300)]
        static void CopyLODGroupToFirstLevelChildren()
        {
            // Get the currently selected GameObject in the Editor
            GameObject selected = Selection.activeGameObject;

            // Ensure something is selected
            if (selected == null)
            {
                Debug.LogError("No GameObject selected.");
                return;
            }

            // Get the LODGroup component from the selected GameObject
            LODGroup rootLODGroup = selected.GetComponent<LODGroup>();

            // If there's no LODGroup on the selected object, display a warning
            if (rootLODGroup == null)
            {
                Debug.LogError("Selected GameObject does not have an LODGroup component.");
                return;
            }

            // Iterate only through the first-level children
            foreach (Transform child in selected.transform)
            {
                // Ensure the child is valid
                if (child == null) continue;

                // Check if the child already has an LODGroup
                LODGroup childLODGroup = child.GetComponent<LODGroup>();

                // If the child has an existing LODGroup, remove it
                if (childLODGroup != null)
                {
                    Undo.DestroyObjectImmediate(childLODGroup);
                }

                // Add a new LODGroup component to the child
                LODGroup newLODGroup = child.gameObject.AddComponent<LODGroup>();

                // Copy LOD settings from the root to the new LODGroup on the child
                CopyLODGroupSettings(rootLODGroup, newLODGroup);

                // Remove all renderers from the new LODGroup
                RemoveAllRenderersFromLODGroup(newLODGroup);

                // Assign new renderers based on child names
                AssignRenderersToLODGroup(newLODGroup, child);

                // Set Transition % Screen Size for the child LODGroup
                SetTransitionScreenSize(newLODGroup);
            }

            // Remove LODGroup component from the parent object
            Undo.DestroyObjectImmediate(rootLODGroup);

            Debug.Log(
                "LOD Group successfully copied to first-level children, renderers assigned, transition sizes set, and LOD Group removed from parent.");
        }

        static void CopyLODGroupSettings(LODGroup source, LODGroup destination)
        {
            if (source == null || destination == null)
            {
                Debug.LogError("Source or destination LODGroup is null.");
                return;
            }

            LOD[] lods = source.GetLODs();
            destination.SetLODs(lods);
            destination.fadeMode = source.fadeMode;
            destination.animateCrossFading = source.animateCrossFading;
        }

        static void RemoveAllRenderersFromLODGroup(LODGroup lodGroup)
        {
            // Clear all renderers for each LOD in the LODGroup
            LOD[] lods = lodGroup.GetLODs();
            for (int i = 0; i < lods.Length; i++)
            {
                lods[i].renderers = new Renderer[0]; // Clear previous renderers
            }

            lodGroup.SetLODs(lods); // Update the LODGroup with cleared renderers
        }

        static void AssignRenderersToLODGroup(LODGroup lodGroup, Transform child)
        {
            // Create lists to hold the renderers
            List<Renderer> renderersLOD0 = new List<Renderer>();
            List<Renderer> renderersLOD1 = new List<Renderer>();
            List<Renderer> renderersLOD2 = new List<Renderer>();

            // Find all the immediate children of the child and categorize them based on naming convention
            foreach (Transform lodChild in child)
            {
                string lodName = lodChild.gameObject.name;

                // Check naming convention to assign to the correct LOD group
                if (lodName.EndsWith("_LOD0"))
                {
                    renderersLOD0.Add(lodChild.GetComponent<Renderer>());
                }
                else if (lodName.EndsWith("_LOD1"))
                {
                    renderersLOD1.Add(lodChild.GetComponent<Renderer>());
                }
                else if (lodName.EndsWith("_LOD2"))
                {
                    renderersLOD2.Add(lodChild.GetComponent<Renderer>());
                }
            }

            // Create new LODs with renderers
            List<LOD> lods = new List<LOD>();

            // Add LOD0 if there are any renderers
            if (renderersLOD0.Count > 0)
            {
                lods.Add(new LOD(1f, renderersLOD0.ToArray())); // Full priority
            }

            // Add LOD1 if there are any renderers
            if (renderersLOD1.Count > 0)
            {
                lods.Add(new LOD(0.5f, renderersLOD1.ToArray())); // Half priority
            }

            // Add LOD2 if there are any renderers
            if (renderersLOD2.Count > 0)
            {
                lods.Add(new LOD(0.25f, renderersLOD2.ToArray())); // Quarter priority
            }

            // Assign LODs to the LODGroup
            lodGroup.SetLODs(lods.ToArray()); // Set the newly created LODs to the LODGroup
        }

        static void SetTransitionScreenSize(LODGroup lodGroup)
        {
            // Get the LODs for the new LODGroup
            LOD[] lods = lodGroup.GetLODs();

            // Set Transition % Screen Size based on the number of LOD levels
            for (int i = 0; i < lods.Length; i++)
            {
                if (lods.Length == 2) // If there are 2 LODs
                {
                    switch (i)
                    {
                        case 0:
                            lods[i].screenRelativeTransitionHeight = 0.25f; // LOD0 transition size
                            break;
                        case 1:
                            lods[i].screenRelativeTransitionHeight = 0.01f; // LOD1 transition size
                            break;
                    }
                }
                else if (lods.Length == 3) // If there are 3 LODs
                {
                    switch (i)
                    {
                        case 0:
                            lods[i].screenRelativeTransitionHeight = 0.25f; // LOD0 transition size
                            break;
                        case 1:
                            lods[i].screenRelativeTransitionHeight = 0.13f; // LOD1 transition size
                            break;
                        case 2:
                            lods[i].screenRelativeTransitionHeight = 0.01f; // LOD2 transition size
                            break;
                    }
                }
                // Add more conditions for additional LODs if necessary
            }

            // Update the LODGroup with the modified LODs
            lodGroup.SetLODs(lods);
        }

        [MenuItem(MyTools.ASSETS_AND_PREFABS_MENU + "Force Refresh Assets #r", priority = 300)] // Shift+R
        private static void ForceRefreshSelectedAsset()
        {
            // Get the selected assets in the Project Window
            var selectedObjects = Selection.objects;

            if (selectedObjects == null || selectedObjects.Length == 0)
            {
                // If no assets are selected, refresh all assets
                AssetDatabase.Refresh();
                Debug.Log("MyTools: All assets have been refreshed.");
            }
            else
            {
                foreach (var obj in selectedObjects)
                {
                    // Get the path of the selected asset
                    string assetPath = AssetDatabase.GetAssetPath(obj);

                    if (!string.IsNullOrEmpty(assetPath))
                    {
                        // Force refresh the specific asset
                        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                        Debug.Log($"MyTools: {assetPath} has been refreshed.");
                    }
                }
            }
        }


        [MenuItem(MyTools.UNITY_EDITOR_MENU + "Toggle Lock %&l", priority = 200)] // Ctrl+Alt+L
        static void ToggleWindowLock()
        {
            // "EditorWindow.focusedWindow" can be used instead
            EditorWindow windowToBeLocked = EditorWindow.mouseOverWindow;

            if (windowToBeLocked != null && windowToBeLocked.GetType().Name == "InspectorWindow")
            {
                Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");
                PropertyInfo propertyInfo = type.GetProperty("isLocked");
                bool value = (bool)propertyInfo.GetValue(windowToBeLocked, null);
                propertyInfo.SetValue(windowToBeLocked, !value, null);
                windowToBeLocked.Repaint();
            }
            else if (windowToBeLocked != null && windowToBeLocked.GetType().Name == "ProjectBrowser")
            {
                Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.ProjectBrowser");
                PropertyInfo propertyInfo = type.GetProperty("isLocked",
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                bool value = (bool)propertyInfo.GetValue(windowToBeLocked, null);
                propertyInfo.SetValue(windowToBeLocked, !value, null);
                windowToBeLocked.Repaint();
            }
            else if (windowToBeLocked != null && windowToBeLocked.GetType().Name == "SceneHierarchyWindow")
            {
                Type type = Assembly.GetAssembly(typeof(Editor))
                    .GetType("UnityEditor.SceneHierarchyWindow");

                FieldInfo fieldInfo = type.GetField("m_SceneHierarchy",
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                PropertyInfo propertyInfo = fieldInfo.FieldType.GetProperty("isLocked",
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                object value = fieldInfo.GetValue(windowToBeLocked);
                bool value2 = (bool)propertyInfo.GetValue(value);
                propertyInfo.SetValue(value, !value2, null);
                windowToBeLocked.Repaint();
            }
        }

        [MenuItem(MyTools.UNITY_EDITOR_MENU + "Clear Console &c", priority = 200)] // Alt+C
        static void ClearConsole()
        {
            MyTools.ClearConsole();
        }

        [MenuItem(MyTools.UNITY_EDITOR_MENU + "Maximize %b", priority = 200)] // Ctrl+B
        static void Maximize()
        {
            MyTools.ActivateWindowUnderCursor();
            EditorWindow window = EditorWindow.focusedWindow;
            // Assume the game view is focused.
            if (window)
            {
                window.maximized = !window.maximized;
            }
        }

        [MenuItem(MyTools.UNITY_EDITOR_MENU + "Close Tab &w", priority = 200)] // Alt+W
        static void CloseTab()
        {
            MyTools.ActivateWindowUnderCursor();
            EditorWindow window = EditorWindow.focusedWindow;
            // Assume the game view is focused.
            if (window)
            {
                window.Close();
            }
        }

        
        [MenuItem(MyTools.SCENE_VIEW_MENU + "Toggle Grid %&#g", priority = 100)] // Ctrl+Alt+Shift+G
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
        
        [MenuItem(MyTools.SCENE_VIEW_MENU + "Toggle Grid Snapping &j", priority = 100)] // Alt+J
        public static void ToggleGridSnapping()
        {
#if UNITY_6000
            EditorSnapSettings.snapEnabled = !EditorSnapSettings.snapEnabled;
#else
            Debug.Log("MyTools: Snapping shortcut is not supported in this version.");
#endif
        }

        [MenuItem(MyTools.SCENE_VIEW_MENU + "Toggle Grid Snapping &j", true)]
        public static bool ValidateToggleGridSnapping()
        {
#if UNITY_6000
            return true;
#else
            return false;
#endif
        }

        [MenuItem(MyTools.SCENE_VIEW_MENU + "Toggle Isolation on Selection #\\", false, 100)]
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
    }
}
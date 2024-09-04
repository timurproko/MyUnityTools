using System;
using UnityEditor;
using System.Reflection;
using UnityEngine;

namespace MyTools
{
    static class Shortcuts
    {
        // Grid
        [MenuItem("My Tools/Toggle Grid %&#g", priority = 11)] // Ctrl+Alt+Shift+G
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

        [MenuItem("My Tools/Toggle Grid Snapping &j", priority = 11)] // Alt+J
        public static void ToggleGridSnapping()
        {
            EditorSnapSettings.snapEnabled = !EditorSnapSettings.snapEnabled;
        }

        // Panels
        [MenuItem("My Tools/Toggle Lock %&l", priority = 12)] // Ctrl+Alt+L
        static void ToggleWindowLock()
        {
            // "EditorWindow.focusedWindow" can be used instead
            EditorWindow windowToBeLocked = EditorWindow.mouseOverWindow;

            if (windowToBeLocked != null && windowToBeLocked.GetType().Name == "InspectorWindow")
            {
                Type type = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.InspectorWindow");
                PropertyInfo propertyInfo = type.GetProperty("isLocked");
                bool value = (bool)propertyInfo.GetValue(windowToBeLocked, null);
                propertyInfo.SetValue(windowToBeLocked, !value, null);
                windowToBeLocked.Repaint();
            }
            else if (windowToBeLocked != null && windowToBeLocked.GetType().Name == "ProjectBrowser")
            {
                Type type = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.ProjectBrowser");
                PropertyInfo propertyInfo = type.GetProperty("isLocked",
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                bool value = (bool)propertyInfo.GetValue(windowToBeLocked, null);
                propertyInfo.SetValue(windowToBeLocked, !value, null);
                windowToBeLocked.Repaint();
            }
            else if (windowToBeLocked != null && windowToBeLocked.GetType().Name == "SceneHierarchyWindow")
            {
                Type type = Assembly.GetAssembly(typeof(UnityEditor.Editor))
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

        // Console
        [MenuItem("My Tools/Clear Console &c", priority = 13)] // Alt+C
        static void ClearConsole()
        {
            var logEntries = Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
            var clearMethod = logEntries.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
            clearMethod.Invoke(null, null);
        }

        // View
        [MenuItem("My Tools/Maximize %b", priority = 14)] // Ctrl+B
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

        // Tabs
        [MenuItem("My Tools/Close Tab &w", priority = 15)] // Alt+W
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

        // Assets
        [MenuItem("My Tools/Force Refresh Assets &#r", priority = 16)] // Alt+Shift+R
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
    }
}
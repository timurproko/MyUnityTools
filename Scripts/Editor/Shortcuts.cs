using System;
using UnityEditor;
using System.Reflection;
using UnityEngine;

namespace MyTools.Shortcuts
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
            Tools.ActivateWindowUnderCursor();
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
            Tools.ActivateWindowUnderCursor();
            EditorWindow window = EditorWindow.focusedWindow;
            // Assume the game view is focused.
            if (window)
            {
                window.Close();
            }
        }
    }
}

[InitializeOnLoad]
public class HandToolActivator
{
    static HandToolActivator()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
        {
            if (EditorWindow.focusedWindow == SceneView.lastActiveSceneView)
            {
                Tools.current = Tool.View;
                Event.current.Use();
            }
        }
    }
}
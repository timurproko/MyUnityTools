using System;
using UnityEditor;
using System.Reflection;

namespace MyTools.Shortcuts
{
    static class Shortcuts
    {
        // Toggle Gizmos
        [MenuItem("My Tools/Toogle All Gizmos &g", priority = 10)] // Alt+G
        public static void ToggleSceneViewGizmos()
        {
            var currentValue = GetSceneViewGizmosEnabled();
            SetSceneViewGizmos(!currentValue);
        }

        public static void SetSceneViewGizmos(bool gizmosOn)
        {
#if UNITY_EDITOR
            SceneView sv = EditorWindow.GetWindow<SceneView>(null, false);
            sv.drawGizmos = gizmosOn;
#endif
        }

        public static bool GetSceneViewGizmosEnabled()
        {
#if UNITY_EDITOR
            SceneView sv = EditorWindow.GetWindow<SceneView>(null, false);
            return sv.drawGizmos;
#else
            return false;
#endif
        }


#if !UNITY_5
        static float iconSize;
        static bool use3dGizmos;
#endif

        [MenuItem("My Tools/Toogle 3D Icons", priority = 11)]
        public static void HideGizmoIcons()
        {
            Assembly asm = Assembly.GetAssembly(typeof(Editor));
            Type type = asm.GetType("UnityEditor.AnnotationUtility");
            if (type != null)
            {
                PropertyInfo use3dGizmosProperty =
                    type.GetProperty("use3dGizmos", BindingFlags.Static | BindingFlags.NonPublic);
                PropertyInfo iconSizeProperty =
                    type.GetProperty("iconSize", BindingFlags.Static | BindingFlags.NonPublic);

                float nowIconSize = (float)iconSizeProperty.GetValue(asm, null);
                if (nowIconSize > 0) // to hide
                {
#if UNITY_5
					EditorPrefs.SetFloat(Strings.prefs_use3dGizmos, nowIconSize);
#endif
                    iconSize = nowIconSize;
                    iconSizeProperty.SetValue(asm, 0, null);

#if UNITY_5
					bool use3dGizmos = (bool) use3dGizmosProperty.GetValue( asm, null );
					EditorPrefs.SetBool(Strings.prefs_use3dGizmos, use3dGizmos);
#else
                    use3dGizmos = (bool)use3dGizmosProperty.GetValue(asm, null);
#endif
                    use3dGizmosProperty.SetValue(asm, true, null);
                }
                else // to show
                {
#if UNITY_5
					float iconSize = EditorPrefs.GetFloat(Strings.prefs_iconSize);
#endif
                    if (iconSize <= 0)
                        iconSize = 0.03162277f; // Mathf.Pow(10f, -3f + 3f * 0.5f), see to Convert01ToTexelWorldSize()
                    iconSizeProperty.SetValue(asm, iconSize, null);

#if UNITY_5
					bool use3dGizmos = EditorPrefs.GetBool(Strings.prefs_use3dGizmos);
#endif
                    use3dGizmosProperty.SetValue(asm, use3dGizmos, null);
                }
            }
        }


        [MenuItem("My Tools/Toogle Selection Outline", priority = 12)]
        public static void HideOutline()
        {
            Assembly asm = Assembly.GetAssembly(typeof(Editor));
            Type type = asm.GetType("UnityEditor.AnnotationUtility");
            if (type != null)
            {
                PropertyInfo property = type.GetProperty("showSelectionOutline",
                    BindingFlags.Static | BindingFlags.NonPublic);
                bool flag = (bool)property.GetValue(asm, null);
                property.SetValue(asm, !flag, null);
            }
        }


        [MenuItem("My Tools/Toogle Selection Wire", priority = 13)]
        public static void HideWireframe()
        {
            Assembly asm = Assembly.GetAssembly(typeof(Editor));
            Type type = asm.GetType("UnityEditor.AnnotationUtility");
            if (type != null)
            {
                PropertyInfo property = type.GetProperty("showSelectionWire",
                    BindingFlags.Static | BindingFlags.NonPublic);
                bool flag = (bool)property.GetValue(asm, null);
                property.SetValue(asm, !flag, null);
            }
        }
        
        
        [MenuItem("My Tools/Toggle Grid %&#g", priority = 14)] // Ctrl+Alt+Shift+G
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
        
        
        // Clear Console
        [MenuItem("My Tools/Clear Console &c", priority = 21)] // Alt+C
        static void ClearConsole()
        {
            var logEntries = Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
            var clearMethod = logEntries.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
            clearMethod.Invoke(null, null);
        }


        // Lock Panels
        [MenuItem("My Tools/Toggle Lock %&l", priority = 20)] // Ctrl+Alt+L
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
    }
}
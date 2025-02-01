using System;
using System.Reflection;
using UnityEditor;

namespace MyTools
{
    internal static class EditorTools
    {
        [MenuItem(Menus.EDITOR_MENU + "Maximize Tab %b", priority = Menus.EDITOR_INDEX + 200)] // Ctrl+B
        static void Maximize()
        {
            Functions.ActivateWindowUnderCursor();
            EditorWindow window = EditorWindow.focusedWindow;
            if (window)
            {
                window.maximized = !window.maximized;
            }
        }

        [MenuItem(Menus.EDITOR_MENU + "Close Tab %w", priority = Menus.EDITOR_INDEX + 201)] // Ctrl+W
        static void CloseTab()
        {
            Functions.ActivateWindowUnderCursor();
            EditorWindow window = EditorWindow.focusedWindow;
            if (window)
            {
                window.Close();
            }
        }

        [MenuItem(Menus.EDITOR_MENU + "Lock Tab %&l", priority = Menus.EDITOR_INDEX + 202)] // Ctrl+Alt+L
        static void ToggleWindowLock()
        {
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

        [MenuItem(Menus.EDITOR_MENU + "Clear Console %l", priority = Menus.EDITOR_INDEX + 300)] // Ctrl+L
        static void ClearConsole()
        {
            Functions.ClearConsole();
        }
    }
}
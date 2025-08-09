#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using Object = UnityEngine.Object;

namespace MyTools
{
    [InitializeOnLoad]
    public static class Tabs
    {
        static Tabs()
        {
            EditorApplication.update += Update;
        }

        private static void Update()
        {
            if (State.disabled) return;

            if (Event.current != null && Event.current.type == EventType.KeyDown)
            {
                if (Event.current.control && Event.current.keyCode == KeyCode.LeftArrow)
                {
                    SwitchTab(-1);
                    Event.current.Use();
                }
                else if (Event.current.control && Event.current.keyCode == KeyCode.RightArrow)
                {
                    SwitchTab(1);
                    Event.current.Use();
                }
            }
        }

        [MenuItem(Menus.EDITOR_MENU + "Next Tab %RIGHT", priority = Menus.EDITOR_INDEX + 100)]
        private static void NextTab()
        {
            if (State.disabled) return;
            SwitchTab(1);
        }

        [MenuItem(Menus.EDITOR_MENU + "Next Tab %RIGHT", validate = true)]
        private static bool ValidateNextTab() => !State.disabled;

        [MenuItem(Menus.EDITOR_MENU + "Previous Tab %LEFT", priority = Menus.EDITOR_INDEX + 101)]
        private static void PreviousTab()
        {
            if (State.disabled) return;
            SwitchTab(-1);
        }

        [MenuItem(Menus.EDITOR_MENU + "Previous Tab %LEFT", validate = true)]
        private static bool ValidatePreviousTab() => !State.disabled;

        private static void SwitchTab(int direction)
        {
            EditorWindow focusedWindow = EditorWindow.focusedWindow;
            if (focusedWindow == null) return;

            var dockArea = GetDockArea(focusedWindow);
            if (dockArea == null) return;

            var windowsInTabGroup = GetWindowsInTabGroup(dockArea);
            if (windowsInTabGroup.Count == 0) return;

            int currentIndex = windowsInTabGroup.IndexOf(focusedWindow);
            if (currentIndex == -1) return;

            int nextIndex = (currentIndex + direction + windowsInTabGroup.Count) % windowsInTabGroup.Count;
            windowsInTabGroup[nextIndex].Focus();
        }

        private static Object GetDockArea(EditorWindow window)
        {
            var parent = window?.GetType().GetField("m_Parent", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(window);
            if (parent != null && parent.GetType().Name == "DockArea")
                return parent as Object;
            return null;
        }

        private static System.Collections.Generic.List<EditorWindow> GetWindowsInTabGroup(Object dockArea)
        {
            var panes = dockArea?.GetType().GetField("m_Panes", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(dockArea) as System.Collections.Generic.List<EditorWindow>;
            return panes ?? new System.Collections.Generic.List<EditorWindow>();
        }

        [MenuItem(Menus.EDITOR_MENU + "Maximize Tab %b", priority = Menus.EDITOR_INDEX + 102)]
        static void Maximize()
        {
            if (State.disabled) return;

            Functions.ActivateWindowUnderCursor();
            EditorWindow window = EditorWindow.focusedWindow;
            if (window)
            {
                window.maximized = !window.maximized;
            }
        }

        [MenuItem(Menus.EDITOR_MENU + "Maximize Tab %b", validate = true)]
        static bool ValidateMaximize() => !State.disabled;

        [MenuItem(Menus.EDITOR_MENU + "Close Tab %w", priority = Menus.EDITOR_INDEX + 103)]
        static void CloseTab()
        {
            if (State.disabled) return;

            Functions.ActivateWindowUnderCursor();
            EditorWindow window = EditorWindow.focusedWindow;
            if (window)
            {
                window.Close();
            }
        }

        [MenuItem(Menus.EDITOR_MENU + "Close Tab %w", validate = true)]
        static bool ValidateCloseTab() => !State.disabled;

        [MenuItem(Menus.EDITOR_MENU + "Lock Tab %&l", priority = Menus.EDITOR_INDEX + 104)]
        static void ToggleWindowLock()
        {
            if (State.disabled) return;

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

        [MenuItem(Menus.EDITOR_MENU + "Lock Tab %&l", validate = true)]
        static bool ValidateToggleWindowLock() => !State.disabled;
    }
}
#endif
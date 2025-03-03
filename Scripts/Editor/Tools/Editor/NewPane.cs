using MyTools;
using UnityEditor;
using UnityEngine;

public class NewPane : EditorWindow
{
    [MenuItem(Menus.NEW_PANE_MENU + "Project View %#q", priority = Menus.EDITOR_INDEX + 100)] // Ctrl+Shift+Q
    public static void ShowProjectWindow()
    {
        OpenWindow("UnityEditor.ProjectBrowser", "Project", "UnityEditor.ProjectBrowser");
    }

    [MenuItem(Menus.NEW_PANE_MENU + "Hierarchy View %#w", priority = Menus.EDITOR_INDEX + 101)] // Ctrl+Shift+W
    public static void ShowHierarchyWindow()
    {
        OpenWindow("UnityEditor.SceneHierarchyWindow", "Hierarchy", "UnityEditor.SceneHierarchyWindow");
    }

    private static void OpenWindow(string windowTypeName, string title, string iconName)
    {
        var windowType = typeof(EditorWindow).Assembly.GetType(windowTypeName);

        if (Resources.FindObjectsOfTypeAll(windowType) is EditorWindow[] { Length: > 0 } existingWindow)
        {
            existingWindow[0].Focus();
            return;
        }

        var newWindow = CreateInstance(windowType) as EditorWindow;

        if (newWindow != null)
        {
            newWindow.titleContent = new GUIContent(title, EditorGUIUtility.IconContent(iconName).image);
            newWindow.Show();
            newWindow.Focus();
        }
    }
}
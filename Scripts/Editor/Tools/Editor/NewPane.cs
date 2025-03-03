using MyTools;
using UnityEditor;
using UnityEngine;

public class NewPane : EditorWindow
{
    [MenuItem(Menus.NEW_PANE_MENU + "Hierarchy View %#w", priority = Menus.EDITOR_INDEX + 100)] // Ctrl+Shift+W
    public static void ShowWindow()
    {
        OpenHierarchyPane();
    }

    private static void OpenHierarchyPane()
    {
        var hierarchyWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
        var hierarchyWindow = CreateInstance(hierarchyWindowType) as EditorWindow;

        hierarchyWindow.titleContent = new GUIContent("Hierarchy",
            EditorGUIUtility.IconContent("UnityEditor.SceneHierarchyWindow").image);

        hierarchyWindow.Show();
        hierarchyWindow.Focus();
    }
}
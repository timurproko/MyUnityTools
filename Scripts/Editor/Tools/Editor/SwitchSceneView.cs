using MyTools;
using UnityEditor;

public static class SwitchSceneView
{
    [MenuItem(Menus.EDITOR_MENU + "Switch Scene View _TAB", priority = Menus.EDITOR_INDEX + 300)]
    private static void SwitchView()
    {
        EditorWindow focusedWindow = EditorWindow.focusedWindow;

        if (focusedWindow != null)
        {
            string windowType = focusedWindow.GetType().Name;

            if (windowType == "SceneView")
            {
                EditorApplication.ExecuteMenuItem("Window/General/Game");
            }
            else if (windowType == "GameView")
            {
                EditorApplication.ExecuteMenuItem("Window/General/Scene");
            }
        }
    }
}
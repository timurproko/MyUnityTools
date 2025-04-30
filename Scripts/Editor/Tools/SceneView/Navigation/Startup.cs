using UnityEditor;

namespace SceneViewTools
{
    [InitializeOnLoad]
    public static class Startup
    {
        static Startup()
        {
            EditorApplication.update += WaitForSceneView;
        }

        private static void WaitForSceneView()
        {
            if (SceneView.lastActiveSceneView == null)
                return;

            EditorApplication.update -= WaitForSceneView;
            EditorApplication.delayCall += TryRestoreView;
        }

        private static void TryRestoreView()
        {
            if (!EditorPrefs.HasKey("MyTools.SceneViewTools.CurrentViewType"))
                return;

            var viewType = SceneViewNavigationIO.ReadFromEditorPrefs();
            SceneViewNavigationMenu.SetSceneView(viewType);
            SceneViewNavigationMenu.SetSceneView(viewType);
        }
    }
}
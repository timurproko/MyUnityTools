using UnityEditor;

namespace SceneViewTools
{
    [InitializeOnLoad]
    public static class Startup
    {
        static Startup()
        {
            EditorApplication.delayCall += TryRestoreView;
        }

        private static void TryRestoreView()
        {
            if (SceneView.lastActiveSceneView == null)
                return;

            if (!EditorPrefs.HasKey("MyTools.SceneViewTools.CurrentViewType"))
                return;

            var viewType = SceneViewNavigationIO.ReadFromEditorPrefs();

            SceneViewNavigationMenu.SetSceneView(viewType);
            SceneViewNavigationMenu.SetSceneView(viewType);
        }
    }
}
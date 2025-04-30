using UnityEditor;

namespace SceneViewTools
{
    [InitializeOnLoad]
    public static class Startup
    {
        private static bool _isRestored;

        static Startup()
        {
            if (_isRestored) return;
            EditorApplication.update += TryRestoreView;
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

            EditorApplication.update -= TryRestoreView;
            _isRestored = true;
        }
    }
}
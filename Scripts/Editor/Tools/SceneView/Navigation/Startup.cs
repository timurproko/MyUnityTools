using UnityEditor;
using UnityEngine;

namespace SceneViewTools
{
    [InitializeOnLoad]
    public static class Startup
    {
        static Startup()
        {
            EditorApplication.update += TryRestoreView;
        }

        private static bool applied;

        private static void TryRestoreView()
        {
            if (applied || SceneView.lastActiveSceneView == null)
                return;

            if (!EditorPrefs.HasKey("MyTools.SceneViewTools.CurrentViewType"))
                return;

            var viewType = SceneViewNavigationIO.ReadFromEditorPrefs();
            
            SceneViewNavigationMenu.SetSceneView(viewType);
            SceneViewNavigationMenu.SetSceneView(viewType);
        }
    }
}
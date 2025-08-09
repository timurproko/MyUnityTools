#if UNITY_EDITOR
using UnityEditor;

namespace SceneViewTools
{
    [InitializeOnLoad]
    public static class Startup
    {
        static Startup()
        {
            EditorApplication.delayCall += InitializePrefs;
            EditorApplication.delayCall += TryRestoreView;
        }

        private static void InitializePrefs()
        {
            if (!EditorPrefs.HasKey(SceneViewNavigationIO.CurrentViewTypeKey))
            {
                var viewType = SceneViewType.Perspective;
                
                SceneViewNavigationIO.WriteToEditorPrefs(viewType);
                SceneViewNavigationMenu.SetSceneView(viewType);
                SceneViewNavigationMenu.SetSceneView(viewType);
            }
        }

        private static void TryRestoreView()
        {
            if (SessionState.GetBool(SceneViewNavigationIO.CurrentViewTypeKey, false))
                return;

            SessionState.SetBool(SceneViewNavigationIO.CurrentViewTypeKey, true);

            if (SceneView.lastActiveSceneView == null)
                return;

            if (!EditorPrefs.HasKey(SceneViewNavigationIO.CurrentViewTypeKey))
                return;

            var viewType = SceneViewNavigationIO.ReadFromEditorPrefs();
            
            SceneViewNavigationMenu.SetSceneView(viewType);
            SceneViewNavigationMenu.SetSceneView(viewType);
        }
    }
}
#endif
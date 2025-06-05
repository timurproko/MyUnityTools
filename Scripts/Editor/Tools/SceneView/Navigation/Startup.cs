#if UNITY_EDITOR
using UnityEditor;

namespace SceneViewTools
{
    [InitializeOnLoad]
    public static class Startup
    {
        private const string HasRestoredViewKey = "MyTools.SceneViewTools.HasRestoredView";
        private const string CurrentViewTypeKey = "MyTools.SceneViewTools.CurrentViewType";

        static Startup()
        {
            EditorApplication.delayCall += InitializePrefs;
            EditorApplication.delayCall += TryRestoreView;
        }

        private static void InitializePrefs()
        {
            if (!EditorPrefs.HasKey(CurrentViewTypeKey))
            {
                var viewType = SceneViewType.Perspective;
                
                SceneViewNavigationIO.WriteToEditorPrefs(viewType);
                SceneViewNavigationMenu.SetSceneView(viewType);
                SceneViewNavigationMenu.SetSceneView(viewType);
            }
        }

        private static void TryRestoreView()
        {
            if (SessionState.GetBool(HasRestoredViewKey, false))
                return;

            SessionState.SetBool(HasRestoredViewKey, true);

            if (SceneView.lastActiveSceneView == null)
                return;

            if (!EditorPrefs.HasKey(CurrentViewTypeKey))
                return;

            var viewType = SceneViewNavigationIO.ReadFromEditorPrefs();
            
            SceneViewNavigationMenu.SetSceneView(viewType);
            SceneViewNavigationMenu.SetSceneView(viewType);
        }
    }
}
#endif
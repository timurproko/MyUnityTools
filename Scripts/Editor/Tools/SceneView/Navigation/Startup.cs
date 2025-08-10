#if UNITY_EDITOR
using MyTools;
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
            if (State.disabled) return;

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
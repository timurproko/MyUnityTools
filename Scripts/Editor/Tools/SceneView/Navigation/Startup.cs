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

            const string restoredOnceKey = "SceneViewTools.RestoredOnce";
            if (SessionState.GetBool(restoredOnceKey, false))
                return;
            SessionState.SetBool(restoredOnceKey, true);

            if (SceneView.lastActiveSceneView == null)
                return;

            if (SceneViewNavigationIO.TryGetLastViewState(out _))
            {
                SceneViewNavigationIO.RequestUseLastPoseCalls(2);
            }

            if (!EditorPrefs.HasKey(SceneViewNavigationIO.CurrentViewTypeKey))
                return;

            var viewType = SceneViewNavigationIO.ReadFromEditorPrefs();

            SceneViewNavigationMenu.SetSceneView(viewType);
            SceneViewNavigationMenu.SetSceneView(viewType);
        }
    }
}
#endif
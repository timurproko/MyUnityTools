#if UNITY_EDITOR
using MyTools;
using SceneViewTools;
using UnityEditor;

namespace Autosave
{
    [InitializeOnLoad]
    public static class SceneViewAutoSave
    {
        static SceneViewAutoSave()
        {
            EditorApplication.quitting += SaveCurrentView;
            EditorApplication.playModeStateChanged += _ => SaveCurrentView(); // extra safety
            EditorApplication.focusChanged += _ => SaveCurrentView();         // extra safety
        }

        private static void SaveCurrentView()
        {
            if (State.disabled) return;

            var sv = SceneView.lastActiveSceneView;
            if (sv == null) return;

            SceneViewNavigationIO.SaveLastViewState(sv.size, sv.rotation, sv.pivot, sv.orthographic);

            ActiveSceneView.sceneView = sv;
            SceneViewNavigationIO.SaveViewState(
                ActiveSceneView.SceneViewType,
                sv.size, sv.rotation, sv.pivot, sv.orthographic
            );
        }
    }
}
#endif
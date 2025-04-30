using SceneViewTools;
using UnityEditor;

namespace Autosave
{
    [InitializeOnLoad]
    public static class SceneViewAutoSave
    {
        static SceneViewAutoSave()
        {
            EditorApplication.quitting += SaveCurrentViewOnExit;
        }

        private static void SaveCurrentViewOnExit()
        {
            if (SceneView.lastActiveSceneView == null)
                return;

            var viewType = SceneViewNavigationIO.ReadFromEditorPrefs();
            SceneViewNavigationManager.SaveSceneView(viewType);
        }
    }
}
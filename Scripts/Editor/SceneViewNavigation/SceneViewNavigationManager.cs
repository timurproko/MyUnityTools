using UnityEditor;

namespace SceneViewNavigation
{
    public static class SceneViewNavigationManager
    {
        public static void SetView(SceneViewType viewType)
        {
            ActiveSceneView.sceneView = SceneView.lastActiveSceneView;

            if (ActiveSceneView.sceneView == null)
                return;

            if (ActiveSceneView.sceneView.in2DMode)
            {
                ActiveSceneView.sceneView.orthographic = !ActiveSceneView.sceneView.orthographic;
            }
            else
            {
                if (viewType == SceneViewType.Perspective)
                {
                    EnableSkybox();
                }
                else
                {
                    DisableSkybox();
                }

                if (SceneViewNavigationSave.TryGetViewState(viewType, out var savedState))
                {
                    ApplyNewValues(savedState);
                }
                else
                {
                    ApplyDefaultValues(viewType);
                }
            }

            ActiveSceneView.sceneView.Repaint();
        }

        public static void ApplyDefaultValues(SceneViewType viewType)
        {
            ActiveSceneView.sceneView.size = DefaultValues.size;
            ActiveSceneView.sceneView.pivot = DefaultValues.pivot;
            ActiveSceneView.sceneView.rotation = SceneViewNavigationReset.GetDefaultRotation(viewType);
            ActiveSceneView.sceneView.orthographic = SceneViewNavigationReset.GetDefaultOrthographic(viewType);
        }

        public static void ApplyNewValues(SceneViewNavigationSave.ViewState savedState)
        {
            ActiveSceneView.sceneView.size = savedState.size;
            ActiveSceneView.sceneView.pivot = savedState.pivot;
            ActiveSceneView.sceneView.orthographic = savedState.orthographic;
            if (!ActiveSceneView.sceneView.in2DMode)
            {
                ActiveSceneView.sceneView.rotation = savedState.rotation;
            }
        }

        public static void SaveSceneView(SceneViewType viewType)
        {
            if (ActiveSceneView.sceneView != null)
            {
                SceneViewNavigationSave.SaveViewState(
                    ActiveSceneView.SceneViewType,
                    ActiveSceneView.sceneView.size,
                    ActiveSceneView.sceneView.rotation,
                    ActiveSceneView.sceneView.pivot,
                    ActiveSceneView.sceneView.orthographic
                );
                SceneViewNavigationSave.WriteToEditorPrefs(viewType);
            }
        }
        
        public static void SetSceneViewGizmos(bool gizmosOn)
        {
#if UNITY_EDITOR
            SceneView sv = EditorWindow.GetWindow<SceneView>(null, false);
            sv.drawGizmos = gizmosOn;
#endif
        }

        public static bool GetSceneViewGizmosEnabled()
        {
#if UNITY_EDITOR
            SceneView sv = EditorWindow.GetWindow<SceneView>(null, false);
            return sv.drawGizmos;
#else
            return false;
#endif
        }

        public static void DisableSkybox()
        {
            ActiveSceneView.sceneView = SceneView.lastActiveSceneView;
            ActiveSceneView.sceneView.sceneViewState.showSkybox = false;
        }
        
        public static void EnableSkybox()
        {
            ActiveSceneView.sceneView = SceneView.lastActiveSceneView;
            ActiveSceneView.sceneView.sceneViewState.showSkybox = true;
        }
    }
}
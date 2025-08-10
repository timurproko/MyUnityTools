#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace SceneViewTools
{
    public static class SceneViewNavigationManager
    {
        public static void SetView(SceneViewType viewType)
        {
            ActiveSceneView.sceneView = SceneView.lastActiveSceneView;
            if (ActiveSceneView.sceneView == null)
                return;

            if (SceneViewNavigationIO.TryConsumeUseLastPoseRequest() &&
                SceneViewNavigationIO.TryGetLastViewState(out var last))
            {
                ApplyNewValues(last);
                ActiveSceneView.sceneView.Repaint();
                return;
            }

            if (SceneViewNavigationIO.TryGetViewState(viewType, out var savedState))
            {
                ApplyNewValues(savedState);
            }
            else
            {
                ApplyDefaultValues(viewType);
            }

            ActiveSceneView.sceneView.Repaint();
        }

        private static void ApplyDefaultValues(SceneViewType viewType)
        {
            ActiveSceneView.sceneView.size = DefaultValues.size;
            ActiveSceneView.sceneView.pivot = DefaultValues.pivot;
            ActiveSceneView.sceneView.rotation = GetDefaultRotation(viewType);
            ActiveSceneView.sceneView.orthographic = IsOrthographic(viewType);
        }

        private static void ApplyNewValues(SceneViewNavigationIO.ViewState savedState)
        {
            ActiveSceneView.sceneView.size = savedState.size;
            ActiveSceneView.sceneView.pivot = savedState.pivot;
            ActiveSceneView.sceneView.orthographic = savedState.orthographic;
            ActiveSceneView.sceneView.rotation = savedState.rotation;
        }

        public static void SaveSceneView(SceneViewType viewType)
        {
            if (ActiveSceneView.sceneView != null)
            {
                var sv = ActiveSceneView.sceneView;

                SceneViewNavigationIO.SaveViewState(
                    ActiveSceneView.SceneViewType,
                    sv.size, sv.rotation, sv.pivot, sv.orthographic
                );
                SceneViewNavigationIO.SaveLastViewState(sv.size, sv.rotation, sv.pivot, sv.orthographic);

                SceneViewNavigationIO.WriteToEditorPrefs(viewType);
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

        public static void ResetAllSceneViews()
        {
            var sceneViewTypes = Enum.GetValues(typeof(SceneViewType)) as SceneViewType[];
            foreach (var viewType in sceneViewTypes)
            {
                ResetView(viewType);
            }
            RedrawLastSavedSceneView();
        }

        public static void ResetView(SceneViewType viewType)
        {
            float size = DefaultValues.size;
            Vector3 pivot = DefaultValues.pivot;
            Quaternion rotation = GetDefaultRotation(viewType);
            bool orthographic = IsOrthographic(viewType);

            SceneViewNavigationIO.SaveViewState(viewType, size, rotation, pivot, orthographic);
        }

        public static void RedrawLastSavedSceneView()
        {
            var lastSavedViewType = SceneViewNavigationIO.ReadFromEditorPrefs();
            Quaternion rotation = GetDefaultRotation(lastSavedViewType);
            bool orthographic = IsOrthographic(lastSavedViewType);

            ActiveSceneView.sceneView = SceneView.lastActiveSceneView;
            ActiveSceneView.sceneView.size = DefaultValues.size;
            ActiveSceneView.sceneView.pivot = DefaultValues.pivot;

            ActiveSceneView.sceneView.rotation = rotation;
            ActiveSceneView.sceneView.orthographic = orthographic;

            ActiveSceneView.sceneView.Repaint();
        }

        private static Quaternion GetDefaultRotation(SceneViewType viewType)
        {
            return viewType switch
            {
                SceneViewType.Perspective => DefaultRotation.Perspective,
                SceneViewType.Top => DefaultRotation.Top,
                SceneViewType.Bottom => DefaultRotation.Bottom,
                SceneViewType.Front => DefaultRotation.Front,
                SceneViewType.Back => DefaultRotation.Back,
                SceneViewType.Left => DefaultRotation.Left,
                SceneViewType.Right => DefaultRotation.Right,
                _ => throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null)
            };
        }

        private static bool IsOrthographic(SceneViewType viewType)
        {
            return viewType != SceneViewType.Perspective;
        }
    }
}
#endif

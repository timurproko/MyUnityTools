using System;
using UnityEditor;
using UnityEngine;

namespace SceneViewNavigation
{
    public static class SceneViewNavigationManager
    {
        public static void SetView(SceneViewType viewType)
        {
            ActiveSceneView.sceneView = SceneView.lastActiveSceneView;

            if (ActiveSceneView.sceneView == null)
                return;

            if (Is2DMode())
            {
                ActiveSceneView.sceneView.orthographic = !ActiveSceneView.sceneView.orthographic;
            }
            else
            {
                // if (viewType == SceneViewType.Perspective)
                // {
                //     EnableSkybox();
                // }
                // else
                // {
                //     DisableSkybox();
                // }

                if (SceneViewNavigationIO.TryGetViewState(viewType, out var savedState))
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
            if (!Is2DMode())
            {
                ActiveSceneView.sceneView.rotation = savedState.rotation;
            }
        }

        public static void SaveSceneView(SceneViewType viewType)
        {
            if (ActiveSceneView.sceneView != null)
            {
                SceneViewNavigationIO.SaveViewState(
                    ActiveSceneView.SceneViewType,
                    ActiveSceneView.sceneView.size,
                    ActiveSceneView.sceneView.rotation,
                    ActiveSceneView.sceneView.pivot,
                    ActiveSceneView.sceneView.orthographic
                );
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
            if (Is2DMode())
            {
                ActiveSceneView.sceneView.orthographic = true;
            }
            else
            {
                ActiveSceneView.sceneView.rotation = rotation;
                ActiveSceneView.sceneView.orthographic = orthographic;
            }

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
            if (viewType != SceneViewType.Perspective)
            {
                return true;
            }

            return false;
        }

        private static bool Is2DMode()
        {
            if (SceneView.lastActiveSceneView.in2DMode)
            {
                return true;
            }

            return false;
        }
    }
}
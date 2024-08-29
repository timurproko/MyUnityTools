using System;
using UnityEditor;
using UnityEngine;

namespace SceneViewNavigation
{
    public static class SceneViewNavigationReset
    {
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
            bool orthographic = GetDefaultOrthographic(viewType);

            SceneViewNavigationSave.SaveViewState(viewType, size, rotation, pivot, orthographic);
        }

        public static void RedrawLastSavedSceneView()
        {
            var lastSavedViewType = SceneViewNavigationSave.ReadFromEditorPrefs();
            Quaternion rotation = GetDefaultRotation(lastSavedViewType);
            bool orthographic = GetDefaultOrthographic(lastSavedViewType);

            ActiveSceneView.sceneView = SceneView.lastActiveSceneView;
            ActiveSceneView.sceneView.size = DefaultValues.size;
            ActiveSceneView.sceneView.pivot = DefaultValues.pivot;
            if (!ActiveSceneView.sceneView.in2DMode)
            {
                ActiveSceneView.sceneView.rotation = rotation;
                ActiveSceneView.sceneView.orthographic = orthographic;
            }
            else
            {
                ActiveSceneView.sceneView.orthographic = true;
            }

            ActiveSceneView.sceneView.Repaint();
        }

        public static Quaternion GetDefaultRotation(SceneViewType viewType)
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

        public static bool GetDefaultOrthographic(SceneViewType viewType)
        {
            if (viewType == SceneViewType.Perspective)
            {
                return false;
            }

            return true;
        }
    }
}
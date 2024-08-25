using System;
using UnityEditor;
using UnityEngine;

namespace MyTools.SceneViewTools
{
    public static class SceneViewResetAll
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
            var size = DefaultValues.size;
            var pivot = DefaultValues.pivot;
            var rotation = GetDefaultRotation(viewType);
            var orthographic = GetDefaultOrthographic(viewType);

            SceneViewSaveData.SaveViewState(viewType, size, rotation, pivot, orthographic);
        }

        public static void RedrawLastSavedSceneView()
        {
            var lastSavedViewType = SceneViewSaveData.GetLastSavedSceneViewType();
            var rotation = GetDefaultRotation(lastSavedViewType);
            var orthographic = GetDefaultOrthographic(lastSavedViewType);

            SceneViewRef.sceneView = SceneView.lastActiveSceneView;
            SceneViewRef.sceneView.size = DefaultValues.size;
            SceneViewRef.sceneView.pivot = DefaultValues.pivot;
            SceneViewRef.sceneView.rotation = rotation;
            SceneViewRef.sceneView.orthographic = orthographic;
            SceneViewRef.sceneView.Repaint();
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
            return viewType switch
            {
                SceneViewType.Perspective => DefaultOrthographic.Perspective,
                SceneViewType.Top => DefaultOrthographic.Top,
                SceneViewType.Bottom => DefaultOrthographic.Bottom,
                SceneViewType.Front => DefaultOrthographic.Front,
                SceneViewType.Back => DefaultOrthographic.Back,
                SceneViewType.Left => DefaultOrthographic.Left,
                SceneViewType.Right => DefaultOrthographic.Right,
                _ => throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null)
            };
        }
    }
}
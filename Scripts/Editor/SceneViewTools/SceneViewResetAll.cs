using System;
using UnityEditor;
using UnityEngine;

namespace MyTools.SceneViewTools
{
    public static class SceneViewResetAll
    {
        public static void ResetAllSceneViews()
        {
            var sceneViewTypes = Enum.GetValues(typeof(SceneViewTypes)) as SceneViewTypes[];

            foreach (var viewType in sceneViewTypes)
            {
                ResetView(viewType);
            }

            RedrawLastSavedSceneView();
        }

        private static void ResetView(SceneViewTypes viewTypes)
        {
            var size = DefaultValues.size;
            var pivot = DefaultValues.pivot;
            var rotation = GetDefaultRotation(viewTypes);

            SceneViewSaveData.SaveViewState(viewTypes, size, rotation, pivot);
        }

        private static void RedrawLastSavedSceneView()
        {
            var lastSavedViewType = SceneViewSaveData.GetLastSavedSceneViewType();
            var rotation = GetDefaultRotation(lastSavedViewType);

            SceneViewRef.sceneView = SceneView.lastActiveSceneView;
            SceneViewRef.sceneView.size = DefaultValues.size;
            SceneViewRef.sceneView.pivot = DefaultValues.pivot;
            SceneViewRef.sceneView.rotation = rotation;
            SceneViewRef.sceneView.Repaint();
        }

        private static Quaternion GetDefaultRotation(SceneViewTypes viewTypes)
        {
            return viewTypes switch
            {
                SceneViewTypes.Perspective => DefaultRotation.Perspective,
                SceneViewTypes.Top => DefaultRotation.Top,
                SceneViewTypes.Bottom => DefaultRotation.Bottom,
                SceneViewTypes.Front => DefaultRotation.Front,
                SceneViewTypes.Back => DefaultRotation.Back,
                SceneViewTypes.Left => DefaultRotation.Left,
                SceneViewTypes.Right => DefaultRotation.Right,
                _ => throw new ArgumentOutOfRangeException(nameof(viewTypes), viewTypes, null)
            };
        }
    }
}
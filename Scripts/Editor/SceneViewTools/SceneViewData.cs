using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace MyTools.SceneViewTools
{
    public class SceneViewData : ScriptableObject
    {
        private float size;
        private Quaternion rotation;
        private Vector3 pivot;
        private static SceneView sceneView;


        [Serializable]
        public struct ViewState
        {
            public SceneViewType sceneViewType;
            public float size;
            public Quaternion rotation;
            public Vector3 pivot;
        }

        [SerializeField] private List<ViewState> viewStateList = new();
        public SceneViewType lastActiveSceneViewType;
        private Dictionary<SceneViewType, ViewState> viewStateDictionary;

        private void OnEnable()
        {
            // Initialize the dictionary from the list when the ScriptableObject is loaded
            viewStateDictionary = new Dictionary<SceneViewType, ViewState>();
            foreach (var viewState in viewStateList)
            {
                viewStateDictionary[viewState.sceneViewType] = viewState;
            }

            if (SceneViewToggleResetAll._enabled)
            {
                ResetAllSceneViews();
            }
        }

        public void SaveViewState(SceneViewType viewType, float size, Quaternion rotation, Vector3 pivot)
        {
            var viewState = new ViewState
                { sceneViewType = viewType, size = size, rotation = rotation, pivot = pivot };
            viewStateDictionary[viewType] = viewState;
            UpdateViewStateList();
        }

        public bool TryGetViewState(SceneViewType viewType, out ViewState viewState)
        {
            return viewStateDictionary.TryGetValue(viewType, out viewState);
        }

        private void UpdateViewStateList()
        {
            viewStateList.Clear();
            viewStateList.AddRange(viewStateDictionary.Values);
        }

        private void ResetAllSceneViews()
        {
            var viewActions = new Dictionary<SceneViewType, Action<bool>>
            {
                {
                    SceneViewType.Perspective, shouldRedraw =>
                    {
                        if (shouldRedraw) RedrawPerspectiveView();
                        else ResetPerspectiveView();
                    }
                },
                {
                    SceneViewType.Top, shouldRedraw =>
                    {
                        if (shouldRedraw) RedrawTopView();
                        else ResetTopView();
                    }
                },
                {
                    SceneViewType.Bottom, shouldRedraw =>
                    {
                        if (shouldRedraw) RedrawBottomView();
                        else ResetBottomView();
                    }
                },
                {
                    SceneViewType.Front, shouldRedraw =>
                    {
                        if (shouldRedraw) RedrawFrontView();
                        else ResetFrontView();
                    }
                },
                {
                    SceneViewType.Back, shouldRedraw =>
                    {
                        if (shouldRedraw) RedrawBackView();
                        else ResetBackView();
                    }
                },
                {
                    SceneViewType.Right, shouldRedraw =>
                    {
                        if (shouldRedraw) RedrawRightView();
                        else ResetRightView();
                    }
                },
                {
                    SceneViewType.Left, shouldRedraw =>
                    {
                        if (shouldRedraw) RedrawLeftView();
                        else ResetLeftView();
                    }
                },
            };

            foreach (var kvp in viewActions)
            {
                kvp.Value(false); // Reset
                if (kvp.Key == lastActiveSceneViewType)
                {
                    kvp.Value(true); // Redraw only the last active view
                }
            }

            UpdateViewStateList();
        }

        void ResetView(SceneViewType viewType, Quaternion defaultRotation)
        {
            if (sceneView != null)
            {
                size = DefaultsValue.size;
                pivot = DefaultsValue.pivot;
                rotation = defaultRotation;
                SaveViewState(viewType, size, rotation, pivot);
            }
        }

        void RedrawView(Quaternion defaultRotation)
        {
            if (sceneView != null)
            {
                sceneView.size = DefaultsValue.size;
                sceneView.pivot = DefaultsValue.pivot;
                if (!sceneView.in2DMode)
                {
                    sceneView.rotation = defaultRotation;
                }

                sceneView.Repaint();
            }
        }

        void ResetPerspectiveView() => ResetView(SceneViewType.Perspective, DefaultRotation.Perspective);
        void ResetTopView() => ResetView(SceneViewType.Top, DefaultRotation.Top);
        void ResetBottomView() => ResetView(SceneViewType.Bottom, DefaultRotation.Bottom);
        void ResetFrontView() => ResetView(SceneViewType.Front, DefaultRotation.Front);
        void ResetBackView() => ResetView(SceneViewType.Back, DefaultRotation.Back);
        void ResetRightView() => ResetView(SceneViewType.Right, DefaultRotation.Right);
        void ResetLeftView() => ResetView(SceneViewType.Left, DefaultRotation.Left);
        void RedrawPerspectiveView() => RedrawView(DefaultRotation.Perspective);
        void RedrawTopView() => RedrawView(DefaultRotation.Top);
        void RedrawBottomView() => RedrawView(DefaultRotation.Bottom);
        void RedrawFrontView() => RedrawView(DefaultRotation.Front);
        void RedrawBackView() => RedrawView(DefaultRotation.Back);
        void RedrawRightView() => RedrawView(DefaultRotation.Right);
        void RedrawLeftView() => RedrawView(DefaultRotation.Left);
    }
}
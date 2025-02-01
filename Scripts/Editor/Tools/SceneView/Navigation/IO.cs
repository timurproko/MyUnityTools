using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SceneViewTools
{
    public static class SceneViewNavigationIO
    {
        private const string key = "MyTools.SceneViewTools.";

        [Serializable]
        public struct ViewState
        {
            public SceneViewType sceneViewType;
            public float size;
            public Quaternion rotation;
            public Vector3 pivot;
            public bool orthographic;
        }

        private static Dictionary<SceneViewType, ViewState> _viewStateDictionary;

        // Lazy-loaded property for view state dictionary
        private static Dictionary<SceneViewType, ViewState> ViewStateDictionary
        {
            get
            {
                if (_viewStateDictionary == null)
                {
                    LoadViewStates();
                }

                return _viewStateDictionary;
            }
        }

        // Generate a consistent key for storing view state
        private static string GetViewStateKey(SceneViewType viewType) => $"{key}{viewType}";

        private static void LoadViewStates()
        {
            _viewStateDictionary = new Dictionary<SceneViewType, ViewState>();

            foreach (SceneViewType viewType in Enum.GetValues(typeof(SceneViewType)))
            {
                string key = GetViewStateKey(viewType);
                if (EditorPrefs.HasKey(key))
                {
                    string json = EditorPrefs.GetString(key);
                    ViewState viewState = JsonUtility.FromJson<ViewState>(json);
                    _viewStateDictionary[viewType] = viewState;
                }
            }
        }

        public static void SaveViewState(SceneViewType viewType, float size, Quaternion rotation, Vector3 pivot,
            bool orthographic)
        {
            var viewState = new ViewState
            {
                sceneViewType = viewType,
                size = size,
                rotation = rotation,
                pivot = pivot,
                orthographic = orthographic
            };

            ViewStateDictionary[viewType] = viewState;

            string json = JsonUtility.ToJson(viewState);
            EditorPrefs.SetString(GetViewStateKey(viewType), json);
        }

        public static bool TryGetViewState(SceneViewType viewType, out ViewState viewState)
        {
            return ViewStateDictionary.TryGetValue(viewType, out viewState);
        }

        public static SceneViewType ReadFromEditorPrefs()
        {
            var json = EditorPrefs.GetString(key);
            return JsonUtility.FromJson<SceneViewType>(json);
        }

        public static void WriteToEditorPrefs(SceneViewType viewType)
        {
            var json = JsonUtility.ToJson(viewType);
            EditorPrefs.SetString(key, json);
        }
    }
}
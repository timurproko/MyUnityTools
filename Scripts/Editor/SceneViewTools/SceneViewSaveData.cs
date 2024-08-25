using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;

namespace MyTools.SceneViewTools
{
    public static class SceneViewSaveData
    {
        private const string EditorPrefsKeyPrefix = "MyTools.SceneViewTools.";
        private const string LastActiveViewTypeKey = EditorPrefsKeyPrefix + "LastActiveSceneViewType";

        [Serializable]
        public struct ViewState
        {
            [FormerlySerializedAs("sceneViewTypes")] public SceneViewType sceneViewType;
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
        private static string GetViewStateKey(SceneViewType viewType) => $"{EditorPrefsKeyPrefix}{viewType}";

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

        public static void SaveViewState(SceneViewType viewType, float size, Quaternion rotation, Vector3 pivot, bool orthographic)
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

        public static void SaveLastActiveSceneViewType(SceneViewType viewType)
        {
            EditorPrefs.SetInt(LastActiveViewTypeKey, (int)viewType);
        }

        public static SceneViewType GetLastSavedSceneViewType()
        {
            return (SceneViewType)EditorPrefs.GetInt(LastActiveViewTypeKey, (int)SceneViewType.Perspective);
        }
    }
}
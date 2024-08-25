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
            [FormerlySerializedAs("sceneViewType")] public SceneViewTypes sceneViewTypes;
            public float size;
            public Quaternion rotation;
            public Vector3 pivot;
        }

        private static Dictionary<SceneViewTypes, ViewState> _viewStateDictionary;

        // Lazy-loaded property for view state dictionary
        private static Dictionary<SceneViewTypes, ViewState> ViewStateDictionary
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
        private static string GetViewStateKey(SceneViewTypes viewTypes) => $"{EditorPrefsKeyPrefix}{viewTypes}";

        private static void LoadViewStates()
        {
            _viewStateDictionary = new Dictionary<SceneViewTypes, ViewState>();

            foreach (SceneViewTypes viewType in Enum.GetValues(typeof(SceneViewTypes)))
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

        public static void SaveViewState(SceneViewTypes viewTypes, float size, Quaternion rotation, Vector3 pivot)
        {
            var viewState = new ViewState
            {
                sceneViewTypes = viewTypes,
                size = size,
                rotation = rotation,
                pivot = pivot
            };

            ViewStateDictionary[viewTypes] = viewState;

            string json = JsonUtility.ToJson(viewState);
            EditorPrefs.SetString(GetViewStateKey(viewTypes), json);
        }

        public static bool TryGetViewState(SceneViewTypes viewTypes, out ViewState viewState)
        {
            return ViewStateDictionary.TryGetValue(viewTypes, out viewState);
        }

        public static void SaveLastActiveSceneViewType(SceneViewTypes viewTypes)
        {
            EditorPrefs.SetInt(LastActiveViewTypeKey, (int)viewTypes);
        }

        public static SceneViewTypes GetLastSavedSceneViewType()
        {
            return (SceneViewTypes)EditorPrefs.GetInt(LastActiveViewTypeKey, (int)SceneViewTypes.Perspective);
        }
    }
}
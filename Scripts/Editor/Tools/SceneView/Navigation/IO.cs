using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SceneViewTools
{
    public static class SceneViewNavigationIO
    {
        private const string ViewStateKeyPrefix = "MyTools.SceneViewTools.ViewState";
        private const string CurrentViewTypeKey = "MyTools.SceneViewTools.CurrentViewType";

        [Serializable]
        public struct ViewState
        {
            public float size;
            public Quaternion rotation;
            public Vector3 pivot;
            public bool orthographic;
        }

        private static Dictionary<SceneViewType, ViewState> _viewStateDictionary;
        private static readonly SceneViewType[] AllSceneViewTypes = (SceneViewType[])Enum.GetValues(typeof(SceneViewType));

        private static Dictionary<SceneViewType, ViewState> ViewStateDictionary
        {
            get
            {
                if (_viewStateDictionary == null)
                    LoadViewStates();
                return _viewStateDictionary;
            }
        }

        private static string GetViewStateKey(SceneViewType viewType) => $"{ViewStateKeyPrefix}{viewType}";

        private static void LoadViewStates()
        {
            _viewStateDictionary = new Dictionary<SceneViewType, ViewState>();
            foreach (var viewType in AllSceneViewTypes)
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
            return (SceneViewType)EditorPrefs.GetInt(CurrentViewTypeKey);
        }

        public static void WriteToEditorPrefs(SceneViewType viewType)
        {
            EditorPrefs.SetInt(CurrentViewTypeKey, (int)viewType);
        }
    }
}

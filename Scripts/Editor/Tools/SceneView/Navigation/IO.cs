#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MyTools
{
    public static class SceneViewNavigationIO
    {
        public static string CurrentViewTypeKey => $"{ProjectUniqueKey}.CurrentViewType";

        private static string ProjectUniqueKey => $"{Application.productName}_{Application.identifier}_SceneViewTools";
        private static string ViewStateKeyPrefix => $"{ProjectUniqueKey}.ViewState";
        private static string LastViewStateKey => $"{ProjectUniqueKey}.LastViewState";

        private static string UseLastPoseCallsKey => $"{ProjectUniqueKey}.UseLastPoseCalls";

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

        public static void SaveLastViewState(float size, Quaternion rotation, Vector3 pivot, bool orthographic)
        {
            var viewState = new ViewState
            {
                size = size,
                rotation = rotation,
                pivot = pivot,
                orthographic = orthographic
            };
            string json = JsonUtility.ToJson(viewState);
            EditorPrefs.SetString(LastViewStateKey, json);
        }

        public static bool TryGetLastViewState(out ViewState viewState)
        {
            if (EditorPrefs.HasKey(LastViewStateKey))
            {
                string json = EditorPrefs.GetString(LastViewStateKey);
                viewState = JsonUtility.FromJson<ViewState>(json);
                return true;
            }
            viewState = default;
            return false;
        }

        public static SceneViewType ReadFromEditorPrefs()
        {
            return (SceneViewType)EditorPrefs.GetInt(CurrentViewTypeKey);
        }

        public static void WriteToEditorPrefs(SceneViewType viewType)
        {
            EditorPrefs.SetInt(CurrentViewTypeKey, (int)viewType);
        }

        public static void RequestUseLastPoseCalls(int calls)
        {
            SessionState.SetInt(UseLastPoseCallsKey, calls);
        }

        public static bool TryConsumeUseLastPoseRequest()
        {
            int remaining = SessionState.GetInt(UseLastPoseCallsKey, 0);
            if (remaining > 0)
            {
                remaining--;
                SessionState.SetInt(UseLastPoseCallsKey, remaining);
                return true;
            }
            return false;
        }
    }
}
#endif

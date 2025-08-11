#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace SceneViewTools
{
    public static class SceneViewTools
    {
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
    }
}
#endif

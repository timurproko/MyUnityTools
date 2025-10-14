#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SceneViewTools
{
    public static class SceneViewTools
    {
        private const string VR_RIG_PREF_KEY = "MyTools_VRRig_InstanceID";
        private static GameObject cachedVRRig;

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

        public static void ToggleVRRig()
        {
            GameObject vrRig = GetVRRig();

            if (vrRig)
            {
                vrRig.SetActive(!vrRig.activeSelf);
            }
            else
            {
                Debug.LogWarning("No OVRCameraRig found in the scene.");
            }
        }

        private static GameObject GetVRRig()
        {
            if (cachedVRRig && IsValidSceneObject(cachedVRRig))
            {
                return cachedVRRig;
            }

            int savedInstanceID = EditorPrefs.GetInt(VR_RIG_PREF_KEY, 0);
            if (savedInstanceID != 0)
            {
                cachedVRRig = EditorUtility.InstanceIDToObject(savedInstanceID) as GameObject;
                if (cachedVRRig && IsValidSceneObject(cachedVRRig))
                {
                    return cachedVRRig;
                }
            }

            cachedVRRig = FindVRRigInScene();

            if (cachedVRRig)
            {
                EditorPrefs.SetInt(VR_RIG_PREF_KEY, cachedVRRig.GetInstanceID());
            }

            return cachedVRRig;
        }

        private static GameObject FindVRRigInScene()
        {
            var allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>()
                .Where(obj => obj.scene.IsValid())
                .ToArray();

            foreach (GameObject obj in allGameObjects)
            {
                var component = obj.GetComponent("OVRCameraRig");
                if (component)
                {
                    return obj;
                }
            }

            return allGameObjects.FirstOrDefault(obj =>
                obj.name == "OVRCameraRig" ||
                obj.name == "VRRig" ||
                obj.name == "CameraRig");
        }

        private static bool IsValidSceneObject(GameObject obj)
        {
            return obj && obj.scene.IsValid();
        }
    }
}
#endif
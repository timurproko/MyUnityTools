#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SceneViewTools
{
    public static class SceneViewTools
    {
        private const string VR_RIG_PREF_KEY = "MyTools_VRRig_InstanceID";
        private static GameObject cachedVRRig;

        [InitializeOnEnterPlayMode]
        static void OnEnterPlayMode()
        {
            Debug.Log("OnEnterPlayMode - Enabling VR Rig FIRST");
            EnableVRRigImmediately();
        }

        [InitializeOnLoadMethod]
        static void Initialize()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                Debug.Log("ExitingEditMode - Pre-enabling VR Rig");
                EnableVRRigImmediately();
            }
            else if (state == PlayModeStateChange.EnteredPlayMode)
            {
                Debug.Log("EnteredPlayMode - Ensuring VR Rig is enabled");
                EditorApplication.delayCall += () =>
                {
                    EnsureVRRigEnabled();
                };
            }
        }

        private static void EnableVRRigImmediately()
        {
            cachedVRRig = null;
            EditorPrefs.DeleteKey(VR_RIG_PREF_KEY);
            
            GameObject vrRig = GetVRRig();
            
            if (vrRig)
            {
                if (!vrRig.activeSelf)
                {
                    vrRig.SetActive(true);
                    Debug.Log($"IMMEDIATELY ENABLED VR Rig: {vrRig.name}");
                }
                else
                {
                    Debug.Log($"VR Rig {vrRig.name} was already enabled");
                }
            }
            else
            {
                Debug.LogWarning("No VR Rig found for immediate enable");
            }
        }

        private static void EnsureVRRigEnabled()
        {
            GameObject vrRig = GetVRRig();
            
            if (vrRig && !vrRig.activeSelf)
            {
                vrRig.SetActive(true);
                Debug.Log($"ENSURE ENABLED VR Rig: {vrRig.name}");
            }
        }

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
                Debug.Log($"VR Rig {(vrRig.activeSelf ? "enabled" : "disabled")}: {vrRig.name}");
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

            if (!Application.isPlaying)
            {
                int savedInstanceID = EditorPrefs.GetInt(VR_RIG_PREF_KEY, 0);
                if (savedInstanceID != 0)
                {
                    cachedVRRig = EditorUtility.InstanceIDToObject(savedInstanceID) as GameObject;
                    
                    if (cachedVRRig && IsValidSceneObject(cachedVRRig) && IsVRRig(cachedVRRig))
                    {
                        return cachedVRRig;
                    }
                    
                    cachedVRRig = null;
                    EditorPrefs.DeleteKey(VR_RIG_PREF_KEY);
                }
            }

            cachedVRRig = FindVRRigInScene();

            if (cachedVRRig && !Application.isPlaying)
            {
                EditorPrefs.SetInt(VR_RIG_PREF_KEY, cachedVRRig.GetInstanceID());
            }

            return cachedVRRig;
        }

        private static bool IsVRRig(GameObject obj)
        {
            if (!obj) return false;
            
            if (obj.GetComponent("OVRCameraRig")) return true;
            
            return obj.name == "OVRCameraRig" || 
                   obj.name == "VRRig" || 
                   obj.name == "CameraRig";
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

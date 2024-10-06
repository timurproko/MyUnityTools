using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MyTools.Runtime
{
    public class AddInstances : MonoBehaviour
    {
        [InlineButton("FindInstances", "Locate")] [SerializeField]
        private GameObject instanceFile;

        [SerializeField] private string instanceFileSuffix = "Instances";
        [SerializeField] private bool instantiateAtRuntime = true;
        [SerializeField] private bool logMessagesToConsole = true;

        private GameObject geoAsset;
        private static bool preferFbx = true; // Track preference for FBX or prefab

        private void OnValidate()
        {
            LoadGeoAsset();
        }

        private void Awake()
        {
            if (instantiateAtRuntime)
            {
                RemoveExistingChildren();
                AttachMatchingObjectsToPoints();
            }

            CheckInstanceFile();
        }

        [ContextMenu("Find Instances")]
        [InlineButton("FindInstances")]
        private void FindInstances()
        {
            string foundAssetPath = FindInstanceAssetPath();
            if (!string.IsNullOrEmpty(foundAssetPath))
            {
                instanceFile = AssetDatabase.LoadAssetAtPath<GameObject>(foundAssetPath);
                if (instanceFile != null)
                {
                    Log($"Instance file found at {foundAssetPath}");
                }
                else
                {
                    LogWarning("Failed to load GameObject from the found asset path.");
                }
            }
        }

        private string FindInstanceAssetPath()
        {
            GameObject selectedObject = Selection.activeGameObject;
            if (selectedObject != null)
            {
                string path = FindPrefabPath(selectedObject);
                if (!string.IsNullOrEmpty(path))
                {
                    string assetName = System.IO.Path.GetFileName(path);
                    string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(assetName);
                    string[] nameParts = fileNameWithoutExtension.Split('_');

                    if (nameParts.Length > 1)
                    {
                        string dynamicSuffix = nameParts[^1];
                        string modifiedAssetName = fileNameWithoutExtension.Replace(dynamicSuffix, instanceFileSuffix);

                        // Find both FBX and prefab paths
                        string fbxPath = FindAssetPathRecursively(modifiedAssetName + ".fbx");
                        string prefabPath = FindAssetPathRecursively(modifiedAssetName + ".prefab");

                        // Always try to load FBX first if instanceFile is null
                        if (instanceFile == null) 
                        {
                            if (!string.IsNullOrEmpty(fbxPath))
                            {
                                instanceFile = AssetDatabase.LoadAssetAtPath<GameObject>(fbxPath);
                                Log($"FBX instance file found at {fbxPath}");
                                preferFbx = false; // Next toggle will look for prefab
                            }
                            else if (!string.IsNullOrEmpty(prefabPath))
                            {
                                instanceFile = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                                Log($"Prefab instance file found at {prefabPath}");
                            }
                        }
                        else // If instanceFile is not null, continue toggling
                        {
                            if (preferFbx)
                            {
                                if (!string.IsNullOrEmpty(fbxPath))
                                {
                                    instanceFile = AssetDatabase.LoadAssetAtPath<GameObject>(fbxPath);
                                    Log($"FBX instance file found at {fbxPath}");
                                }
                                else
                                {
                                    // Fallback to prefab if FBX not found
                                    if (!string.IsNullOrEmpty(prefabPath))
                                    {
                                        instanceFile = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                                        Log($"Prefab instance file found at {prefabPath}");
                                        preferFbx = true; // Next toggle will look for FBX again
                                    }
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(prefabPath))
                                {
                                    instanceFile = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                                    Log($"Prefab instance file found at {prefabPath}");
                                }
                                else
                                {
                                    // Fallback to FBX if prefab not found
                                    if (!string.IsNullOrEmpty(fbxPath))
                                    {
                                        instanceFile = AssetDatabase.LoadAssetAtPath<GameObject>(fbxPath);
                                        Log($"FBX instance file found at {fbxPath}");
                                    }
                                }
                            }

                            // Reset the preference regardless of what was found
                            preferFbx = !preferFbx;
                        }
                    }
                    else
                    {
                        LogWarning("The asset name does not follow the expected format with a suffix.");
                    }
                }
                else
                {
                    LogWarning("The selected GameObject is not part of a prefab or is not a prefab asset.");
                }
            }

            return null;
        }

        private static string FindPrefabPath(GameObject go)
        {
            if (PrefabUtility.IsPartOfPrefabInstance(go))
            {
                return PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go);
            }

            string assetPath = AssetDatabase.GetAssetPath(go);
            return !string.IsNullOrEmpty(assetPath) && assetPath.EndsWith(".prefab") ? assetPath : null;
        }

        private static string FindAssetPathRecursively(string assetName)
        {
            string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
            List<string> matchingAssets = new List<string>();

            foreach (var path in allAssetPaths)
            {
                if (path.EndsWith(".fbx") || path.EndsWith(".prefab"))
                {
                    if (System.IO.Path.GetFileName(path) == assetName)
                    {
                        matchingAssets.Add(path);
                    }
                }
            }

            return matchingAssets.Count > 0 ? matchingAssets[0] : null;
        }

        private void CheckInstanceFile()
        {
            LogIfNoInstanceAssigned();
        }

        private void LoadGeoAsset()
        {
            if (instanceFile == null)
            {
                LogIfNoInstanceAssigned();
                return;
            }

            geoAsset = instanceFile;

            if (geoAsset == null)
            {
                LogWarning($"Failed to load instance GameObject.");
            }
        }

        private void AttachMatchingObjectsToPoints()
        {
            if (geoAsset == null)
            {
                LogWarning("geoAsset is null, trying to load...");
                LoadGeoAsset();
                if (geoAsset == null)
                {
                    LogError("geoAsset is still null after attempting to load. Stopping instance attachment.");
                    return;
                }
            }

            int totalAttached = 0;

            foreach (Transform point in transform)
            {
                if (point.name.StartsWith("point"))
                {
                    string remainingName = point.gameObject.name.Substring(5);
                    string[] splitName = remainingName.Split('_');

                    if (splitName.Length > 1)
                    {
                        string suffix = splitName[1];
                        Log($"Looking for child with suffix '{suffix}' in '{geoAsset.name}'");

                        Transform matchingChild = geoAsset.transform.Find(suffix);
                        if (matchingChild != null)
                        {
                            GameObject instance = Instantiate(matchingChild.gameObject);
                            instance.transform.SetParent(point);
                            instance.transform.localPosition = Vector3.zero;
                            instance.transform.localRotation = Quaternion.identity;
                            instance.transform.localScale = Vector3.one;
                            totalAttached++;
                            Log($"Created instance of {matchingChild.gameObject.name} at point {point.name}");
                        }
                        else
                        {
                            LogWarning($"No matching child with name '{suffix}' found in the '{geoAsset.name}' file.");
                        }
                    }
                }
            }

            Log($"Attached {totalAttached} instances at Runtime.");
        }

        private void RemoveExistingChildren()
        {
            foreach (Transform point in transform)
            {
                if (point.name.StartsWith("point"))
                {
                    List<Transform> childrenToRemove = new List<Transform>();

                    foreach (Transform child in point)
                    {
                        childrenToRemove.Add(child);
                    }

                    foreach (Transform child in childrenToRemove)
                    {
                        DestroyImmediate(child.gameObject);
                    }
                }
            }
        }

        [ButtonGroup("Instances")]
        [Button("Add Instances")]
        [ContextMenu("Add Instances")]
        public void AddInstancesAsChildren()
        {
            if (instanceFile == null)
            {
                LogIfNoInstanceAssigned();
                return;
            }

            LoadGeoAsset();

            if (geoAsset == null)
            {
                LogWarning("No valid instance GameObject loaded. Cannot add instances.");
                return;
            }

            int totalAdded = 0;

            foreach (Transform point in transform)
            {
                if (point.name.StartsWith("point"))
                {
                    if (point.childCount > 0)
                    {
                        continue;
                    }

                    string remainingName = point.gameObject.name.Substring(5);
                    string[] splitName = remainingName.Split('_');

                    if (splitName.Length > 1)
                    {
                        string suffix = splitName[1];

                        Transform matchingChild = geoAsset.transform.Find(suffix);
                        if (matchingChild != null)
                        {
                            GameObject instance = Instantiate(matchingChild.gameObject);
                            instance.transform.SetParent(point);
                            instance.transform.localPosition = Vector3.zero;
                            instance.transform.localRotation = Quaternion.identity;
                            instance.transform.localScale = Vector3.one;
                            totalAdded++;
                        }
                        else
                        {
                            LogWarning($"No matching object with suffix '{suffix}' found in '{geoAsset.name}'.");
                        }
                    }
                }
            }

            Log($"Added {totalAdded} instances.");
        }

        [ButtonGroup("Instances")]
        [Button("Remove Instances")]
        [ContextMenu("Remove Instances")]
        public void RemoveInstancesAsChildren()
        {
            int totalRemoved = 0;

            foreach (Transform rootChild in transform)
            {
                if (rootChild.name.StartsWith("point"))
                {
                    List<Transform> childrenToRemove = new List<Transform>();

                    foreach (Transform child in rootChild)
                    {
                        childrenToRemove.Add(child);
                    }

                    foreach (Transform child in childrenToRemove)
                    {
                        DestroyImmediate(child.gameObject);
                        totalRemoved++;
                    }
                }
            }

            List<Transform> rootChildrenToRemove = new List<Transform>();

            foreach (Transform rootChild in transform)
            {
                if (!rootChild.name.StartsWith("point"))
                {
                    rootChildrenToRemove.Add(rootChild);
                }
            }

            foreach (Transform rootChild in rootChildrenToRemove)
            {
                DestroyImmediate(rootChild.gameObject);
                totalRemoved++;
            }

            if (totalRemoved > 0)
            {
                Log($"Removed {totalRemoved} instances from points.");
            }
        }

        private void LogIfNoInstanceAssigned()
        {
            if (geoAsset == null && instanceFile == null)
            {
                LogWarning("No instance GameObject is assigned.");
            }
            else if (instanceFile == null)
            {
                LogWarning("No instance GameObject is assigned.");
            }
        }

        private void Log(string message)
        {
            if (logMessagesToConsole)
            {
                Debug.Log($"MyTools: {message}");
            }
        }

        private void LogWarning(string message)
        {
            if (logMessagesToConsole)
            {
                Debug.LogWarning($"MyTools: {message}");
            }
        }

        private void LogError(string message)
        {
            if (logMessagesToConsole)
            {
                Debug.LogError($"MyTools: {message}");
            }
        }
    }
}

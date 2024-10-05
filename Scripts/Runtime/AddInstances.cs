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
        [InlineButton("GetPath")] [SerializeField]
        private string instanceFilePath;

        [SerializeField] private string instanceFileSuffix = "Instances";
        [SerializeField] private bool instantiateAtRuntime = true;
        [SerializeField] private bool logMessagesToConsole = true;

        private GameObject geoAsset;
        private bool isStarted;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(instanceFilePath))
            {
                GetPath();
            }

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

        [ContextMenu("Get Instance Path")]
        [InlineButton("GetPath", "Get Path")]
        private void GetPath()
        {
            instanceFilePath = GetInstancePath();
            Log($"Instance file path set to: {instanceFilePath}");
        }

        private static string FindPrefabPath(GameObject go)
        {
            if (PrefabUtility.IsPartOfPrefabInstance(go))
            {
                string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go);
                return path;
            }

            string assetPath = AssetDatabase.GetAssetPath(go);
            if (!string.IsNullOrEmpty(assetPath) && assetPath.EndsWith(".prefab"))
            {
                return assetPath;
            }

            return null;
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

        private string GetInstancePath()
        {
            GameObject selectedObject = Selection.activeGameObject;
            if (selectedObject != null)
            {
                string path = FindPrefabPath(selectedObject);
                if (!string.IsNullOrEmpty(path))
                {
                    string assetName = System.IO.Path.GetFileName(path);
                    string modifiedAssetName = assetName.Replace("Points", instanceFileSuffix);
                    string foundAssetPath = FindAssetPathRecursively(modifiedAssetName);
                    if (!string.IsNullOrEmpty(foundAssetPath))
                    {
                        string truncatedPath = System.IO.Path.GetDirectoryName(foundAssetPath);
                        string finalPath = truncatedPath.Replace("\\", "/");
                        return finalPath;
                    }

                    LogWarning($"Asset '{modifiedAssetName}' not found in the Assets folder.");
                }
                else
                {
                    LogWarning("The selected GameObject is not part of a prefab or is not a prefab asset.");
                }
            }

            return null;
        }

        private void CheckInstanceFile()
        {
            if (geoAsset == null)
            {
                LogWarning($"No instance file found at path '{instanceFilePath}'.");
            }
        }

        private void LoadGeoAsset()
        {
            string objectName = gameObject.name;
            if (!objectName.EndsWith("_Points"))
            {
                LogWarning("The GameObject must be named following the pattern {name}_Points.");
                return;
            }

            string baseName = objectName.Substring(0, objectName.Length - "_Points".Length);
            string geoAssetPath = $"{instanceFilePath}/{baseName}_{instanceFileSuffix}.fbx";

            Log($"Loading geoAsset from path: {geoAssetPath}");
            geoAsset = AssetDatabase.LoadAssetAtPath<GameObject>(geoAssetPath);

            if (geoAsset == null)
            {
                LogError($"Failed to load geoAsset at path: {geoAssetPath}. Please verify the path and asset.");
            }
            else
            {
                Log($"Successfully loaded geoAsset: {geoAsset.name}");
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
            if (geoAsset == null)
            {
                LoadGeoAsset();
                if (geoAsset == null)
                {
                    CheckInstanceFile();
                    return;
                }
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
                            LogWarning($"No matching object with suffix '{suffix}' found in '{geoAsset.name}'");
                        }
                    }
                }
            }

            Log($"Added {totalAdded} instances to points as children.");
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

            Log($"Removed {totalRemoved} instances from points.");
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
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
        [SerializeField] private string instanceFilePath = "Assets/Geometry/";
        [SerializeField] private string instanceSuffix = "Instances";
        [SerializeField] private bool instantiateOnRuntime = true;
        [SerializeField] private bool logMessagesToConsole;

        private GameObject geoAsset;

        private void Awake()
        {
            // Only remove existing children if instantiateOnRuntime is enabled
            if (instantiateOnRuntime)
            {
                RemoveExistingChildren();
                AttachMatchingObjectsToPoints();
            }
        }

        private void LoadGeoAsset()
        {
            // Extract the {name} part from the GameObject this script is attached to
            string objectName = gameObject.name;
            if (!objectName.EndsWith("_Points"))
            {
                LogError("The GameObject must be named following the pattern {name}_Points.");
                return;
            }

            // Extract the {name} from {name}_Points
            string baseName = objectName.Substring(0, objectName.Length - "_Points".Length);

            // Build the full path to the instance file (either .fbx or .prefab)
            string geoAssetPath = instanceFilePath + baseName + "_" + instanceSuffix + ".fbx";
            Log($"Looking for asset at path: {geoAssetPath}");

            // Try to load the asset at the specified path
            geoAsset = AssetDatabase.LoadAssetAtPath<GameObject>(geoAssetPath);

            if (geoAsset == null)
            {
                LogError($"No file found at path '{geoAssetPath}' or with the name '{baseName}_{instanceSuffix}'.");
            }
        }

        private void AttachMatchingObjectsToPoints()
        {
            // Ensure geoAsset is loaded
            if (geoAsset == null)
            {
                LoadGeoAsset();
                if (geoAsset == null) return; // Exit if geoAsset is still null
            }

            int totalAttached = 0; // Count of total attached instances
            // Traverse all child objects of the GameObject this script is attached to
            foreach (Transform point in transform)
            {
                if (point.name.StartsWith("point"))
                {
                    string remainingName = point.gameObject.name.Substring(5); // Remove "point"
                    string[] splitName = remainingName.Split('_');

                    if (splitName.Length > 1)
                    {
                        string suffix = splitName[1]; // Extract the suffix (e.g., box, sphere, etc.)
                        Log($"Searching for matching child in '{geoAsset.name}' with suffix '{suffix}'");

                        Transform matchingChild = geoAsset.transform.Find(suffix);
                        if (matchingChild != null)
                        {
                            // Instantiate the matching child object
                            GameObject instance = Instantiate(matchingChild.gameObject);

                            // Attach the instance as a child of the point object
                            instance.transform.SetParent(point);
                            instance.transform.localPosition = Vector3.zero; // Optionally, reset position
                            instance.transform.localRotation = Quaternion.identity; // Optionally, reset rotation
                            instance.transform.localScale = Vector3.one; // Reset the scale to 1 (default size)
                            totalAttached++; // Increment count of attached instances
                        }
                        else
                        {
                            LogWarning($"No matching child with name '{suffix}' found in the '{geoAsset.name}' file.");
                        }
                    }
                }
            }

            // Log the total number of attached instances
            Log($"Successfully attached {totalAttached} instances.");
        }

        // Removes existing children from all points
        private void RemoveExistingChildren()
        {
            foreach (Transform point in transform)
            {
                if (point.name.StartsWith("point"))
                {
                    // Gather children to a list to avoid modifying the collection during iteration
                    List<Transform> childrenToRemove = new List<Transform>();

                    foreach (Transform child in point)
                    {
                        childrenToRemove.Add(child);
                    }

                    // Now remove each child
                    foreach (Transform child in childrenToRemove)
                    {
                        Log($"Removing existing instance '{child.name}' from point '{point.name}'");
                        DestroyImmediate(child.gameObject); // Immediately remove the child object
                    }
                }
            }
        }

        // Adds existing instances from the geometry file as children of the points
        [Button("Add Instances")]
        [ContextMenu("Add Instances")]
        public void AddInstancesAsChildren()
        {
            // Ensure geoAsset is loaded
            if (geoAsset == null)
            {
                LoadGeoAsset();
                if (geoAsset == null) return; // Exit if geoAsset is still null
            }

            int totalAdded = 0; // Count of total added instances

            foreach (Transform point in transform)
            {
                if (point.name.StartsWith("point"))
                {
                    // Check if there are already children under this point
                    if (point.childCount > 0)
                    {
                        Log($"Skipping '{point.name}' as it already has children.");
                        continue; // Skip to the next point if there are already children
                    }

                    string remainingName = point.gameObject.name.Substring(5); // Remove "point"
                    string[] splitName = remainingName.Split('_');

                    if (splitName.Length > 1)
                    {
                        string suffix = splitName[1]; // Extract the suffix (e.g., box, sphere, etc.)
                        Log($"Looking for existing object in geoAsset with suffix '{suffix}'");

                        // Find the object inside the geoAsset using its suffix
                        Transform matchingChild = geoAsset.transform.Find(suffix);
                        if (matchingChild != null)
                        {
                            // Clone and attach the matching child as a child of the point object
                            GameObject instance = Instantiate(matchingChild.gameObject);
                            instance.transform.SetParent(point);
                            instance.transform.localPosition = Vector3.zero; // Reset position
                            instance.transform.localRotation = Quaternion.identity; // Reset rotation
                            instance.transform.localScale = Vector3.one; // Reset scale
                            totalAdded++; // Increment count of added instances
                        }
                        else
                        {
                            LogWarning($"No matching object with suffix '{suffix}' found in '{geoAsset.name}'");
                        }
                    }
                }
            }

            // Log the total number of added instances
            Log($"Successfully added {totalAdded} existing instances as children.");
        }

        // Removes all instances that were previously added as children of the points
        [Button("Remove Instances")]
        [ContextMenu("Remove Instances")]
        public void RemoveInstancesAsChildren()
        {
            Log("Removing instances from points");

            int totalRemoved = 0; // Count of total removed instances

            foreach (Transform point in transform)
            {
                if (point.name.StartsWith("point"))
                {
                    // Gather children to a list to avoid modifying the collection during iteration
                    List<Transform> childrenToRemove = new List<Transform>();

                    foreach (Transform child in point)
                    {
                        childrenToRemove.Add(child);
                    }

                    // Now remove each child
                    foreach (Transform child in childrenToRemove)
                    {
                        Log($"Removing instance '{child.name}' from point '{point.name}'");
                        DestroyImmediate(child.gameObject); // Immediately remove the child object
                        totalRemoved++; // Increment count of removed instances
                    }
                }
            }

            // Log the total number of removed instances
            Log($"Successfully removed {totalRemoved} instances from points.");
        }

        // Helper method for logging
        private void Log(string message)
        {
            if (logMessagesToConsole)
            {
                Debug.Log($"MyTools: {message}");
            }
        }

        // Helper method for logging warnings
        private void LogWarning(string message)
        {
            if (logMessagesToConsole)
            {
                Debug.LogWarning($"MyTools: {message}");
            }
        }

        // Helper method for logging errors
        private void LogError(string message)
        {
            if (logMessagesToConsole)
            {
                Debug.LogError($"MyTools: {message}");
            }
        }
    }
}

using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MyTools.Runtime
{
    public class AddInstances : MonoBehaviour
    {
        [TitleGroup("Instance Asset")] 
        [InlineButton("LocateInstanceFile", "Locate")] 
        [SerializeField] private GameObject file;
        [SerializeField] private bool customizeSuffix;
        [ShowIf("customizeSuffix")]
        [SerializeField] private string suffix = "Instances";
        [TitleGroup("Instantiation")]
        [SerializeField] [PropertyOrder(1)] private bool instantiateAtRuntime = true;
        [TitleGroup("Debugging")]
        [SerializeField] [PropertyOrder(2)] private bool logMessagesToConsole;

        private GameObject geoAsset;
        private static bool preferFbx = true;

        private void OnValidate()
        {
            LoadAsset();
        }

        private void Awake()
        {
            if (instantiateAtRuntime)
            {
                RemoveExistingChildren();
                AttachMatchingObjectsToPoints();
            }

            LogIfNoInstanceAssigned();
        }

        [ContextMenu("Locate Instance File")]
        [InlineButton("LocateInstanceFile")]
        private void LocateInstanceFile()
        {
            string foundAssetPath = FindInstanceAssetPath();
            if (!string.IsNullOrEmpty(foundAssetPath))
            {
                file = AssetDatabase.LoadAssetAtPath<GameObject>(foundAssetPath);
                if (file != null)
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
                    string assetName = Path.GetFileName(path);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(assetName);
                    string[] nameParts = fileNameWithoutExtension.Split('_');

                    if (nameParts.Length > 1)
                    {
                        string dynamicSuffix = nameParts[^1];
                        string modifiedAssetName = fileNameWithoutExtension.Replace(dynamicSuffix, suffix);

                        // Find both FBX and prefab paths
                        string fbxPath = FindAssetPathRecursively(modifiedAssetName + ".fbx");
                        string prefabPath = FindAssetPathRecursively(modifiedAssetName + ".prefab");

                        // Determine what to do based on current state of 'file'
                        if (file == null) // File is not yet located
                        {
                            if (!string.IsNullOrEmpty(fbxPath)) // Locate FBX first
                            {
                                file = AssetDatabase.LoadAssetAtPath<GameObject>(fbxPath);
                                Log($"FBX instance file found at {fbxPath}");
                            }
                        }
                        else // File is already located
                        {
                            // Check the type of the located file
                            bool isPrefab = prefabPath != null && prefabPath.Equals(AssetDatabase.GetAssetPath(file));

                            if (isPrefab) // If current file is a prefab, locate FBX
                            {
                                if (!string.IsNullOrEmpty(fbxPath))
                                {
                                    file = AssetDatabase.LoadAssetAtPath<GameObject>(fbxPath);
                                    Log($"FBX instance file found at {fbxPath}");
                                }
                            }
                            else // Current file is FBX, locate prefab
                            {
                                if (!string.IsNullOrEmpty(prefabPath))
                                {
                                    file = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                                    Log($"Prefab instance file found at {prefabPath}");
                                }
                            }
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
                    if (Path.GetFileName(path) == assetName)
                    {
                        matchingAssets.Add(path);
                    }
                }
            }

            return matchingAssets.Count > 0 ? matchingAssets[0] : null;
        }

        private void LoadAsset()
        {
            if (file == null)
            {
                LogIfNoInstanceAssigned();
                return;
            }

            geoAsset = file;

            if (geoAsset == null)
            {
                LogWarning($"Failed to load instance GameObject.");
            }
        }

        [ContextMenu("Add Instances")]
        [TitleGroup("Instantiation")]
        [HorizontalGroup("Instantiation/Instance Actions")]
        [Button("Add Instances")]
        public void AddPointInstances()
        {
            if (file == null)
            {
                LogIfNoInstanceAssigned();
                return;
            }

            LoadAsset();

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

        [ContextMenu("Remove Instances")]
        [TitleGroup("Instantiation")]
        [HorizontalGroup("Instantiation/Instance Actions")]
        [Button("Remove Instances")]
        public void RemovePointInstances()
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

        [ContextMenu("Create Prefab")]
        [TitleGroup("Instance Asset")]
        [HorizontalGroup("Instance Asset/Prefab Actions")]
        [Button("Create Prefab")]
        private void CreatePrefab()
        {
            GameObject selectedObject = Selection.activeGameObject;
            if (selectedObject != null)
            {
                string path = FindPrefabPath(selectedObject);
                if (!string.IsNullOrEmpty(path))
                {
                    string assetName = Path.GetFileName(path);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(assetName);
                    string[] nameParts = fileNameWithoutExtension.Split('_');

                    if (nameParts.Length > 1)
                    {
                        string dynamicSuffix = nameParts[^1];
                        string modifiedAssetName = fileNameWithoutExtension.Replace(dynamicSuffix, suffix);

                        // Step 1: Find the FBX and create the prefab
                        string fbxPath = FindAssetPathRecursively(modifiedAssetName + ".fbx");
                        GameObject prefabFile = null;

                        if (!string.IsNullOrEmpty(fbxPath))
                        {
                            GameObject fbxFile = AssetDatabase.LoadAssetAtPath<GameObject>(fbxPath);
                            if (fbxFile != null)
                            {
                                prefabFile = CreatePrefabFromFBX(fbxFile); // Returns the created prefab
                            }
                        }

                        // Step 2: Apply LODs to the newly created prefab if it was created successfully
                        if (prefabFile != null)
                        {
                            CopyLODGroupToFirstLevelChildren(prefabFile);
                        }
                        else
                        {
                            Debug.LogError("Failed to create prefab from FBX.");
                        }
                    }
                }
            }
        }

        [ContextMenu("Remove Prefab")]
        [TitleGroup("Instance Asset")]
        [HorizontalGroup("Instance Asset/Prefab Actions")]
        [Button("Remove Prefab")]
        private void RemovePrefab()
        {
            GameObject selectedObject = Selection.activeGameObject;
            if (selectedObject != null)
            {
                string path = FindPrefabPath(selectedObject);
                if (!string.IsNullOrEmpty(path))
                {
                    string assetName = Path.GetFileName(path);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(assetName);
                    string[] nameParts = fileNameWithoutExtension.Split('_');

                    if (nameParts.Length > 1)
                    {
                        string dynamicSuffix = nameParts[^1];
                        string modifiedAssetName = fileNameWithoutExtension.Replace(dynamicSuffix, suffix);

                        // Step 1: Remove the prefab file
                        string prefabPath = FindAssetPathRecursively(modifiedAssetName + ".prefab");
                        if (!string.IsNullOrEmpty(prefabPath))
                        {
                            bool deletionConfirmed = AssetDatabase.DeleteAsset(prefabPath);
                            if (deletionConfirmed)
                            {
                                Debug.Log($"Prefab successfully deleted: {prefabPath}");
                            }
                            else
                            {
                                Debug.LogError("Failed to delete prefab.");
                                return;
                            }
                        }
                        else
                        {
                            Debug.LogWarning("No prefab found to remove.");
                            return;
                        }

                        // Step 2: Locate the corresponding FBX file
                        string fbxPath = FindAssetPathRecursively(modifiedAssetName + ".fbx");
                        if (!string.IsNullOrEmpty(fbxPath))
                        {
                            GameObject fbxFile = AssetDatabase.LoadAssetAtPath<GameObject>(fbxPath);
                            if (fbxFile != null)
                            {
                                Debug.Log($"FBX located at: {fbxPath}");
                                // You can perform additional operations on the FBX if needed (e.g., set it as the new instance file)
                            }
                            else
                            {
                                Debug.LogError("Failed to load FBX.");
                            }
                        }
                        else
                        {
                            Debug.LogError("Corresponding FBX file not found.");
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("No selected object to remove prefab from.");
            }
        }

        private void AttachMatchingObjectsToPoints()
        {
            if (geoAsset == null)
            {
                LogWarning("geoAsset is null, trying to load...");
                LoadAsset();
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

        private void LogIfNoInstanceAssigned()
        {
            if (geoAsset == null && file == null)
            {
                LogWarning("No instance GameObject is assigned.");
            }
            else if (file == null)
            {
                LogWarning("No instance GameObject is assigned.");
            }
        }

        private GameObject CreatePrefabFromFBX(GameObject fbxFile)
        {
            string fbxPath = AssetDatabase.GetAssetPath(fbxFile);
            if (string.IsNullOrEmpty(fbxPath))
            {
                Debug.LogError("FBX file or path is invalid.");
                return null;
            }

            // Create prefab at the same location as FBX with .prefab extension
            string prefabPath = Path.ChangeExtension(fbxPath, ".prefab");
            GameObject createdPrefab = PrefabUtility.SaveAsPrefabAsset(fbxFile, prefabPath);

            Debug.Log("Prefab created at: " + prefabPath);
            return createdPrefab; // Return the created prefab
        }

        static void CopyLODGroupToFirstLevelChildren(GameObject prefab)
        {
            // Get the LODGroup component from the prefab (root object)
            LODGroup rootLODGroup = prefab.GetComponent<LODGroup>();

            // If there's no LODGroup on the prefab, display a warning
            if (rootLODGroup == null)
            {
                Debug.LogError("The prefab does not have an LODGroup component.");
                return;
            }

            // Iterate only through the first-level children
            foreach (Transform child in prefab.transform)
            {
                if (child == null) continue;

                // Check if the child already has an LODGroup
                LODGroup childLODGroup = child.GetComponent<LODGroup>();

                // If the child has an existing LODGroup, remove it
                if (childLODGroup != null)
                {
                    Undo.DestroyObjectImmediate(childLODGroup);
                }

                // Add a new LODGroup component to the child
                LODGroup newLODGroup = child.gameObject.AddComponent<LODGroup>();

                // Copy LOD settings from the root to the new LODGroup on the child
                CopyLODGroupSettings(rootLODGroup, newLODGroup);

                // Remove all renderers from the new LODGroup
                RemoveAllRenderersFromLODGroup(newLODGroup);

                // Assign new renderers based on child names
                AssignRenderersToLODGroup(newLODGroup, child);

                // Set Transition % Screen Size for the child LODGroup
                SetTransitionScreenSize(newLODGroup);
            }

            // Remove the LODGroup component from the parent object
            Undo.DestroyObjectImmediate(rootLODGroup);

            Debug.Log("LOD Group successfully copied to first-level children, and LOD Group removed from parent.");
        }

        static void CopyLODGroupSettings(LODGroup source, LODGroup destination)
        {
            if (source == null || destination == null)
            {
                Debug.LogError("Source or destination LODGroup is null.");
                return;
            }

            LOD[] lods = source.GetLODs();
            destination.SetLODs(lods);
            destination.fadeMode = source.fadeMode;
            destination.animateCrossFading = source.animateCrossFading;
        }

        static void RemoveAllRenderersFromLODGroup(LODGroup lodGroup)
        {
            // Clear all renderers for each LOD in the LODGroup
            LOD[] lods = lodGroup.GetLODs();
            for (int i = 0; i < lods.Length; i++)
            {
                lods[i].renderers = new Renderer[0]; // Clear previous renderers
            }

            lodGroup.SetLODs(lods); // Update the LODGroup with cleared renderers
        }

        static void AssignRenderersToLODGroup(LODGroup lodGroup, Transform child)
        {
            // Create lists to hold the renderers
            List<Renderer> renderersLOD0 = new List<Renderer>();
            List<Renderer> renderersLOD1 = new List<Renderer>();
            List<Renderer> renderersLOD2 = new List<Renderer>();

            // Find all the immediate children of the child and categorize them based on naming convention
            foreach (Transform lodChild in child)
            {
                string lodName = lodChild.gameObject.name;

                // Check naming convention to assign to the correct LOD group
                if (lodName.EndsWith("_LOD0"))
                {
                    renderersLOD0.Add(lodChild.GetComponent<Renderer>());
                }
                else if (lodName.EndsWith("_LOD1"))
                {
                    renderersLOD1.Add(lodChild.GetComponent<Renderer>());
                }
                else if (lodName.EndsWith("_LOD2"))
                {
                    renderersLOD2.Add(lodChild.GetComponent<Renderer>());
                }
            }

            // Create new LODs with renderers
            List<LOD> lods = new List<LOD>();

            // Add LOD0 if there are any renderers
            if (renderersLOD0.Count > 0)
            {
                lods.Add(new LOD(1f, renderersLOD0.ToArray())); // Full priority
            }

            // Add LOD1 if there are any renderers
            if (renderersLOD1.Count > 0)
            {
                lods.Add(new LOD(0.5f, renderersLOD1.ToArray())); // Half priority
            }

            // Add LOD2 if there are any renderers
            if (renderersLOD2.Count > 0)
            {
                lods.Add(new LOD(0.25f, renderersLOD2.ToArray())); // Quarter priority
            }

            // Assign LODs to the LODGroup
            lodGroup.SetLODs(lods.ToArray()); // Set the newly created LODs to the LODGroup
        }

        static void SetTransitionScreenSize(LODGroup lodGroup)
        {
            // Get the LODs for the new LODGroup
            LOD[] lods = lodGroup.GetLODs();

            // Set Transition % Screen Size based on the number of LOD levels
            for (int i = 0; i < lods.Length; i++)
            {
                if (lods.Length == 2) // If there are 2 LODs
                {
                    switch (i)
                    {
                        case 0:
                            lods[i].screenRelativeTransitionHeight = 0.25f; // LOD0 transition size
                            break;
                        case 1:
                            lods[i].screenRelativeTransitionHeight = 0.01f; // LOD1 transition size
                            break;
                    }
                }
                else if (lods.Length == 3) // If there are 3 LODs
                {
                    switch (i)
                    {
                        case 0:
                            lods[i].screenRelativeTransitionHeight = 0.25f; // LOD0 transition size
                            break;
                        case 1:
                            lods[i].screenRelativeTransitionHeight = 0.13f; // LOD1 transition size
                            break;
                        case 2:
                            lods[i].screenRelativeTransitionHeight = 0.01f; // LOD2 transition size
                            break;
                    }
                }
                // Add more conditions for additional LODs if necessary
            }

            // Update the LODGroup with the modified LODs
            lodGroup.SetLODs(lods);
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
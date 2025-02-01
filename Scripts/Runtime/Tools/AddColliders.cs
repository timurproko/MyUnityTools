using System;
using System.Reflection;
using UnityEngine;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;

namespace MyTools.Runtime
{
    public class AddColliders : MonoBehaviour
    {
        private void OnValidate()
        {
            Assign();
        }
        [Button("Force Update Colliders")]
        [ContextMenu("Force Update Colliders")]
        public void UpdateColliders()
        {
            Remove();
            Assign();
        }

        public void Remove()
        {
            // Recursively remove colliders from all children
            RemoveCollidersRecursively(transform);
        }

        public void Assign()
        {
            // Recursively assign colliders to all children
            AssignCollidersRecursively(transform);
        }

        private void RemoveCollidersRecursively(Transform parent)
        {
            foreach (Transform child in parent)
            {
                string childName = child.gameObject.name;

                if (Regex.IsMatch(childName, @"MeshCollider.*$"))
                {
                    Delete<MeshCollider>(child);
                }
                else if (Regex.IsMatch(childName, @"BoxCollider.*$"))
                {
                    Delete<BoxCollider>(child);
                }
                else if (Regex.IsMatch(childName, @"SphereCollider.*$"))
                {
                    Delete<SphereCollider>(child);
                }
                else if (Regex.IsMatch(childName, @"CapsuleCollider.*$"))
                {
                    Delete<CapsuleCollider>(child);
                }

                // Recursively process the child
                RemoveCollidersRecursively(child);
            }
        }

private void AssignCollidersRecursively(Transform parent)
{
    foreach (Transform child in parent)
    {
        string childName = child.gameObject.name;

        if (Regex.IsMatch(childName, @"MeshCollider.*$"))
        {
            Add<MeshCollider>(child);
        }
        else if (Regex.IsMatch(childName, @"BoxCollider.*$"))
        {
            Add<BoxCollider>(child);
            BoxCollider collider = child.GetComponent<BoxCollider>();
        }
        else if (Regex.IsMatch(childName, @"SphereCollider.*$"))
        {
            Add<SphereCollider>(child);
            SphereCollider collider = child.GetComponent<SphereCollider>();
        }
        else if (Regex.IsMatch(childName, @"CapsuleColliderX.*$"))
        {
            Add<CapsuleCollider>(child);
            CapsuleCollider collider = child.GetComponent<CapsuleCollider>();
            collider.direction = 0;

            float radius = ExtractFloatFromPattern(childName, @"R([0-9]*\.?[0-9]+)");
            float height = ExtractFloatFromPattern(childName, @"H([0-9]*\.?[0-9]+)");

            collider.radius = radius;
            collider.height = height;
        }
        else if (Regex.IsMatch(childName, @"CapsuleColliderY.*$"))
        {
            Add<CapsuleCollider>(child);
            CapsuleCollider collider = child.GetComponent<CapsuleCollider>();
            collider.direction = 1;

            float radius = ExtractFloatFromPattern(childName, @"R([0-9]*\.?[0-9]+)");
            float height = ExtractFloatFromPattern(childName, @"H([0-9]*\.?[0-9]+)");

            collider.radius = radius;
            collider.height = height;
        }
        else if (Regex.IsMatch(childName, @"CapsuleColliderZ.*$"))
        {
            Add<CapsuleCollider>(child);
            CapsuleCollider collider = child.GetComponent<CapsuleCollider>();
            collider.direction = 2;

            float radius = ExtractFloatFromPattern(childName, @"R([0-9]*\.?[0-9]+)");
            float height = ExtractFloatFromPattern(childName, @"H([0-9]*\.?[0-9]+)");

            collider.radius = radius;
            collider.height = height;
        }

        // Recursively process the child
        AssignCollidersRecursively(child);
    }
}

// Helper method to extract float values using regex
private float ExtractFloatFromPattern(string input, string pattern)
{
    Match match = Regex.Match(input, pattern);
    if (match.Success)
    {
        // Convert the captured group (i.e., the number) to a float
        return float.Parse(match.Groups[1].Value);
    }
    return 0f; // Return a default value if no match is found
}

        
        private void Delete<T>(Transform child) where T : Collider
        {
            T[] colliders = child.gameObject.GetComponents<T>();

            foreach (T collider in colliders)
            {
                DestroyImmediate(collider);
            }
        }

        private void Add<T>(Transform child) where T : Collider
        {
            T collider = child.gameObject.GetComponent<T>();
            if (collider == null)
            {
                collider = child.gameObject.AddComponent<T>();
            }

            if (collider is MeshCollider meshCollider)
            {
                meshCollider.convex = true;
            }

            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
                ClearConsole();
            }
        }

        public static void ClearConsole()
        {
            var logEntries = Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
            var clearMethod = logEntries.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
            clearMethod.Invoke(null, null);
        }
    }
}

using System;
using System.Reflection;
using UnityEngine;
using System.Text.RegularExpressions;

namespace MyTools.Runtime
{
    public class AssignColliders : MonoBehaviour
    {
        private void OnValidate()
        {
            Assign();
        }

        [ContextMenu("Update Colliders")]
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
                }
                else if (Regex.IsMatch(childName, @"SphereCollider.*$"))
                {
                    Add<SphereCollider>(child);
                }
                else if (Regex.IsMatch(childName, @"CapsuleCollider.*$"))
                {
                    Add<CapsuleCollider>(child);
                }

                // Recursively process the child
                AssignCollidersRecursively(child);
            }
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

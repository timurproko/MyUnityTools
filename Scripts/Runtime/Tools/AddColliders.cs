using System;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using Sirenix.OdinInspector;

namespace MyTools.Runtime
{
    public class AddColliders : MonoBehaviour
    {
        [SerializeField]
        [TitleGroup("Settings", Order = 2)]
        [Tooltip("When enabled, processes all children recursively. When disabled, only processes direct children.")]
        private bool _processRecursively = true;

        [SerializeField]
        [TitleGroup("Settings", Order = 3)]
        private bool _addCollidersAtRoot;

        [TitleGroup("Actions", Order = 1)]
        [HorizontalGroup("Actions/Buttons", MarginLeft = 10, MarginRight = 10)]
        [Button(ButtonSizes.Medium)]
        [GUIColor(0.4f, 0.8f, 1f)]
        public void Add()
        {
            Remove();
            Assign();
        }

        [HorizontalGroup("Actions/Buttons")]
        [Button(ButtonSizes.Medium)]
        [GUIColor(1f, 0.4f, 0.4f)]
        private void Remove()
        {
            RemoveCollidersFromObject(transform);
            if (_processRecursively)
            {
                RemoveCollidersRecursively(transform);
            }
        }

        private void Assign()
        {
            if (_processRecursively)
            {
                AssignCollidersRecursively(transform);
            }
            else
            {
                AssignCollidersToDirectChildren(transform);
            }

            if (_addCollidersAtRoot)
            {
                MoveCollidersToRoot();
            }
        }

        private void RemoveCollidersFromObject(Transform target)
        {
            foreach (var collider in target.GetComponents<Collider>())
            {
                DestroyImmediate(collider);
            }
        }

        private void RemoveCollidersRecursively(Transform parent)
        {
            foreach (Transform child in parent)
            {
                RemoveCollidersFromObject(child);
                RemoveCollidersRecursively(child);
            }
        }

        private void AssignCollidersRecursively(Transform parent)
        {
            foreach (Transform child in parent)
            {
                ProcessColliderAssignment(child);
                AssignCollidersRecursively(child);
            }
        }

        private void AssignCollidersToDirectChildren(Transform parent)
        {
            foreach (Transform child in parent)
            {
                ProcessColliderAssignment(child);
            }
        }

        private void ProcessColliderAssignment(Transform child)
        {
            string childName = child.gameObject.name;

            if (Regex.IsMatch(childName, @"MeshCollider.*$"))
            {
                Add<MeshCollider>(child, childName);
            }
            else if (Regex.IsMatch(childName, @"BoxCollider.*$"))
            {
                Add<BoxCollider>(child, childName);
            }
            else if (Regex.IsMatch(childName, @"SphereCollider.*$"))
            {
                Add<SphereCollider>(child, childName);
            }
            else if (Regex.IsMatch(childName, @"CapsuleCollider.*$"))
            {
                AddCapsuleCollider(child, childName);
            }
        }

        private void MoveCollidersToRoot()
        {
            if (_processRecursively)
            {
                MoveCollidersRecursively(transform);
            }
            else
            {
                MoveDirectChildrenColliders(transform);
            }
        }

        private void MoveCollidersRecursively(Transform parent)
        {
            foreach (Transform child in parent)
            {
                foreach (var originalCollider in child.GetComponents<Collider>())
                {
                    CopyColliderToRoot(originalCollider);
                    DestroyImmediate(originalCollider);
                }
                MoveCollidersRecursively(child);
            }
        }

        private void MoveDirectChildrenColliders(Transform parent)
        {
            foreach (Transform child in parent)
            {
                foreach (var originalCollider in child.GetComponents<Collider>())
                {
                    CopyColliderToRoot(originalCollider);
                    DestroyImmediate(originalCollider);
                }
            }
        }

        private void CopyColliderToRoot(Collider original)
        {
            Collider newCollider = null;

            if (original is BoxCollider box)
            {
                newCollider = gameObject.AddComponent<BoxCollider>();
                ((BoxCollider)newCollider).center =
                    transform.InverseTransformPoint(original.transform.position) + box.center;
                ((BoxCollider)newCollider).size = box.size;
            }
            else if (original is SphereCollider sphere)
            {
                newCollider = gameObject.AddComponent<SphereCollider>();
                ((SphereCollider)newCollider).center =
                    transform.InverseTransformPoint(original.transform.position) + sphere.center;
                ((SphereCollider)newCollider).radius = sphere.radius;
            }
            else if (original is CapsuleCollider capsule)
            {
                newCollider = gameObject.AddComponent<CapsuleCollider>();
                ((CapsuleCollider)newCollider).center =
                    transform.InverseTransformPoint(original.transform.position) + capsule.center;
                ((CapsuleCollider)newCollider).radius = capsule.radius;
                ((CapsuleCollider)newCollider).height = capsule.height;
                ((CapsuleCollider)newCollider).direction = capsule.direction;
            }
            else if (original is MeshCollider mesh)
            {
                newCollider = gameObject.AddComponent<MeshCollider>();
                ((MeshCollider)newCollider).sharedMesh = mesh.sharedMesh;
                ((MeshCollider)newCollider).convex = mesh.convex;
            }

            if (newCollider != null && original.gameObject.name.EndsWith("_Trigger"))
            {
                newCollider.isTrigger = true;
            }
        }

        private void AddCapsuleCollider(Transform target, string name)
        {
            var collider = target.gameObject.GetComponent<CapsuleCollider>();

            if (collider == null)
            {
                collider = target.gameObject.AddComponent<CapsuleCollider>();
            }

            if (Regex.IsMatch(name, @"CapsuleCollider.*_X.*$")) collider.direction = 0;
            else if (Regex.IsMatch(name, @"CapsuleCollider.*_Y.*$")) collider.direction = 1;
            else if (Regex.IsMatch(name, @"CapsuleCollider.*_Z.*$")) collider.direction = 2;

            float radius = ExtractFloatFromPattern(name, @"R([0-9]*\.?[0-9]+)");
            float height = ExtractFloatFromPattern(name, @"H([0-9]*\.?[0-9]+)");

            collider.radius = radius > 0 ? radius : 0.5f;
            collider.height = height > 0 ? height : 2f;

            if (name.EndsWith("_Trigger"))
            {
                collider.isTrigger = true;
            }
    
            var meshRenderer = target.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }
        }

        private float ExtractFloatFromPattern(string input, string pattern)
        {
            Match match = Regex.Match(input, pattern);
            return match.Success ? float.Parse(match.Groups[1].Value) : 0f;
        }

        private void Add<T>(Transform child, string name) where T : Collider
        {
            if (!child.gameObject.TryGetComponent<T>(out var collider))
            {
                collider = child.gameObject.AddComponent<T>();
            }

            if (collider is MeshCollider meshCollider)
            {
                meshCollider.convex = true;
            }

            if (child.TryGetComponent<MeshRenderer>(out var meshRenderer))
            {
                meshRenderer.enabled = false;
                ClearConsole();
            }

            if (name.EndsWith("_Trigger"))
            {
                collider.isTrigger = true;
            }
        }

        private static void ClearConsole()
        {
            var logEntries = Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
            var clearMethod = logEntries.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
            clearMethod.Invoke(null, null);
        }
    }
}
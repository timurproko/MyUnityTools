using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyTools.Runtime
{
    [AddComponentMenu("My Tools/Utility/" + nameof(DebugPhysics))]
    public class DebugPhysics : MonoBehaviour
    {
        [FormerlySerializedAs("_drawCollisionGeometry")]
        [FormerlySerializedAs("_DrawCollisionGeometry")]
        [FormerlySerializedAs("_enableDrawCollisionGeometry")]
        [Header("Collision")]
        [SerializeField]
        private bool _drawGeometry = true;

        [SerializeField] private bool _drawContact = true;
        [LabelText("Color")] [SerializeField] private Color _collisionsColor = Color.white;

        [LabelText("Duration (seconds)")] [SerializeField]
        private float _drawCollisionDuration = 10f;

        [SerializeField] private float _distance = 5f;

        [Header("Ghost")] [SerializeField] private bool _drawGhost = true;
        [LabelText("Color")] [SerializeField] private Color _ghostColor = Color.white;

        [LabelText("Duration (frames)")] [SerializeField]
        private int _drawGhostDuration = 100; // The number of frames between actions

        private readonly List<Vector3> _debugPositionList = new();
        private static Mesh _mesh;
        private int frameCounter;

        [Header("Path")] [LabelText("Color")] [SerializeField]
        private Color _pathColor = Color.white;

        [SerializeField] private bool _drawPath = true;
        private readonly List<Vector3> _positions = new();
        private Vector3 _lastPosition;

        private void Awake()
        {
            MeshFilter meshFilter = GetComponentInChildren<MeshFilter>();
            _mesh = meshFilter.mesh;
        }

        private void Start()
        {
            _lastPosition = transform.position;
            _positions.Add(_lastPosition);
        }

        void Update()
        {
            AddGhost();
            DrawPath();
        }

        private void OnDrawGizmos()
        {
            DrawGhost();
            DrawCollisionGeometry();
        }

        private void DrawCollisionGeometry()
        {
            foreach (Transform child in GetComponentsInChildren<Transform>())
            {
                if (_drawGeometry)
                {
                    // Check if the child's name contains "Collision" or "collision"
                    if (child.name.Contains("Collision") || child.name.Contains("collision"))
                    {
                        // Try to get the MeshFilter component
                        MeshFilter meshFilter = child.GetComponent<MeshFilter>();
                        if (meshFilter != null && meshFilter.sharedMesh != null)
                        {
                            // Draw the mesh at the child's position and rotation
                            Gizmos.DrawWireMesh(meshFilter.sharedMesh, child.position, child.rotation);
                        }
                    }
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            DrawCollision(collision);
        }

        private void DrawPath()
        {
            if (_drawPath)
            {
                if (Vector3.Distance(_lastPosition, transform.position) > 0.01f)
                {
                    _positions.Add(transform.position);
                    _lastPosition = transform.position;
                }

                for (int i = 0; i < _positions.Count - 1; i++)
                {
                    Debug.DrawLine(_positions[i], _positions[i + 1], _pathColor, 0f);
                }
            }
        }

        private void AddGhost()
        {
            frameCounter++;
            if (_drawGhost)
            {
                if (frameCounter % _drawGhostDuration == 0)
                {
                    _debugPositionList.Add(transform.position);
                }
            }
        }

        private void DrawGhost()
        {
            foreach (Vector3 debugPosition in _debugPositionList)
            {
                Gizmos.color = _ghostColor;
                Gizmos.DrawWireMesh(_mesh, debugPosition);
            }
        }

        private void DrawCollision(Collision collision)
        {
            if (_drawContact)
            {
                foreach (ContactPoint contact in collision.contacts)
                {
                    Debug.DrawRay(contact.point, contact.normal.normalized * _distance, _collisionsColor,
                        _drawCollisionDuration);
                }
            }
        }
    }
}
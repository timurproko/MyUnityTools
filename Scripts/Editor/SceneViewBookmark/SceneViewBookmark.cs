using UnityEditor;
using UnityEngine;

namespace SceneViewBookmarks
{
    struct SceneViewBookmark
    {
        public Vector3 pivot;
        public Quaternion rotation;
        public float size;
        public bool orthographic;

        public SceneViewBookmark(SceneView sceneView)
        {
            pivot = sceneView.pivot;
            rotation = sceneView.rotation;
            size = sceneView.size;
            orthographic = sceneView.orthographic;
        }
    }
}

using UnityEditor;
using UnityEngine;

namespace SceneViewTools
{
    internal struct SceneViewBookmark
    {
        public Vector3 pivot;
        public Quaternion rotation;
        public float size;
        public bool orthographic;
        public SceneViewType type;


        public SceneViewBookmark(UnityEditor.SceneView sceneView)
        {
            pivot = sceneView.pivot;
            rotation = sceneView.rotation;
            size = sceneView.size;
            orthographic = sceneView.orthographic;
            type = SceneViewNavigationIO.ReadFromEditorPrefs();
        }
    }
}

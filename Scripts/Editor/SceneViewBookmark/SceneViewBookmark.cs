using UnityEditor;
using UnityEngine;
using SceneViewNavigation;

namespace SceneViewBookmarks
{
    struct SceneViewBookmark
    {
        public Vector3 pivot;
        public Quaternion rotation;
        public float size;
        public bool orthographic;
        public SceneViewType type;


        public SceneViewBookmark(SceneView sceneView)
        {
            pivot = sceneView.pivot;
            rotation = sceneView.rotation;
            size = sceneView.size;
            orthographic = sceneView.orthographic;
            type = SceneViewNavigationIO.ReadFromEditorPrefs();
        }
    }
}

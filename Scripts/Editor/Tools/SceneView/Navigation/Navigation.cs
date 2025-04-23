using UnityEditor;
using UnityEngine;

namespace SceneViewTools
{
    public enum SceneViewType
    {
        Perspective = 0,
        Top = 1,
        Bottom = 2,
        Front = 3,
        Back = 4,
        Left = 5,
        Right = 6,
    }
    
    public static class ActiveSceneView
    {
        public static UnityEditor.SceneView sceneView;
        public static SceneViewType SceneViewType;
    }
    
    public static class DefaultValues
    {
        public static float size = 10f;
        public static Vector3 pivot = Vector3.zero;
    }

    public struct DefaultRotation
    {
        public static readonly Quaternion Perspective = Quaternion.Euler(26.33425f, 225f, 0f);
        public static readonly Quaternion Top = Quaternion.Euler(90f, 0f, 0f);
        public static readonly Quaternion Bottom = Quaternion.Euler(-90f, 0f, 0f);
        public static readonly Quaternion Front = Quaternion.Euler(0f, 180f, 0f);
        public static readonly Quaternion Back = Quaternion.Euler(0f, 0f, 0f);
        public static readonly Quaternion Left = Quaternion.Euler(0f, 90f, 0f);
        public static readonly Quaternion Right = Quaternion.Euler(0f, 270f, 0f);
    }
}
using UnityEditor;
using UnityEngine;

namespace MyTools.SceneViewTools

{
    public static class DefaultValues
    {
        public static float size = 10f;
        public static Vector3 pivot = Vector3.zero;
    }

    public static class SceneViewRef
    {
        public static SceneView sceneView;
        public static SceneViewType SceneViewType;
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

    public struct DefaultOrthographic
    {
        public static readonly bool Perspective = false;
        public static readonly bool Top = true;
        public static readonly bool Bottom = true;
        public static readonly bool Front = true;
        public static readonly bool Back = true;
        public static readonly bool Left = true;
        public static readonly bool Right = true;
    }
}
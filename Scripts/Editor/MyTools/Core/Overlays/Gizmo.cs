using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyTools
{
    [Overlay(typeof(SceneView), "My Gizmos")]
    public class ToggleGizmosOverlay : Overlay
    {
        public override VisualElement CreatePanelContent()
        {
            var root = new VisualElement();

            AddToggle(root, "Icons", ToggleAllIcons.IconsEnabled, _ => ToggleAllIcons.ToggleIcons());
            AddToggle(root, "3D Icons", GizmoUtility.use3dIcons, evt => ToggleGizmos.ToggleIcons(evt.newValue));
            AddToggle(root, "Camera", ToggleGizmos.GetCameraGizmoState(),
                evt => ToggleGizmos.ToggleCameraGizmos(evt.newValue));
            AddToggle(root, "Canvas", ToggleGizmos.GetCanvasGizmoState(),
                evt => ToggleGizmos.ToggleCanvasGizmos(evt.newValue));
            AddToggle(root, "Selection Outline", ToggleGizmos.GetSelectionOutlineState(),
                evt => ToggleGizmos.ToggleSelectionOutline(evt.newValue));
            AddToggle(root, "Selection Wire", ToggleGizmos.GetSelectionWireState(),
                evt => ToggleGizmos.ToggleSelectionWire(evt.newValue));
            AddToggle(root, "Colliders", ToggleColliders.Non2DCollidersEnabled,
                evt => ToggleColliders.ToggleNon2DColliders(evt.newValue));
            AddToggle(root, "2D Colliders", ToggleColliders.Colliders2DEnabled,
                evt => ToggleColliders.Toggle2DColliders(evt.newValue));

            return root;
        }

        
        private void AddToggle(VisualElement root, string label, bool initialValue,
            EventCallback<ChangeEvent<bool>> callback)
        {
            var toggle = new Toggle(label) { value = initialValue };
            toggle.RegisterValueChangedCallback(callback);
            root.Add(toggle);
        }
    }
    
    public static class ToggleColliders
    {
        private static bool non2DCollidersEnabled = true;
        private static bool colliders2DEnabled = true;

        public static bool Non2DCollidersEnabled => non2DCollidersEnabled;
        public static bool Colliders2DEnabled => colliders2DEnabled;

        public static void ToggleNon2DColliders(bool state)
        {
            non2DCollidersEnabled = state;
            var colliderTypes = new[]
                { typeof(BoxCollider), typeof(CapsuleCollider), typeof(MeshCollider), typeof(SphereCollider) };

            SetGizmosEnabledForTypes(colliderTypes, state);
            Functions.ClearConsole();
        }

        public static void Toggle2DColliders(bool state)
        {
            colliders2DEnabled = state;
            var collider2DTypes = new[]
                { typeof(BoxCollider2D), typeof(CircleCollider2D), typeof(PolygonCollider2D), typeof(EdgeCollider2D) };

            SetGizmosEnabledForTypes(collider2DTypes, state);
            Functions.ClearConsole();
        }

        private static void SetGizmosEnabledForTypes(Type[] types, bool state)
        {
            foreach (var type in types)
            {
                GizmoUtility.SetGizmoEnabled(type, state, true);
            }
        }
    }
    
    public static class ToggleGizmos
    {
        public static void ToggleIcons(bool state) => GizmoUtility.use3dIcons = state;

        public static void ToggleSelectionOutline(bool state) =>
            SetAnnotationUtilityProperty("showSelectionOutline", state);

        public static bool GetSelectionOutlineState() => GetAnnotationUtilityProperty("showSelectionOutline");

        public static void ToggleSelectionWire(bool state) => SetAnnotationUtilityProperty("showSelectionWire", state);
        public static bool GetSelectionWireState() => GetAnnotationUtilityProperty("showSelectionWire");

        public static void ToggleCameraGizmos(bool state) => GizmoUtility.SetGizmoEnabled(typeof(Camera), state, true);
        public static bool GetCameraGizmoState() => true; // Placeholder implementation

        public static void ToggleCanvasGizmos(bool state) => GizmoUtility.SetGizmoEnabled(typeof(Canvas), state, true);
        public static bool GetCanvasGizmoState() => true; // Placeholder implementation

        private static void SetAnnotationUtilityProperty(string propertyName, bool state)
        {
            var property = GetAnnotationUtilityPropertyInfo(propertyName);
            property?.SetValue(null, state);
        }

        private static bool GetAnnotationUtilityProperty(string propertyName)
        {
            var property = GetAnnotationUtilityPropertyInfo(propertyName);
            return property != null && (bool)property.GetValue(null);
        }

        private static PropertyInfo GetAnnotationUtilityPropertyInfo(string propertyName)
        {
            var asm = Assembly.GetAssembly(typeof(Editor));
            var type = asm?.GetType("UnityEditor.AnnotationUtility");
            return type?.GetProperty(propertyName, BindingFlags.Static | BindingFlags.NonPublic);
        }
    }
    
    public static class ToggleAllIcons
    {
        private static bool iconsEnabled = true;
        public static bool IconsEnabled => iconsEnabled;

        public static void ToggleIcons()
        {
            iconsEnabled = !iconsEnabled;
            var componentTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(Component).IsAssignableFrom(type) && !type.IsAbstract);

            foreach (var type in componentTypes)
            {
                GizmoUtility.SetIconEnabled(type, iconsEnabled);
            }

            Functions.ClearConsole();
        }
    }
}
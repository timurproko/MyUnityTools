using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;
using System.Reflection;

namespace MyTools
{
    [Overlay(typeof(SceneView), "Gizmos")]
    public class ToggleGizmosOverlay : Overlay
    {
        public override VisualElement CreatePanelContent()
        {
            var root = new VisualElement();

            // Icons Toggle (new option)
            var allIconsToggle = new Toggle("Icons") { value = ToggleAllIcons.IconsEnabled };
            allIconsToggle.RegisterValueChangedCallback(evt => ToggleAllIcons.ToggleIcons());
            root.Add(allIconsToggle);

            // 3D Icons Toggle
            var iconsToggle = new Toggle("3D Icons") { value = GizmoUtility.use3dIcons };
            iconsToggle.RegisterValueChangedCallback(evt => ToggleGizmos.ToggleIcons(evt.newValue));
            root.Add(iconsToggle);

            // Camera Gizmos Toggle
            var cameraGizmosToggle = new Toggle("Camera") { value = ToggleGizmos.GetCameraGizmoState() };
            cameraGizmosToggle.RegisterValueChangedCallback(evt => ToggleGizmos.ToggleCameraGizmos(evt.newValue));
            root.Add(cameraGizmosToggle);

            // Canvas Gizmos Toggle
            var canvasGizmosToggle = new Toggle("Canvas") { value = ToggleGizmos.GetCanvasGizmoState() };
            canvasGizmosToggle.RegisterValueChangedCallback(evt => ToggleGizmos.ToggleCanvasGizmos(evt.newValue));
            root.Add(canvasGizmosToggle);

            // Selection Outline Toggle
            var outlineToggle = new Toggle("Selection Outline") { value = ToggleGizmos.GetSelectionOutlineState() };
            outlineToggle.RegisterValueChangedCallback(evt => ToggleGizmos.ToggleSelectionOutline(evt.newValue));
            root.Add(outlineToggle);

            // Selection Wire Toggle
            var wireToggle = new Toggle("Selection Wire") { value = ToggleGizmos.GetSelectionWireState() };
            wireToggle.RegisterValueChangedCallback(evt => ToggleGizmos.ToggleSelectionWire(evt.newValue));
            root.Add(wireToggle);

            return root;
        }
    }

    public static class ToggleGizmos
    {
        // Toggle 3D Icons
        public static void ToggleIcons(bool state)
        {
            GizmoUtility.use3dIcons = state;
        }

        // Toggle Selection Outline
        public static void ToggleSelectionOutline(bool state)
        {
            Assembly asm = Assembly.GetAssembly(typeof(Editor));
            Type type = asm.GetType("UnityEditor.AnnotationUtility");
            if (type != null)
            {
                PropertyInfo property =
                    type.GetProperty("showSelectionOutline", BindingFlags.Static | BindingFlags.NonPublic);
                property.SetValue(asm, state, null);
            }
        }

        // Get Selection Outline State
        public static bool GetSelectionOutlineState()
        {
            Assembly asm = Assembly.GetAssembly(typeof(Editor));
            Type type = asm.GetType("UnityEditor.AnnotationUtility");
            if (type != null)
            {
                PropertyInfo property =
                    type.GetProperty("showSelectionOutline", BindingFlags.Static | BindingFlags.NonPublic);
                return (bool)property.GetValue(asm, null);
            }

            return false;
        }

        // Toggle Selection Wire
        public static void ToggleSelectionWire(bool state)
        {
            Assembly asm = Assembly.GetAssembly(typeof(Editor));
            Type type = asm.GetType("UnityEditor.AnnotationUtility");
            if (type != null)
            {
                PropertyInfo property =
                    type.GetProperty("showSelectionWire", BindingFlags.Static | BindingFlags.NonPublic);
                property.SetValue(asm, state, null);
            }
        }

        // Get Selection Wire State
        public static bool GetSelectionWireState()
        {
            Assembly asm = Assembly.GetAssembly(typeof(Editor));
            Type type = asm.GetType("UnityEditor.AnnotationUtility");
            if (type != null)
            {
                PropertyInfo property =
                    type.GetProperty("showSelectionWire", BindingFlags.Static | BindingFlags.NonPublic);
                return (bool)property.GetValue(asm, null);
            }

            return false;
        }

        // Toggle Camera Gizmos
        public static void ToggleCameraGizmos(bool state)
        {
            GizmoUtility.SetGizmoEnabled(typeof(Camera), state, true);
        }

        // Get Camera Gizmo State
        public static bool GetCameraGizmoState()
        {
            return true;
        }

        // Toggle Canvas Gizmos
        public static void ToggleCanvasGizmos(bool state)
        {
            GizmoUtility.SetGizmoEnabled(typeof(Canvas), state, true);
        }

        // Get Canvas Gizmo State
        public static bool GetCanvasGizmoState()
        {
            return true;
        }
    }

    // Toggle Icons State
    public static class ToggleAllIcons
    {
        private static bool iconsEnabled = true;

        public static bool IconsEnabled
        {
            get { return iconsEnabled; }
        }

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

            MyTools.ClearConsole();
        }
    }
}
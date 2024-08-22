using System;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine.UIElements;
using System.Reflection;
using UnityEditor.Toolbars;

namespace MyTools.SceneViewTools
{
    [Overlay(typeof(SceneView), "Toggle Gizmos")]
    public class ToggleGizmosOverlay : Overlay
    {
        public override VisualElement CreatePanelContent()
        {
            var root = new VisualElement();

            var iconsButton = new EditorToolbarButton() { text = "3D Icons" };
            iconsButton.clicked += ToggleGizmos.ToggleIcons;
            root.Add(iconsButton);

            var outlineButton = new EditorToolbarButton() { text = "Selection Outline" };
            outlineButton.clicked += ToggleGizmos.ToggleSelectionOutline;
            root.Add(outlineButton);

            var wireButton = new EditorToolbarButton() { text = "Selection Wire" };
            wireButton.clicked += ToggleGizmos.ToggleSelectionWire;
            root.Add(wireButton);

            return root;
        }
    }


    public class ToggleGizmos
    {
#if !UNITY_5
        static float iconSize;
        static bool use3dGizmos;
#endif

        public static void ToggleIcons()
        {
            Assembly asm = Assembly.GetAssembly(typeof(Editor));
            Type type = asm.GetType("UnityEditor.AnnotationUtility");
            if (type != null)
            {
                PropertyInfo use3dGizmosProperty =
                    type.GetProperty("use3dGizmos", BindingFlags.Static | BindingFlags.NonPublic);
                PropertyInfo iconSizeProperty =
                    type.GetProperty("iconSize", BindingFlags.Static | BindingFlags.NonPublic);

                float nowIconSize = (float)iconSizeProperty.GetValue(asm, null);
                if (nowIconSize > 0) // to hide
                {
#if UNITY_5
					EditorPrefs.SetFloat(Strings.prefs_use3dGizmos, nowIconSize);
#endif
                    iconSize = nowIconSize;
                    iconSizeProperty.SetValue(asm, 0, null);

#if UNITY_5
					bool use3dGizmos = (bool) use3dGizmosProperty.GetValue( asm, null );
					EditorPrefs.SetBool(Strings.prefs_use3dGizmos, use3dGizmos);
#else
                    use3dGizmos = (bool)use3dGizmosProperty.GetValue(asm, null);
#endif
                    use3dGizmosProperty.SetValue(asm, true, null);
                }
                else // to show
                {
#if UNITY_5
					float iconSize = EditorPrefs.GetFloat(Strings.prefs_iconSize);
#endif
                    if (iconSize <= 0)
                        iconSize = 0.03162277f; // Mathf.Pow(10f, -3f + 3f * 0.5f), see to Convert01ToTexelWorldSize()
                    iconSizeProperty.SetValue(asm, iconSize, null);

#if UNITY_5
					bool use3dGizmos = EditorPrefs.GetBool(Strings.prefs_use3dGizmos);
#endif
                    use3dGizmosProperty.SetValue(asm, use3dGizmos, null);
                }
            }
        }


        public static void ToggleSelectionOutline()
        {
            Assembly asm = Assembly.GetAssembly(typeof(Editor));
            Type type = asm.GetType("UnityEditor.AnnotationUtility");
            if (type != null)
            {
                PropertyInfo property = type.GetProperty("showSelectionOutline",
                    BindingFlags.Static | BindingFlags.NonPublic);
                bool flag = (bool)property.GetValue(asm, null);
                property.SetValue(asm, !flag, null);
            }
        }


        public static void ToggleSelectionWire()
        {
            Assembly asm = Assembly.GetAssembly(typeof(Editor));
            Type type = asm.GetType("UnityEditor.AnnotationUtility");
            if (type != null)
            {
                PropertyInfo property = type.GetProperty("showSelectionWire",
                    BindingFlags.Static | BindingFlags.NonPublic);
                bool flag = (bool)property.GetValue(asm, null);
                property.SetValue(asm, !flag, null);
            }
        }
    }
}
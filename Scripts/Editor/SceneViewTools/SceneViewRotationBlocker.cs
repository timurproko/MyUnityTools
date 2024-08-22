using UnityEditor;
using UnityEngine;

namespace MyTools.SceneViewTools
{
    [InitializeOnLoad]
    public class SceneViewRotationBlocker
    {
        static SceneViewRotationBlocker()
        {
            // Subscribe to the Scene View's `duringSceneGui` event
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void OnSceneGUI(UnityEditor.SceneView sceneView)
        {
            if (SceneViewShortcuts.sceneViewType != SceneViewType.Perspective)
            {
                // Check if Alt is held and LMB is pressed
                Event e = Event.current;
                if (e.alt && e.button == 0 && e.type == EventType.MouseDrag)
                {
                    // Consume the event to prevent the rotation
                    e.Use();
                }
            }
        }
    }
}


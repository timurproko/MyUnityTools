#if UNITY_EDITOR
using MyTools;
using UnityEditor;
using UnityEngine;

namespace MyTools
{
    [InitializeOnLoad]
    public class SceneViewNavigationBlocker
    {
        private static bool isPanning;
        private static Vector2 lastMousePosition;

        static SceneViewNavigationBlocker()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            if (State.disabled) return;

            Event e = Event.current;

            if (ActiveSceneView.SceneViewType != SceneViewType.Perspective && !sceneView.in2DMode && sceneView.orthographic)
            {
                if (e.alt && e.button == 0)
                {
                    EditorGUIUtility.AddCursorRect(sceneView.position, MouseCursor.Pan);

                    switch (e.type)
                    {
                        case EventType.MouseDown:
                            StartPanning(e);
                            break;

                        case EventType.MouseDrag:
                            if (isPanning)
                            {
                                PerformPanning(sceneView, e);
                            }
                            break;

                        case EventType.MouseUp:
                            if (isPanning)
                            {
                                StopPanning(e);
                            }
                            break;
                    }
                }
            }
        }

        private static void StartPanning(Event e)
        {
            isPanning = true;
            lastMousePosition = e.mousePosition;
            e.Use();
            SceneView.RepaintAll();
        }

        private static void PerformPanning(SceneView sceneView, Event e)
        {
            Vector2 delta = e.mousePosition - lastMousePosition;
            delta *= sceneView.size / 500f;
            Vector3 move = new Vector3(-delta.x, delta.y, 0);
            sceneView.pivot += sceneView.rotation * move;
            lastMousePosition = e.mousePosition;
            e.Use();
        }

        private static void StopPanning(Event e)
        {
            isPanning = false;
            e.Use();
            SceneView.RepaintAll();
        }
    }
}
#endif

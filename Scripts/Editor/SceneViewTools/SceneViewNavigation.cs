using UnityEditor;
using UnityEngine;

namespace MyTools.SceneViewTools
{
    static class SceneViewNavigation
    {
        private static SceneView sceneView;
        internal static SceneViewType sceneViewType;
        private static SceneViewData sceneViewData;

        private static string sceneViewDataPath =
            "Packages/com.timurproko.mytools/Scripts/Editor/SceneViewTools/SceneViewData.asset";

        static SceneViewNavigation()
        {
            // Load or create the ScriptableObject
            sceneViewData = AssetDatabase.LoadAssetAtPath<SceneViewData>(sceneViewDataPath);
            if (sceneViewData == null)
            {
                sceneViewData = ScriptableObject.CreateInstance<SceneViewData>();
                AssetDatabase.CreateAsset(sceneViewData, sceneViewDataPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        [MenuItem("My Tools/Scene View Toolset/Reset View &h", priority = 100)] //  Alt+H
        static void ResetSceneViewCamera()
        {
            Tools.ActivateWindowUnderCursor();
            sceneView = GetLastActiveSceneView();

            if (sceneView != null)
            {
                sceneView.size = DefaultsValue.size;
                sceneView.pivot = DefaultsValue.pivot;
                if (!sceneView.in2DMode)
                {
                    switch (sceneViewType)
                    {
                        case SceneViewType.Perspective:
                            sceneView.rotation = DefaultRotation.Perspective;
                            break;
                        case SceneViewType.Top:
                            sceneView.rotation = DefaultRotation.Top;
                            break;
                        case SceneViewType.Bottom:
                            sceneView.rotation = DefaultRotation.Bottom;
                            break;
                        case SceneViewType.Front:
                            sceneView.rotation = DefaultRotation.Front;
                            break;
                        case SceneViewType.Back:
                            sceneView.rotation = DefaultRotation.Back;
                            break;
                        case SceneViewType.Left:
                            sceneView.rotation = DefaultRotation.Left;
                            break;
                        case SceneViewType.Right:
                            sceneView.rotation = DefaultRotation.Right;
                            break;
                    }
                }

                sceneView.Repaint();
            }
        }

        [MenuItem("My Tools/Scene View Toolset/Perspective &1", priority = 200)]
        static void PerspectiveView()
        {
            Tools.ActivateWindowUnderCursor();
            SaveActiveSceneViewData(SceneViewType.Perspective);
            sceneViewType = SceneViewType.Perspective;
            SetPerspectiveView();
            SetPerspectiveView();
        }

        [MenuItem("My Tools/Scene View Toolset/Toggle Top-Bottom &2", priority = 201)]
        static void ToggleTopBottomView()
        {
            Tools.ActivateWindowUnderCursor();

            if (sceneViewType == SceneViewType.Top)
            {
                BottomView();
                sceneViewType = SceneViewType.Bottom;
            }
            else
            {
                TopView();
                sceneViewType = SceneViewType.Top;
            }
        }

        [MenuItem("My Tools/Scene View Toolset/Toggle Front-Back &3", priority = 202)]
        static void ToggleFrontBackView()
        {
            Tools.ActivateWindowUnderCursor();

            if (sceneViewType == SceneViewType.Front)
            {
                BackView();
                sceneViewType = SceneViewType.Back;
            }
            else
            {
                FrontView();
                sceneViewType = SceneViewType.Front;
            }
        }

        [MenuItem("My Tools/Scene View Toolset/Toggle Right-Left &4", priority = 203)]
        static void ToggleRightLeftView()
        {
            Tools.ActivateWindowUnderCursor();

            if (sceneViewType == SceneViewType.Right)
            {
                LeftView();
                sceneViewType = SceneViewType.Left;
            }
            else
            {
                RightView();
                sceneViewType = SceneViewType.Right;
            }
        }

        [MenuItem("My Tools/Scene View Toolset/Top", priority = 300)]
        static void TopView()
        {
            SaveActiveSceneViewData(SceneViewType.Top);
            sceneViewType = SceneViewType.Top;
            SetTopView();
            SetTopView();
        }

        [MenuItem("My Tools/Scene View Toolset/Bottom", priority = 301)]
        static void BottomView()
        {
            SaveActiveSceneViewData(SceneViewType.Bottom);
            sceneViewType = SceneViewType.Bottom;
            SetBottomView();
            SetBottomView();
        }

        [MenuItem("My Tools/Scene View Toolset/Front", priority = 302)]
        static void FrontView()
        {
            SaveActiveSceneViewData(SceneViewType.Front);
            sceneViewType = SceneViewType.Front;
            SetFrontView();
            SetFrontView();
        }

        [MenuItem("My Tools/Scene View Toolset/Back", priority = 303)]
        static void BackView()
        {
            SaveActiveSceneViewData(SceneViewType.Back);
            sceneViewType = SceneViewType.Back;
            SetBackView();
            SetBackView();
        }

        [MenuItem("My Tools/Scene View Toolset/Left", priority = 305)]
        static void LeftView()
        {
            SaveActiveSceneViewData(SceneViewType.Left);
            sceneViewType = SceneViewType.Left;
            SetLeftView();
            SetLeftView();
        }

        [MenuItem("My Tools/Scene View Toolset/Right", priority = 304)]
        static void RightView()
        {
            SaveActiveSceneViewData(SceneViewType.Right);
            sceneViewType = SceneViewType.Right;
            SetRightView();
            SetRightView();
        }

        static void SetPerspectiveView() => SetView(SceneViewType.Perspective, DefaultRotation.Perspective, false);
        static void SetTopView() => SetView(SceneViewType.Top, DefaultRotation.Top, true);
        static void SetBottomView() => SetView(SceneViewType.Bottom, DefaultRotation.Bottom, true);
        static void SetFrontView() => SetView(SceneViewType.Front, DefaultRotation.Front, true);
        static void SetBackView() => SetView(SceneViewType.Back, DefaultRotation.Back, true);
        static void SetRightView() => SetView(SceneViewType.Right, DefaultRotation.Right, true);
        static void SetLeftView() => SetView(SceneViewType.Left, DefaultRotation.Left, true);

        private static void SetView(SceneViewType viewType, Quaternion defaultRotation, bool isOrthographic)
        {
            sceneView = GetLastActiveSceneView();

            if (sceneView == null)
                return;

            if (sceneView.in2DMode)
            {
                sceneView.orthographic = !sceneView.orthographic;
            }
            else
            {
                sceneView.orthographic = isOrthographic;
                if (sceneView.orthographic)
                {
                    SceneViewShortcuts.DisableSkybox();
                }
                else
                {
                    SceneViewShortcuts.EnableSkybox();
                }

                if (sceneViewData.TryGetViewState(viewType, out var savedState))
                {
                    ApplyNewValues(savedState);
                }
                else
                {
                    ApplyDefaultValues(defaultRotation);
                }

                if (SceneViewShortcuts.wasIn2DMode)
                {
                    if (sceneView != null)
                    {
                        ResetSceneViewCamera();
                        // sceneView.Repaint();
                    }
                }
            }


            sceneView.Repaint();
        }

        public static void ApplyDefaultValues(Quaternion rotation)
        {
            sceneView.size = DefaultsValue.size;
            sceneView.pivot = DefaultsValue.pivot;
            sceneView.rotation = rotation;
        }

        public static void ApplyNewValues(SceneViewData.ViewState savedState)
        {
            sceneView.size = savedState.size;
            sceneView.pivot = savedState.pivot;
            if (!sceneView.in2DMode)
            {
                sceneView.rotation = savedState.rotation;
            }
        }

        public static SceneView GetLastActiveSceneView()
        {
            return SceneView.lastActiveSceneView;
        }

        public static void SaveActiveSceneViewData(SceneViewType viewType)
        {
            if (sceneView != null)
            {
                sceneViewData.SaveViewState(sceneViewType, sceneView.size, sceneView.rotation, sceneView.pivot);
                EditorUtility.SetDirty(sceneViewData);
                sceneViewData.lastActiveSceneViewType = viewType;
            }
        }
    }
}
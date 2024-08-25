using UnityEditor;
using UnityEngine;

namespace MyTools.SceneViewTools
{
    static class SceneViewNavigation
    {
        [MenuItem("My Tools/Scene View Toolset/Perspective &1", priority = 200)]
        static void PerspectiveView()
        {
            Tools.ActivateWindowUnderCursor();
            SaveActiveSceneViewData(SceneViewTypes.Perspective);
            SceneViewRef.SceneViewTypes = SceneViewTypes.Perspective;
            SetPerspectiveView();
            SetPerspectiveView();
        }

        [MenuItem("My Tools/Scene View Toolset/Toggle Top-Bottom &2", priority = 201)]
        static void ToggleTopBottomView()
        {
            Tools.ActivateWindowUnderCursor();

            if (SceneViewRef.SceneViewTypes == SceneViewTypes.Top)
            {
                BottomView();
                SceneViewRef.SceneViewTypes = SceneViewTypes.Bottom;
            }
            else
            {
                TopView();
                SceneViewRef.SceneViewTypes = SceneViewTypes.Top;
            }
        }

        [MenuItem("My Tools/Scene View Toolset/Toggle Front-Back &3", priority = 202)]
        static void ToggleFrontBackView()
        {
            Tools.ActivateWindowUnderCursor();

            if (SceneViewRef.SceneViewTypes == SceneViewTypes.Front)
            {
                BackView();
                SceneViewRef.SceneViewTypes = SceneViewTypes.Back;
            }
            else
            {
                FrontView();
                SceneViewRef.SceneViewTypes = SceneViewTypes.Front;
            }
        }

        [MenuItem("My Tools/Scene View Toolset/Toggle Right-Left &4", priority = 203)]
        static void ToggleRightLeftView()
        {
            Tools.ActivateWindowUnderCursor();

            if (SceneViewRef.SceneViewTypes == SceneViewTypes.Right)
            {
                LeftView();
                SceneViewRef.SceneViewTypes = SceneViewTypes.Left;
            }
            else
            {
                RightView();
                SceneViewRef.SceneViewTypes = SceneViewTypes.Right;
            }
        }

        [MenuItem("My Tools/Scene View Toolset/Top", priority = 300)]
        static void TopView()
        {
            SaveActiveSceneViewData(SceneViewTypes.Top);
            SceneViewRef.SceneViewTypes = SceneViewTypes.Top;
            SetTopView();
            SetTopView();
        }

        [MenuItem("My Tools/Scene View Toolset/Bottom", priority = 301)]
        static void BottomView()
        {
            SaveActiveSceneViewData(SceneViewTypes.Bottom);
            SceneViewRef.SceneViewTypes = SceneViewTypes.Bottom;
            SetBottomView();
            SetBottomView();
        }

        [MenuItem("My Tools/Scene View Toolset/Front", priority = 302)]
        static void FrontView()
        {
            SaveActiveSceneViewData(SceneViewTypes.Front);
            SceneViewRef.SceneViewTypes = SceneViewTypes.Front;
            SetFrontView();
            SetFrontView();
        }

        [MenuItem("My Tools/Scene View Toolset/Back", priority = 303)]
        static void BackView()
        {
            SaveActiveSceneViewData(SceneViewTypes.Back);
            SceneViewRef.SceneViewTypes = SceneViewTypes.Back;
            SetBackView();
            SetBackView();
        }

        [MenuItem("My Tools/Scene View Toolset/Left", priority = 305)]
        static void LeftView()
        {
            SaveActiveSceneViewData(SceneViewTypes.Left);
            SceneViewRef.SceneViewTypes = SceneViewTypes.Left;
            SetLeftView();
            SetLeftView();
        }

        [MenuItem("My Tools/Scene View Toolset/Right", priority = 304)]
        static void RightView()
        {
            SaveActiveSceneViewData(SceneViewTypes.Right);
            SceneViewRef.SceneViewTypes = SceneViewTypes.Right;
            SetRightView();
            SetRightView();
        }

        static void SetPerspectiveView() => SetView(SceneViewTypes.Perspective, false);
        static void SetTopView() => SetView(SceneViewTypes.Top, true);
        static void SetBottomView() => SetView(SceneViewTypes.Bottom, true);
        static void SetFrontView() => SetView(SceneViewTypes.Front, true);
        static void SetBackView() => SetView(SceneViewTypes.Back, true);
        static void SetRightView() => SetView(SceneViewTypes.Right, true);
        static void SetLeftView() => SetView(SceneViewTypes.Left, true);

        private static void SetView(SceneViewTypes viewTypes, bool isOrthographic)
        {
            SceneViewRef.sceneView = SceneView.lastActiveSceneView;

            if (SceneViewRef.sceneView == null)
                return;

            if (SceneViewRef.sceneView.in2DMode)
            {
                SceneViewRef.sceneView.orthographic = !SceneViewRef.sceneView.orthographic;
            }
            else
            {
                SceneViewRef.sceneView.orthographic = isOrthographic;
                if (SceneViewRef.sceneView.orthographic)
                {
                    SceneViewShortcuts.DisableSkybox();
                }
                else
                {
                    SceneViewShortcuts.EnableSkybox();
                }

                if (SceneViewSaveData.TryGetViewState(viewTypes, out var savedState))
                {
                    ApplyNewValues(savedState);
                }
                else
                {
                    ApplyDefaultValues(viewTypes);
                }
            }

            SceneViewRef.sceneView.Repaint();
        }

        public static void ApplyDefaultValues(SceneViewTypes viewTypes)
        {
            SceneViewRef.sceneView.size = DefaultValues.size;
            SceneViewRef.sceneView.pivot = DefaultValues.pivot;
            SceneViewRef.sceneView.rotation = GetDefaultRotation(viewTypes);
        }

        public static void ApplyNewValues(SceneViewSaveData.ViewState savedState)
        {
            SceneViewRef.sceneView.size = savedState.size;
            SceneViewRef.sceneView.pivot = savedState.pivot;
            if (!SceneViewRef.sceneView.in2DMode)
            {
                SceneViewRef.sceneView.rotation = savedState.rotation;
            }
        }

        public static void SaveActiveSceneViewData(SceneViewTypes viewTypes)
        {
            if (SceneViewRef.sceneView != null)
            {
                SceneViewSaveData.SaveViewState(
                    SceneViewRef.SceneViewTypes,
                    SceneViewRef.sceneView.size,
                    SceneViewRef.sceneView.rotation,
                    SceneViewRef.sceneView.pivot);
                SceneViewSaveData.SaveLastActiveSceneViewType(viewTypes);
            }
        }

        private static Quaternion GetDefaultRotation(SceneViewTypes viewTypes)
        {
            return viewTypes switch
            {
                SceneViewTypes.Perspective => DefaultRotation.Perspective,
                SceneViewTypes.Top => DefaultRotation.Top,
                SceneViewTypes.Bottom => DefaultRotation.Bottom,
                SceneViewTypes.Front => DefaultRotation.Front,
                SceneViewTypes.Back => DefaultRotation.Back,
                SceneViewTypes.Left => DefaultRotation.Left,
                SceneViewTypes.Right => DefaultRotation.Right,
                _ => DefaultRotation.Perspective
            };
        }
    }
}
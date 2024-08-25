using UnityEditor;

namespace MyTools.SceneViewTools
{
    static class SceneViewNavigation
    {
        [MenuItem("My Tools/Scene View Toolset/Perspective &1", priority = 200)]
        static void PerspectiveView()
        {
            Tools.ActivateWindowUnderCursor();
            SaveActiveSceneViewData(SceneViewType.Perspective);
            SceneViewRef.SceneViewType = SceneViewType.Perspective;
            SetPerspectiveView();
            SetPerspectiveView();
        }

        [MenuItem("My Tools/Scene View Toolset/Toggle Top-Bottom &2", priority = 201)]
        static void ToggleTopBottomView()
        {
            Tools.ActivateWindowUnderCursor();

            if (SceneViewRef.SceneViewType == SceneViewType.Top)
            {
                BottomView();
                SceneViewRef.SceneViewType = SceneViewType.Bottom;
            }
            else
            {
                TopView();
                SceneViewRef.SceneViewType = SceneViewType.Top;
            }
        }

        [MenuItem("My Tools/Scene View Toolset/Toggle Front-Back &3", priority = 202)]
        static void ToggleFrontBackView()
        {
            Tools.ActivateWindowUnderCursor();

            if (SceneViewRef.SceneViewType == SceneViewType.Front)
            {
                BackView();
                SceneViewRef.SceneViewType = SceneViewType.Back;
            }
            else
            {
                FrontView();
                SceneViewRef.SceneViewType = SceneViewType.Front;
            }
        }

        [MenuItem("My Tools/Scene View Toolset/Toggle Right-Left &4", priority = 203)]
        static void ToggleRightLeftView()
        {
            Tools.ActivateWindowUnderCursor();

            if (SceneViewRef.SceneViewType == SceneViewType.Right)
            {
                LeftView();
                SceneViewRef.SceneViewType = SceneViewType.Left;
            }
            else
            {
                RightView();
                SceneViewRef.SceneViewType = SceneViewType.Right;
            }
        }

        [MenuItem("My Tools/Scene View Toolset/Top", priority = 300)]
        static void TopView()
        {
            SaveActiveSceneViewData(SceneViewType.Top);
            SceneViewRef.SceneViewType = SceneViewType.Top;
            SetTopView();
            SetTopView();
        }

        [MenuItem("My Tools/Scene View Toolset/Bottom", priority = 301)]
        static void BottomView()
        {
            SaveActiveSceneViewData(SceneViewType.Bottom);
            SceneViewRef.SceneViewType = SceneViewType.Bottom;
            SetBottomView();
            SetBottomView();
        }

        [MenuItem("My Tools/Scene View Toolset/Front", priority = 302)]
        static void FrontView()
        {
            SaveActiveSceneViewData(SceneViewType.Front);
            SceneViewRef.SceneViewType = SceneViewType.Front;
            SetFrontView();
            SetFrontView();
        }

        [MenuItem("My Tools/Scene View Toolset/Back", priority = 303)]
        static void BackView()
        {
            SaveActiveSceneViewData(SceneViewType.Back);
            SceneViewRef.SceneViewType = SceneViewType.Back;
            SetBackView();
            SetBackView();
        }

        [MenuItem("My Tools/Scene View Toolset/Left", priority = 305)]
        static void LeftView()
        {
            SaveActiveSceneViewData(SceneViewType.Left);
            SceneViewRef.SceneViewType = SceneViewType.Left;
            SetLeftView();
            SetLeftView();
        }

        [MenuItem("My Tools/Scene View Toolset/Right", priority = 304)]
        static void RightView()
        {
            SaveActiveSceneViewData(SceneViewType.Right);
            SceneViewRef.SceneViewType = SceneViewType.Right;
            SetRightView();
            SetRightView();
        }

        static void SetPerspectiveView() => SetView(SceneViewType.Perspective);
        public static void SetTopView() => SetView(SceneViewType.Top);
        static void SetBottomView() => SetView(SceneViewType.Bottom);
        static void SetFrontView() => SetView(SceneViewType.Front);
        static void SetBackView() => SetView(SceneViewType.Back);
        static void SetRightView() => SetView(SceneViewType.Right);
        static void SetLeftView() => SetView(SceneViewType.Left);

        private static void SetView(SceneViewType viewType)
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
                if (viewType == SceneViewType.Perspective)
                {
                    SceneViewShortcuts.EnableSkybox();
                }
                else
                {
                    SceneViewShortcuts.DisableSkybox();
                }

                if (SceneViewSaveData.TryGetViewState(viewType, out var savedState))
                {
                    ApplyNewValues(savedState);
                }
                else
                {
                    ApplyDefaultValues(viewType);
                }
            }

            SceneViewRef.sceneView.Repaint();
        }

        public static void ApplyDefaultValues(SceneViewType viewType)
        {
            SceneViewRef.sceneView.size = DefaultValues.size;
            SceneViewRef.sceneView.pivot = DefaultValues.pivot;
            SceneViewRef.sceneView.rotation = SceneViewResetAll.GetDefaultRotation(viewType);
            SceneViewRef.sceneView.orthographic = SceneViewResetAll.GetDefaultOrthographic(viewType);
        }

        public static void ApplyNewValues(SceneViewSaveData.ViewState savedState)
        {
            SceneViewRef.sceneView.size = savedState.size;
            SceneViewRef.sceneView.pivot = savedState.pivot;
            SceneViewRef.sceneView.orthographic = savedState.orthographic;
            if (!SceneViewRef.sceneView.in2DMode)
            {
                SceneViewRef.sceneView.rotation = savedState.rotation;
            }
        }

        public static void SaveActiveSceneViewData(SceneViewType viewType)
        {
            if (SceneViewRef.sceneView != null)
            {
                SceneViewSaveData.SaveViewState(
                    SceneViewRef.SceneViewType,
                    SceneViewRef.sceneView.size,
                    SceneViewRef.sceneView.rotation,
                    SceneViewRef.sceneView.pivot,
                    SceneViewRef.sceneView.orthographic
                );
                SceneViewSaveData.SaveLastActiveSceneViewType(viewType);
            }
        }
    }
}
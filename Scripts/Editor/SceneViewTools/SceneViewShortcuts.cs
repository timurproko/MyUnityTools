using System;
using UnityEditor;
using System.Reflection;
using UnityEngine;

namespace MyTools.SceneViewTools
{
    static class SceneViewShortcuts
    {
        public static SceneViewType sceneViewType;

        [MenuItem("My Tools/Scene View Toolset/Log Camera Data", priority = 999)] //  Alt+H
        static void LogSceneViewCameraData()
        {
            SceneView sceneView = SceneView.lastActiveSceneView;
            Debug.Log(
                $"X:{sceneView.rotation.eulerAngles.x}, Y:{sceneView.rotation.eulerAngles.y}, Z:{sceneView.rotation.eulerAngles.z}");
        }

        [MenuItem("My Tools/Scene View Toolset/Reset View &h", priority = 100)] //  Alt+H
        static void ResetSceneViewCamera()
        {
            SceneView sceneView = SceneView.lastActiveSceneView;

            if (sceneView != null)
            {
                sceneView.pivot = Vector3.zero;
                sceneView.size = 10f;
                sceneView.Repaint();
            }
        }

        [MenuItem("My Tools/Scene View Toolset/Frame Selected &f", priority = 101)] //  Alt+F
        static void ResetSceneViewRotate()
        {
            SceneView.FrameLastActiveSceneView();
        }

        [MenuItem("My Tools/Scene View Toolset/Toggle Skybox &s", priority = 102)]
        static void ToggleSkybox()
        {
            SceneView sceneView = SceneView.lastActiveSceneView;
            if (sceneView.sceneViewState.skyboxEnabled)
            {
                DisableSkybox();
            }
            else
            {
                EnableSkybox();
            }
        }
        
        [MenuItem("My Tools/Scene View Toolset/Perspective &1", priority = 200)]
        static void PerspectiveView()
        {
            sceneViewType = SceneViewType.Perspective;
            EnableSkybox();
            SetPerspectiveView();
            SetPerspectiveView();
        }

        static void SetPerspectiveView()
        {
            SceneView sceneView = SceneView.lastActiveSceneView;

            if (sceneView != null)
            {
                sceneView.orthographic = false;
                sceneView.rotation = Quaternion.Euler(26.33425f, 225, 0f);
                sceneView.pivot = Vector3.zero;
                sceneView.Repaint();
            }
        }

        [MenuItem("My Tools/Scene View Toolset/Toggle Top-Bottom &2", priority = 201)]
        static void ToggleTopBottomView()
        {
            DisableSkybox();

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
            DisableSkybox();

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
            DisableSkybox();

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
            sceneViewType = SceneViewType.Top;
            DisableSkybox();
            SetTopView();
            SetTopView();
        }

        static void SetTopView()
        {
            SceneView sceneView = SceneView.lastActiveSceneView;

            if (sceneView != null)
            {
                sceneView.orthographic = true;
                sceneView.rotation = Quaternion.Euler(90f, 0f, 0f);
                sceneView.pivot = Vector3.zero;
                sceneView.Repaint();
            }
        }

        [MenuItem("My Tools/Scene View Toolset/Bottom", priority = 301)]
        static void BottomView()
        {
            sceneViewType = SceneViewType.Bottom;
            DisableSkybox();
            SetBottomView();
            SetBottomView();
        }

        static void SetBottomView()
        {
            SceneView sceneView = SceneView.lastActiveSceneView;

            if (sceneView != null)
            {
                sceneView.orthographic = true;
                sceneView.rotation = Quaternion.Euler(-90f, 0f, 0f);
                sceneView.pivot = Vector3.zero;
                sceneView.Repaint();
            }
        }

        [MenuItem("My Tools/Scene View Toolset/Front", priority = 302)]
        static void FrontView()
        {
            sceneViewType = SceneViewType.Front;
            DisableSkybox();
            SetFrontView();
            SetFrontView();
        }

        static void SetFrontView()
        {
            SceneView sceneView = SceneView.lastActiveSceneView;

            if (sceneView != null)
            {
                sceneView.orthographic = true;
                sceneView.rotation = Quaternion.Euler(0f, 180f, 0f);
                sceneView.pivot = Vector3.zero;
                sceneView.Repaint();
            }
        }

        [MenuItem("My Tools/Scene View Toolset/Back", priority = 303)]
        static void BackView()
        {
            sceneViewType = SceneViewType.Back;
            DisableSkybox();
            SetBackView();
            SetBackView();
        }

        static void SetBackView()
        {
            SceneView sceneView = SceneView.lastActiveSceneView;

            if (sceneView != null)
            {
                sceneView.orthographic = true;
                sceneView.rotation = Quaternion.Euler(0f, 0f, 0f);
                sceneView.pivot = Vector3.zero;
                sceneView.Repaint();
            }
        }

        [MenuItem("My Tools/Scene View Toolset/Right", priority = 304)]
        static void RightView()
        {
            sceneViewType = SceneViewType.Right;
            DisableSkybox();
            SetRightView();
            SetRightView();
        }

        static void SetRightView()
        {
            SceneView sceneView = SceneView.lastActiveSceneView;

            if (sceneView != null)
            {
                sceneView.orthographic = true;
                sceneView.rotation = Quaternion.Euler(0f, -90f, 0f);
                sceneView.pivot = Vector3.zero;
                sceneView.Repaint();
            }
        }

        [MenuItem("My Tools/Scene View Toolset/Left", priority = 305)]
        static void LeftView()
        {
            sceneViewType = SceneViewType.Left;
            DisableSkybox();
            SetLeftView();
            SetLeftView();
        }

        static void SetLeftView()
        {
            SceneView sceneView = SceneView.lastActiveSceneView;

            if (sceneView != null)
            {
                sceneView.orthographic = true;
                sceneView.rotation = Quaternion.Euler(0f, 90f, 0f);
                sceneView.pivot = Vector3.zero;
                sceneView.Repaint();
            }
        }

        static void DisableSkybox()
        {
            SceneView sceneView = SceneView.lastActiveSceneView;
            sceneView.sceneViewState.showSkybox = false;
        }

        static void EnableSkybox()
        {
            SceneView sceneView = SceneView.lastActiveSceneView;
            sceneView.sceneViewState.showSkybox = true;
        }
    }
}
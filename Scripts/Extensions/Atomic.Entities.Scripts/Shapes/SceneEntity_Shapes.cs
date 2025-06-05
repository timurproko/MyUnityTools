// #if SHAPES_URP && UNITY_EDITOR
// using UnityEngine;
// using UnityEngine.Rendering;
//
// namespace Atomic.Entities
// {
//     public partial class SceneEntity
//     {
//         private void OnEnable()
//         {
//             RenderPipelineManager.beginCameraRendering += HandleCameraRender;
//         }
//
//         private void OnDisable()
//         {
//             RenderPipelineManager.beginCameraRendering -= HandleCameraRender;
//         }
//
//         private void HandleCameraRender(ScriptableRenderContext context, Camera cam)
//         {
//             if (_entity == null) return;
//
//             for (int i = 0; i < _entity._behaviourCount; i++)
//             {
//                 if (_entity._behaviours[i] is IEntityShapes drawer)
//                     drawer.OnShapesDraw(cam, _entity);
//             }
//         }
//     }
// }
// #endif
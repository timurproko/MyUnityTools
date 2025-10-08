using UnityEngine;

namespace MyTools.Runtime
{
    [AddComponentMenu("My Tools/XR/" + "XR Settings")]

    public class XRSettings : MonoBehaviour
    {
        [SerializeField] private bool _disableOcclusionMesh = true;

        private void Start()
        {
            if (_disableOcclusionMesh)
            {
                UnityEngine.XR.XRSettings.useOcclusionMesh = false;
            }
        }
    }
}
using UnityEngine;
using UnityEngine.XR;

namespace MyTools.Runtime
{
    [AddComponentMenu("My Tools/XR/" + "Meta Tools")]

    public class MetaTools : MonoBehaviour
    {
        [SerializeField] private bool _disableOcclusionMesh = true;

        private void Start()
        {
            if (_disableOcclusionMesh)
            {
                XRSettings.useOcclusionMesh = false;
            }
        }
    }
}
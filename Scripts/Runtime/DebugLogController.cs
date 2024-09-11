using UnityEngine;

namespace MyTools.Runtime
{
    [AddComponentMenu("My Tools/Utility/" + nameof(DebugLogController))]
    public class DebugLogController : MonoBehaviour
    {
        [SerializeField] private bool _enableLogging = true;

        void Awake()
        {
            Debug.unityLogger.logEnabled = _enableLogging;
        }
    }
}
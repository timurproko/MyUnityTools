using UnityEngine;

namespace MyTools.Runtime
{
    [AddComponentMenu("My Tools/Debug/" + "Debug.Log Controller")]
    public class DebugLogController : MonoBehaviour
    {
        [SerializeField] private bool _enableLogging = true;

        void Awake()
        {
            Debug.unityLogger.logEnabled = _enableLogging;
        }
    }
}
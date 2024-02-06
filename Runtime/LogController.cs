using UnityEngine;
[AddComponentMenu("My Tools/Animation/" + nameof(LogController))]

namespace MyTools
{
    public class LogController : MonoBehaviour
    {
        [SerializeField] private bool _enableLogging = true;
        void Awake()
        {
            Debug.unityLogger.logEnabled = _enableLogging;
        }
    }
}
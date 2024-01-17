using UnityEngine;
[AddComponentMenu("--- My Tools ---/Utility/" + nameof(LogController))]

public class LogController : MonoBehaviour
{
    [SerializeField] private bool _enableLogging = true;
    void Awake()
    {
        Debug.unityLogger.logEnabled = _enableLogging;
    }
}
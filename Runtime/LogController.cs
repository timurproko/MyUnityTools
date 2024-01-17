using UnityEngine;
[AddComponentMenu("My Tools/Unitls/")]

public class LogController : MonoBehaviour
{
    [SerializeField] private bool _enableLogging = true;
    void Awake()
    {
        Debug.unityLogger.logEnabled = _enableLogging;
    }
}

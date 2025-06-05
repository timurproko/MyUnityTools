using UnityEngine;
using UnityEngine.UI;

namespace MyTools.Runtime
{
    [AddComponentMenu("My Tools/Debug/" + "Device Simulator UI Scaler")]
    public class DeviceSimulatorCanvasScaler : MonoBehaviour
    {
        GameObject _xrDeviceSimulator;
        Canvas _canvas;
        CanvasScaler _canvasScaler;

        [SerializeField] bool helpUI = true;
        [SerializeField, Range(0.4f, 1.0f)] float scaleFactor = 0.5f;

        void Start()
        {
            _xrDeviceSimulator = gameObject;
            _canvas = _xrDeviceSimulator.GetComponentInChildren<Canvas>();
            _canvasScaler = _xrDeviceSimulator.GetComponentInChildren<CanvasScaler>();
        }

        void Update()
        {
            if (_xrDeviceSimulator != null)
            {
                _canvas.enabled = helpUI;
                _canvas.pixelPerfect = true;
                _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                _canvasScaler.scaleFactor = scaleFactor;
            }
            else
            {
                Debug.LogError("MyTools: XR Device Simulator UI(Clone) not found.");
            }
        }
    }
}
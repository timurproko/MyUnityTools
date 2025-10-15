using Sirenix.OdinInspector;
using UnityEngine;

namespace MyTools
{
    public sealed class DebugController : MonoBehaviour
    {
        [InlineEditor(Expanded = true)]
        [SerializeField] private DebugConfig config;

        private void Awake()
        {
            Debug.SetConfig(config);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (config) config.RebuildLookup();
        }
#endif
    }
}
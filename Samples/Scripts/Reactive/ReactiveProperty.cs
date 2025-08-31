#if R3
using R3;
using UnityEngine;

namespace Reactive
{
    public class ReactiveProperty : MonoBehaviour
    {
        readonly CompositeDisposable _disposable = new();

        // Reactive Property
        private ReactiveProperty<bool> _isAlive;
        private ReactiveProperty<int> _score;
        private ReactiveProperty<float> _distance;
        private ReactiveProperty<string> _name;
        private ReactiveProperty<Vector3> _targetPos;

        // Serializable Reactive Property
        [SerializeField] private SerializableReactiveProperty<bool> _bool;
        [SerializeField] private SerializableReactiveProperty<int> _int;
        [SerializeField] private SerializableReactiveProperty<float> _float;
        [SerializeField] private SerializableReactiveProperty<string> _string;
        [SerializeField] private SerializableReactiveProperty<Vector3> _vector3;

        private void Start()
        {
            // Reactive Property
            // Subscribe
            _bool.Subscribe(_ => { Debug.Log("Bool is triggered"); }).AddTo(_disposable);
            // Change Value
            _bool.Value = true;
        }
    }
}
#endif
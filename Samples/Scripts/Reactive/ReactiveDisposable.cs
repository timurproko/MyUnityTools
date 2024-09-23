using R3;
using UnityEngine;

namespace Reactive
{
    public class ReactiveDisposable : MonoBehaviour
    {
        private readonly CompositeDisposable _disposable = new();
        
        [SerializeField] private SerializableReactiveProperty<int> _n = new(0);
        private ReactiveProperty<int> _p = new(0);

        private void OnEnable()
        {
            _n.Skip(1).Subscribe(_ =>
            {
                _p.Value += 2;
                Debug.Log(_p.Value.ToString());
            }).AddTo(_disposable);
        }

        private void OnDisable()
        {
            _disposable.Clear();
            _p.Value = 0;
        }
    }
}
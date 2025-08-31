#if R3
using R3;
using R3.Triggers;
using UnityEngine;

namespace Reactive
{
    public class ReactiveTriggers : MonoBehaviour
    {
        readonly CompositeDisposable _disposable = new();
        
        [SerializeField] private Collider _collider;
        private int n;

        private void Start()
        {
            _collider.OnCollisionEnterAsObservable()
                .Where(collision =>
                {
                    Renderer renderer = collision.gameObject.transform.Find("Geometry")?.GetComponent<MeshRenderer>();
                    return renderer != null && renderer.material != null && renderer.material.name == "Red (Instance)";
                })
                // .Take(3)
                .Skip(3)
                .Subscribe(_ =>
                {
                    n++;
                    Debug.Log($"Collider OnCollisionEnter: {n}");
                    if (n >= 6)
                    {
                        _disposable.Clear();
                        _collider.enabled = false;
                        WakeUpRB();
                    }
                }).AddTo(_disposable);
        }

        private void OtherTypes()
        {
            _collider.OnCollisionEnterAsObservable().Subscribe(_ => { }).AddTo(_disposable);
            _collider.OnCollisionExitAsObservable().Subscribe(_ => { }).AddTo(_disposable);
            _collider.OnCollisionStayAsObservable().Subscribe(_ => { }).AddTo(_disposable);

            _collider.OnTriggerEnterAsObservable().Subscribe(_ => { }).AddTo(_disposable);
            _collider.OnTriggerExitAsObservable().Subscribe(_ => { }).AddTo(_disposable);
            _collider.OnTriggerStayAsObservable().Subscribe(_ => { }).AddTo(_disposable);
        }

        private void WakeUpRB()
        {
            Rigidbody[] allRigidbodies = FindObjectsOfType<Rigidbody>();
            foreach (Rigidbody rb in allRigidbodies)
            {
                rb.WakeUp();
            }
        }
    }
}
#endif
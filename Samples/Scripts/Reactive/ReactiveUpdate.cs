using R3;
using UnityEngine;

namespace Reactive
{
    public class ReactiveUpdate : MonoBehaviour
    {
        readonly CompositeDisposable _disposable = new();
        
        private int x;
        private int y;
        private int z;
        private int totalValue = 10;

        private void Start()
        {
            reactiveUpdate();
        }

        private void reactiveUpdate()
        {
            Observable.EveryUpdate().Subscribe(_ =>
            {
                Debug.Log($"EveryUpdate in Action: {x}");
                x++;
                if (x >= totalValue)
                {
                    _disposable.Clear();
                    Debug.Log("EveryUpdate Finished");
                }
            }).AddTo(_disposable);

            // Unity's TimeProvider and FrameProvider
            Observable.EveryUpdate(UnityFrameProvider.FixedUpdate).Subscribe(_ =>
            {
                Debug.Log($"FixedUpdate in Action: {y}");
                y++;
                if (y >= totalValue)
                {
                    _disposable.Clear();
                    Debug.Log("FixedUpdate Finished");
                }
            }).AddTo(_disposable);

            Observable.EveryUpdate(UnityFrameProvider.Initialization).Subscribe(_ =>
            {
                Debug.Log($"Initialization in Action: {z}");
                z++;
                if (z >= totalValue)
                {
                    _disposable.Clear();
                    Debug.Log("Initialization Finished");
                }
            }).AddTo(_disposable);
        }
    }
}
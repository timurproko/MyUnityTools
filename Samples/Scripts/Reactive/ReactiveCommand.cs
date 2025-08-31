#if R3
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReactiveCommand : MonoBehaviour
{
    readonly CompositeDisposable _disposable = new();
    readonly CompositeDisposable _disposableForMove = new();

    // ReactiveCommands
    private ReactiveCommand<Unit> _command = new(); // Unit used for no value types
    private ReactiveCommand<bool> _boolCmd = new();
    private ReactiveCommand<int> _intCmd = new();
    private ReactiveCommand<float> _floatCmd = new();
    private ReactiveCommand<string> _stringCmd = new();
    private ReactiveCommand<Vector3> _vector3Cmd = new();

    // Example1
    private int _clickCounter;
    private ReactiveProperty<int> _n = new();
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _text;

    // Example2
    private ReactiveCommand<Transform> moveGameObject = new();
    [SerializeField] private Transform _gameObject;

    private void Start()
    {
        SubscribeExample();
        OnClickExample();
        MoveExample();
        _n
            .Where(_n => _n > 5)
            .Subscribe(_ => { moveGameObject.Execute(_gameObject); }).AddTo(_disposable);
    }

    private void MoveExample()
    {
        moveGameObject.Subscribe(_ => { MoveGameObject(_gameObject); }).AddTo(_disposable);
    }

    private void MoveGameObject(Transform transform)
    {
        float dist = transform.position.x + 10f;
        Observable.EveryUpdate().Subscribe(_ =>
        {
            transform.position = new Vector3(
                Mathf.Lerp(transform.position.x, dist, Time.deltaTime),
                transform.position.y,
                transform.position.x
            );
            if (Mathf.Approximately(transform.position.z, dist))
                _disposableForMove.Clear();
        }).AddTo(_disposableForMove);
    }

    private void OnClickExample()
    {
        _button.OnClickAsObservable()
            // .Where(_ => Random.value > 0.5f)     // Filter clicks randomly
            .Where(_ => ++_clickCounter % 2 == 0) // Only allow every other click
            .Subscribe(_ =>
            {
                _n.Value++;
                _text.text = $"Count: {_n.Value:00}";
            }).AddTo(_disposable);
    }

    private void SubscribeExample()
    {
        // Subscribe
        _boolCmd.Subscribe(_ => { Debug.Log("Command is executed"); }).AddTo(_disposable);
        // Invoke
        _boolCmd.Execute(true);
    }
}
#endif
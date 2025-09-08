#if VCONTAINER
using System;
using VContainer.Unity;

public sealed class CounterPresenter : IStartable, IDisposable
{
    private readonly ICounterService _service;
    private readonly CounterView _view;

    public CounterPresenter(ICounterService service, CounterView view)
    {
        _service = service;
        _view = view;
    }

    public void Start()
    {
        _view.Button.onClick.AddListener(OnClick);
        Refresh();
    }
    
    public void Dispose()
    {
        _view.Button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        _service.Increment();
        Refresh();
    }

    private void Refresh()
    {
        _view.Label.text = $"Count: {_service.Value}";
    }
}
#endif
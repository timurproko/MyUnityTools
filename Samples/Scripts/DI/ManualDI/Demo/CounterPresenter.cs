#if MANUAL_DI
using System;

public sealed class CounterPresenter : IDisposable
{
    private readonly ICounterService _service;
    private readonly CounterView _view;

    public CounterPresenter(ICounterService service, CounterView view)
    {
        _service = service;
        _view = view;
    }

    public void Initialize()
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
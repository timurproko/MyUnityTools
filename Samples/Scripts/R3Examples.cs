using System;
using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;
using UnityEngine;

// R3 Github https://github.com/Cysharp/R3
// Examples from https://www.youtube.com/watch?v=OhuUIdhM_6w
public class R3Examples
{
    // Example1
    // Field
    // On Subscribe you will get the current value
    private static ReactiveProperty<int> _health;

    public static void Example1()
    {
        _health = new ReactiveProperty<int>(200);
        Debug.Log($"Current Value: {_health.CurrentValue}");
        {
            _health.Subscribe(newValue => { Debug.Log($"Health: {newValue}"); });
        }
        _health.Value = 100;
        _health.Value = 90;
        _health.Value -= 50;
        _health.Value += 10;
        _health.OnNext(2); // Same as above, just different notation
    }

    // Example2
    // Property
    // On Subscribe you will get the current value
    private static readonly ReactiveProperty<int> _health2 = new();
    public static ReadOnlyReactiveProperty<int> Health2 => _health2;

    public static void Example2()
    {
        Debug.Log($"Current Value: {Health2.CurrentValue}");
        {
            Health2.Subscribe(newValue => { Debug.Log($"Health: {newValue}"); });
        }
        _health2.Value = 10;
        _health2.Value = 20;
        _health2.Value -= 45;
        _health2.Value += 30;
        _health2.OnNext(22);
    }

    // Example3
    // Observable
    // You can get value using Observable in case you don't need to change it
    private static readonly ReactiveProperty<int> _health3 = new();
    public static Observable<int> Health3 => _health3;

    public static void Example3()
    {
        // Debug.Log($"Current Value: {Health2.CurrentValue}"); // CurrentValue not allowed for Observables
        {
            Health3.Subscribe(newValue => { Debug.Log($"Health: {newValue}"); });
        }
        _health3.Value = 1;
        _health3.Value = 5;
        _health3.Value -= 25;
        _health3.Value += 10;
        _health3.OnNext(32);
    }

    // Example4
    // Subject
    // Analogue of Events you can Subscribe and use OnNext() and OnComplete() only
    private static readonly Subject<int> _health4 = new();
    public static Observable<int> Health4 => _health4;

    public static void Example4()
    {
        _health4.OnNext(33); // Will not do anything, Subject don't have value before subscribe
        {
            Health4.Subscribe(newValue => { Debug.Log($"Health: {newValue}"); });
        }
        _health4.OnNext(1);
        _health4.OnNext(5);
        _health4.OnNext(25);
        _health4.OnNext(10);
        _health4.OnNext(32);
    }

    // Example5
    // Dispose
    private static readonly ReactiveProperty<int> _health5 = new();
    public static Observable<int> Health5 => _health5;
    private static IDisposable _disposable5;

    public static void Example5()
    {
        _health5.Value = 1;
        {
            _disposable5 = Health5.Subscribe(newValue => { Debug.Log($"Health: {newValue}"); });
        }
        _health5.Value = 1000;
        {
            _disposable5.Dispose(); // after Dispose() you will no longer get updates
        }
        _health5.Value = 999;
    }

    // Example6
    // Bulk Dispose
    private static readonly ReactiveProperty<int> _health6 = new();
    private static readonly ReactiveProperty<int> _armor6 = new();
    public static Observable<int> Health6 => _health6;
    public static Observable<int> Armor6 => _armor6;
    private static readonly CompositeDisposable _compositeDisposable = new();

    public static void Example6()
    {
        _health6.Value = 1;
        _armor6.Value = 1;
        {
            var subscribtionHealth = Health6.Subscribe(newValue => { Debug.Log($"Health: {newValue}"); });
            var subscribtionArmor = Armor6.Subscribe(newValue => { Debug.Log($"Armor: {newValue}"); });
            _compositeDisposable.Add(subscribtionHealth);
            _compositeDisposable.Add(subscribtionArmor);
        }
        _health6.Value = 100;
        _armor6.Value = 100;
        {
            _compositeDisposable.Dispose();
        }
        _health6.Value = 999;
        _armor6.Value = 999;
    }

    // Example7
    // Filter Events
    private static readonly ReactiveProperty<int> _health7 = new();
    public static Observable<int> Health7 => _health7;

    public static void Example7()
    {
        {
            Health7.Where(v => v > 40).Subscribe(_ => { Debug.Log($"Health: {_health7.CurrentValue}"); });
        }
        _health7.Value = 11;
        _health7.Value = 13;
        _health7.Value = 27;
        _health7.Value = 28;
        _health7.Value = 48;
        _health7.Value = 58;
    }

    // Example8
    // Merge Events
    private static readonly ReactiveProperty<int> _health8 = new();
    private static readonly ReactiveProperty<int> _armor8 = new();
    public static Observable<int> Health8 => _health8;
    public static Observable<int> Armor8 => _armor8;

    public static void Example8()
    {
        {
            Health8.Merge(Armor8).Subscribe(_ =>
            {
                Debug.Log($"Health: {_health8.CurrentValue}, Armor: {_armor8.CurrentValue}");
            });
        }
        _health8.Value = 11;
        _armor8.Value = 9;
        _armor8.Value = 0;
        _armor8.Value = 10;
        _health8.Value = 99;
        _health8.Value = 0;
    }

    // Example9
    // Observable from Action
    private static event Action<int> ValueChanged;

    public static void Example9()
    {
        Observable.FromEvent<int>(a => ValueChanged += a, a => ValueChanged -= a)
            .Subscribe(v => { Debug.Log($"Value: {v}"); });

        ValueChanged?.Invoke(10);
        ValueChanged?.Invoke(20);
    }

    // Example10
    // Collections
    private static readonly ObservableList<string> _observableCollection = new();
    public static IObservableCollection<string> ObservableCollection => _observableCollection;

    public static void Example10()
    {
        // Add elements before subscription
        _observableCollection.Add("Element1");
        _observableCollection.Add("Element2");

        ObservableCollection.ObserveAdd().Subscribe(e => { Debug.Log($"Element Added: {e.Value}"); });

        ObservableCollection.ObserveRemove().Subscribe(e => { Debug.Log($"Element Removed: {e.Value}"); });

        _observableCollection.Add("Element3");
        _observableCollection.Add("Element4");
        _observableCollection.Remove("Element4");
        _observableCollection.Add("Element5");

        // Show all elements in the Collection
        foreach (var item in ObservableCollection)
        {
            Debug.Log(new string('=', 50));
            Debug.Log(item);
        }
    }

    // Example11
    // Subscription with Deferred Execution
    private static readonly ReactiveProperty<int> _health9 = new();
    public static Observable<int> Health9 => _health9;

    public static void Example11()
    {
        List<string> texts = new();
        for (int i = 0; i < 10; i++)
        {
            texts.Add($"Step:{i}");
        }
        
        // Prepare Deferred Functions
        var deferredPrinting = texts.Select((text, i) => Observable.Defer(() => Print(text)));
        
        // Create Queue for Deferred Functions
        var queuedPrinting = Observable.Concat(deferredPrinting);
        
        // Take Last Action
        var lastPrinting = queuedPrinting.TakeLast(1);
        
        // Subscribe to Last Action
        lastPrinting.Subscribe(_ =>
        {
            Debug.Log("All elements is over");
        });
    }

    // If you need to return void you should use <Unit> and return Observable
    // If you want to have blank return use: return Observable.Return(Unit.Default);
    private static Observable<Unit> Print(string text)
    {
        Debug.Log(text);
        return Observable.Timer(TimeSpan.FromSeconds(1), UnityTimeProvider.EarlyUpdateIgnoreTimeScale);
    }
}
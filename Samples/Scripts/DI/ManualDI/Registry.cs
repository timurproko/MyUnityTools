#if MANUAL_DI
using System;
using System.Collections.Generic;
using ManualDi.Sync;

public static class Registry
{
    private static readonly List<Action<DiContainerBindings>> _registry = new(32);

    public static void Register(Action<DiContainerBindings> installer)
    {
        if (installer != null) _registry.Add(installer);
    }

    public static void Register(IInstaller installer)
    {
        if (installer != null) _registry.Add(installer.Install);
    }

    public static void Unregister(Action<DiContainerBindings> installer)
    {
        if (installer != null) _registry.Remove(installer);
    }

    public static void Unregister(IInstaller installer)
    {
        if (installer != null) _registry.Remove(installer.Install);
    }

    public static void Bind(DiContainerBindings bindings)
    {
        var snapshot = _registry.ToArray();
        foreach (var a in snapshot) a(bindings);
    }
}
#endif
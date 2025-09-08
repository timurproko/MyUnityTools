#if VCONTAINER
using System;
using System.Collections.Generic;
using VContainer;

public static class Registry
{
    private static readonly List<Action<IContainerBuilder>> _items = new(32);

    public static void Register(Action<IContainerBuilder> installer)
    {
        if (installer != null) _items.Add(installer);
    }

    public static void Unregister(Action<IContainerBuilder> installer)
    {
        if (installer != null) _items.Remove(installer);
    }

    public static void Bind(IContainerBuilder builder)
    {
        var snapshot = _items.ToArray();
        foreach (var i in snapshot) i(builder);
    }
}
#endif
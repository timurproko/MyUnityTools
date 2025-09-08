#if MANUAL_DI
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

public static class AutoRegister
{
    static bool _initialized;
    static readonly HashSet<Type> _seen = new();

    public static void Register(Assembly targetAssembly)
    {
        if (_initialized || targetAssembly == null) return;
        _initialized = true;

        var baseType = typeof(PlainInstaller);
        var attrType = typeof(PlainInstaller.AutoInstallAttribute);

        TypeInfo[] typeInfos;

        try
        {
            var list = new List<TypeInfo>(128);
            foreach (var ti in targetAssembly.DefinedTypes)
                list.Add(ti);
            typeInfos = list.ToArray();
        }
        catch (ReflectionTypeLoadException ex)
        {
            var src = ex.Types;
            var list = new List<TypeInfo>(src?.Length ?? 0);
            if (src != null)
            {
                for (int i = 0; i < src.Length; i++)
                {
                    var t = src[i];
                    if (t != null) list.Add(t.GetTypeInfo());
                }
            }

            typeInfos = list.ToArray();
        }

        if (typeInfos == null || typeInfos.Length == 0) return;

        for (int i = 0; i < typeInfos.Length; i++)
        {
            var ti = typeInfos[i];
            if (ti == null) continue;

            if (ti.IsAbstract || ti.IsInterface) continue;

            var t = ti.AsType();
            if (!baseType.IsAssignableFrom(t)) continue;
            if (!ti.IsDefined(attrType, inherit: true)) continue;
            if (!_seen.Add(t)) continue;

            var ctor = t.GetConstructor(Type.EmptyTypes);
            if (ctor == null) continue;

            var factory = Expression.Lambda<Func<PlainInstaller>>(Expression.New(ctor)).Compile();

            Registry.Register(b => factory().Install(b));
        }
    }
}
#endif
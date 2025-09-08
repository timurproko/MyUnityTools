#if MANUAL_DI
using System;
using ManualDi.Sync;
using UnityEngine.Scripting;

[Preserve]
[AutoInstall]
public abstract class PlainInstaller
{
    public abstract void Install(DiContainerBindings c);

    public static void Register<T>() where T : PlainInstaller, new()
    {
        Registry.Register(bindings => new T().Install(bindings));
    }
    
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AutoInstallAttribute : Attribute { }
}
#endif

#if VCONTAINER
using System;
using VContainer;

[AutoInstall]
public abstract class PlainInstaller
{
    public abstract void Install(IContainerBuilder builder);

   [AttributeUsage(AttributeTargets.Class)]
    public sealed class AutoInstallAttribute : Attribute { }
}
#endif
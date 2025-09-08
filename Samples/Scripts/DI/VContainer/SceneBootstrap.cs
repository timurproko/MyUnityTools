#if VCONTAINER
using VContainer;
using VContainer.Unity;

public sealed class SceneBootstrap : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Install(typeof(SceneBootstrap));
        
        Registry.Bind(builder);
    }
}
#endif
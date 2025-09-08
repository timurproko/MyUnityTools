#if VCONTAINER
using VContainer;
using VContainer.Unity;

public class CounterInstaller : PlainInstaller
{
    public override void Install(IContainerBuilder builder)
    {
        builder.Register<CounterService>(Lifetime.Singleton).As<ICounterService>();
        builder.RegisterEntryPoint<CounterPresenter>();
    }
}
#endif
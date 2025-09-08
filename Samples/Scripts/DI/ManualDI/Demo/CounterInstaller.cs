#if MANUAL_DI
using ManualDi.Sync;

public sealed class CounterInstaller : PlainInstaller
{
    public override void Install(DiContainerBindings b)
    {
        b.Bind<ICounterService, CounterService>().Default().FromConstructor();
        b.Bind<CounterPresenter>().Default().FromConstructor();
    }
}
#endif
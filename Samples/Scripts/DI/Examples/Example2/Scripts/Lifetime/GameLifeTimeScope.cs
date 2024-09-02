using VContainer;
using VContainer.Unity;

namespace Example2
{
    public class GameLifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<KeyboardInput>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<PlayerMovement>();
        }
    }
}
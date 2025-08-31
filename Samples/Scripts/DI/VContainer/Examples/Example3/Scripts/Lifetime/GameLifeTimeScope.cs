#if VCONTAINER
using VContainer;
using VContainer.Unity;

namespace Example3
{
    public class GameLifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<KeyboardInput>(Lifetime.Scoped);
            //Difference from the video:
            //There is no need of registering the IObjectResolver, it is already in the container
        }
    }
}
#endif
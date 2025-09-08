#if VCONTAINER
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    // Invoke Registration
    protected override void Configure(IContainerBuilder builder)
    {
        RegisterSomeService(builder);
    }

    // Register SomeService Example
    private void RegisterSomeService(IContainerBuilder builder)
    {
        builder.Register<Logger>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.RegisterComponentInHierarchy<InjectExample>();
        builder.RegisterEntryPoint<GameEntryPoint>();
    }
    
    // Register Entry Points
    // On Launch Entry Point will be passed to VContainer loop system
    // Entry points used as alternative to MonoBehaviour(MB) loop system for non MB Classes
    // builder.RegisterEntryPoint<GameEntryPoint>();

    // To Pass multiple entry points use this notation
    // builder.UseEntryPoints(points =>
    // {
    //     points.Add<GameEntryPoint>();
    //     points.Add<GameEntryPoint>();
    // });


    // Register Services and Components
    // Class
    // builder.Register<SomeService>(Lifetime.Singleton);  // Single Instance for every Injection
    // builder.Register<SomeService>(Lifetime.Transient);  // Instance for every Injection
    // builder.Register<SomeService>(Lifetime.Scoped);     // Container will decide how to Inject

    // Interface
    // builder.Register<ISomeService, SomeService>(Lifetime.Singleton);
    // builder.Register<SomeService>(Lifetime.Singleton).As<ISomeService>(); // Alternative notation

    // Multiple Interfaces
    // builder.Register<SomeService>(Lifetime.Singleton).As<ISomeService1, ISomeService2>();
    // builder.Register<SomeService>(Lifetime.Singleton).AsImplementedInterfaces(); // Will utilize all interfaces

    // As Self
    // builder.Register<SomeService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();


    // Register Instance
    // var someService = new SomeService();
    // builder.RegisterInstance(someService);

    // Register Instance with Interface
    // builder.RegisterInstance<ISomeService>(someService);

    // Register Instance with Multiple Interfaces
    // builder.RegisterInstance(someService).As<ISomeService1, ISomeService2>();
    // builder.RegisterInstance(someService).AsImplementedInterfaces();


    // Register MonoBehaviour
    // MonoBehaviour Class
    // builder.RegisterComponent(SomeMonoBehaviourClass);

    // MonoBehaviour on Scene
    // builder.RegisterComponentInHierarchy<SomeMonoBehaviourClassOnScene>();
}
#endif
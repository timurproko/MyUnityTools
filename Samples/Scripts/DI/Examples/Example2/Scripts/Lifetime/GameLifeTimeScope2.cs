using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Example2
{
    public class GameLifeTimeScope2 : LifetimeScope
    {
        [SerializeField] private PlayerMovement _playerMovement;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<KeyboardInput>(Lifetime.Scoped);
            builder.RegisterComponent(_playerMovement);
        }
    }
}
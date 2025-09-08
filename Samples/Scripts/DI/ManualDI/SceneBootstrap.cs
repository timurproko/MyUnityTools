#if MANUAL_DI
using ManualDi.Sync;
using UnityEngine;

public sealed class SceneBootstrap : MonoBehaviour
{
    private IDiContainer _scene;

    private void Start()
    {
        var bindings = new DiContainerBindings();

        AutoRegister.Register(typeof(SceneBootstrap).Assembly);

        Registry.Bind(bindings);

        _scene = bindings.Build();
    }

    private void OnDestroy()
    {
        _scene?.Dispose();
        _scene = null;
    }
}
#endif
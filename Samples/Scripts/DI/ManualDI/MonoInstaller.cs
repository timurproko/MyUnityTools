#if MANUAL_DI
using UnityEngine;
using ManualDi.Sync;

public abstract class MonoInstaller : MonoBehaviour, IInstaller
{
    public abstract void Install(DiContainerBindings b);

    protected virtual void Awake()
    {
        Registry.Register(this);
    }

    protected virtual void OnDestroy()
    {
        Registry.Unregister(this);
    }
}
#endif
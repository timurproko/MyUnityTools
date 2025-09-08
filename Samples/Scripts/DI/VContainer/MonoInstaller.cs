#if VCONTAINER
using UnityEngine;
using VContainer;

[DefaultExecutionOrder(-10000)]
public abstract class MonoInstaller : MonoBehaviour
{
    public abstract void Install(IContainerBuilder builder);

    protected virtual void Awake()
    {
        Registry.Register(Install);
    }

    protected virtual void OnDestroy()
    {
        Registry.Unregister(Install);
    }
}
#endif
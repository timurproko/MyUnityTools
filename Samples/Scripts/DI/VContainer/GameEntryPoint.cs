using UnityEngine;
using VContainer.Unity;

public class GameEntryPoint : ITickable, IStartable
{
    public void Tick()
    {
    }

    public void Start()
    {
        Debug.Log("VContainer Start() has worked");
    }
}
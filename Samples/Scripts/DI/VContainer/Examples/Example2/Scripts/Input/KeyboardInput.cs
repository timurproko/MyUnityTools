#if VCONTAINER
using System;
using UnityEngine;
using VContainer.Unity;

namespace Example2
{
    public class KeyboardInput : IInput, ITickable
    {
        public event Action<MovementDirection> InputEvent;

        public void Tick()
        {
            if (Input.GetKey(KeyCode.W))
                InputEvent?.Invoke(MovementDirection.Up);
            
            if (Input.GetKey(KeyCode.S))
                InputEvent?.Invoke(MovementDirection.Down);
            
            if (Input.GetKey(KeyCode.D))
                InputEvent?.Invoke(MovementDirection.Right);
            
            if (Input.GetKey(KeyCode.A))
                InputEvent?.Invoke(MovementDirection.Left);
        }
    }
}
#endif
#if VCONTAINER
using System;
using UnityEngine;
using VContainer.Unity;

namespace Example3
{
    public class KeyboardInput : IInput, ITickable
    {
        public event Action<MovementDirection> InputEvent;

        void ITickable.Tick()
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
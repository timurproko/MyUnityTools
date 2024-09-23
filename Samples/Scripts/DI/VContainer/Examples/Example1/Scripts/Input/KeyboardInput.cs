using System;
using UnityEngine;

namespace Example1
{
    public class KeyboardInput : MonoBehaviour, IInput
    {
        public event Action<MovementDirection> InputEvent;

        private void Update()
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
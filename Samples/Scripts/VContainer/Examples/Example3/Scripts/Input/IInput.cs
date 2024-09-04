using System;
using Unity.VisualScripting;

namespace Example3
{
    public interface IInput
    {
        event Action<MovementDirection> InputEvent;
    }
}
using System;

namespace Example2
{
    public interface IInput
    {
        event Action<MovementDirection> InputEvent;
    }
}
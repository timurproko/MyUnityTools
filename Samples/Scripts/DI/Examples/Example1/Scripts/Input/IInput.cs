using System;

namespace Example1
{
    public interface IInput
    {
        event Action<MovementDirection> InputEvent;
    }
}
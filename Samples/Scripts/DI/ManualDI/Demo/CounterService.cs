#if MANUAL_DI
public interface ICounterService
{
    int Value { get; }
    void Increment();
}

public sealed class CounterService : ICounterService
{
    public int Value { get; private set; }
    
    public void Increment()
    {
        Value++;
    }
}
#endif
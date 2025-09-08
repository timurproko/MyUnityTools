#if VCONTAINER
using UnityEngine;

public interface ILogger
{
    public void PrintLog();
}
public interface IMessanger
{
    public void PrintMessage();
}

public class Logger : ILogger, IMessanger
{
    public void PrintMessage()
    {
        Debug.Log("Hello VContainer Message");
    }

    public void PrintLog()
    {
        Debug.Log("Hello VContainer Log");
    }
}

public class Logger2 : ILogger
{
    public void PrintLog()
    {
        Debug.Log("Hello VContainer Log2");
    }
}
#endif
using UnityEngine;
using VContainer;

public class InjectExample : MonoBehaviour
{
    // Injection Types
    // Field & Property Injection
    [Inject] private ILogger _logger;
    [Inject] private ILogger Logger { get; set; }

    // // Constructor Injection
    public InjectExample(ILogger logger)
    {
        _logger = logger;
    }
    
    // Method Injection
    [Inject]
    private void Construct(ILogger logger)
    {
        _logger = logger;
    }

    private void Start()
    {
        _logger.PrintLog();
    }
}
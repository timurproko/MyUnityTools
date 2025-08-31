#if UNITASK
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class UniTaskExamples
{
    // Example1
    // Basic Delay
    public static async void Example1()
    {
        Debug.Log("Start Delay");
        await UniTask.Delay(2000); // Delay for 2 seconds
        Debug.Log("End Delay");
    }

    
    // Example2
    // Awaiting Multiple Tasks
    public static async void Example2()
    {
        var task1 = Task1();
        var task2 = Task2();
        await UniTask.WhenAll(task1, task2);
        Debug.Log("Both Tasks Completed");
    }

    private static async UniTask Task1()
    {
        await UniTask.Delay(1000);
        Debug.Log("Task1 Completed");
    }

    private static async UniTask Task2()
    {
        await UniTask.Delay(2000);
        Debug.Log("Task2 Completed");
    }
    
    
    // Example3
    // Chaining Tasks
    public static async void Example3()
    {
        await FirstTask();
        await SecondTask();
        Debug.Log("Chained tasks completed");
    }

    private static async UniTask FirstTask()
    {
        await UniTask.Delay(2000);
        Debug.Log("First Task completed");
    }

    private static async UniTask SecondTask()
    {
        await UniTask.Delay(1000);
        Debug.Log("Second Task completed");
    }
    
    
    // Example4
    // Returning Values
    public static async void Example4()
    {
        int result = await GetNumberAfterDelay();
        Debug.Log($"Result: {result}");
    }

    private static async UniTask<int> GetNumberAfterDelay()
    {
        await UniTask.Delay(2000);
        return 42;
    }
    
    
    // Example5
    // Using await foreach with IAsyncEnumerable
    public static async void Example5()
    {
        await foreach (var number in GenerateNumbersAsync())
        {
            Debug.Log(number);
        }
    }

    private static async IAsyncEnumerable<int> GenerateNumbersAsync()
    {
        for (int i = 0; i < 5; i++)
        {
            await UniTask.Delay(500);
            yield return i;
        }
    }
    
    
    // Example6
    // Handling Exceptions
    public static async void Example6()
    {
        try
        {
            await DoSomethingThatFails();
        }
        catch (System.Exception ex)
        {
            Debug.Log($"Caught exception: {ex.Message}");
        }
    }

    private static async UniTask DoSomethingThatFails()
    {
        await UniTask.Delay(1000);
        throw new System.Exception("Something went wrong!");
    }
    
    
    // Example7
    // Using UniTask with Coroutines
    public static async void Example7()
    {
        await MyCoroutine().ToUniTask();
        Debug.Log("Coroutine completed");
    }

    private static IEnumerator MyCoroutine()
    {
        yield return new WaitForSeconds(2);
    }
    
    
    // Example8
    // Running Tasks on a Background Thread
    public static async void Example8()
    {
        int result = await CalculateOnBackgroundThread();
        Debug.Log($"Calculation result: {result}");
    }

    private static async UniTask<int> CalculateOnBackgroundThread()
    {
        await UniTask.SwitchToThreadPool(); // Switch to background thread
        int result = LongCalculation();
        await UniTask.SwitchToMainThread(); // Switch back to the main thread
        return result;
    }

    private static int LongCalculation()
    {
        // Simulate a long calculation
        System.Threading.Thread.Sleep(1000);
        return 100;
    }
    
    
    // Example9
    // Working with Async Events
    public static event System.Action<int> OnNumberGenerated;

    public static async void Example9()
    {
        OnNumberGenerated += HandleNumberGenerated;
        await GenerateNumbers();
    }

    private static async UniTask GenerateNumbers()
    {
        for (int i = 0; i < 5; i++)
        {
            await UniTask.Delay(500);
            OnNumberGenerated?.Invoke(i);
        }
    }

    private static void HandleNumberGenerated(int number)
    {
        Debug.Log($"Generated number: {number}");
    }
    
    
    // Example10
    // Handling Multiple Asynchronous Animations
    public static async void Example10()
    {
        await PlayAnimation("Rotate");
        await PlayAnimation("MoveUp");
        await StopPlayback();
    }

    private static async UniTask StopPlayback()
    {
        Test.animator.enabled = false;
    }

    private static async UniTask PlayAnimation(string animationName)
    {
        Test.animator.Play(animationName);
        await UniTask.Delay(1000); // Assume each animation lasts 1 second
    }
    
    
    // Example11
    // Asynchronous Networking
    public static async void Example11()
    {
        string response = await FetchDataFromServer("https://gbfs.citibikenyc.com/gbfs/en/station_information.json");
        Debug.Log(response);
    }

    private static async UniTask<string> FetchDataFromServer(string url)
    {
        using HttpClient client = new HttpClient();
        return await client.GetStringAsync(url);
    }
    
    
    // Example12
    // Asynchronous Scene Loading with Progress
    public static async void Example12()
    {
        await LoadSceneAsync("SampleScene");
    }

    private static async UniTask LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            Debug.Log($"{operation.progress * 100}%");
            await UniTask.Yield();
        }
    }
    
    
    // Example13
    // Asynchronous Scene Loading with Progress
    public static async void Example13()
    {
        await WaitForKeyPress(KeyCode.Space);
        Debug.Log("Space key pressed");
    }

    private static async UniTask WaitForKeyPress(KeyCode key)
    {
        while (!Input.GetKeyDown(key))
        {
            await UniTask.Yield();
        }
    }
}
#endif
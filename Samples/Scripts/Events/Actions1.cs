using System;
using UnityEngine;

// Invoke Action and Action with params using Lambda
public class Actions1 : MonoBehaviour
{
    private Action action;
    private Action lambdaWithParam;
    private void Awake()
    {
        action += SayHello;
        
        // To be able unsubscribe lambda it need to be stored in a separate delegate
        lambdaWithParam = () => SaySomething("Buy buy");
        action += lambdaWithParam;
        action -= lambdaWithParam;
    }

    private void Start()
    {
        action?.Invoke();
    }

    private void SayHello()
    {
        Debug.Log("SayHello");
    }

    private void SaySomething(string text)
    {
        Debug.Log(text);
    }
}
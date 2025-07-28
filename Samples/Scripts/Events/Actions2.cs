using System;
using System.Collections.Generic;
using UnityEngine;

// Iterate on actions using List
public class Actions2 : MonoBehaviour
{
    private List<Action> actions = new();
    private Action lambdaWithParam;

    private void Awake()
    {
        actions.Add(SayHello);

        lambdaWithParam = () => SaySomething("Buy buy");
        actions.Add(lambdaWithParam);
        actions.Remove(lambdaWithParam);
    }

    private void Start()
    {
        InvokeActions();
    }
    
    private void InvokeActions()
    {
        foreach (Action action in actions)
        {
            action?.Invoke();
        }
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
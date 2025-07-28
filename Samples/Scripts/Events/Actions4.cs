using System;
using UnityEngine;

// Invoke different Actions on user Input
public class Actions4 : MonoBehaviour
{
    private Action action;
    private Action[] actions;
    private int index;


    private void Start()
    {
        actions = new Action[]
        {
            FireGun,
            FireKnife
        };
        index = 0;
        action = actions[index];
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            action?.Invoke();
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            index = (index + 1) % actions.Length;
            action = actions[index];
        }
    }

    private void FireGun()
    {
        Debug.Log("I have used a Gun");
    }
    
    private void FireKnife()
    {
        Debug.Log("I have used a Knife");
    }
}
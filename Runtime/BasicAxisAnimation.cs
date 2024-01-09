using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAxisAnimation : MonoBehaviour
{
    private Transform MyTransform = null;
    [Header("Axis")]
    // Customize these values to control the animation:
    [SerializeField] bool x_axis = false; 
    [SerializeField] bool y_axis = false; 
    [SerializeField] bool z_axis = false; 
    [Header("Animation")]
    [SerializeField] float speed = 1f;  // How fast the object moves
    [SerializeField] float amplitude = 0.1f;  // How far the object moves horizontally
    [SerializeField] float frequency = 20f;  // How quickly the object oscillates


    // Start is called before the first frame update
    void Start()
    {
        MyTransform = GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.time * speed;
        Vector3 newPosition = MyTransform.position;

        if (x_axis)
            newPosition.x = amplitude * Mathf.Sin(time * frequency);
        if (y_axis)
            newPosition.y = amplitude * Mathf.Sin(time * frequency);
        if (z_axis)
            newPosition.z = amplitude * Mathf.Sin(time * frequency);

        MyTransform.position = newPosition;
    }
}
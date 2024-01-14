using UnityEngine;

public class AnimateOrientation : MonoBehaviour
{
    private Transform MyTransform = null;
    [SerializeField] Vector3 rotation = new Vector3(0f, 0f, 0f);
    [SerializeField, Range(0.0f, 100.0f)] float speed = 0;
    [SerializeField] bool randomize = false;
    [SerializeField] bool setSeed = false;
    [SerializeField] int seed = 0;
    int initialSeed;
    float initialRandom;
    float random;

    void Start()
    {
        MyTransform = GetComponent<Transform>();
        initialSeed = (int)Random.Range(0.0f, 9999.0f);
        initialRandom = Random.Range(0.0f, 360.0f);

        // Always initialize with initialSeed on Start
        Random.InitState(initialSeed);
    }

    void Update()
    {
        if (setSeed)
        {
            // If setSeed is true, use the provided seed
            Random.InitState(seed);
            random = Random.Range(0.0f, 360.0f);
        }
        else
        {
            // If setSeed is false, use the initial seed
            Random.InitState(initialSeed);
            random = randomize ? initialRandom : 0.0f;
        }

        float time = Time.time;
        Vector3 currentRotation = rotation * time * speed;

        if (randomize && !setSeed)
        {
            // Add initialRandom only when randomize is true and setSeed is false
            currentRotation += new Vector3(random, random, random);
        }

        MyTransform.rotation = Quaternion.Euler(currentRotation);
    }
}
using UnityEngine;

namespace MyTools
{
[AddComponentMenu("My Tools/Animation/" + nameof(AnimateTransforms))]
    public class AnimateOrientation : MonoBehaviour
    {
        private Transform MyTransform = null;
        [SerializeField] Vector3 rotation = new Vector3(0f,0f,0f);
        [SerializeField, Range(0.0f, 100.0f)] float speed = 0;
        [SerializeField] bool randomize = false;
        [SerializeField] bool setSeed = false;
        [SerializeField] int seed = 0;
        int initialSeed;
        float initialRandom;
        float random;
        Random.State stateOnStart;

        void Start()
        {
            MyTransform = GetComponent<Transform>();
            initialSeed = (int)Random.Range(0.0f, 9999.0f);
            initialRandom = Random.Range(0.0f, 360.0f);
            Random.InitState(initialSeed);
        }

        void Update()
        {
            if (setSeed){
                Random.InitState(seed);
                random = Random.Range(0.0f, 360.0f);
            }
            
            Random.InitState(initialSeed);
            float time = Time.time;
            Vector3 currentRotation = rotation * time * speed;

            if (randomize){
                if (setSeed){
                    currentRotation += new Vector3(random, random, random);
                }
            currentRotation += new Vector3(initialRandom, initialRandom, initialRandom);
            }
            
            MyTransform.rotation = Quaternion.Euler(currentRotation);
        }
    }
}
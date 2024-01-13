using UnityEngine;

public class AnimateOrientation : MonoBehaviour
{
    private Transform MyTransform = null;
    [SerializeField] Vector3 rotation = new Vector3(0f,0f,0f);
    [SerializeField] float speed = 0;

    void Start()
    {
        MyTransform = GetComponent<Transform>();
    }

    void Update()
    {
        float time = Time.time * speed;
        MyTransform.rotation = Quaternion.Euler(rotation * time);
    }
}

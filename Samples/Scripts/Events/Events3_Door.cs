using System.Collections;
using UnityEngine;

// Events for Objects
public class Events3_Door : MonoBehaviour
{
    private bool isOpen;
    private Coroutine openCoroutine;

    public void _Open()
    {
        if (!isOpen)
        {
            isOpen = true;
            if (openCoroutine != null)
            {
                StopCoroutine(openCoroutine);
            }
            openCoroutine = StartCoroutine(OpenCoroutine());
        }
    }

    private IEnumerator OpenCoroutine()
    {
        Transform cube = transform;
        Vector3 targetPosition = Vector3.up * 2;

        while (Vector3.Distance(cube.localPosition, targetPosition) > 0.01f)
        {
            cube.localPosition = Vector3.MoveTowards(cube.localPosition, targetPosition, Time.deltaTime * 2);
            yield return null;
        }

        cube.localPosition = targetPosition;
    }
}
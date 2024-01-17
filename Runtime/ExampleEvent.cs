using UnityEngine;
using UnityEngine.Events;
[AddComponentMenu("My Tools/Examples/" + nameof(ExampleEvent))]

public class ExampleEvent: MonoBehaviour
{
	public UnityEvent EventName;

	private void Start()
	{
		EventName.Invoke();
	}
}
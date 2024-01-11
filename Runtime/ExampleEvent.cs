using UnityEngine;
using UnityEngine.Events;

public class ExampleEvent: MonoBehaviour
{
	public UnityEvent EventName;

	private void Start()
	{
		EventName.Invoke();
	}
}
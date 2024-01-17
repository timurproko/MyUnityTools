using UnityEngine;
[AddComponentMenu("--- My Tools ---/Examples/" + nameof(ExampleEventListener))]

public class ExampleEventListener: MonoBehaviour
{
	[SerializeField]
	private ExampleEvent RefToObjectWithEvent;
	
	// Assigned to Awake() to make it work with EventsExample class Start()
	private void Awake()
	{
	RefToObjectWithEvent.EventName.AddListener(HandleEvent);	
	}

	private void HandleEvent()
	{
		Debug.Log("Event triggered!");
	}
}
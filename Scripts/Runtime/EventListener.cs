using UnityEngine;

namespace MyTools.Runtime
{
[AddComponentMenu("My Tools/Examples/" + nameof(EventListener))]
	public class EventListener: MonoBehaviour
	{
		[SerializeField]
		private Event RefToObjectWithEvent;
		
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
}
using UnityEngine;
using UnityEngine.Events;

namespace MyTools.Components
{
[AddComponentMenu("My Tools/Examples/" + nameof(Event))]
	public class Event: MonoBehaviour
	{
		public UnityEvent EventName;

		private void Start()
		{
			EventName.Invoke();
		}
	}
}
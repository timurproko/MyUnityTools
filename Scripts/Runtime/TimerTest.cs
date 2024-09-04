using UnityEngine;

namespace MyTools.Components
{
[AddComponentMenu("My Tools/Time/" + nameof(TimerTest))]
    public class TimerTest : MonoBehaviour
    {
        private Timer _timer;
        private float _startTime;
        [SerializeField] private float _timerDuration = 1.0f;
        
        private void Start()
        {
            _timer = gameObject.AddComponent<Timer>();
            _timer.Duration = _timerDuration;
            _timer.Run();
            _startTime = Time.time;
        }
        
        private void Update()
        {
            if (_timer.Finished)
            {
                float elapsedTime = Time.time - _startTime;
                Debug.Log("Timer ran for " + elapsedTime + "  seconds.");

                _startTime = Time.time;
                _timer.Run();
            }
        }
    }
}
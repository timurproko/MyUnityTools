using UnityEngine;

namespace MyTools.Components
{
[AddComponentMenu("My Tools/Time/" + nameof(Timer))]
    public class Timer : MonoBehaviour
    {
        #region Fields
        
        float _totalSeconds, _elapsedSeconds;
        bool _started , _running;
        
        public float Duration
        {
            set
            {
                if (!_running)
                {
                    _totalSeconds = value;
                }
            }
        }        
        
        public bool Finished
        {
            get
            {
                return _started && !_running;
            }
        }        
        
        public bool Running
        {
            get
            {
                return _running;
            }
        }
        
        #endregion
        
        public void Run()
        {
            if (_totalSeconds > 0)
            {
                _started = true;
                _running = true;
                _elapsedSeconds = 0;
            }
        }
        
        private void Update()
        {
            if (_running)
            {
                _elapsedSeconds += Time.deltaTime;
                if (_elapsedSeconds >= _totalSeconds)
                {
                    _running = false;
                }
            }
        }
    }
}

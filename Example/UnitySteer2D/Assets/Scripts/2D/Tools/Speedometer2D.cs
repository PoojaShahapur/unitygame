using TickedPriorityQueue;
using UnityEngine;
using UnitySteer.Attributes;

namespace UnitySteer2D.Tools
{
    public class Speedometer2D : MonoBehaviour
    {
        private Vector2 _lastRecordedPosition;
        
        private float[] _squaredDistanceSamples;

        private float _cachedSpeed;
        private float _lastAverageCalculationTime;


        private Transform _transform;

        private TickedObject _tickedObject;
        private UnityTickedQueue _queue;

        private string _queueName = "Steering";

        private int _lastSampleIndex;

        private float _cachedSpeedRefreshRate = 1f;

        private float _measuringSpeed = 0.25f;
        private int _numberSamples = 10;

        public float Speed
        {
            get
            {
                if (Time.time > _lastAverageCalculationTime + _cachedSpeedRefreshRate)
                {
                    _lastAverageCalculationTime = Time.time;
                    _cachedSpeed = 0;
                    for (var i = 0; i < _numberSamples; i++)
                    {
                        _cachedSpeed += _squaredDistanceSamples[i];
                    }
                    _cachedSpeed /= _numberSamples;
                    _cachedSpeed = Mathf.Sqrt(_cachedSpeed);
                    _cachedSpeed /= _measuringSpeed;
                }
                return _cachedSpeed;
            }
        }


        private void Awake()
        {
            _transform = transform;
            _lastRecordedPosition = _transform.position;
            _squaredDistanceSamples = new float[_numberSamples];
        }

        protected void OnEnable()
        {
            _tickedObject = new TickedObject(OnMeasureSpeed);
            _tickedObject.TickLength = _measuringSpeed;
            _queue = UnityTickedQueue.GetInstance(_queueName);
            _queue.Add(_tickedObject);
        }

        protected void OnDisable()
        {
            if (_queue != null)
            {
                _queue.Remove(_tickedObject);
            }
        }


        private void OnMeasureSpeed(object param)
        {
            if (++_lastSampleIndex >= _numberSamples)
            {
                _lastSampleIndex = 0;
            }
            _squaredDistanceSamples[_lastSampleIndex] = ((Vector2)_transform.position - _lastRecordedPosition).sqrMagnitude;
            _lastRecordedPosition = _transform.position;
        }
    }
}
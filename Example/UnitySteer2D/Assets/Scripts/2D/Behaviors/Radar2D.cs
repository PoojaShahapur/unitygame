using System;
using System.Collections.Generic;
using TickedPriorityQueue;
using UnityEngine;
using UnitySteer.Attributes;

namespace UnitySteer2D.Behaviors
{
    public class Radar2D : MonoBehaviour
    {
        private static IDictionary<int, DetectableObject2D> _knownDetectableObjects =
            new SortedDictionary<int, DetectableObject2D>();

        private Transform _transform;
        private TickedObject _tickedObject;
        private UnityTickedQueue _steeringQueue;

        private string _queueName = "Radar2D";
        private int _maxQueueProcessedPerUpdate = 20;
        private float _tickLength = 0.5f;

        private float _detectionRadius = 5;
        private bool _detectDisabledVehicles;
        private LayerMask _layersChecked;
        private bool _drawGizmos;
        private int _preAllocateSize = 30;

        private Collider2D[] _detectedColliders;
        private List<DetectableObject2D> _detectedObjects;
        private List<Vehicle2D> _vehicles;
        private List<DetectableObject2D> _obstacles;

        public IEnumerable<Collider2D> Detected
        {
            get
            {
                return _detectedColliders;
            }
        }

        public float DetectionRadius
        {
            get
            {
                return _detectionRadius;
            }

            set
            {
                _detectionRadius = value;
            }
        }

        public bool DetectDisabledVehicles
        {
            get
            {
                return _detectDisabledVehicles;
            }

            set
            {
                _detectDisabledVehicles = value;
            }
        }

        public bool DrawGizmos
        {
            get
            {
                return _drawGizmos;
            }

            set
            {
                _drawGizmos = value;
            }
        }

        public List<DetectableObject2D> Obstacles
        {
            get
            {
                return _obstacles;
            }
        }

        public Vector2 Position
        {
            get
            {
                return (Vehicle != null) ? Vehicle.Position : (Vector2)_transform.position;
            }
        }

        public Action<Radar2D> OnDetected = delegate { };
        public Vehicle2D Vehicle { get; private set; }
        public List<Vehicle2D> Vehicles
        {
            get
            {
                return _vehicles;
            }
        }

        public LayerMask LayersChecked
        {
            get
            {
                return _layersChecked;
            }

            set
            {
                _layersChecked = value;
            }
        }

        public static void AddDetectableObject2D(DetectableObject2D obj)
        {
            _knownDetectableObjects[obj.Collider.GetInstanceID()] = obj;
        }
        
        public static bool RemoveDetectableObject2D(DetectableObject2D obj)
        {
            return _knownDetectableObjects.Remove(obj.Collider.GetInstanceID());
        }

        protected virtual void Awake()
        {
            Vehicle = GetComponent<Vehicle2D>();
            _transform = transform;
            _vehicles = new List<Vehicle2D>(_preAllocateSize);
            _obstacles = new List<DetectableObject2D>(_preAllocateSize);
            _detectedObjects = new List<DetectableObject2D>(_preAllocateSize * 3);
        }

        private void OnEnable()
        {
            _tickedObject = new TickedObject(OnUpdateRadar) {TickLength = _tickLength};
            _steeringQueue = UnityTickedQueue.GetInstance(_queueName);
            _steeringQueue.Add(_tickedObject);
            _steeringQueue.MaxProcessedPerUpdate = _maxQueueProcessedPerUpdate;
        }

        private void OnDisable()
        {
            if (_steeringQueue != null)
            {
                _steeringQueue.Remove(_tickedObject);
            }
        }

        private void OnUpdateRadar(object obj)
        {
            UnityEngine.Profiling.Profiler.BeginSample("OnUpdateRadar");
            _detectedColliders = Detect();
            FilterDetected();
            if (OnDetected != null)
            {
                UnityEngine.Profiling.Profiler.BeginSample("Detection event handler");
                OnDetected(this);
                UnityEngine.Profiling.Profiler.EndSample();
            }
#if TRACEDETECTED
		if (DrawGizmos)
		{
			Debug.Log(gameObject.name+" detected at "+Time.time);
			var sb = new System.Text.StringBuilder(); 
			foreach (var v in Vehicles)
			{
				sb.Append(v.gameObject.name);
				sb.Append(" ");
				sb.Append(v.Position);
				sb.Append(" ");
			}
			foreach (var o in Obstacles)
			{
				sb.Append(o.gameObject.name);
				sb.Append(" ");
				sb.Append(o.Position);
				sb.Append(" ");
			}
			Debug.Log(sb.ToString());
		}
#endif
            UnityEngine.Profiling.Profiler.EndSample();
        }

        public void UpdateRadar()
        {
            OnUpdateRadar(null);
        }

        protected virtual Collider2D[] Detect()
        {
            return Physics2D.OverlapCircleAll(Position, DetectionRadius, LayersChecked);
        }

        protected virtual void FilterDetected()
        {
            UnityEngine.Profiling.Profiler.BeginSample("Base FilterDetected");

            _vehicles.Clear();
            _obstacles.Clear();
            _detectedObjects.Clear();


            UnityEngine.Profiling.Profiler.BeginSample("Initial detection");
            for (var i = 0; i < _detectedColliders.Length; i++)
            {
                var id = _detectedColliders[i].GetInstanceID();
                if (!_knownDetectableObjects.ContainsKey(id))
                    continue;
                var detectable = _knownDetectableObjects[id];

                if (detectable != null &&
                    detectable != Vehicle &&
                    !detectable.Equals(null))
                {
                    _detectedObjects.Add(detectable);
                }
            }
            UnityEngine.Profiling.Profiler.EndSample();

            UnityEngine.Profiling.Profiler.BeginSample("Filtering out vehicles");
            for (var i = 0; i < _detectedObjects.Count; i++)
            {
                var d = _detectedObjects[i];
                var v = d as Vehicle2D;
                if (v != null && (v.enabled || _detectDisabledVehicles))
                {
                    _vehicles.Add(v);
                }
                else
                {
                    _obstacles.Add(d);
                }
            }
            UnityEngine.Profiling.Profiler.EndSample();
            UnityEngine.Profiling.Profiler.EndSample();
        }

        private void OnDrawGizmos()
        {
            if (_drawGizmos)
            {
                var pos = (Vehicle == null) ? (Vector2)transform.position : Vehicle.Position;

                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(pos, DetectionRadius);
#if TRACEDETECTED
			if (Application.isPlaying) {
				foreach (var v in Vehicles)
					Gizmos.DrawLine(pos, v.gameObject.transform.position);
				foreach (var o in Obstacles)
					Gizmos.DrawLine(pos, o.gameObject.transform.position);
			}
#endif
            }
        }
    }
}
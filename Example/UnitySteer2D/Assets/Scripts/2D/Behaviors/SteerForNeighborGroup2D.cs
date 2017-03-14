using System.Collections.Generic;
using UnityEngine;
using UnitySteer.Attributes;

namespace UnitySteer2D.Behaviors
{
    // Require (Radar2D)
    public class SteerForNeighborGroup2D : Steering2D
    {
        private float _minRadius = 3f;
        private float _maxRadius = 7.5f;

        private bool _drawNeighbors = false;
        // Range (0, 360)
        private float _angleCos = 0.7f;

        private SteerForNeighbors2D[] _behaviors;
        private readonly List<Vehicle2D> _neighbors = new List<Vehicle2D>(20);

        public float AngleCos
        {
            get
            {
                return _angleCos;
            }

            set
            {
                _angleCos = Mathf.Clamp(value, -1.0f, 1.0f);
            }
        }
        
        public float AngleDegrees
        {
            get
            {
                return OpenSteerUtility.DegreesFromCos(_angleCos);
            }

            set
            {
                _angleCos = OpenSteerUtility.CosFromDegrees(value);
            }
        }

        public float MinRadius
        {
            get
            {
                return _minRadius;
            }

            set
            {
                _minRadius = value;
            }
        }

        public float MaxRadius
        {
            get
            {
                return _maxRadius;
            }

            set
            {
                _maxRadius = value;
            }
        }

        public List<Vehicle2D> Neighbors
        {
            get
            {
                return _neighbors;
            }
        }

        protected override void Start()
        {
            base.Start();
            _behaviors = GetComponents<SteerForNeighbors2D>();
            foreach (var b in _behaviors)
            {
                // Ensure UnitySteer does not call them
                b.enabled = false;
                // ... and since Unity may not initialize them either, initialize them ourselves.
                b.Initialize();
            }
            Vehicle.Radar.OnDetected += HandleDetection;
        }

        private void HandleDetection(Radar2D radar)
        {
            _neighbors.Clear();

            for (var i = 0; i < radar.Vehicles.Count; i++)
            {
                var other = radar.Vehicles[i];
                if (Vehicle.IsInNeighborhood(other, MinRadius, MaxRadius, AngleCos))
                {
                    _neighbors.Add(other);
                }
            }
        }

        protected override Vector2 CalculateForce()
        {
            var steering = Vector2.zero;
            UnityEngine.Profiling.Profiler.BeginSample("SteerForNeighborGroup.Looping over neighbors");

            for (var i = 0; i < _neighbors.Count; i++)
            {
                var other = _neighbors[i];
                if (other == null || other.Equals(null))
                {
                    continue;
                }
                var direction = other.Position - Vehicle.Position;
                if (!other.GameObject.Equals(null))
                {
                    if (_drawNeighbors)
                    {
                        Debug.DrawLine(Vehicle.Position, other.Position, Color.magenta);
                    }
                    UnityEngine.Profiling.Profiler.BeginSample("SteerForNeighborGroup.Adding");
                    for (var bi = 0; bi < _behaviors.Length; bi++)
                    {
                        var behavior = _behaviors[bi];
                        if (behavior.IsDirectionInRange(direction))
                        {
                            steering += behavior.CalculateNeighborContribution(other) * behavior.Weight;
                        }
                    }
                    UnityEngine.Profiling.Profiler.EndSample();
                }
            }
            ;
            UnityEngine.Profiling.Profiler.EndSample();

            UnityEngine.Profiling.Profiler.BeginSample("Normalizing");

            steering.Normalize();
            UnityEngine.Profiling.Profiler.EndSample();

            return steering;
        }
    }
}
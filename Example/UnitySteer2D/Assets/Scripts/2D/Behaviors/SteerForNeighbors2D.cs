using System;
using UnityEngine;

namespace UnitySteer2D.Behaviors
{
    public abstract class SteerForNeighbors2D : Steering2D
    {
        private float _minDistance = 1;
        private float _maxDistance = 10;

        public float MinDistance
        {
            get
            {
                return _minDistance;
            }

            set
            {
                _minDistance = Mathf.Max(value, 0);
                MinDistanceSquared = _minDistance * _minDistance;
            }
        }

        public float MinDistanceSquared
        {
            get;
            private set;
        }

        public float MaxDistance
        {
            get
            {
                return _maxDistance;
            }

            set
            {
                _maxDistance = Mathf.Max(value, 0);
                MaxDistanceSquared = _maxDistance * _maxDistance;
            }
        }

        public float MaxDistanceSquared
        {
            get;

            private set;
        }

        protected override sealed Vector2 CalculateForce()
        {
            throw new NotImplementedException("SteerForNeighbors2D.CalculateForce should never be called directly.  " +
                                              "Did you enable a SteerForNeighbors2D subclass manually? They are disabled by SteerForNeighborGroup2D on Start.");
        }

        public abstract Vector2 CalculateNeighborContribution(Vehicle2D other);

        protected override void Awake()
        {
            base.Awake();
            MaxDistanceSquared = _maxDistance * _maxDistance;
			MinDistanceSquared = _minDistance * _minDistance;
        }

        public void Initialize()
        {
            Awake();
            Start();
        }
        
        public bool IsDirectionInRange(Vector2 difference)
        {
            return
                OpenSteerUtility.IntervalComparison(difference.sqrMagnitude, MinDistanceSquared, MaxDistanceSquared) ==
                0;
        }
    }
}
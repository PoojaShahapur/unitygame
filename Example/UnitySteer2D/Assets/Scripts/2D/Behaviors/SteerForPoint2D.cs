using UnityEngine;

namespace UnitySteer2D.Behaviors
{
    public class SteerForPoint2D : Steering2D
    {
        private Vector2 _targetPoint = Vector2.zero;
        private bool _considerVelocity;
        private bool _defaultToCurrentPosition = true;

        public Vector2 TargetPoint
        {
            get
            {
                return _targetPoint;
            }

            set
            {
                if (_targetPoint == value)
                {
                    return;
                }
                _targetPoint = value;
                ReportedArrival = false;
            }
        }
        
        public bool ConsiderVelocity
        {
            get
            {
                return _considerVelocity;
            }

            set
            {
                _considerVelocity = value;
            }
        }

        protected override void Start()
        {
            base.Start();

            if (_defaultToCurrentPosition && TargetPoint == Vector2.zero)
            {
                enabled = false;
            }
        }

        protected override Vector2 CalculateForce()
        {
            return Vehicle.GetSeekVector(TargetPoint, _considerVelocity);
        }
    }
}
using UnityEngine;

namespace UnitySteer2D.Behaviors
{
    public class SteerForMinimumSpeed2D : Steering2D
    {
        private float _minimumSpeed = 4;
        private bool _moveForwardWhenZero = true;

        public override bool IsPostProcess
        {
            get
            {
                return true;
            }
        }

        public float MinimumSpeed
        {
            get
            {
                return _minimumSpeed;
            }

            set
            {
                _minimumSpeed = value;
            }
        }

        protected override Vector2 CalculateForce()
        {
            var result = Vehicle.DesiredVelocity;
            if (_moveForwardWhenZero && Mathf.Approximately(Vehicle.TargetSpeed, 0))
            {
                result = Vehicle.Forward * _minimumSpeed;
            }
            else if (Vehicle.TargetSpeed < _minimumSpeed)
            {
                result = Vehicle.DesiredVelocity.normalized * _minimumSpeed;
            }
            return result;
        }
    }
}
using System;
using UnityEngine;

namespace UnitySteer2D.Behaviors
{
    public class AutonomousVehicle2D : TickedVehicle2D
    {
        private float _speed;
        private float _accelerationRate = 5;
        private float _decelerationRate = 8;

        public override float Speed
        {
            get { return _speed; }
        }
        
        public override Vector2 Velocity
        {
            get
            {
                return Forward * Speed;
            }

            protected set
            {
                throw new NotSupportedException("Cannot set the velocity directly on AutonomousVehicle2D");
            }
        }

        protected override void SetCalculatedVelocity(Vector2 velocity)
        {
            TargetSpeed = velocity.magnitude;
            OrientationVelocity = Mathf.Approximately(_speed, 0) ? Forward : velocity / TargetSpeed;
        }

        protected override Vector2 CalculatePositionDelta(float deltaTime)
        {
            var targetSpeed = Mathf.Clamp(TargetSpeed, 0, MaxSpeed);
            if (Mathf.Approximately(_speed, targetSpeed))
            {
                _speed = targetSpeed;
            }
            else
            {
                var rate = TargetSpeed > _speed ? _accelerationRate : _decelerationRate;
                _speed = Mathf.Lerp(_speed, targetSpeed, deltaTime * rate);
            }

            return Velocity * deltaTime;
        }

        protected override void ZeroVelocity()
        {
            TargetSpeed = 0;
        }
    }
}
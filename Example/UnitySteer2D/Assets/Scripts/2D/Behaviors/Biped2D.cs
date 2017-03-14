using UnityEngine;

namespace UnitySteer2D.Behaviors
{
    public class Biped2D : TickedVehicle2D
    {
        private float _speed;
        private Vector2 _velocity;
        
        public override float Speed
        {
            get
            {
                return Speedometer2D == null ? _speed : Speedometer2D.Speed;
            }
        }

        public override Vector2 Velocity
        {
            get
            {
                return _velocity;
            }

            protected set
            {
                _velocity = Vector2.ClampMagnitude(value, MaxSpeed);
                _speed = _velocity.magnitude;
                TargetSpeed = _speed;
                OrientationVelocity = !Mathf.Approximately(_speed, 0) ? _velocity / _speed : Vector2.zero;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Velocity = Vector2.zero;
        }

        protected override void SetCalculatedVelocity(Vector2 velocity)
        {
            Velocity = velocity;
        }

        protected override Vector2 CalculatePositionDelta(float deltaTime)
        {
            return Velocity * deltaTime;
        }

        protected override void ZeroVelocity()
        {
            Velocity = Vector2.zero;
        }
    }
}
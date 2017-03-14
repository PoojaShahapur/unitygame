using UnityEngine;

namespace UnitySteer2D.Behaviors
{
    public class SteerForTether2D : Steering2D
    {
        private float _maximumDistance = 30f;
        private Vector2 _tetherPosition;

        public override bool IsPostProcess
        {
            get
            {
                return true;
            }
        }

        public float MaximumDistance
        {
            get
            {
                return _maximumDistance;
            }
            set
            {
                _maximumDistance = Mathf.Clamp(value, 0, float.MaxValue);
            }
        }

        public Vector2 TetherPosition
        {
            get
            {
                return _tetherPosition;
            }

            set
            {
                _tetherPosition = value;
            }
        }

        protected override Vector2 CalculateForce()
        {
            var steering = Vector2.zero;
            var difference = TetherPosition - Vehicle.Position;
            var distance = difference.magnitude;
            if (distance > _maximumDistance)
            {
                steering = (difference + Vehicle.DesiredVelocity) / 2;
            }
            return steering;
        }
    }
}
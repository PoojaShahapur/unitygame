using UnityEngine;

namespace UnitySteer2D.Behaviors
{
    // Require(SteerForNeighborGroup2D)
    public class SteerForSeparation2D : SteerForNeighbors2D
    {
        private float _comfortDistance = 1;
        private float _multiplierInsideComfortDistance = 1;
        private float _vehicleRadiusImpact = 0;

        private float _comfortDistanceSquared;

        public float ComfortDistance
        {
            get
            {
                return _comfortDistance;
            }

            set
            {
                _comfortDistance = value;
                _comfortDistanceSquared = _comfortDistance * _comfortDistance;
            }
        }

        protected override void Start()
        {
            _comfortDistanceSquared = _comfortDistance * _comfortDistance;
        }

        public override Vector2 CalculateNeighborContribution(Vehicle2D other)
        {
            var steering = Vector2.zero;

            var offset = other.Position - Vehicle.Position;
            var offsetSqrMag = offset.sqrMagnitude;

            steering = (offset / -offsetSqrMag);
            if (!Mathf.Approximately(_multiplierInsideComfortDistance, 1) && offsetSqrMag < _comfortDistanceSquared)
            {
                steering *= _multiplierInsideComfortDistance;
            }

            if (_vehicleRadiusImpact > 0)
            {
                steering *= (other.Radius + Vehicle.Radius) * _vehicleRadiusImpact;
            }

            return steering;
        }
    }
}
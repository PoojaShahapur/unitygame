using UnityEngine;

namespace UnitySteer2D.Behaviors
{
    public class SteerForPathSimplified2D : Steering2D
    {
        private float _predictionTime = 1.5f;
        private float _minSpeedToConsider = 0.25f;
        private IPathway _path;

        public float PredictionTime
        {
            get
            {
                return _predictionTime;
            }

            set
            {
                _predictionTime = Mathf.Max(value, 0);
            }
        }
        
        public float DistanceAlongPath
        {
            get;
            private set;
        }

        public float PathPercentTraversed
        {
            get
            {
                return (Path != null && Path.TotalPathLength > 0) ? DistanceAlongPath / Path.TotalPathLength : 0;
            }
        }

        public float MinSpeedToConsider
        {
            get
            {
                return _minSpeedToConsider;
            }

            set
            {
                _minSpeedToConsider = value;
            }
        }

        public IPathway Path
        {
            get
            {
                return _path;
            }

            set
            {
                _path = value;
                DistanceAlongPath = 0;
            }
        }

        protected override Vector2 CalculateForce()
        {
            if (Path == null || Path.SegmentCount < 2)
            {
                return Vector2.zero;
            }
            
            var speed = Mathf.Max(Vehicle.Speed, _minSpeedToConsider);
            var pathDistanceOffset = _predictionTime * speed;
            DistanceAlongPath = Path.MapPointToPathDistance(Vehicle.Position);

            var targetPathDistance = DistanceAlongPath + pathDistanceOffset;
            var target = Path.MapPathDistanceToPoint(targetPathDistance);
            var seek = Vehicle.GetSeekVector(target);

            if (seek == Vector2.zero && targetPathDistance <= Path.TotalPathLength)
            {
                target = Path.MapPathDistanceToPoint(targetPathDistance + 2f * Vehicle.ArrivalRadius);
                seek = Vehicle.GetSeekVector(target);
            }

            return seek;
        }

        protected void OnDrawGizmosSelected()
        {
            if (Path != null)
            {
                Path.DrawGizmos();
            }
        }
    }
}
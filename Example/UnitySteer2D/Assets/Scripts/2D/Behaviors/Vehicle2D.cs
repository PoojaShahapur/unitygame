using System.Linq;
using UnityEngine;
using UnitySteer.Attributes;
using UnitySteer2D.Tools;

namespace UnitySteer2D.Behaviors
{
    public abstract class Vehicle2D : DetectableObject2D
    {
        private float _minSpeedForTurning = 0.1f;
        private int _movementPriority;

        private float _turnTime = 0.25f;
        private float _mass = 1;
        private Vector2 _allowedMovementAxes = Vector2.one;
        private float _arrivalRadius = 0.25f;

        private float _maxSpeed = 1;
        private float _maxForce = 10;
        private bool _canMove = true;

        private SpriteForwardDirection _forward;

        public Vector2 AllowedMovementAxes
        {
            get
            {
                return _allowedMovementAxes;
            }
        }

        public bool CanMove
        {
            get
            {
                return _canMove;
            }

            set
            {
                _canMove = value;
            }
        }

        public Vector2 DesiredVelocity
        {
            get;
            protected set;
        }

        public GameObject GameObject
        {
            get;
            private set;
        }

        public float Mass
        {
            get
            {
                return _mass;
            }

            set
            {
                _mass = Mathf.Max(0, value);
            }
        }

        public float MaxForce
        {
            get
            {
                return _maxForce;
            }

            set
            {
                _maxForce = Mathf.Clamp(value, 0, float.MaxValue);
            }
        }

        public float MaxSpeed
        {
            get
            {
                return _maxSpeed;
            }

            set
            {
                _maxSpeed = Mathf.Clamp(value, 0, float.MaxValue);
            }
        }

        public float MinSpeedForTurning
        {
            get
            {
                return _minSpeedForTurning;
            }
        }

        public int MovementPriority
        {
            get
            {
                return _movementPriority;
            }
        }

        public Radar2D Radar
        {
            get;
            private set;
        }

        public Rigidbody2D Rigidbody
        {
            get;
            private set;
        }

        public Speedometer2D Speedometer2D
        {
            get;
            protected set;
        }
        
        public float ArrivalRadius
        {
            get
            {
                return _arrivalRadius;
            }

            set
            {
                _arrivalRadius = Mathf.Clamp(value, 0.01f, float.MaxValue);
                SquaredArrivalRadius = _arrivalRadius * _arrivalRadius;
            }
        }

        public float SquaredArrivalRadius
        {
            get;
            private set;
        }
        
        public Vector2 LastRawForce
        {
            get;
            protected set;
        }

        public abstract float Speed
        {
            get;
        }

        public float TurnTime
        {
            get
            {
                return _turnTime;
            }

            set
            {
                _turnTime = Mathf.Max(0, value);
            }
        }

        public Steering2D[] Steerings
        {
            get;
            private set;
        }

        public Steering2D[] SteeringPostprocessors
        {
            get;
            private set;
        }

        public abstract Vector2 Velocity
        {
            get;
            protected set;
        }

        public float TargetSpeed
        {
            get;
            protected set;
        }

        public virtual float DeltaTime
        {
            get
            {
                return Time.deltaTime;
            }
        }

        public Vector2 Forward
        {
            get
            {
                switch (_forward)
                {
                    case SpriteForwardDirection.Up:
                        return transform.up;

                    case SpriteForwardDirection.Down:
                        return -(transform.up);

                    case SpriteForwardDirection.Right:
                        return transform.right;

                    case SpriteForwardDirection.Left:
                        return -(transform.right);

                    default:
                        throw new System.Exception("Somehow no direction was chosen.");
                }
            }

            protected set
            {
                switch (_forward)
                {
                    case SpriteForwardDirection.Up:
                        transform.up = value;
                        break;

                    case SpriteForwardDirection.Down:
                        transform.up = -value;
                        break;

                    case SpriteForwardDirection.Right:
                        transform.right = value;
                        break;

                    case SpriteForwardDirection.Left:
                        transform.right = -value;
                        break;

                    default:
                        throw new System.Exception("Somehow no direction was chosen.");
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            GameObject = gameObject;
            Rigidbody = GetComponent<Rigidbody2D>();
            var allSteerings = GetComponents<Steering2D>();
            Steerings = allSteerings.Where(x => !x.IsPostProcess).ToArray();
            SteeringPostprocessors = allSteerings.Where(x => x.IsPostProcess).ToArray();

            if (_movementPriority == 0)
            {
                _movementPriority = gameObject.GetInstanceID();
            }
            Radar = GetComponent<Radar2D>();
            Speedometer2D = GetComponent<Speedometer2D>();
            SquaredArrivalRadius = ArrivalRadius * ArrivalRadius;
        }

        public override Vector2 PredictFuturePosition(float predictionTime)
        {
            return (Vector2)Transform.position + (Velocity * predictionTime);
        }

        public Vector2 PredictFutureDesiredPosition(float predictionTime)
        {
            return (Vector2)Transform.position + (DesiredVelocity * predictionTime);
        }

        public bool IsInNeighborhood(Vehicle2D other, float minDistance, float maxDistance, float cosMaxAngle)
        {
            var result = false;
            if (other != this)
            {
                var offset = other.Position - Position;
                var distanceSquared = offset.sqrMagnitude;

                if (distanceSquared < (minDistance * minDistance))
                {
                    result = true;
                }
                else
                {
                    if (distanceSquared <= (maxDistance * maxDistance))
                    {
                        var unitOffset = offset / Mathf.Sqrt(distanceSquared);
                        var forwardness = Vector2.Dot(Forward, unitOffset);
                        result = forwardness > cosMaxAngle;
                    }
                }
            }
            return result;
        }

        public Vector2 GetSeekVector(Vector2 target, bool considerVelocity = false)
        {
            var force = Vector2.zero;

            var difference = target - Position;
            var d = difference.sqrMagnitude;
            if (d > SquaredArrivalRadius)
            {
                force = considerVelocity ? difference - Velocity : difference;
            }
            return force;
        }

        public Vector2 GetTargetSpeedVector(float targetSpeed)
        {
            var mf = MaxForce;
            var speedError = targetSpeed - Speed;
            return Forward * Mathf.Clamp(speedError, -mf, +mf);
        }

        public float DistanceFromPerimeter(Vehicle2D other)  {
            var diff = Position - other.Position;
            return diff.magnitude - Radius - other.Radius;
        }

        public void ResetOrientation()
        {
            Transform.up = Vector3.up;
            Transform.forward = Vector3.forward;
        }

        public float PredictNearestApproachTime(Vehicle2D other)  {
            var otherVelocity = other.Velocity;
            var relVelocity = otherVelocity - Velocity;
            var relSpeed = relVelocity.magnitude;
            
            if (Mathf.Approximately(relSpeed, 0))
            {
                return 0;
            }
            
            var relTangent = relVelocity / relSpeed;

            var relPosition = Position - other.Position;
            var projection = Vector2.Dot(relTangent, relPosition);

            return projection / relSpeed;
        }

        public float ComputeNearestApproachPositions(Vehicle2D other, float time,
            ref Vector2 ourPosition,
            ref Vector2 hisPosition)
        {
            return ComputeNearestApproachPositions(other, time, ref ourPosition, ref hisPosition, Speed,
                Forward);
        }
        
        public float ComputeNearestApproachPositions(Vehicle2D other, float time,
            ref Vector2 ourPosition,
            ref Vector2 hisPosition,
            float ourSpeed,
            Vector2 ourForward)
        {
            var myTravel = ourForward * ourSpeed * time;
            var otherTravel = other.Forward * other.Speed * time;

            ourPosition = Position + myTravel;
            hisPosition = other.Position + otherTravel;

            return Vector2.Distance(ourPosition, hisPosition);
        }

        protected override void OnDrawGizmos()
        {
            if (_drawGizmos)
            {
                base.OnDrawGizmos();
                Gizmos.color = Color.grey;
                Gizmos.DrawWireSphere(Position, _arrivalRadius);
            }
        }
    }

    public enum SpriteForwardDirection
    {
        Up,
        Down,
        Right,
        Left
    }
}
using System.Diagnostics;
using TickedPriorityQueue;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace UnitySteer2D.Behaviors
{
    public abstract class TickedVehicle2D : Vehicle2D
    {
        private TickedObject _tickedObject;
        private UnityTickedQueue _steeringQueue;
        
        private string _queueName = "Steering2D";
        private float _tickLength = 0.1f;

        private int _maxQueueProcessedPerUpdate = 20;

        private bool _traceAdjustments = false;

        public float PreviousTickTime { get; private set; }
        public float CurrentTickTime { get; private set; }

        public override float DeltaTime
        {
            get
            {
                return CurrentTickTime - PreviousTickTime;
            }
        }

        public Vector2 OrientationVelocity
        {
            get;
            protected set;
        }

        public string QueueName
        {
            get
            {
                return _queueName;
            }

            set
            {
                _queueName = value;
            }
        }

        public UnityTickedQueue SteeringQueue
        {
            get
            {
                return _steeringQueue;
            }
        }

        public TickedObject TickedObject
        {
            get;
            private set;
        }

        private void Start()
        {
            PreviousTickTime = Time.time;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            TickedObject = new TickedObject(OnUpdateSteering);
            TickedObject.TickLength = _tickLength;
            _steeringQueue = UnityTickedQueue.GetInstance(QueueName);
            _steeringQueue.Add(TickedObject);
            _steeringQueue.MaxProcessedPerUpdate = _maxQueueProcessedPerUpdate;
        }

        protected override void OnDisable()
        {
            DeQueue();
            base.OnDisable();
        }
        
        private void DeQueue()
        {
            if (_steeringQueue != null)
            {
                _steeringQueue.Remove(TickedObject);
            }
        }

        protected void OnUpdateSteering(object obj)
        {
            if (enabled)
            {
                CalculateForces();
            }
            else
            {
                DeQueue();
            }
        }


        protected void CalculateForces()
        {
            PreviousTickTime = CurrentTickTime;
            CurrentTickTime = Time.time;

            if (!CanMove || Mathf.Approximately(MaxForce, 0) || Mathf.Approximately(MaxSpeed, 0))
            {
                return;
            }
            UnityEngine.Profiling.Profiler.BeginSample("Calculating vehicle forces");

            var force = Vector2.zero;

            UnityEngine.Profiling.Profiler.BeginSample("Adding up basic steerings");
            for (var i = 0; i < Steerings.Length; i++)
            {
                var s = Steerings[i];
                if (s.enabled)
                {
                    force += s.WeighedForce;
                }
            }
            UnityEngine.Profiling.Profiler.EndSample();
            LastRawForce = force;

            var newVelocity = Vector2.ClampMagnitude(force / Mass, MaxForce);

            if (newVelocity.sqrMagnitude == 0)
            {
                ZeroVelocity();
                DesiredVelocity = Vector2.zero;
            }
            else
            {
                DesiredVelocity = newVelocity;
            }

            var adjustedVelocity = Vector2.zero;
            UnityEngine.Profiling.Profiler.BeginSample("Adding up post-processing steerings");
            for (var i = 0; i < SteeringPostprocessors.Length; i++)
            {
                var s = SteeringPostprocessors[i];
                if (s.enabled)
                {
                    adjustedVelocity += s.WeighedForce;
                }
            }
            UnityEngine.Profiling.Profiler.EndSample();


            if (adjustedVelocity != Vector2.zero)
            {
                adjustedVelocity = Vector2.ClampMagnitude(adjustedVelocity, MaxSpeed);
                TraceDisplacement(adjustedVelocity, Color.cyan);
                TraceDisplacement(newVelocity, Color.white);
                newVelocity = adjustedVelocity;
            }

            SetCalculatedVelocity(newVelocity);
            UnityEngine.Profiling.Profiler.EndSample();
        }

        private void ApplySteeringForce(float elapsedTime)
        {
            var acceleration = CalculatePositionDelta(elapsedTime);
            acceleration = Vector2.Scale(acceleration, AllowedMovementAxes);

            if (Rigidbody == null || Rigidbody.isKinematic)
            {
                Transform.position += (Vector3)acceleration;
            }
            else
            {
                Rigidbody.MovePosition(Rigidbody.position + acceleration);
            }
        }

        protected void AdjustOrientation(float deltaTime)
        {
            if (TargetSpeed > MinSpeedForTurning && Velocity != Vector2.zero)
            {
                var newForward = Vector2.Scale(OrientationVelocity, AllowedMovementAxes).normalized;
                if (TurnTime > 0)
                {
                    newForward = Vector3.Slerp(Forward, newForward, deltaTime / TurnTime);
                }

                Forward = newForward;
            }
        }

        protected abstract void SetCalculatedVelocity(Vector2 velocity);
        protected abstract Vector2 CalculatePositionDelta(float deltaTime);
        protected abstract void ZeroVelocity();

        private void Update()
        {
            if (CanMove)
            {
                ApplySteeringForce(Time.deltaTime);
                AdjustOrientation(Time.deltaTime);
            }
        }

        private void TraceDisplacement(Vector2 delta, Color color)
        {
            if (_traceAdjustments)
            {
                Debug.DrawLine(Transform.position, (Vector2)Transform.position + delta, color);
            }
        }

        public void Stop()
        {
            CanMove = false;
            ZeroVelocity();
        }
    }
}
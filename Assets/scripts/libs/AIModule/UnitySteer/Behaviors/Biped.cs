using UnityEngine;

namespace UnitySteer.Behaviors
{
    /// <summary>
    /// Vehicle subclass oriented towards autonomous bipeds, which have a movement
    /// vector which can be separate from their forward vector (can side-step or
	/// walk backwards).
    /// </summary>
    public class Biped : TickedVehicle
    {
        #region Internal state values

        /// <summary>
        /// The magnitude of the last velocity vector assigned to the vehicle 
        /// </summary>
        private float _speed = 1;

        /// <summary>
        /// The biped's current velocity vector
        /// </summary>
        private Vector3 _velocity;

        #endregion

        /// <summary>
        /// Current vehicle speed
        /// </summary>	
        /// <remarks>
        /// If the vehicle has a speedometer, then we return the actual measured
        /// value instead of simply the length of the velocity vector.
        /// </remarks>
        public override float Speed
        {
            get { return Speedometer == null ? _speed : Speedometer.Speed; }
        }

        /// <summary>
        /// Current vehicle velocity
        /// </summary>
        public override Vector3 Velocity
        {
            get { return _velocity; }
            protected set
            {
                _velocity = value;
                _velocity.Normalize();
                //_velocity = Vector3.ClampMagnitude(value, MaxSpeed);
                //_speed = _velocity.magnitude;
                //TargetSpeed = _speed;
                //OrientationVelocity = !Mathf.Approximately(_speed, 0) ? _velocity / _speed : Vector3.zero;
                OrientationVelocity = !Mathf.Approximately(_speed, 0) ? _velocity : Vector3.zero;
            }
        }

        // …Ë÷√ÀŸ¬ 
        public void setSpeed(float value)
        {
            _speed = value;
            TargetSpeed = _speed;
        }

        #region Methods

        protected override void OnEnable()
        {
            base.OnEnable();
            Velocity = Vector3.zero;
        }


        /// <summary>
        /// Assigns a new velocity vector to the biped.
        /// </summary>
        /// <param name="velocity">Newly calculated velocity</param>
        protected override void SetCalculatedVelocity(Vector3 velocity)
        {
            Velocity = velocity;
        }

        /// <summary>
        /// Calculates how much the agent's position should change in a manner that
        /// is specific to the vehicle's implementation.
        /// </summary>
        /// <param name="deltaTime">Time delta to use in position calculations</param>
        protected override Vector3 CalculatePositionDelta(float deltaTime)
        {
            //return Velocity * deltaTime;
            return Velocity * deltaTime * _speed;
        }

        /// <summary>
        /// Zeros this vehicle's velocity vector.
        /// </summary>
        protected override void ZeroVelocity()
        {
            Velocity = Vector3.zero;
        }

        #endregion
    }
}
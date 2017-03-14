using System;
using UnityEngine;

namespace UnitySteer2D.Behaviors
{
    public class PassiveVehicle2D : Vehicle2D
    {
        private float _speed = 0;
        private Vector2 _velocity = Vector2.zero;
        private bool _isBiped;
        private Vector2 _lastPosition = Vector2.zero;

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
                return _isBiped ? _velocity : Forward * _speed;
            }

            protected set
            {
                throw new NotSupportedException("Cannot set the velocity directly on PassiveVehicle2D");
            }
        }

        private void Update()
        {
            if (!CanMove)
            {
                Velocity = Vector2.zero; //Doesn't this throw an exception constantly?
            }
            else if (Position != _lastPosition)
            {
                _velocity = Position - _lastPosition;
                _lastPosition = Position;
            }
            else
            {
                _velocity = Vector2.zero;
            }
        }
    }
}
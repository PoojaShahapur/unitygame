using System;
using UnityEngine;
using UnitySteer.Attributes;

namespace UnitySteer2D.Behaviors
{
    public abstract class Steering2D : MonoBehaviour
    {
        private Vector2 _force = Vector2.zero;
        private Vehicle2D _vehicle;

        private float _weight = 1;

        public Vector2 Force
        {
            get
            {
                _force = CalculateForce();
                if (_force != Vector2.zero)
                {
                    if (!ReportedMove && OnStartMoving != null)
                    {
                        OnStartMoving(this);
                    }
                    ReportedArrival = false;
                    ReportedMove = true;
                }
                else if (!ReportedArrival)
                {
                    if (OnArrival != null)
                    {
                        OnArrival(this);

                        if (ShouldRetryForce)
                        {
                            _force = CalculateForce();
                            ShouldRetryForce = false;
                        }
                    }

                    if (_force == Vector2.zero)
                    {
                        ReportedArrival = true;
                        ReportedMove = false;
                    }
                }
                return _force;
            }
        }

        public virtual bool IsPostProcess
        {
            get
            {
                return false;
            }
        }

        public Action<Steering2D> OnArrival = delegate { };
        public Action<Steering2D> OnStartMoving
        {
            get;
            set;
        }
        
        public bool ShouldRetryForce
        {
            get;
            set;
        }
        
        public bool ReportedArrival
        {
            get;
            protected set;
        }

        public bool ReportedMove
        {
            get;
            protected set;
        }

        public Vector2 WeighedForce
        {
            get
            {
                return Force * _weight;
            }
        }

        public Vehicle2D Vehicle
        {
            get
            {
                return _vehicle;
            }
        }

        public float Weight
        {
            get
            {
                return _weight;
            }

            set
            {
                _weight = value;
            }
        }

        protected virtual void Awake()
        {
            _vehicle = GetComponent<Vehicle2D>();
            ReportedArrival = true; // Default to true to avoid unnecessary notifications
        }

        protected virtual void Start()
        {
        }

        protected abstract Vector2 CalculateForce();
    }
}
using UnityEngine;
using UnitySteer.Attributes;

namespace UnitySteer2D.Behaviors
{
    public class DetectableObject2D : MonoBehaviour
    {
        private Transform _transform;
        protected bool _drawGizmos = false;
        private Vector2 _center;
        private float _radius = 1;
        public Collider2D Collider
        {
            get;
            private set;
        }

        public Vector2 Position
        {
            get
            {
                return (Vector2)Transform.position + _center;
            }
        }

        public Vector2 Center
        {
            get
            {
                return _center;
            }

            set
            {
                _center = value;
            }
        }
        
        public float Radius
        {
            get
            {
                return _radius;
            }

            set
            {
                _radius = Mathf.Clamp(value, 0.01f, float.MaxValue);
                SquaredRadius = _radius * _radius;
            }
        }

        public float SquaredRadius
        {
            get;
            private set;
        }

        public Transform Transform
        {
            get
            {
                if (_transform == null)
                {
                    _transform = transform;
                }
                return _transform;
            }
        }

        protected virtual void Awake()
        {
            Collider = GetComponent<Collider2D>();

            SquaredRadius = _radius * _radius;
        }

        protected virtual void OnEnable()
        {
            if (Collider)
            {
                Radar2D.AddDetectableObject2D(this);
            }
        }

        protected virtual void OnDisable()
        {
            if (Collider)
            {
                Radar2D.RemoveDetectableObject2D(this);
            }
        }

        public virtual Vector2 PredictFuturePosition(float predictionTime)
        {
            return Transform.position;
        }

        public void ScaleRadiusWithTransform(float baseRadius)
        {
            var scale = Transform.lossyScale;
            _radius = baseRadius * Mathf.Max(scale.x, Mathf.Max(scale.y, scale.z));
        }

        protected virtual void OnDrawGizmos()
        {
            if (!_drawGizmos)
            {
                return;
            }
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Position, Radius);
        }
    }
}
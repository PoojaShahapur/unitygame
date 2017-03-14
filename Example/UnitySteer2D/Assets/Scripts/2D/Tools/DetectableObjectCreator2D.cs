using UnityEngine;
using UnitySteer2D.Behaviors;
using UnitySteer.Attributes;

namespace UnitySteer2D.Tools
{
	public class DetectableObjectCreator2D : MonoBehaviour
	{
        void Awake()
		{
			CreateDetectableObject();
			Destroy(this);
		}

		void CreateDetectableObject()
		{
			var radius = 0.0f;

            var colliders = gameObject.GetComponentsInChildren<Collider2D>();

            if (colliders == null)
			{
				Debug.LogError(string.Format("Obstacle {0} has no colliders", gameObject.name));
				return;
			}

			foreach (var collider in colliders)
			{
				if(collider.isTrigger)
				{
					continue;
				}

				float maxExtents = Mathf.Max(Mathf.Max(collider.bounds.extents.x, collider.bounds.extents.y),
				                             collider.bounds.extents.z);

			    float distanceToCollider = Vector2.Distance(gameObject.transform.position, collider.bounds.center);
	            var currentRadius = distanceToCollider + maxExtents;
				if (currentRadius > radius)
				{
					radius = currentRadius;
				}
			}

			var scale  = transform.lossyScale;
			radius /= Mathf.Max(scale.x, Mathf.Max(scale.y, scale.z));

			var detectable = gameObject.AddComponent<DetectableObject2D>();
			detectable.Center = Vector2.zero;
			detectable.Radius = radius;
		}
	}
}
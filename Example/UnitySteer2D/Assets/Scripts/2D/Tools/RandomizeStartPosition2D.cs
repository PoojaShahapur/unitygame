using UnityEngine;
using UnitySteer.Attributes;

namespace UnitySteer2D.Tools
{
    public class RandomizeStartPosition2D : MonoBehaviour
	{
        public Vector2 Radius = Vector2.one;
        public bool RandomizeRotation = true;
		public Vector2 AllowedAxes = Vector2.one;

		void Start()
		{
			var pos = Vector2.Scale(Random.insideUnitSphere, Radius);
            pos = Vector2.Scale(pos, AllowedAxes);
            transform.position += (Vector3)pos;

			if (RandomizeRotation) 
			{
				var rot = Random.insideUnitSphere;

                AllowedAxes.x = 0;
                AllowedAxes.y = 0;

                if (AllowedAxes.y == 0)
				{
					rot.x = 0;
					rot.z = 0;
				}
				if (AllowedAxes.x == 0)
				{
					rot.y = 0;
					rot.z = 0;
				}

				transform.rotation = Quaternion.Euler(rot * 360);	
			}
			Destroy(this);
		}
	}
}
using UnityEngine;

namespace UnitySteer2D.Behaviors
{
    // Require (SteerForNeighborGroup2D)
    public class SteerForCohesion2D : SteerForNeighbors2D
    {
        public override Vector2 CalculateNeighborContribution(Vehicle2D other)
        {
            var distance = other.Position - Vehicle.Position;
            var sqrMag = distance.sqrMagnitude;

            distance *= 1 / sqrMag;
            return distance;
        }

        private void OnDrawGizmos()
        {
#if DEBUG_COMFORT_DISTANCE
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(transform.position, ComfortDistance);
#endif
        }
    }
}
using UnityEngine;

namespace UnitySteer2D.Behaviors
{
    // Require (SteerForNeighborGroup2D)
    public class SteerForMatchingVelocity2D : SteerForNeighbors2D
    {
        public override Vector2 CalculateNeighborContribution(Vehicle2D other)
        {
            return other.Velocity;
        }
    }
}
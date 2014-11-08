using UnityEngine;

namespace UnitySteer.Behaviors
{
    /// <summary>
    /// Steers a vehicle in alignment with detected neighbors
    /// </summary>
    /// <seealso cref="SteerForMatchingVelocity"/>
    public class SteerForAlignment : SteerForNeighbors
    {
        public override Vector3 CalculateNeighborContribution(Vehicle other)
        {
            // accumulate sum of neighbor's heading
            return other.Transform.forward;
        }
    }
}
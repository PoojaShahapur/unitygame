using UnityEngine;

namespace UnitySteer2D
{
    public struct PathRelativePosition
    {
        public float Outside;

        public Vector2 Tangent;

        public int SegmentIndex;
    }

    public interface IPathway
    {
        float TotalPathLength { get; }

        Vector2 FirstPoint { get; }
        Vector2 LastPoint { get; }

        int SegmentCount { get; }
        float Radius { get; set; }

        Vector2 MapPointToPath(Vector2 point, ref PathRelativePosition pathRelative);

        Vector2 MapPathDistanceToPoint(float pathDistance);

        float MapPointToPathDistance(Vector2 point);

        bool IsInsidePath(Vector2 point);

        float HowFarOutsidePath(Vector2 point);

        void DrawGizmos();
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnitySteer2D
{
    public class Vector2Pathway : IPathway
    {
        protected IList<float> Lengths
        {
            get;
            private set;
        }

        protected IList<Vector2> Normals
        {
            get;
            private set;
        }

        public IList<Vector2> Path
        {
            get;
            protected set;
        }

        public Vector2 FirstPoint
        {
            get
            {
                return Path.FirstOrDefault();
            }
        }

        public Vector2 LastPoint
        {
            get
            {
                return Path.LastOrDefault();
            }
        }

        public float TotalPathLength
        {
            get; protected set;
        }

        public int SegmentCount
        {
            get
            {
                return Path.Count;
            }
        }

        public float Radius { get; set; }

        public Vector2Pathway()
        {
            Lengths = null;
            Normals = null;
        }

        public Vector2Pathway(IList<Vector2> path, float radius)
        {
            Initialize(path, radius);
        }

        public void Initialize(IList<Vector2> path, float radius)
        {
            Path = new List<Vector2>(path);
            Radius = radius;

            PrecalculatePathData();
        }

        protected virtual void PrecalculatePathData()
        {
            var pointCount = Path.Count;
            TotalPathLength = 0;

            Lengths = new List<float>(pointCount);
            Normals = new List<Vector2>(pointCount);

            Lengths.Add(0);
            Normals.Add(Vector2.zero);

            for (var i = 1; i < pointCount; i++)
            {
                var normal = Path[i] - Path[i - 1];
                var length = normal.magnitude;
                Lengths.Add(length);
                Normals.Add(normal / length);
                TotalPathLength += length;
            }
        }

        public virtual Vector2 MapPointToPath(Vector2 point, ref PathRelativePosition pathRelative)
        {
            var minDistance = float.MaxValue;
            var onPath = Vector2.zero;

            pathRelative.SegmentIndex = -1;

            for (var i = 1; i < Path.Count; i++)
            {
                var segmentLength = Lengths[i];
                var segmentNormal = Normals[i];
                var chosenPoint = Vector2.zero;
                var d = OpenSteerUtility.PointToSegmentDistance(point, Path[i - 1], Path[i],
                    segmentNormal, segmentLength,
                    ref chosenPoint);
                if (!(d < minDistance)) continue;
                minDistance = d;
                onPath = chosenPoint;
                pathRelative.Tangent = segmentNormal;
                pathRelative.SegmentIndex = i;
            }

            pathRelative.Outside = (onPath - point).magnitude - Radius;

            return onPath;
        }

        public virtual float MapPointToPathDistance(Vector2 point)
        {
            if (Path.Count < 2)
                return 0;

            var minDistance = float.MaxValue;
            float segmentLengthTotal = 0;
            float pathDistance = 0;

            for (var i = 1; i < Path.Count; i++)
            {
                var segmentProjection = 0f;
                var segmentLength = Lengths[i];
                var segmentNormal = Normals[i];
                var d = OpenSteerUtility.PointToSegmentDistance(point, Path[i - 1], Path[i],
                    segmentNormal, segmentLength,
                    ref segmentProjection);
                if (d < minDistance)
                {
                    minDistance = d;
                    pathDistance = segmentLengthTotal + segmentProjection;
                }
                segmentLengthTotal += segmentLength;
            }

            return pathDistance;
        }

        public virtual Vector2 MapPathDistanceToPoint(float pathDistance)
        {
            var remaining = pathDistance;
            if (pathDistance < 0)
                return Path.First();
            if (pathDistance >= TotalPathLength)
                return Path.Last();

            var result = Vector2.zero;
            for (var i = 1; i < Path.Count; i++)
            {
                var segmentLength = Lengths[i];
                if (segmentLength < remaining)
                {
                    remaining -= segmentLength;
                }
                else
                {
                    var ratio = remaining / segmentLength;
                    result = Vector2.Lerp(Path[i - 1], Path[i], ratio);
                    break;
                }
            }
            return result;
        }

        public bool IsInsidePath(Vector2 point)
        {
            var tStruct = new PathRelativePosition();

            MapPointToPath(point, ref tStruct);
            return tStruct.Outside < 0;
        }

        public float HowFarOutsidePath(Vector2 point)
        {
            var tStruct = new PathRelativePosition();

            MapPointToPath(point, ref tStruct);
            return tStruct.Outside;
        }

        public virtual void DrawGizmos()
        {
            for (var i = 0; i < Path.Count - 1; i++)
            {
                Debug.DrawLine(Path[i], Path[i + 1], Color.green);
            }
        }
    }
}
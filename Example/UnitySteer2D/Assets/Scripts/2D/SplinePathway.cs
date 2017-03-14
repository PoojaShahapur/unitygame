using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnitySteer2D
{
    public class SplinePathway : Vector2Pathway
    {
        private Vector2[] _splineNodes;

        private int _pathDrawResolution = 50;

        public SplinePathway()
        {
        }
        
        public SplinePathway(IList<Vector2> path, float radius) : base(path, radius)
        {
        }

        protected override void PrecalculatePathData()
        {
            base.PrecalculatePathData();
            var splineNodeLength = Path.Count + 2;
            _splineNodes = new Vector2[splineNodeLength];

            _splineNodes[0] = Path[0] - Normals[1] * 2;
            for (var i = 0; i < Path.Count; i++)
            {
                _splineNodes[i + 1] = Path[i];
            }
            _splineNodes[splineNodeLength - 1] = Path.Last() + Normals.Last() * 4;
        }

        public override Vector2 MapPointToPath(Vector2 point, ref PathRelativePosition pathRelative)
        {
            var onPath = base.MapPointToPath(point, ref pathRelative);

            var distance = MapPointToPathDistance(onPath) / TotalPathLength;
            var splinePoint = CalculateCatmullRomPoint(1, distance);

            return splinePoint;
        }

        public override Vector2 MapPathDistanceToPoint(float pathDistance)
        {
            if (_splineNodes.Length < 5)
            {
                return base.MapPathDistanceToPoint(pathDistance);
            }

            pathDistance = Mathf.Min(TotalPathLength, pathDistance);

            var nodeForDistance = 0;
            var lastTotal = 0f;
            var totalLength = 0f;

            for (var i = 1; i < Lengths.Count && nodeForDistance == 0; i++)
            {
                lastTotal = totalLength;
                totalLength += Lengths[i];
                if (totalLength >= pathDistance)
                {
                    nodeForDistance = i;
                }
            }

            var segmentLength = Lengths[nodeForDistance];
            var remainingLength = pathDistance - lastTotal;
            var pctComplete = Mathf.Approximately(segmentLength, 0) ? 1 : (remainingLength / segmentLength);

            return CalculateCatmullRomPoint(nodeForDistance, pctComplete);
        }

        private Vector2 CalculateCatmullRomPoint(int currentNode, float percentComplete)
        {
            var percentCompleteSquared = percentComplete * percentComplete;
            var percentCompleteCubed = percentCompleteSquared * percentComplete;

            var start = _splineNodes[currentNode];
            var end = _splineNodes[currentNode + 1];
            var previous = _splineNodes[currentNode - 1];
            var next = _splineNodes[currentNode + 2];

            return previous * (-0.5f * percentCompleteCubed + percentCompleteSquared - 0.5f * percentComplete) +
                   start * (1.5f * percentCompleteCubed - 2.5f * percentCompleteSquared + 1.0f) +
                   end * (-1.5f * percentCompleteCubed + 2.0f * percentCompleteSquared + 0.5f * percentComplete) +
                   next * (0.5f * percentCompleteCubed - 0.5f * percentCompleteSquared);
        }

        public override void DrawGizmos()
        {
            Debug.DrawLine(_splineNodes[0], Path[0], Color.gray);
            var lastPosition = Path[0];
            for (var i = 0; i < Path.Count - 1; i++)
            {
                for (var segment = 0; segment < _pathDrawResolution; segment++)
                {
                    var nextPosition = CalculateCatmullRomPoint(i + 1, segment / (float) _pathDrawResolution);
                    Debug.DrawLine(lastPosition, nextPosition, Color.green);
                    lastPosition = nextPosition;
                }
            }
            Debug.DrawLine(lastPosition, _splineNodes.Last(), Color.gray);
        }
    }
}
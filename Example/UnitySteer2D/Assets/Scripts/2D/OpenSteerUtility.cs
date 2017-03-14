using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnitySteer2D
{
    public static class OpenSteerUtility
    {
        public static Vector2 RandomUnitVectorOnXZPlane()
        {
            var tVector = Random.insideUnitSphere;
            tVector.y = 0;
            tVector.Normalize();
            return tVector;
        }

        public static Vector2 LimitMaxDeviationAngle(Vector2 source, float cosineOfConeAngle, Vector2 basis)
        {
            return VecLimitDeviationAngleUtility(true, // force source INSIDE cone
                source,
                cosineOfConeAngle,
                basis);
        }

        public static Vector2 VecLimitDeviationAngleUtility(bool insideOrOutside, Vector2 source,
            float cosineOfConeAngle, Vector2 basis)
        {
            var sourceLength = source.magnitude;
            if (sourceLength == 0) return source;

            var direction = source / sourceLength;
            var cosineOfSourceAngle = Vector2.Dot(direction, basis);

            if (insideOrOutside)
            {
                if (cosineOfSourceAngle >= cosineOfConeAngle) return source;
            }
            else
            {
                if (cosineOfSourceAngle <= cosineOfConeAngle) return source;
            }

            var perp = PerpendicularComponent(source, basis);

            var perpDist = (float) Math.Sqrt(1 - (cosineOfConeAngle * cosineOfConeAngle));
            var c0 = basis * cosineOfConeAngle;
            var c1 = perp.normalized * perpDist;
            return (c0 + c1) * sourceLength;
        }

        public static Vector2 ParallelComponent(Vector2 source, Vector2 unitBasis)
        {
            var projection = Vector2.Dot(source, unitBasis);
            return unitBasis * projection;
        }

        public static Vector2 PerpendicularComponent(Vector2 source, Vector2 unitBasis)
        {
            return source - ParallelComponent(source, unitBasis);
        }

        public static Vector2 SphericalWrapAround(Vector2 source, Vector2 center, float radius)
        {
            var offset = source - center;
            var r = offset.magnitude;

            var result = (r > radius) ? source + ((offset / r) * radius * -2) : source;
            return result;
        }

        public static float ScalarRandomWalk(float initial, float walkSpeed, float min, float max)
        {
            var next = initial + ((Random.value * 2 - 1) * walkSpeed);
            next = Mathf.Clamp(next, min, max);
            return next;
        }

        public static int IntervalComparison(float x, float lowerBound, float upperBound)
        {
            if (x < lowerBound) return -1;
            if (x > upperBound) return +1;
            return 0;
        }

        public static float PointToSegmentDistance(Vector2 point, Vector2 ep0, Vector2 ep1,
            ref float segmentProjection)
        {
            var cp = Vector2.zero;
            return PointToSegmentDistance(point, ep0, ep1, ref cp, ref segmentProjection);
        }

        public static float PointToSegmentDistance(Vector2 point, Vector2 ep0, Vector2 ep1,
            ref Vector2 chosenPoint)
        {
            float sp = 0;
            return PointToSegmentDistance(point, ep0, ep1, ref chosenPoint, ref sp);
        }

        public static float PointToSegmentDistance(Vector2 point, Vector2 ep0, Vector2 ep1,
            ref Vector2 chosenPoint,
            ref float segmentProjection)
        {
            var normal = ep1 - ep0;
            var length = normal.magnitude;
            normal *= 1 / length;

            return PointToSegmentDistance(point, ep0, ep1, normal, length,
                ref chosenPoint, ref segmentProjection);
        }

        public static float PointToSegmentDistance(Vector2 point, Vector2 ep0, Vector2 ep1,
            Vector2 segmentNormal, float segmentLength,
            ref float segmentProjection)
        {
            var cp = Vector2.zero;
            return PointToSegmentDistance(point, ep0, ep1, segmentNormal, segmentLength,
                ref cp, ref segmentProjection);
        }

        public static float PointToSegmentDistance(Vector2 point, Vector2 ep0, Vector2 ep1,
            Vector2 segmentNormal, float segmentLength,
            ref Vector2 chosenPoint)
        {
            float sp = 0;
            return PointToSegmentDistance(point, ep0, ep1, segmentNormal, segmentLength,
                ref chosenPoint, ref sp);
        }

        public static float PointToSegmentDistance(Vector2 point, Vector2 ep0, Vector2 ep1,
            Vector2 segmentNormal, float segmentLength,
            ref Vector2 chosenPoint,
            ref float segmentProjection)
        {
            var local = point - ep0;

            segmentProjection = Vector2.Dot(segmentNormal, local);

            if (segmentProjection < 0)
            {
                chosenPoint = ep0;
                segmentProjection = 0;
                return (point - ep0).magnitude;
            }
            if (segmentProjection > segmentLength)
            {
                chosenPoint = ep1;
                segmentProjection = segmentLength;
                return (point - ep1).magnitude;
            }

            chosenPoint = segmentNormal * segmentProjection;
            chosenPoint += ep0;
            return Vector2.Distance(point, chosenPoint);
        }

        public static float CosFromDegrees(float angle)
        {
            return Mathf.Cos(angle * Mathf.Deg2Rad);
        }

        public static float DegreesFromCos(float cos)
        {
            return Mathf.Rad2Deg * Mathf.Acos(cos);
        }
    }
}
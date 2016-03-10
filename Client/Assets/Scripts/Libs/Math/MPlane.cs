namespace SDK.Lib
{
    public struct MPlane
    {
        public enum Side
        {
            NO_SIDE,
            POSITIVE_SIDE,
            NEGATIVE_SIDE,
            BOTH_SIDE
        }

        public MVector3 normal;
        public float d;

        public MPlane(int dist = 0)
        {
            normal = MVector3.ZERO;
            d = 0.0f;
        }

        public MPlane(ref MPlane rhs)
        {
            normal = rhs.normal;
            d = rhs.d;
        }

        public MPlane(ref MVector3 rkNormal, float fConstant)
        {
            normal = rkNormal;
            d = -fConstant;
        }

        public MPlane(float a, float b, float c, float _d)
        {
            normal = new MVector3(a, b, c);
            d = _d;
        }

        public MPlane(ref MVector3 rkNormal, ref MVector3 rkPoint)
        {
            normal = MVector3.ZERO;
            d = 0.0f;
            redefine(ref rkNormal, ref rkPoint);
        }

        public MPlane(ref MVector3 rkPoint0, ref MVector3 rkPoint1,
            ref MVector3 rkPoint2)
        {
            normal = MVector3.ZERO;
            d = 0.0f;
            redefine(ref rkPoint0, ref rkPoint1, ref rkPoint2);
        }

        public float getDistance(ref MVector3 rkPoint)
        {
            return normal.dotProduct(ref rkPoint) + d;
        }

        public Side getSide(ref MVector3 rkPoint)
        {
            float fDistance = getDistance(ref rkPoint);

            if (fDistance < 0.0)
                return Side.NEGATIVE_SIDE;

            if (fDistance > 0.0)
                return Side.POSITIVE_SIDE;

            return Side.NO_SIDE;
        }

        public Side getSide(ref MAxisAlignedBox box)
        {
            if (box.isNull())
                return Side.NO_SIDE;
            if (box.isInfinite())
                return Side.BOTH_SIDE;

            MVector3 center = box.getCenter();
            MVector3 half = box.getHalfSize();
            return getSide(ref center, ref half);
        }

        public Side getSide(ref MVector3 centre, ref MVector3 halfSize)
        {
            float dist = getDistance(ref centre);

            float maxAbsDist = normal.absDotProduct(ref halfSize);

            if (dist < -maxAbsDist)
                return Side.NEGATIVE_SIDE;

            if (dist > +maxAbsDist)
                return Side.POSITIVE_SIDE;

            return Side.BOTH_SIDE;
        }

        public void redefine(ref MVector3 rkPoint0, ref MVector3 rkPoint1,
            ref MVector3 rkPoint2)
        {
            MVector3 kEdge1 = rkPoint1 - rkPoint0;
            MVector3 kEdge2 = rkPoint2 - rkPoint0;
            normal = kEdge1.crossProduct(ref kEdge2);
            normal.normalise();
            d = -normal.dotProduct(ref rkPoint0);
        }

        public void redefine(ref MVector3 rkNormal, ref MVector3 rkPoint)
        {
            normal = rkNormal;
            d = -rkNormal.dotProduct(ref rkPoint);
        }

        public MVector3 projectVector(ref MVector3 p)
        {
            MMatrix3 xform = new MMatrix3(0);
            xform[0, 0] = 1.0f - normal.x * normal.x;
            xform[0, 1] = -normal.x * normal.y;
            xform[0, 2] = -normal.x * normal.z;
            xform[1, 0] = -normal.y * normal.x;
            xform[1, 1] = 1.0f - normal.y * normal.y;
            xform[1, 2] = -normal.y * normal.z;
            xform[2, 0] = -normal.z * normal.x;
            xform[2, 1] = -normal.z * normal.y;
            xform[2, 2] = 1.0f - normal.z * normal.z;
            return xform * p;

        }

        public float normalise()
        {
            float fLength = normal.length();

            if (fLength > (float)(0.0f))
            {
                float fInvLength = 1.0f / fLength;
                normal *= fInvLength;
                d *= fInvLength;
            }

            return fLength;
        }

        static public bool operator ==(MPlane lhs, MPlane rhs)
        {
            return (rhs.d == lhs.d && rhs.normal == lhs.normal);
        }
        static public bool operator !=(MPlane lhs, MPlane rhs)
        {
            return (rhs.d != lhs.d || rhs.normal != lhs.normal);
        }

        override public string ToString()
        {
            string o = "Plane( normal x = " + this.normal.x + ", y = " + this.normal.y + ", z = " + this.normal.z + ", d = " + this.d + " )";
            return o;
        }
    }
}
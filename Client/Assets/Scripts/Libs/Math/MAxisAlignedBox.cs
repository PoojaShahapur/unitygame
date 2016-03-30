namespace SDK.Lib
{
    public struct MAxisAlignedBox
    {
        public static MAxisAlignedBox BOX_NULL;
        public static MAxisAlignedBox BOX_INFINITE;

        public enum Extent
        {
            EXTENT_NULL,
            EXTENT_FINITE,
            EXTENT_INFINITE
        }

        public MVector3 mMinimum;
        public MVector3 mMaximum;
        public Extent mExtent;
        public MVector3[] mCorners;

        public enum CornerEnum
        {
            FAR_LEFT_BOTTOM = 0,
            FAR_LEFT_TOP = 1,
            FAR_RIGHT_TOP = 2,
            FAR_RIGHT_BOTTOM = 3,
            NEAR_RIGHT_BOTTOM = 7,
            NEAR_LEFT_BOTTOM = 6,
            NEAR_LEFT_TOP = 5,
            NEAR_RIGHT_TOP = 4
        }

        public MAxisAlignedBox(Extent e = Extent.EXTENT_FINITE)
        {
            mMinimum = MVector3.ZERO;
            mMaximum = MVector3.UNIT_SCALE;
            mCorners = new MVector3[8];
            mExtent = e;
            setMinimum(-0.5f, -0.5f, -0.5f);
            setMaximum(0.5f, 0.5f, 0.5f);
            mExtent = e;
        }

        public MAxisAlignedBox(ref MAxisAlignedBox rkBox)
        {
            mMinimum = MVector3.ZERO;
            mMaximum = MVector3.UNIT_SCALE;
            mCorners = new MVector3[8];
            mExtent = Extent.EXTENT_FINITE;

            if (rkBox.isNull())
                setNull();
            else if (rkBox.isInfinite())
                setInfinite();
            else
                setExtents(ref rkBox.mMinimum, ref rkBox.mMaximum);
        }

        public MAxisAlignedBox(ref MVector3 min, ref MVector3 max)
        {
            mMinimum = MVector3.ZERO;
            mMaximum = MVector3.UNIT_SCALE;
            mCorners = new MVector3[8];
            mExtent = Extent.EXTENT_FINITE;
            setExtents(ref min, ref max);
        }

        public MAxisAlignedBox(
            float mx, float my, float mz,
            float Mx, float My, float Mz)
        {
            mMinimum = MVector3.ZERO;
            mMaximum = MVector3.UNIT_SCALE;
            mCorners = new MVector3[8];
            mExtent = Extent.EXTENT_FINITE;
            setExtents(mx, my, mz, Mx, My, Mz);
        }

        public MAxisAlignedBox assignFrom(ref MAxisAlignedBox rhs)
        {
            if (rhs.isNull())
                setNull();
            else if (rhs.isInfinite())
                setInfinite();
            else
                setExtents(ref rhs.mMinimum, ref rhs.mMaximum);

            return this;
        }

        public MVector3 getMinimum()
        {
            return mMinimum;
        }

        public MVector3 getMaximum()
        {
            return mMaximum;
        }

        public void setMinimum(MVector3 vec)
        {
            mExtent = Extent.EXTENT_FINITE;
            mMinimum = vec;
        }

        public void setMinimum(float x, float y, float z)
        {
            mExtent = Extent.EXTENT_FINITE;
            mMinimum.x = x;
            mMinimum.y = y;
            mMinimum.z = z;
        }

        public void setMinimumX(float x)
        {
            mMinimum.x = x;
        }

        public void setMinimumY(float y)
        {
            mMinimum.y = y;
        }

        public void setMinimumZ(float z)
        {
            mMinimum.z = z;
        }

        public void setMaximum(MVector3 vec)
        {
            mExtent = Extent.EXTENT_FINITE;
            mMaximum = vec;
        }

        public void setMaximum(float x, float y, float z)
        {
            mExtent = Extent.EXTENT_FINITE;
            mMaximum.x = x;
            mMaximum.y = y;
            mMaximum.z = z;
        }

        public void setMaximumX(float x)
        {
            mMaximum.x = x;
        }

        public void setMaximumY(float y)
        {
            mMaximum.y = y;
        }

        public void setMaximumZ(float z)
        {
            mMaximum.z = z;
        }

        public void setExtents(ref MVector3 min, ref MVector3 max)
        {
            UtilApi.assert((min.x <= max.x && min.y <= max.y && min.z <= max.z),
                "The minimum corner of the box must be less than or equal to maximum corner");

            mExtent = Extent.EXTENT_FINITE;
            mMinimum.assignFrom(ref min);
            mMaximum.assignFrom(ref max);
        }

        public void setExtents(
            float mx, float my, float mz,
            float Mx, float My, float Mz)
        {
            UtilApi.assert((mx <= Mx && my <= My && mz <= Mz),
                "The minimum corner of the box must be less than or equal to maximum corner");

            mExtent = Extent.EXTENT_FINITE;

            mMinimum.x = mx;
            mMinimum.y = my;
            mMinimum.z = mz;

            mMaximum.x = Mx;
            mMaximum.y = My;
            mMaximum.z = Mz;
        }

        public MVector3[] getAllCorners()
        {
            UtilApi.assert((mExtent == Extent.EXTENT_FINITE), "Can't get corners of a null or infinite AAB");

            if (mCorners == null)
                mCorners = new MVector3[8];

            mCorners[0] = mMinimum;
            mCorners[1].x = mMinimum.x; mCorners[1].y = mMaximum.y; mCorners[1].z = mMinimum.z;
            mCorners[2].x = mMaximum.x; mCorners[2].y = mMaximum.y; mCorners[2].z = mMinimum.z;
            mCorners[3].x = mMaximum.x; mCorners[3].y = mMinimum.y; mCorners[3].z = mMinimum.z;

            mCorners[4] = mMaximum;
            mCorners[5].x = mMinimum.x; mCorners[5].y = mMaximum.y; mCorners[5].z = mMaximum.z;
            mCorners[6].x = mMinimum.x; mCorners[6].y = mMinimum.y; mCorners[6].z = mMaximum.z;
            mCorners[7].x = mMaximum.x; mCorners[7].y = mMinimum.y; mCorners[7].z = mMaximum.z;

            return mCorners;
        }

        public MVector3 getCorner(CornerEnum cornerToGet)
        {
            switch (cornerToGet)
            {
                case CornerEnum.FAR_LEFT_BOTTOM:
                    return mMinimum;
                case CornerEnum.FAR_LEFT_TOP:
                    return new MVector3(mMinimum.x, mMaximum.y, mMinimum.z);
                case CornerEnum.FAR_RIGHT_TOP:
                    return new MVector3(mMaximum.x, mMaximum.y, mMinimum.z);
                case CornerEnum.FAR_RIGHT_BOTTOM:
                    return new MVector3(mMaximum.x, mMinimum.y, mMinimum.z);
                case CornerEnum.NEAR_RIGHT_BOTTOM:
                    return new MVector3(mMaximum.x, mMinimum.y, mMaximum.z);
                case CornerEnum.NEAR_LEFT_BOTTOM:
                    return new MVector3(mMinimum.x, mMinimum.y, mMaximum.z);
                case CornerEnum.NEAR_LEFT_TOP:
                    return new MVector3(mMinimum.x, mMaximum.y, mMaximum.z);
                case CornerEnum.NEAR_RIGHT_TOP:
                    return mMaximum;
                default:
                    return new MVector3(0, 0, 0);
            }
        }

        public string ToString(ref MAxisAlignedBox aab)
        {
            string o = "";
            switch (aab.mExtent)
            {
                case Extent.EXTENT_NULL:
                    o += "AxisAlignedBox(null)";
                    return o;

                case Extent.EXTENT_FINITE:
                    o += ("AxisAlignedBox(min=" + aab.mMinimum + ", max=" + aab.mMaximum + ")");
                    return o;

                case Extent.EXTENT_INFINITE:
                    o += "AxisAlignedBox(infinite)";
                    return o;

                default:
                    UtilApi.assert(false, "Never reached");
                    return o;
            }
        }

        public void merge(MAxisAlignedBox rhs)
        {
            if ((rhs.mExtent == Extent.EXTENT_NULL) || (mExtent == Extent.EXTENT_INFINITE))
            {
                return;
            }
            else if (rhs.mExtent == Extent.EXTENT_INFINITE)
            {
                mExtent = Extent.EXTENT_INFINITE;
            }
            else if (mExtent == Extent.EXTENT_NULL)
            {
                setExtents(ref rhs.mMinimum, ref rhs.mMaximum);
            }
            else
            {
                MVector3 min = mMinimum;
                MVector3 max = mMaximum;
                max.makeCeil(rhs.mMaximum);
                min.makeFloor(rhs.mMinimum);

                setExtents(ref min, ref max);
            }
        }

        public void merge(ref MVector3 point)
        {
            switch (mExtent)
            {
                case Extent.EXTENT_NULL:
                    setExtents(ref point, ref point);
                    return;

                case Extent.EXTENT_FINITE:
                    mMaximum.makeCeil(point);
                    mMinimum.makeFloor(point);
                    return;

                case Extent.EXTENT_INFINITE:
                    return;
            }

            UtilApi.assert(false, "Never reached");
        }

        public void transform(ref MMatrix4 matrix)
        {
            if (mExtent != Extent.EXTENT_FINITE)
                return;

            MVector3 oldMin, oldMax, currentCorner;

            oldMin = mMinimum;
            oldMax = mMaximum;

            setNull();
            MVector3 tranVec;

            currentCorner = oldMin;
            tranVec = matrix * currentCorner;
            merge(ref tranVec);

            currentCorner.z = oldMax.z;
            tranVec = matrix * currentCorner;
            merge(ref tranVec);

            currentCorner.y = oldMax.y;
            tranVec = matrix * currentCorner;
            merge(ref tranVec);

            currentCorner.z = oldMin.z;
            tranVec = matrix * currentCorner;
            merge(ref tranVec);

            currentCorner.x = oldMax.x;
            tranVec = matrix * currentCorner;
            merge(ref tranVec);

            currentCorner.z = oldMax.z;
            tranVec = matrix * currentCorner;
            merge(ref tranVec);

            currentCorner.y = oldMin.y;
            tranVec = matrix * currentCorner;
            merge(ref tranVec);

            currentCorner.z = oldMin.z;
            tranVec = matrix * currentCorner;
            merge(ref tranVec);
        }

        public void transformAffine(ref MMatrix4 m)
        {
            UtilApi.assert(m.isAffine());

            if (mExtent != Extent.EXTENT_FINITE)
                return;

            MVector3 centre = getCenter();
            MVector3 halfSize = getHalfSize();

            MVector3 newCentre = m.transformAffine(centre);
            MVector3 newHalfSize = new MVector3(
                UtilMath.Abs(m[0, 0]) * halfSize.x + UtilMath.Abs(m[0, 1]) * halfSize.y + UtilMath.Abs(m[0, 2]) * halfSize.z,
                UtilMath.Abs(m[1, 0]) * halfSize.x + UtilMath.Abs(m[1, 1]) * halfSize.y + UtilMath.Abs(m[1, 2]) * halfSize.z,
                UtilMath.Abs(m[2, 0]) * halfSize.x + UtilMath.Abs(m[2, 1]) * halfSize.y + UtilMath.Abs(m[2, 2]) * halfSize.z);

            MVector3 min = newCentre - newHalfSize;
            MVector3 max = newCentre + newHalfSize;
            setExtents(ref min, ref max);
        }

        public void setNull()
        {
            mExtent = Extent.EXTENT_NULL;
        }

        public bool isNull()
        {
            return (mExtent == Extent.EXTENT_NULL);
        }

        public bool isFinite()
        {
            return (mExtent == Extent.EXTENT_FINITE);
        }

        public void setInfinite()
        {
            mExtent = Extent.EXTENT_INFINITE;
        }

        public bool isInfinite()
        {
            return (mExtent == Extent.EXTENT_INFINITE);
        }

        public bool intersects(ref MAxisAlignedBox b2)
        {
            if (this.isNull() || b2.isNull())
                return false;

            if (this.isInfinite() || b2.isInfinite())
                return true;

            if (mMaximum.x < b2.mMinimum.x)
                return false;
            if (mMaximum.y < b2.mMinimum.y)
                return false;
            if (mMaximum.z < b2.mMinimum.z)
                return false;

            if (mMinimum.x > b2.mMaximum.x)
                return false;
            if (mMinimum.y > b2.mMaximum.y)
                return false;
            if (mMinimum.z > b2.mMaximum.z)
                return false;

            return true;
        }

        public MAxisAlignedBox intersection(ref MAxisAlignedBox b2)
        {
            if (this.isNull() || b2.isNull())
            {
                return new MAxisAlignedBox();
            }
            else if (this.isInfinite())
            {
                return b2;
            }
            else if (b2.isInfinite())
            {
                return this;
            }

            MVector3 intMin = mMinimum;
            MVector3 intMax = mMaximum;

            intMin.makeCeil(b2.getMinimum());
            intMax.makeFloor(b2.getMaximum());

            if (intMin.x < intMax.x &&
                intMin.y < intMax.y &&
                intMin.z < intMax.z)
            {
                return new MAxisAlignedBox(ref intMin, ref intMax);
            }

            return new MAxisAlignedBox();
        }

        public float volume()
        {
            switch (mExtent)
            {
                case Extent.EXTENT_NULL:
                    return 0.0f;

                case Extent.EXTENT_FINITE:
                    {
                        MVector3 diff = mMaximum - mMinimum;
                        return diff.x * diff.y * diff.z;
                    }

                case Extent.EXTENT_INFINITE:
                    return UtilMath.POS_INFINITY;

                default:
                    UtilApi.assert(false, "Never reached");
                    return 0.0f;
            }
        }

        public void scale(ref MVector3 s)
        {
            if (mExtent != Extent.EXTENT_FINITE)
                return;

            MVector3 min = mMinimum * s;
            MVector3 max = mMaximum * s;
            setExtents(ref min, ref max);
        }

        public bool intersects(ref MPlane p)
        {
            return UtilMath.intersects(p, this);
        }

        public bool intersects(ref MVector3 v)
        {
            switch (mExtent)
            {
                case Extent.EXTENT_NULL:
                    return false;

                case Extent.EXTENT_FINITE:
                    return (v.x >= mMinimum.x && v.x <= mMaximum.x &&
                        v.y >= mMinimum.y && v.y <= mMaximum.y &&
                        v.z >= mMinimum.z && v.z <= mMaximum.z);

                case Extent.EXTENT_INFINITE:
                    return true;

                default:
                    UtilApi.assert(false, "Never reached");
                    return false;
            }
        }

        public MVector3 getCenter()
        {
            UtilApi.assert((mExtent == Extent.EXTENT_FINITE), "Can't get center of a null or infinite AAB");

            return new MVector3(
                (mMaximum.x + mMinimum.x) * 0.5f,
                (mMaximum.y + mMinimum.y) * 0.5f,
                (mMaximum.z + mMinimum.z) * 0.5f);
        }

        public MVector3 getSize()
        {
            switch (mExtent)
            {
                case Extent.EXTENT_NULL:
                    return MVector3.ZERO;

                case Extent.EXTENT_FINITE:
                    return mMaximum - mMinimum;

                case Extent.EXTENT_INFINITE:
                    return new MVector3(
                        UtilMath.POS_INFINITY,
                        UtilMath.POS_INFINITY,
                        UtilMath.POS_INFINITY);

                default:
                    UtilApi.assert(false, "Never reached");
                    return MVector3.ZERO;
            }
        }

        public MVector3 getHalfSize()
        {
            switch (mExtent)
            {
                case Extent.EXTENT_NULL:
                    return MVector3.ZERO;

                case Extent.EXTENT_FINITE:
                    return (mMaximum - mMinimum) * 0.5f;

                case Extent.EXTENT_INFINITE:
                    return new MVector3(
                        UtilMath.POS_INFINITY,
                        UtilMath.POS_INFINITY,
                        UtilMath.POS_INFINITY);

                default:
                    UtilApi.assert(false, "Never reached");
                    return MVector3.ZERO;
            }
        }

        public bool contains(ref MVector3 v)
        {
            if (isNull())
                return false;
            if (isInfinite())
                return true;

            return mMinimum.x <= v.x && v.x <= mMaximum.x &&
                   mMinimum.y <= v.y && v.y <= mMaximum.y &&
                   mMinimum.z <= v.z && v.z <= mMaximum.z;
        }

        public float squaredDistance(ref MVector3 v)
        {
            if (this.contains(ref v))
                return 0;
            else
            {
                MVector3 maxDist = new MVector3(0, 0, 0);

                if (v.x < mMinimum.x)
                    maxDist.x = mMinimum.x - v.x;
                else if (v.x > mMaximum.x)
                    maxDist.x = v.x - mMaximum.x;

                if (v.y < mMinimum.y)
                    maxDist.y = mMinimum.y - v.y;
                else if (v.y > mMaximum.y)
                    maxDist.y = v.y - mMaximum.y;

                if (v.z < mMinimum.z)
                    maxDist.z = mMinimum.z - v.z;
                else if (v.z > mMaximum.z)
                    maxDist.z = v.z - mMaximum.z;

                return maxDist.squaredLength();
            }
        }

        public float distance(ref MVector3 v)
        {
            return UtilMath.Sqrt(squaredDistance(ref v));
        }

        bool contains(ref MAxisAlignedBox other)
        {
            if (other.isNull() || this.isInfinite())
                return true;

            if (this.isNull() || other.isInfinite())
                return false;

            return this.mMinimum.x <= other.mMinimum.x &&
                   this.mMinimum.y <= other.mMinimum.y &&
                   this.mMinimum.z <= other.mMinimum.z &&
                   other.mMaximum.x <= this.mMaximum.x &&
                   other.mMaximum.y <= this.mMaximum.y &&
                   other.mMaximum.z <= this.mMaximum.z;
        }

        static public bool operator ==(MAxisAlignedBox lhs, MAxisAlignedBox rhs)
        {
            if (lhs.mExtent != rhs.mExtent)
                return false;

            if (!lhs.isFinite())
                return true;

            return lhs.mMinimum == rhs.mMinimum &&
                   lhs.mMaximum == rhs.mMaximum;
        }

        static public bool operator !=(MAxisAlignedBox lhs, MAxisAlignedBox rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object other)
        {
            if (System.Object.ReferenceEquals(this, other))
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode();
        }

        public void addPoint(float x, float y, float z)
        {
            if(x < this.mMinimum.x)
            {
                this.mMinimum.x = x;
            }
            if (y < this.mMinimum.y)
            {
                this.mMinimum.y = y;
            }
            if (z < this.mMinimum.z)
            {
                this.mMinimum.z = z;
            }

            if (x > this.mMaximum.x)
            {
                this.mMaximum.x = x;
            }
            if (y > this.mMaximum.y)
            {
                this.mMaximum.y = y;
            }
            if (z > this.mMaximum.z)
            {
                this.mMaximum.z = z;
            }
        }
    }
}
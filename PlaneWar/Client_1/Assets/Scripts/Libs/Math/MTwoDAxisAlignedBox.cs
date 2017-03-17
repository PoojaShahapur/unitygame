using UnityEngine;

namespace SDK.Lib
{
    public class MTwoDAxisAlignedBox
    {
        public static MTwoDAxisAlignedBox BOX_NULL = new MTwoDAxisAlignedBox();
        public static MTwoDAxisAlignedBox BOX_INFINITE = new MTwoDAxisAlignedBox();

        public enum Extent
        {
            EXTENT_NULL,
            EXTENT_FINITE,
            EXTENT_INFINITE
        }

        public MVector2 mMinimum;
        public MVector2 mMaximum;
        public Extent mExtent;
        public MVector2[] mCorners;

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

        public MTwoDAxisAlignedBox()
        {
            mMinimum = MVector2.ZERO;
            mMaximum = MVector2.UNIT_SCALE;
            mCorners = new MVector2[6];

            setMinimum(-0.5f, -0.5f);
            setMaximum(0.5f, 0.5f);
            mExtent = Extent.EXTENT_NULL;
        }

        public MTwoDAxisAlignedBox(Extent e)
        {
            mMinimum = MVector2.ZERO;
            mMaximum = MVector2.UNIT_SCALE;
            mCorners = new MVector2[6];

            setMinimum(-0.5f, -0.5f);
            setMaximum(0.5f, 0.5f);
            mExtent = e;
        }

        public MTwoDAxisAlignedBox(MTwoDAxisAlignedBox rkBox)

        {
            mMinimum = MVector2.ZERO;
            mMaximum = MVector2.UNIT_SCALE;
            mCorners = new MVector2[6];

            if (rkBox.isNull())
                setNull();
            else if (rkBox.isInfinite())
                setInfinite();
            else
                setExtents(rkBox.mMinimum, rkBox.mMaximum);
        }

        public MTwoDAxisAlignedBox(MVector2 min, MVector2 max )
        {
            mMinimum = MVector2.ZERO;
            mMaximum = MVector2.UNIT_SCALE;
            mCorners = new MVector2[6];

            setExtents(min, max);
        }

        public MTwoDAxisAlignedBox(
            float mx, float mz,
            float Mx, float Mz)
        {
            mMinimum = MVector2.ZERO;
            mMaximum = MVector2.UNIT_SCALE;
            mCorners = new MVector2[6];

            setExtents(mx, mz, Mx, Mz);
        }

        public MVector2 getMinimum()
        { 
            return mMinimum; 
        }

        public MVector2 getMaximum()
        { 
            return mMaximum;
        }

        public void setMinimum(MVector2 vec )
        {
            mExtent = Extent.EXTENT_FINITE;
            mMinimum = vec;
        }

        public void setMinimum(float x, float y)
        {
            mExtent = Extent.EXTENT_FINITE;
            mMinimum.x = x;
            mMinimum.y = y;
        }

        public void setMinimumX(float x)
        {
            mMinimum.x = x;
        }

        public void setMinimumY(float y)
        {
            mMinimum.y = y;
        }

        public void setMaximum(MVector2 vec )
        {
            mExtent = Extent.EXTENT_FINITE;
            mMaximum = vec;
        }

        public void setMaximum(float x, float y)
        {
            mExtent = Extent.EXTENT_FINITE;
            mMaximum.x = x;
            mMaximum.y = y;
        }

        public void setMaximumX(float x)
        {
            mMaximum.x = x;
        }

        public void setMaximumY(float y)
        {
            mMaximum.y = y;
        }

        public void setExtents(MVector2 min, MVector2 max )
        {
            UtilApi.assert((min.x <= max.x && min.y <= max.y),
                "The minimum corner of the box must be less than or equal to maximum corner");

            mExtent = Extent.EXTENT_FINITE;
            mMinimum = min;
            mMaximum = max;
        }

        public void setExtents(
            float mx, float my,
            float Mx, float My)
        {
            UtilApi.assert((mx <= Mx && my <= My),
                "The minimum corner of the box must be less than or equal to maximum corner");

            mExtent = Extent.EXTENT_FINITE;

            mMinimum.x = mx;
            mMinimum.y = my;

            mMaximum.x = Mx;
            mMaximum.y = My;
        }

        public MVector2[] getAllCorners()
        {
            UtilApi.assert((mExtent == Extent.EXTENT_FINITE), "Can't get corners of a null or infinite AAB");

            if (mCorners == null)
                mCorners = new MVector2[4];

            mCorners[0] = mMinimum;
            mCorners[1].x = mMinimum.x; mCorners[1].y = mMaximum.y;

            mCorners[2] = mMaximum;
            mCorners[3].x = mMinimum.x; mCorners[5].y = mMaximum.y;

            return mCorners;
        }

        public MVector2 getCorner(CornerEnum cornerToGet)
        {
            switch(cornerToGet)
            {
            case CornerEnum.FAR_LEFT_BOTTOM:
                return mMinimum;
            case CornerEnum.FAR_LEFT_TOP:
                return new MVector2(mMinimum.x, mMaximum.y);
            case CornerEnum.FAR_RIGHT_TOP:
                return new MVector2(mMaximum.x, mMaximum.y);
            case CornerEnum.FAR_RIGHT_BOTTOM:
                return new MVector2(mMaximum.x, mMinimum.y);
            case CornerEnum.NEAR_RIGHT_BOTTOM:
                return new MVector2(mMaximum.x, mMinimum.y);
            case CornerEnum.NEAR_LEFT_BOTTOM:
                return new MVector2(mMinimum.x, mMinimum.y);
            case CornerEnum.NEAR_LEFT_TOP:
                return new MVector2(mMinimum.x, mMaximum.y);
            case CornerEnum.NEAR_RIGHT_TOP:
                return mMaximum;
            default:
                return new MVector2(0, 0);
            }
        }

        public void merge(MTwoDAxisAlignedBox rhs )
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
                setExtents(rhs.mMinimum, rhs.mMaximum);
            }
            else
            {
                MVector2 min = mMinimum;
                MVector2 max = mMaximum;
                max.makeCeil(rhs.mMaximum);
                min.makeFloor(rhs.mMinimum);

                setExtents(min, max);
            }
        }

        public void merge(MVector2 point )
        {
            switch (mExtent)
            {
                case Extent.EXTENT_NULL:
                    setExtents(point, point);
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

        bool isInfinite()
        {
            return (mExtent == Extent.EXTENT_INFINITE);
        }

        public bool intersects(MTwoDAxisAlignedBox b2)
        {
            if (this.isNull() || b2.isNull())
                return false;

            if (this.isInfinite() || b2.isInfinite())
                return true;

            if (mMaximum.x < b2.mMinimum.x)
                return false;
            if (mMaximum.y < b2.mMinimum.y)
                return false;

            if (mMinimum.x > b2.mMaximum.x)
                return false;
            if (mMinimum.y > b2.mMaximum.y)
                return false;

            return true;
        }

        public MTwoDAxisAlignedBox intersection(MTwoDAxisAlignedBox b2)
        {
            if (this.isNull() || b2.isNull())
            {
                return new MTwoDAxisAlignedBox();
            }
            else if (this.isInfinite())
            {
                return b2;
            }
            else if (b2.isInfinite())
            {
                return this;
            }

            MVector2 intMin = mMinimum;
            MVector2 intMax = mMaximum;

            intMin.makeCeil(b2.getMinimum());
            intMax.makeFloor(b2.getMaximum());

            if (intMin.x < intMax.x &&
                intMin.y < intMax.y)
            {
                return new MTwoDAxisAlignedBox(intMin, intMax);
            }

            return new MTwoDAxisAlignedBox();
        }

        public float volume()
        {
            switch (mExtent)
            {
            case Extent.EXTENT_NULL:
                return 0.0f;

            case Extent.EXTENT_FINITE:
                {
                    MVector2 diff = mMaximum - mMinimum;
                    return diff.x* diff.y;
                }

            case Extent.EXTENT_INFINITE:
                return float.MaxValue;

            default:
                UtilApi.assert( false, "Never reached" );
                return 0.0f;
            }
        }

        public MVector2 getCenter()
        {
            UtilApi.assert( (mExtent == Extent.EXTENT_FINITE), "Can't get center of a null or infinite AAB" );

            return new MVector2(
                (mMaximum.x + mMinimum.x) * 0.5f,
                (mMaximum.y + mMinimum.y) * 0.5f);
        }

        public MVector2 getSize()
        {
            switch (mExtent)
            {
            case Extent.EXTENT_NULL:
                return MVector2.ZERO;

            case Extent.EXTENT_FINITE:
                return mMaximum - mMinimum;

            case Extent.EXTENT_INFINITE:
                return new MVector2(
                    float.MaxValue,
                    float.MaxValue);

            default:
                UtilApi.assert( false, "Never reached" );
                return MVector2.ZERO;
            }
        }

        public MVector2 getHalfSize()
        {
            switch (mExtent)
            {
            case Extent.EXTENT_NULL:
                return MVector2.ZERO;

            case Extent.EXTENT_FINITE:
                return (mMaximum - mMinimum) * 0.5f;

            case Extent.EXTENT_INFINITE:
                return new MVector2(
                    float.MaxValue,
                    float.MaxValue);

            default:
                UtilApi.assert( false, "Never reached" );
                return MVector2.ZERO;
            }
        }

        public bool contains(MVector2 v)
        {
            if (isNull())
                return false;
            if (isInfinite())
                return true;

            return mMinimum.x <= v.x && v.x <= mMaximum.x &&
                   mMinimum.y <= v.y && v.y <= mMaximum.y;
        }

        public float squaredDistance(MVector2 v)
        {
            if (this.contains(v))
                return 0;
            else
            {
                MVector2 maxDist = new MVector2(0, 0);

                if (v.x < mMinimum.x)
                    maxDist.x = mMinimum.x - v.x;
                else if (v.x > mMaximum.x)
                    maxDist.x = v.x - mMaximum.x;

                if (v.y < mMinimum.y)
                    maxDist.y = mMinimum.y - v.y;
                else if (v.y > mMaximum.y)
                    maxDist.y = v.y - mMaximum.y;

                return maxDist.squaredLength();
            }
        }

        public float distance(MVector2 v)
        {
            return Mathf.Sqrt(squaredDistance(v));
        }

        public bool contains(MTwoDAxisAlignedBox other)
        {
            if (other.isNull() || this.isInfinite())
                return true;

            if (this.isNull() || other.isInfinite())
                return false;

            return this.mMinimum.x <= other.mMinimum.x &&
                   this.mMinimum.y <= other.mMinimum.y &&
                   other.mMaximum.x <= this.mMaximum.x &&
                   other.mMaximum.y <= this.mMaximum.y;
        }

        static public bool operator ==(MTwoDAxisAlignedBox lhs, MTwoDAxisAlignedBox rhs)
        {
            if (lhs.mExtent != rhs.mExtent)
                return false;

            if (!lhs.isFinite())
                return true;

            return lhs.mMinimum == rhs.mMinimum &&
                   lhs.mMaximum == rhs.mMaximum;
        }

        static public bool operator !=(MTwoDAxisAlignedBox lhs, MTwoDAxisAlignedBox rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object other)
        {
            if (System.Object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (((MTwoDAxisAlignedBox)this == null) || ((MTwoDAxisAlignedBox)other == null))
            {
                return false;
            }

            return (this.mMinimum == ((MTwoDAxisAlignedBox)other).mMinimum && this.mMaximum == ((MTwoDAxisAlignedBox)other).mMaximum);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode();
        }
    }
}
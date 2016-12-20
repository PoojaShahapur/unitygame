namespace SDK.Lib
{
    public struct MRangeBox
    {
        public MVector3 mMinimum;
        public MVector3 mMaximum;
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

        public void clear()
        {
            mMinimum = new MVector3(float.MaxValue, float.MaxValue, float.MaxValue);
            mMaximum = new MVector3(float.MinValue, float.MinValue, float.MinValue);
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
            mMinimum = vec;
        }

        public void setMinimum(float x, float y, float z)
        {
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
            mMaximum = vec;
        }

        public void setMaximum(float x, float y, float z)
        {
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

        public void setExtents(ref MVector3 pt)
        {
            if(mMinimum.x > pt.x)
            {
                mMinimum.x = pt.x;
            }
            if (mMinimum.y > pt.y)
            {
                mMinimum.y = pt.y;
            }
            if (mMinimum.z > pt.z)
            {
                mMinimum.z = pt.z;
            }

            if (mMaximum.x < pt.x)
            {
                mMaximum.x = pt.x;
            }
            if (mMaximum.y < pt.y)
            {
                mMaximum.y = pt.y;
            }
            if (mMaximum.z < pt.z)
            {
                mMaximum.z = pt.z;
            }
        }

        public void setExtents(float mx, float my, float mz)
        {
            if (mMinimum.x > mx)
            {
                mMinimum.x = mx;
            }
            if (mMinimum.y > my)
            {
                mMinimum.y = my;
            }
            if (mMinimum.z > mz)
            {
                mMinimum.z = mz;
            }

            if (mMaximum.x < mx)
            {
                mMaximum.x = mx;
            }
            if (mMaximum.y < my)
            {
                mMaximum.y = my;
            }
            if (mMaximum.z < mz)
            {
                mMaximum.z = mz;
            }
        }

        public MVector3[] getAllCorners()
        {
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

        public MVector3 getCenter()
        {
            return new MVector3(
                (mMaximum.x + mMinimum.x) * 0.5f,
                (mMaximum.y + mMinimum.y) * 0.5f,
                (mMaximum.z + mMinimum.z) * 0.5f);
        }

        public float getHalfZ()
        {
            return UtilMath.Abs((mMaximum.x - mMinimum.x) / 2);
        }
    }
}
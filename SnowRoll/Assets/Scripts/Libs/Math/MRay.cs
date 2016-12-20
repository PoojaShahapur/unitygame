using System.Collections.Generic;

namespace SDK.Lib
{
    public class MRay
    {
        protected MVector3 mOrigin;
        protected MVector3 mDirection;

        public MRay()
        {
            mOrigin = MVector3.ZERO;
            mDirection = MVector3.UNIT_Z;
        }

        public MRay(MVector3 origin, MVector3 direction)
        {
            mOrigin = origin;
            mDirection = direction;
        }

        public void setOrigin(MVector3 origin)
        {
            mOrigin = origin;
        }

        public MVector3 getOrigin()
        {
            return mOrigin;
        }

        public void setDirection(MVector3 dir)
        {
            mDirection = dir;
        }

        public MVector3 getDirection()
        {
            return mDirection;
        }

        public MVector3 getPoint(float t)
        {
            return new MVector3(mOrigin + (mDirection * t));
        }

        static public MVector3 operator *(MRay lhs, float t)
        {
            return lhs.getPoint(t);
        }

        public MKeyValuePair<bool, float> intersects(ref MPlane p)
        {
            return UtilMath.intersects(this, p);
        }

        public KeyValuePair<bool, float> intersects(ref MAxisAlignedBox box)
        {
            return UtilMath.intersects(this, ref box);
        }
    }
}
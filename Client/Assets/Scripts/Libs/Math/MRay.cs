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

        public MRay(ref MVector3 origin, ref MVector3 direction)
        {
            mOrigin = origin;
            mDirection = direction;
        }

        public void setOrigin(ref MVector3 origin)
        {
            mOrigin = origin;
        }

        public MVector3 getOrigin()
        {
            return mOrigin;
        }

        public void setDirection(ref MVector3 dir)
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

        //public std::pair<bool, Real> intersects(ref MPlane p)
        //{
        //    return Math::intersects(this, p);
        //}

        //public std::pair<bool, Real> intersects(ref MAxisAlignedBox box)
        //{
        //    return Math::intersects(this, box);
        //}
    }
}
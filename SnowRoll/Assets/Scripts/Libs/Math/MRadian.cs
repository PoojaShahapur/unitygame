namespace SDK.Lib
{
    public struct MRadian
    {
        public float mRad;

        public MRadian(float r = 0)
        {
            mRad = r;
        }

        public MRadian(ref MDegree d)
        {
            mRad = d.valueRadians();
        }

        public MRadian assignFrom(ref float f)
        {
            mRad = f;
            return this;
        }

        public MRadian assingFrom(ref MRadian r)
        {
            mRad = r.mRad;
            return this;
        }

        public MRadian assignFrom(ref MDegree d)
        {
            mRad = d.valueRadians();
            return this;
        }

        public float valueDegrees()
        {
            return UtilMath.RadiansToDegrees(mRad);
        }

        public float valueRadians()
        {
            return mRad;
        }

        public float valueAngleUnits()
        {
            return UtilMath.RadiansToAngleUnits(mRad);
        }

        static public MRadian operator +(MRadian lhs)
        {
            return lhs;
        }

        static public MRadian operator +(MRadian lhs, MRadian r)
        {
            return new MRadian(lhs.mRad + r.mRad);
        }

        static public MRadian operator +(MRadian lhs, MDegree d)
        {
            return new MRadian(lhs.mRad + d.valueRadians());
        }

        static public MRadian operator -(MRadian lhs)
        {
            return new MRadian(-lhs.mRad);
        }

        static public MRadian operator -(MRadian lhs, MRadian r)
        {
            return new MRadian(lhs.mRad - r.mRad);
        }

        static public MRadian operator -(MRadian lhs, MDegree d)
        {
            return new MRadian(lhs.mRad - d.valueRadians());
        }

        static public MRadian operator *(MRadian lhs, float f)
        {
            return new MRadian(lhs.mRad * f);
        }

        static public MRadian operator *(float f, MRadian rhs)
        {
            return new MRadian(rhs.mRad * f);
        }

        static public MRadian operator *(MRadian lhs, MRadian f)
        {
            return new MRadian(lhs.mRad * f.mRad);
        }

        static public MRadian operator /(MRadian lhs, float f)
        {
            return new MRadian(lhs.mRad / f);
        }

        static public bool operator <(MRadian lhs, MRadian r)
        {
            return lhs.mRad < r.mRad;
        }

        static public bool operator <=(MRadian lhs, MRadian r)
        {
            return lhs.mRad <= r.mRad;
        }

        static public bool operator ==(MRadian lhs, MRadian r)
        {
            return lhs.mRad == r.mRad;
        }

        static public bool operator !=(MRadian lhs, MRadian r)
        {
            return lhs.mRad != r.mRad;
        }

        static public bool operator >=(MRadian lhs, MRadian r)
        {
            return lhs.mRad >= r.mRad;
        }

        static public bool operator >(MRadian lhs, MRadian r)
        {
            return lhs.mRad > r.mRad;
        }

        static public string ToString(ref MRadian v)
        {
            string o = "Radian(" + v.valueRadians() + ")";
            return o;
        }
    }
}
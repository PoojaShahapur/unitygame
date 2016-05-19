namespace SDK.Lib
{
    public struct MDegree
    {
        public float mDeg;

        public MDegree(float d = 0)
        {
            mDeg = d;
        }
        public MDegree(ref MRadian r)
        {
            mDeg = r.valueDegrees();
        }

        public MDegree assignFrom(ref float f)
        {
            mDeg = f;
            return this;
        }
        public MDegree assignFrom(ref MDegree d)
        {
            mDeg = d.mDeg;
            return this;
        }
        public MDegree assignFrom(ref MRadian r)
        {
            mDeg = r.valueDegrees();
            return this;
        }

        public float valueDegrees()
        {
            return mDeg;
        }
        public float valueRadians()
        {
            return UtilMath.DegreesToRadians(mDeg);
        }
        public float valueAngleUnits()
        {
            return UtilMath.DegreesToAngleUnits(mDeg);
        }

        static public MDegree operator +(MDegree lhs)
        {
            return lhs;
        }
        static public MDegree operator +(MDegree lhs, MDegree d)
        {
            return new MDegree(lhs.mDeg + d.mDeg);
        }
        static public MDegree operator +(MDegree lhs, MRadian r)
        {
            return new MDegree(lhs.mDeg + r.valueDegrees());
        }

        static public MDegree operator -(MDegree lhs)
        {
            return new MDegree(-lhs.mDeg);
        }
        static public MDegree operator -(MDegree lhs, MDegree d)
        {
            return new MDegree(lhs.mDeg - d.mDeg);
        }
        static public MDegree operator -(MDegree lhs, MRadian r)
        {
            return new MDegree(lhs.mDeg - r.valueDegrees());
        }

        static public MDegree operator *(MDegree lhs, float f)
        {
            return new MDegree(lhs.mDeg * f);
        }
        static public MDegree operator *(MDegree lhs, MDegree f)
        {
            return new MDegree(lhs.mDeg * f.mDeg);
        }

        static public MDegree operator /(MDegree lhs, float f)
        {
            return new MDegree(lhs.mDeg / f);
        }


        static public bool operator <(MDegree lhs, MDegree d)
        {
            return lhs.mDeg < d.mDeg;
        }
        static public bool operator <=(MDegree lhs, MDegree d)
        {
            return lhs.mDeg <= d.mDeg;
        }
        static public bool operator ==(MDegree lhs, MDegree d)
        {
            return lhs.mDeg == d.mDeg;
        }
        static public bool operator !=(MDegree lhs, MDegree d)
        {
            return lhs.mDeg != d.mDeg;
        }
        static public bool operator >=(MDegree lhs, MDegree d)
        {
            return lhs.mDeg >= d.mDeg;
        }
        static public bool operator >(MDegree lhs, MDegree d)
        {
            return lhs.mDeg > d.mDeg;
        }

        static public string ToString(ref MDegree v)
        {
            string o = "Degree(" + v.valueDegrees() + ")";
            return o;
        }
    }
}
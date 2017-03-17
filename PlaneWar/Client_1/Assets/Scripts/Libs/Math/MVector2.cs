using UnityEngine;

namespace SDK.Lib
{
    public struct MVector2
    {
        public static MVector2 ZERO = new MVector2(0, 0);
        public static MVector2 UNIT_X = new MVector2(1, 0);
        public static MVector2 UNIT_Y = new MVector2(0, 1);
        public static MVector2 NEGATIVE_UNIT_X = new MVector2(-1, 0);
        public static MVector2 NEGATIVE_UNIT_Y = new MVector2(0, -1);
        public static MVector2 UNIT_SCALE = new MVector2(1, 1);

        public float x;
        public float y;

        public MVector2(float fX, float fY = 0)
        {
            x = fX;
            y = fY;
        }

        public MVector2(float scaler)
        {
            x = scaler;
            y = scaler;
        }

        public MVector2(ref MVector2 vec)
        {
            x = vec.x;
            y = vec.y;
        }

        public MVector2(ref float[] afCoordinate)
        {
            x = afCoordinate[0];
            y = afCoordinate[1];
        }

        public MVector2(ref int[] afCoordinate)
        {
            x = (float)afCoordinate[0];
            y = (float)afCoordinate[1];
        }

        public void swap(ref MVector2 other)
        {
            float tmp = 0;
            tmp = x;
            x = other.x;
            other.x = tmp;

            tmp = y;
            y = other.y;
            other.y = tmp;
        }

        public float this[uint index]
        {
            get
            {
                UtilApi.assert(index < 2);
                if (index == 0)
                {
                    return x;
                }
                return y;
            }
        }

        /*
        static public MVector2 operator = (MVector2 lhs, MVector2 rhs)
        {
            lhs.x = rhs.x;
            lhs.y = rhs.y;

            return lhs;
        }

        static public MVector2 operator = (MVector2 lhs, float fScalar)
        {
            lhs.x = fScalar;
            lhs.y = fScalar;

            return lhs;
        
        }
        */

        static public bool operator == (MVector2 lhs, MVector2 rkVector)
        {
            return (lhs.x == rkVector.x && lhs.y == rkVector.y);
        }

        static public bool operator != (MVector2 lhs, MVector2 rkVector)
        {
            return (lhs.x != rkVector.x || lhs.y != rkVector.y);
        }

        static public MVector2 operator + (MVector2 lhs, MVector2 rkVector)
        {
            return new MVector2(
                lhs.x + rkVector.x,
                lhs.y + rkVector.y);
        }

        static public MVector2 operator - (MVector2 lhs, MVector2 rkVector )
        {
            return new MVector2(
                lhs.x - rkVector.x,
                lhs.y - rkVector.y);
        }

        static public MVector2 operator * (MVector2 lhs, float fScalar)
        {
            return new MVector2(
                lhs.x * fScalar,
                lhs.y * fScalar);
        }

        static public MVector2 operator * (MVector2 lhs, MVector2 rhs)
        {
            return new MVector2(
                lhs.x * rhs.x,
                lhs.y * rhs.y);
        }

        static public MVector2 operator / (MVector2 lhs, float fScalar)
        {
            UtilApi.assert(fScalar != 0.0 );

            float fInv = 1.0f / fScalar;

            return new MVector2(
                lhs.x * fInv,
                lhs.y * fInv);
        }

        static public MVector2 operator / (MVector2 lhs,  MVector2 rhs)
        {
            return new MVector2(
                lhs.x / rhs.x,
                lhs.y / rhs.y);
        }

        static public MVector2 operator + (MVector2 lhs)
        {
            return lhs;
        }

        static public MVector2 operator - (MVector2 lhs)
        {
            return new MVector2(-lhs.x, -lhs.y);
        }

        static public MVector2 operator * (float fScalar, MVector2 rkVector )
        {
            return new MVector2(
                fScalar * rkVector.x,
                fScalar * rkVector.y);
        }

        static public MVector2 operator / ( float fScalar, MVector2 rkVector )
        {
            return new MVector2(
                fScalar / rkVector.x,
                fScalar / rkVector.y);
        }

        static public MVector2 operator + (MVector2 lhs,  float rhs)
        {
            return new MVector2(
                lhs.x + rhs,
                lhs.y + rhs);
        }

        static public MVector2 operator + (float lhs, MVector2 rhs)
        {
            return new MVector2(
                lhs + rhs.x,
                lhs + rhs.y);
        }

        //static public MVector2 operator -=(MVector2 lhs, float rhs)
        //{
        //    return new MVector2(
        //        lhs.x - rhs,
        //        lhs.y - rhs);
        //}

        //static public MVector2 operator -=(float lhs, MVector2 rhs)
        //{
        //    return new MVector2(
        //        lhs - rhs.x,
        //        lhs - rhs.y);
        //}

        /*
        static public MVector2 operator += (MVector2 lhs, MVector2 rkVector )
        {
            lhs.x += rkVector.x;
            lhs.y += rkVector.y;

            return lhs;
        }

        static public MVector2 operator += (MVector2 lhs, float fScaler)
        {
            lhs.x += fScaler;
            lhs.y += fScaler;

            return lhs;
        }

        static public MVector2 operator -= (MVector2 lhs, MVector2 rkVector )
        {
            lhs.x -= rkVector.x;
            lhs.y -= rkVector.y;

            return lhs;
        }

        static public MVector2 operator -= (MVector2 lhs, float fScaler)
        {
            lhs.x -= fScaler;
            lhs.y -= fScaler;

            return lhs;
        }

        static public MVector2 operator *= (MVector2 lhs, float fScalar)
        {
            lhs.x *= fScalar;
            lhs.y *= fScalar;

            return lhs;
        }

        static public MVector2 operator *= (MVector2 lhs, MVector2 rkVector )
        {
            lhs.x *= rkVector.x;
            lhs.y *= rkVector.y;

            return lhs;
        }

        static public MVector2 operator /= (MVector2 lhs, float fScalar)
        {
            UtilApi.assert(fScalar != 0.0);

            float fInv = 1.0f / fScalar;

            lhs.x *= fInv;
            lhs.y *= fInv;

            return lhs;
        }

        static public MVector2 operator /=(MVector2 lhs, MVector2 rkVector )
        {
            lhs.x /= rkVector.x;
            lhs.y /= rkVector.y;

            return lhs;
        }
        */

        public float length()
        {
            return Mathf.Sqrt( x* x + y* y );
        }


        public float squaredLength()
        {
            return x* x + y* y;
        }


        public float distance(ref MVector2 rhs)
        {
            return (this - rhs).length();
        }


        public float squaredDistance(ref MVector2 rhs)
        {
            return (this - rhs).squaredLength();
        }


        public float dotProduct(ref MVector2 vec)
        {
            return x* vec.x + y* vec.y;
        }


        public float normalise()
        {
            float fLength = Mathf.Sqrt(x * x + y * y);

            if (fLength > (float)(0.0f))
            {
                float fInvLength = 1.0f / fLength;
                x *= fInvLength;
                y *= fInvLength;
            }

            return fLength;
        }

        public MVector2 midPoint(MVector2 vec )
        {
            return new MVector2(
                ( x + vec.x ) * 0.5f,
                ( y + vec.y ) * 0.5f );
        }

        static public bool operator <(MVector2 lhs, MVector2 rhs )
        {
            if(lhs.x < rhs.x && lhs.y <rhs.y )
                return true;
            return false;
        }

        static public bool operator >(MVector2 lhs, MVector2 rhs )
        {
            if(lhs.x > rhs.x && lhs.y > rhs.y )
                return true;
            return false;
        }

        public void makeFloor(MVector2 cmp )
        {
            if (cmp.x < x) x = cmp.x;
            if (cmp.y < y) y = cmp.y;
        }

        public void makeCeil(MVector2 cmp )
        {
            if (cmp.x > x) x = cmp.x;
            if (cmp.y > y) y = cmp.y;
        }

        public MVector2 perpendicular()
        {
            return new MVector2(-y, x);
        }

        public float crossProduct(ref MVector2 rkVector )
        {
            return x* rkVector.y - y* rkVector.x;
        }

        public MVector2 randomDeviant(float angle)
        {
            angle *= UtilApi.rangRandom(-1, 1);
            float cosa = Mathf.Cos(angle);
            float sina = Mathf.Sin(angle);
            return new MVector2(cosa* x - sina* y,
                           sina* x + cosa* y);
        }

        public bool isZeroLength()
        {
            float sqlen = (x * x) + (y * y);
            return (sqlen< (1e-06 * 1e-06));

        }

        public MVector2 normalisedCopy()
        {
            MVector2 ret = this;
            ret.normalise();
            return ret;
        }

        public MVector2 reflect(ref MVector2 normal)
        {
            MVector2 tmp = this - (2 * this.dotProduct(ref normal) * normal);
            return new MVector2(ref tmp);
        }

        public float angleBetween(ref MVector2 other)
        {
            float lenProduct = length() * other.length();

            if(lenProduct< 1e-6f)
                lenProduct = 1e-6f;
        
            float f = dotProduct(ref other) / lenProduct;

            f = Mathf.Clamp(f, (float)-1.0, (float)1.0);
            return Mathf.Acos(f);
        }

        public float angleTo(ref MVector2 other) 
        {
            float angle = angleBetween(ref other);
        
            if (crossProduct(ref other) <0)          
                angle = (float)Mathf.PI * 2 - angle;       

            return angle;
        }

        public override bool Equals(object other)
        {
            if (System.Object.ReferenceEquals(this, other))
            {
                return true;
            }

            //if (((MVector2)this == null) || ((MVector2)other == null))
            //{
            //    return false;
            //}

            return (this.x == ((MVector2)other).x && this.y == ((MVector2)other).y);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode();
        }
    }
}
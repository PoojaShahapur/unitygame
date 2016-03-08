namespace SDK.Lib
{
    public class MVector4
    {
        static public MVector4 ZERO = new MVector4(0, 0, 0, 0);

        public float x;
        public float y;
        public float z;
        public float w;

        public MVector4()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
            this.w = 0;
        }

        public MVector4(float fX, float fY, float fZ, float fW)
        {
            x = fX;
            y = fY;
            z = fZ;
            w = fW;
        }

        public MVector4(float[] afCoordinate)
        {
            x = afCoordinate[0];
            y = afCoordinate[1];
            z = afCoordinate[2];
            w = afCoordinate[3];
        }

        public MVector4(int[] afCoordinate)
        {
            x = (float)afCoordinate[0];
            y = (float)afCoordinate[1];
            z = (float)afCoordinate[2];
            w = (float)afCoordinate[3];
        }

        public MVector4(float scaler)
        {
            x = scaler;
            y = scaler;
            z = scaler;
            w = scaler;
        }

        public MVector4(MVector3 rhs)
        {
            x = rhs.x;
            y = rhs.y;
            z = rhs.z;
            w = 1.0f;
        }

        public void swap(MVector4 other)
        {
            float tmp = 0;
            tmp = this.x;
            this.x = other.x;
            other.x = tmp;

            tmp = this.y;
            this.y = other.y;
            other.y = tmp;

            tmp = this.z;
            this.z = other.z;
            other.z = tmp;

            tmp = this.w;
            this.w = other.w;
            other.w = tmp;
        }

        public float this[int index]
        {
            get
            {
                UtilApi.assert(index < 4);
                if(0 == index)
                {
                    return this.x;
                }
                else if (1 == index)
                {
                    return this.y;
                }
                else if (2 == index)
                {
                    return this.z;
                }
                else if (3 == index)
                {
                    return this.w;
                }

                return this.x;
            }
        }

        public MVector4 assignFrom( MVector4 rkVector )
        {
            this.x = rkVector.x;
            this.y = rkVector.y;
            this.z = rkVector.z;
            this.w = rkVector.w;

            return this;
        }

        public MVector4 assignFrom(float fScalar)
        {
            this.x = fScalar;
            this.y = fScalar;
            this.z = fScalar;
            this.w = fScalar;
            return this;
        }

        static public bool operator == (MVector4 lhs, MVector4 rkVector )
        {
            return (lhs.x == rkVector.x &&
                lhs.y == rkVector.y &&
                lhs.z == rkVector.z &&
                lhs.w == rkVector.w );
        }

        static public bool operator != (MVector4 lhs, MVector4 rkVector )
        {
            return (lhs.x != rkVector.x ||
                lhs.y != rkVector.y ||
                lhs.z != rkVector.z ||
                lhs.w != rkVector.w );
        }

        public MVector4 assignFrom(MVector3 rhs)
        {
            this.x = rhs.x;
            this.y = rhs.y;
            this.z = rhs.z;
            this.w = 1.0f;
            return this;
        }

        static public MVector4 operator + (MVector4 lhs, MVector4 rkVector)
        {
            return new MVector4(
                lhs.x + rkVector.x,
                lhs.y + rkVector.y,
                lhs.z + rkVector.z,
                lhs.w + rkVector.w);
        }

        static public MVector4 operator - (MVector4 lhs, MVector4 rkVector )
        {
            return new MVector4(
                lhs.x - rkVector.x,
                lhs.y - rkVector.y,
                lhs.z - rkVector.z,
                lhs.w - rkVector.w);
        }

        static public MVector4 operator * (MVector4 lhs, float fScalar )
        {
            return new MVector4(
                lhs.x * fScalar,
                lhs.y * fScalar,
                lhs.z * fScalar,
                lhs.w * fScalar);
        }

        static public MVector4 operator * (MVector4 lhs, MVector4 rhs)
        {
            return new MVector4(
                rhs.x * lhs.x,
                rhs.y * lhs.y,
                rhs.z * lhs.z,
                rhs.w * lhs.w);
        }

        static public MVector4 operator / (MVector4 lhs, float fScalar )
        {
            UtilApi.assert( fScalar != 0.0 );

            float fInv = 1.0f / fScalar;

            return new MVector4(
                lhs.x * fInv,
                lhs.y * fInv,
                lhs.z * fInv,
                lhs.w * fInv);
        }

        static public MVector4 operator / (MVector4 lhs, MVector4 rhs)
        {
            return new MVector4(
                lhs.x / rhs.x,
                lhs.y / rhs.y,
                lhs.z / rhs.z,
                lhs.w / rhs.w);
        }

        static public MVector4 operator + (MVector4 lhs)
        {
            return lhs;
        }

        static public MVector4 operator - (MVector4 lhs)
        {
            return new MVector4(-lhs.x, -lhs.y, -lhs.z, -lhs.w);
        }

        static public MVector4 operator * (float fScalar, MVector4 rkVector )
        {
            return new MVector4(
                fScalar * rkVector.x,
                fScalar * rkVector.y,
                fScalar * rkVector.z,
                fScalar * rkVector.w);
        }

        static public MVector4 operator / ( float fScalar, MVector4 rkVector )
        {
            return new MVector4(
                fScalar / rkVector.x,
                fScalar / rkVector.y,
                fScalar / rkVector.z,
                fScalar / rkVector.w);
        }

        static public MVector4 operator + (MVector4 lhs, float rhs)
        {
            return new MVector4(
                lhs.x + rhs,
                lhs.y + rhs,
                lhs.z + rhs,
                lhs.w + rhs);
        }

        static public MVector4 operator + (float lhs, MVector4 rhs)
        {
            return new MVector4(
                lhs + rhs.x,
                lhs + rhs.y,
                lhs + rhs.z,
                lhs + rhs.w);
        }

        static public MVector4 operator - (MVector4 lhs, float rhs)
        {
            return new MVector4(
                lhs.x - rhs,
                lhs.y - rhs,
                lhs.z - rhs,
                lhs.w - rhs);
        }

        static public MVector4 operator - (float lhs, MVector4 rhs)
        {
            return new MVector4(
                lhs - rhs.x,
                lhs - rhs.y,
                lhs - rhs.z,
                lhs - rhs.w);
        }

        /*
        static public MVector4 operator += (MVector4 lhs, MVector4 rkVector )
        {
            lhs.x += rkVector.x;
            lhs.y += rkVector.y;
            lhs.z += rkVector.z;
            lhs.w += rkVector.w;

            return lhs;
        }

        static public MVector4 operator -= (MVector4 lhs, MVector4 rkVector )
        {
            lhs.x -= rkVector.x;
            lhs.y -= rkVector.y;
            lhs.z -= rkVector.z;
            lhs.w -= rkVector.w;

            return lhs;
        }

        static public MVector4 operator *= (MVector4 lhs, float fScalar )
        {
            lhs.x *= fScalar;
            lhs.y *= fScalar;
            lhs.z *= fScalar;
            lhs.w *= fScalar;
            return lhs;
        }

        static public MVector4 operator += (MVector4 lhs, float fScalar )
        {
            lhs.x += fScalar;
            lhs.y += fScalar;
            lhs.z += fScalar;
            lhs.w += fScalar;
            return lhs;
        }

        static public MVector4 operator -= (MVector4 lhs, float fScalar )
        {
            lhs.x -= fScalar;
            lhs.y -= fScalar;
            lhs.z -= fScalar;
            lhs.w -= fScalar;
            return lhs;
        }

        static public MVector4 operator *= (MVector4 lhs, MVector4 rkVector )
        {
            lhs.x *= rkVector.x;
            lhs.y *= rkVector.y;
            lhs.z *= rkVector.z;
            lhs.w *= rkVector.w;

            return lhs;
        }

        static public MVector4 operator /= (MVector4 lhs, float fScalar )
        {
            UtilApi.assert( fScalar != 0.0 );

            float fInv = 1.0f / fScalar;

            lhs.x *= fInv;
            lhs.y *= fInv;
            lhs.z *= fInv;
            lhs.w *= fInv;

            return lhs.;
        }

        static public MVector4 operator /= (MVector4 lhs, MVector4 rkVector )
        {
            lhs.x /= rkVector.x;
            lhs.y /= rkVector.y;
            lhs.z /= rkVector.z;
            lhs.w /= rkVector.w;

            return lhs;
        }
        */

        public float dotProduct(MVector4 vec)
        {
            return this.x * vec.x + this.y * vec.y + this.z * vec.z + this.w * vec.w;
        }

        public bool isNaN()
        {
            return float.IsNaN(x) || float.IsNaN(y) || float.IsNaN(z) || float.IsNaN(w);
        }

        static public string ToString(MVector4 v)
        {
            string dest = "";
            dest = string.Format("Vector4( {0} , {1}, {2}, {3}", v.x, v.y, v.z, v.w);
            return dest;
        }

        public override bool Equals(object other)
        {
            if (System.Object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (((MVector4)this == null) || ((MVector4)other == null))
            {
                return false;
            }

            return (this.x == ((MVector4)other).x && this.y == ((MVector4)other).y);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode();
        }
    }
}
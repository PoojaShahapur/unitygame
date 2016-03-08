namespace SDK.Lib
{
    public struct MVector3
    {
        public static MVector3 ZERO = new MVector3(0, 0, 0);
        public static MVector3 UNIT_X = new MVector3(1, 0, 0);
        public static MVector3 UNIT_Y = new MVector3(0, 1, 0);
        public static MVector3 UNIT_Z = new MVector3(0, 0, 1);
        public static MVector3 NEGATIVE_UNIT_X = new MVector3(-1, 0, 0);
        public static MVector3 NEGATIVE_UNIT_Y = new MVector3(0, -1, 0);
        public static MVector3 NEGATIVE_UNIT_Z = new MVector3(0, 0, -1);
        public static MVector3 UNIT_SCALE = new MVector3(1, 1, 1);

        public float x;
        public float y;
        public float z;

        public MVector3( float fX = 0, float fY = 0, float fZ = 0 )
        {
            x = fX;
            y = fY;
            z = fZ;
        }

        public MVector3(float[] afCoordinate)
        {
            x = afCoordinate[0];
            y = afCoordinate[1];
            z = afCoordinate[2];
        }

        public MVector3(int[] afCoordinate)
        {
            x = (float)afCoordinate[0];
            y = (float)afCoordinate[1];
            z = (float)afCoordinate[2];
        }

        public MVector3( float scaler )
        {
            x = scaler;
            y = scaler;
            z = scaler;
        }

        public MVector3(MVector3 rhs)
        {
            x = rhs.x;
            y = rhs.y;
            z = rhs.z;
        }

        public void swap(MVector3 other)
        {
            UtilMath.swap(ref x, ref other.x);
            UtilMath.swap(ref y, ref other.y);
            UtilMath.swap(ref z, ref other.z);
        }

        public float this[int index]
        {
            get
            {
                UtilApi.assert(index < 3);

                if (0 == index)
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

                return this.z;
            }
            set
            {
                if (0 == index)
                {
                    this.x = value;
                }
                else if (1 == index)
                {
                    this.y = value;
                }
                else if (2 == index)
                {
                    this.z = value;
                }
            }
        }

        public MVector3 assignFrom(MVector3 rkVector )
        {
            this.x = rkVector.x;
            this.y = rkVector.y;
            this.z = rkVector.z;

            return this;
        }

        public MVector3 assignFrom(float fScaler )
        {
            this.x = fScaler;
            this.y = fScaler;
            this.z = fScaler;

            return this;
        }

        static public bool operator == (MVector3 lhs, MVector3 rkVector )
        {
            return (lhs.x == rkVector.x && lhs.y == rkVector.y && lhs.z == rkVector.z );
        }

        static public bool operator != (MVector3 lhs, MVector3 rkVector )
        {
            return (lhs.x != rkVector.x || lhs.y != rkVector.y || lhs.z != rkVector.z );
        }

        static public MVector3 operator + (MVector3 lhs, MVector3 rkVector )
        {
            return new MVector3(
                lhs.x + rkVector.x,
                lhs.y + rkVector.y,
                lhs.z + rkVector.z);
        }

        static public MVector3 operator - (MVector3 lhs, MVector3 rkVector )
        {
            return new MVector3(
                lhs.x - rkVector.x,
                lhs.y - rkVector.y,
                lhs.z - rkVector.z);
        }

        static public MVector3 operator * (MVector3 lhs, float fScalar )
        {
            return new MVector3(
                lhs.x * fScalar,
                lhs.y * fScalar,
                lhs.z * fScalar);
        }

        static public MVector3 operator * (MVector3 lhs, MVector3 rhs)
        {
            return new MVector3(
                lhs.x * rhs.x,
                lhs.y * rhs.y,
                lhs.z * rhs.z);
        }

        static public MVector3 operator / (MVector3 lhs, float fScalar )
        {
            UtilApi.assert( fScalar != 0.0 );

            float fInv = 1.0f / fScalar;

            return new MVector3(
                lhs.x * fInv,
                lhs.y * fInv,
                lhs.z * fInv);
        }

        static public MVector3 operator / (MVector3 lhs, MVector3 rhs)
        {
            return new MVector3(
                lhs.x / rhs.x,
                lhs.y / rhs.y,
                lhs.z / rhs.z);
        }

        static public MVector3 operator + (MVector3 lhs)
        {
            return lhs;
        }

        static public MVector3 operator - (MVector3 lhs)
        {
            return new MVector3(-lhs.x, -lhs.y, -lhs.z);
        }

        static public MVector3 operator * (float fScalar, MVector3 rkVector )
        {
            return new MVector3(
                fScalar * rkVector.x,
                fScalar * rkVector.y,
                fScalar * rkVector.z);
        }

        static public MVector3 operator / ( float fScalar, MVector3 rkVector )
        {
            return new MVector3(
                fScalar / rkVector.x,
                fScalar / rkVector.y,
                fScalar / rkVector.z);
        }

        static public MVector3 operator + (MVector3 lhs, float rhs)
        {
            return new MVector3(
                lhs.x + rhs,
                lhs.y + rhs,
                lhs.z + rhs);
        }

        static public MVector3 operator + (float lhs, MVector3 rhs)
        {
            return new MVector3(
                lhs + rhs.x,
                lhs + rhs.y,
                lhs + rhs.z);
        }

        static public MVector3 operator - (MVector3 lhs, float rhs)
        {
            return new MVector3(
                lhs.x - rhs,
                lhs.y - rhs,
                lhs.z - rhs);
        }

        static public MVector3 operator - (float lhs, MVector3 rhs)
        {
            return new MVector3(
                lhs - rhs.x,
                lhs - rhs.y,
                lhs - rhs.z);
        }

        /*
        static MVector3 operator += (MVector3 lhs, MVector3 rkVector )
        {
            lhs.x += rkVector.x;
            lhs.y += rkVector.y;
            lhs.z += rkVector.z;

            return lhs;
        }
        
        static public MVector3 operator += (MVector3 lhs, float fScalar )
        {
            lhs.x += fScalar;
            lhs.y += fScalar;
            lhs.z += fScalar;
            return lhs;
        }

        static public MVector3 operator -= (MVector3 lhs, MVector3 rkVector )
        {
            lhs.x -= rkVector.x;
            lhs.y -= rkVector.y;
            lhs.z -= rkVector.z;

            return lhs;
        }

        static public MVector3 operator -= (MVector3 lhs, float fScalar )
        {
            lhs.x -= fScalar;
            lhs.y -= fScalar;
            lhs.z -= fScalar;
            return lhs;
        }

        static public MVector3 operator *= (MVector3 lhs, float fScalar )
        {
            lhs.x *= fScalar;
            lhs.y *= fScalar;
            lhs.z *= fScalar;
            return lhs;
        }

        static public MVector3 operator *= (MVector3 lhs, MVector3 rkVector )
        {
            lhs.x *= rkVector.x;
            lhs.y *= rkVector.y;
            lhs.z *= rkVector.z;

            return lhs;
        }

        static public MVector3 operator /= (MVector3 lhs, float fScalar )
        {
            UtilApi.assert( fScalar != 0.0 );

            float fInv = 1.0f / fScalar;

            lhs.x *= fInv;
            lhs.y *= fInv;
            lhs.z *= fInv;

            return lhs;
        }

        static public MVector3 operator /= (MVector3 lhs, MVector3 rkVector )
        {
            lhs.x /= rkVector.x;
            lhs.y /= rkVector.y;
            lhs.z /= rkVector.z;

            return lhs;
        }
        */

        public float length ()
        {
            return UtilMath.Sqrt( x * x + y * y + z * z );
        }

        public float squaredLength ()
        {
            return x * x + y * y + z * z;
        }

        public float distance(MVector3 rhs)
        {
            return (this - rhs).length();
        }

        public float squaredDistance(MVector3 rhs)
        {
            return (this - rhs).squaredLength();
        }

        public float dotProduct(MVector3 vec)
        {
            return x * vec.x + y * vec.y + z * vec.z;
        }

        public float absDotProduct(MVector3 vec)
        {
            return UtilMath.Abs(x * vec.x) + UtilMath.Abs(y * vec.y) + UtilMath.Abs(z * vec.z);
        }

        public float normalise()
        {
            float fLength = UtilMath.Sqrt( x * x + y * y + z * z );

            if ( fLength > (float)(0.0f) )
            {
                float fInvLength = 1.0f / fLength;
                x *= fInvLength;
                y *= fInvLength;
                z *= fInvLength;
            }

            return fLength;
        }

        public MVector3 crossProduct(MVector3 rkVector )
        {
            return new MVector3(
                y * rkVector.z - z * rkVector.y,
                z * rkVector.x - x * rkVector.z,
                x * rkVector.y - y * rkVector.x);
        }

        public MVector3 midPoint( MVector3 vec )
        {
            return new MVector3(
                ( x + vec.x ) * 0.5f,
                ( y + vec.y ) * 0.5f,
                ( z + vec.z ) * 0.5f );
        }

        static public bool operator < (MVector3 lhs, MVector3 rhs )
        {
            if(lhs.x < rhs.x && lhs.y < rhs.y && lhs.z < rhs.z )
                return true;
            return false;
        }

        static public bool operator > (MVector3 lhs, MVector3 rhs )
        {
            if(lhs.x > rhs.x && lhs.y > rhs.y && lhs.z > rhs.z )
                return true;
            return false;
        }

        public void makeFloor(MVector3 cmp )
        {
            x = UtilMath.min( x, cmp.x );
            y = UtilMath.min( y, cmp.y );
            z = UtilMath.min( z, cmp.z );
        }

        public void makeCeil(MVector3 cmp )
        {
            x = UtilMath.max( x, cmp.x );
            y = UtilMath.max( y, cmp.y );
            z = UtilMath.max( z, cmp.z );
        }

        public void makeAbs()
        {
            x = UtilMath.Abs( x );
            y = UtilMath.Abs( y );
            z = UtilMath.Abs( z );
        }

        public MVector3 perpendicular()
        {
            const float fSquareZero = (float)(1e-06 * 1e-06);

            MVector3 perp = this.crossProduct( MVector3.UNIT_X );

            if( perp.squaredLength() < fSquareZero )
            {
                perp = this.crossProduct(MVector3.UNIT_Y );
            }
            perp.normalise();

            return perp;
        }

        public MVector3 randomDeviant(
            float angle,
            MVector3 up/* = MVector3.ZERO*/ )
        {
            MVector3 newUp;

            if (up == MVector3.ZERO)
            {
                newUp = this.perpendicular();
            }
            else
            {
                newUp = up;
            }

            MQuaternion q = new MQuaternion();
            q.FromAngleAxis( (float)(UtilMath.UnitRandom() * UtilMath.TWO_PI), this );
            newUp = q * newUp;

            q.FromAngleAxis( angle, newUp );
            return q * this;
        }

        public float angleBetween(MVector3 dest)
        {
            float lenProduct = length() * dest.length();

            if(lenProduct < 1e-6f)
                lenProduct = 1e-6f;

            float f = dotProduct(dest) / lenProduct;

            f = UtilMath.Clamp(f, (float)-1.0, (float)1.0);
            return UtilMath.ACos(f);
        }

        public MQuaternion getRotationTo(MVector3 dest,
            MVector3 fallbackAxis/* = MVector3.ZERO*/)
        {
            MQuaternion q = new MQuaternion();

            MVector3 v0 = this;
            MVector3 v1 = dest;
            v0.normalise();
            v1.normalise();

            float d = v0.dotProduct(v1);

            if (d >= 1.0f)
            {
                return MQuaternion.IDENTITY;
            }
            if (d < (1e-6f - 1.0f))
            {
                if (fallbackAxis != MVector3.ZERO)
                {
                    q.FromAngleAxis((float)(UtilMath.PI), fallbackAxis);
                }
                else
                {
                    MVector3 axis = MVector3.UNIT_X.crossProduct(this);
                    if (axis.isZeroLength())
                        axis = MVector3.UNIT_Y.crossProduct(this);
                    axis.normalise();
                    q.FromAngleAxis((float)(UtilMath.PI), axis);
                }
            }
            else
            {
                float s = UtilMath.Sqrt( (1+d)*2 );
                float invs = 1 / s;

                MVector3 c = v0.crossProduct(v1);

                q.x = c.x * invs;
                q.y = c.y * invs;
                q.z = c.z * invs;
                q.w = s * 0.5f;
                q.normalise();
            }
            return q;
        }

        public bool isZeroLength()
        {
            float sqlen = (x * x) + (y * y) + (z * z);
            return (sqlen < (1e-06 * 1e-06));
        }

        public MVector3 normalisedCopy()
        {
            MVector3 ret = this;
            ret.normalise();
            return ret;
        }

        public MVector3 reflect(MVector3 normal)
        {
            return new MVector3( this - ( 2 * this.dotProduct(normal) * normal ) );
        }

        public bool positionEquals(MVector3 rhs, float tolerance = 1e-03f)
        {
            return UtilMath.RealEqual(x, rhs.x, tolerance) &&
                UtilMath.RealEqual(y, rhs.y, tolerance) &&
                UtilMath.RealEqual(z, rhs.z, tolerance);

        }

        public bool positionCloses(MVector3 rhs, float tolerance = 1e-03f)
        {
            return squaredDistance(rhs) <=
                (squaredLength() + rhs.squaredLength()) * tolerance;
        }

        public bool directionEquals(MVector3 rhs,
            float tolerance)
        {
            float dot = dotProduct(rhs);
            float angle = UtilMath.ACos(dot);

            return UtilMath.Abs(angle) <= tolerance;

        }

        public bool isNaN()
        {
            return double.IsNaN(x) || double.IsNaN(y) || double.IsNaN(z);
        }

        public MVector3 primaryAxis()
        {
            float absx = UtilMath.Abs(x);
            float absy = UtilMath.Abs(y);
            float absz = UtilMath.Abs(z);
            if (absx > absy)
                if (absx > absz)
                    return x > 0 ? MVector3.UNIT_X : MVector3.NEGATIVE_UNIT_X;
                else
                    return z > 0 ? MVector3.UNIT_Z : MVector3.NEGATIVE_UNIT_Z;
            else
                if (absy > absz)
                    return y > 0 ? MVector3.UNIT_Y : MVector3.NEGATIVE_UNIT_Y;
                else
                    return z > 0 ? MVector3.UNIT_Z : MVector3.NEGATIVE_UNIT_Z;
        }

        static public string ToString(MVector3 v)
        {
            string o = "Vector3(" + v.x + ", " + v.y + ", " + v.z + ")";
            return o;
        }

        public override bool Equals(object other)
        {
            if (System.Object.ReferenceEquals(this, other))
            {
                return true;
            }

            //if (((MVector3)this == null) || ((MVector3)other == null))
            //{
            //    return false;
            //}

            return (this.x == ((MVector3)other).x && this.y == ((MVector3)other).y && this.z == ((MVector3)other).z);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode();
        }
    }
}
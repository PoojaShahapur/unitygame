using UnityEngine;

namespace SDK.Lib
{
    public struct MQuaternion
    {
        public static float msEpsilon = 1e-03f;

        public static MQuaternion ZERO = new MQuaternion(0, 0, 0, 0);
        public static MQuaternion IDENTITY = new MQuaternion(1, 0, 0, 0);
        public static int[] s_iNext = new int[3]{ 1, 2, 0 };

        public float w, x, y, z;

        public MQuaternion(float fW, float fX = 0, float fY = 0, float fZ = 0)
        {
            w = fW;
            x = fX;
            y = fY;
            z = fZ;
        }

        public MQuaternion(ref MMatrix3 rot)
        {
            x = y = z = w = 0;
            this.FromRotationMatrix(ref rot);
        }

        public MQuaternion(MRadian rfAngle, ref MVector3 rkAxis)
        {
            x = y = z = w = 0;
            this.FromAngleAxis(rfAngle, ref rkAxis);
        }

        public MQuaternion(ref MVector3 xaxis, ref MVector3 yaxis, ref MVector3 zaxis)
        {
            x = y = z = w = 0;
            this.FromAxes(xaxis, yaxis, zaxis);
        }

        public MQuaternion(ref MVector3[] akAxis)
        {
            x = y = z = w = 0;
            this.FromAxes(akAxis);
        }

        public void swap(ref MQuaternion other)
        {
            UtilMath.swap(ref w, ref other.w);
            UtilMath.swap(ref x, ref other.x);
            UtilMath.swap(ref y, ref other.y);
            UtilMath.swap(ref z, ref other.z);
        }

        public float this [int index]
        {
            get
            {
                UtilApi.assert(0 <= index && index < 4, "index is out of range");
                if (0 == index)
                {
                    return x;
                }
                else if (0 == index)
                {
                    return y;
                }
                else if (0 == index)
                {
                    return z;
                }
                else if (0 == index)
                {
                    return w;
                }

                return x;
            }
            set
            {
                UtilApi.assert(0 <= index && index < 4, "index is out of range");
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
                else if (3 == index)
                {
                    this.w = value;
                }
            }
        }

        public void FromRotationMatrix(ref MMatrix3 kRot)
        {
            float fTrace = kRot[0, 0] + kRot[1, 1] + kRot[2, 2];
            float fRoot;

            if (fTrace > 0.0)
            {
                fRoot = UtilMath.Sqrt(fTrace + 1.0f);
                w = 0.5f * fRoot;
                fRoot = 0.5f / fRoot;
                x = (kRot[2, 1] - kRot[1, 2]) * fRoot;
                y = (kRot[0, 2] - kRot[2, 0]) * fRoot;
                z = (kRot[1, 0] - kRot[0, 1]) * fRoot;
            }
            else
            {
                //static int[] s_iNext = new int{ 1, 2, 0 };
                int i = 0;
                if (kRot[1, 1] > kRot[0, 0])
                    i = 1;
                if (kRot[2, 2] > kRot[i, i])
                    i = 2;
                int j = s_iNext[i];
                int k = s_iNext[j];

                fRoot = UtilMath.Sqrt(kRot[i, i] - kRot[j, j] - kRot[k, k] + 1.0f);
                float[] apkQuat = { 0, 0, 0 };
                apkQuat[i] = 0.5f * fRoot;
                fRoot = 0.5f / fRoot;
                w = (kRot[k, j] - kRot[j, k]) * fRoot;
                apkQuat[j] = (kRot[j, i] + kRot[i, j]) * fRoot;
                apkQuat[k] = (kRot[k, i] + kRot[i, k]) * fRoot;

                x = apkQuat[0];
                y = apkQuat[1];
                z = apkQuat[2];
            }
        }

        public void ToRotationMatrix(ref MMatrix3 kRot)
        {
            float fTx = x + x;
            float fTy = y + y;
            float fTz = z + z;
            float fTwx = fTx * w;
            float fTwy = fTy * w;
            float fTwz = fTz * w;
            float fTxx = fTx * x;
            float fTxy = fTy * x;
            float fTxz = fTz * x;
            float fTyy = fTy * y;
            float fTyz = fTz * y;
            float fTzz = fTz * z;

            kRot[0, 0] = 1.0f - (fTyy + fTzz);
            kRot[0, 1] = fTxy - fTwz;
            kRot[0, 2] = fTxz + fTwy;
            kRot[1, 0] = fTxy + fTwz;
            kRot[1, 1] = 1.0f - (fTxx + fTzz);
            kRot[1, 2] = fTyz - fTwx;
            kRot[2, 0] = fTxz - fTwy;
            kRot[2, 1] = fTyz + fTwx;
            kRot[2, 2] = 1.0f - (fTxx + fTyy);
        }

        public void FromAngleAxis(MRadian rfAngle, ref MVector3 rkAxis)
        {
            MRadian fHalfAngle = 0.5f * rfAngle;
            float fSin = UtilMath.Sin(fHalfAngle);
            w = UtilMath.Cos(fHalfAngle);
            x = fSin * rkAxis.x;
            y = fSin * rkAxis.y;
            z = fSin * rkAxis.z;
        }

        public void ToAngleAxis(ref MRadian rfAngle, ref MVector3 rkAxis)
        {
            float fSqrLength = x * x + y * y + z * z;
            if (fSqrLength > 0.0)
            {
                rfAngle = new MRadian(2.0f * UtilMath.ACos(w));
                float fInvLength = UtilMath.InvSqrt(fSqrLength);
                rkAxis.x = x * fInvLength;
                rkAxis.y = y * fInvLength;
                rkAxis.z = z * fInvLength;
            }
            else
            {
                rfAngle = new MRadian(0.0f);
                rkAxis.x = 1.0f;
                rkAxis.y = 0.0f;
                rkAxis.z = 0.0f;
            }
        }

        public void ToAngleAxis(ref MDegree dAngle, ref MVector3 rkAxis)
        {
            MRadian rAngle = new MRadian();
            ToAngleAxis(ref rAngle, ref rkAxis);
            dAngle.assignFrom(ref rAngle);
        }

        public void FromAxes(MVector3[] akAxis)
        {
            MMatrix3 kRot = new MMatrix3(0);

            for (int iCol = 0; iCol < 3; iCol++)
            {
                kRot[0, iCol] = akAxis[iCol].x;
                kRot[1, iCol] = akAxis[iCol].y;
                kRot[2, iCol] = akAxis[iCol].z;
            }

            FromRotationMatrix(ref kRot);
        }

        public void FromAxes(MVector3 xaxis, MVector3 yaxis, MVector3 zaxis)
        {
            MMatrix3 kRot = new MMatrix3(0);

            kRot[0, 0] = xaxis.x;
            kRot[1, 0] = xaxis.y;
            kRot[2, 0] = xaxis.z;

            kRot[0, 1] = yaxis.x;
            kRot[1, 1] = yaxis.y;
            kRot[2, 1] = yaxis.z;

            kRot[0, 2] = zaxis.x;
            kRot[1, 2] = zaxis.y;
            kRot[2, 2] = zaxis.z;

            FromRotationMatrix(ref kRot);
        }

        public void ToAxes(ref MVector3[] akAxis)
        {
            MMatrix3 kRot = new MMatrix3(0);

            ToRotationMatrix(ref kRot);

            for (int iCol = 0; iCol < 3; iCol++)
            {
                akAxis[iCol].x = kRot[0, iCol];
                akAxis[iCol].y = kRot[1, iCol];
                akAxis[iCol].z = kRot[2, iCol];
            }
        }

        public void ToAxes(ref MVector3 xaxis, ref MVector3 yaxis, ref MVector3 zaxis)
        {
            MMatrix3 kRot = new MMatrix3(0);

            ToRotationMatrix(ref kRot);

            xaxis.x = kRot[0, 0];
            xaxis.y = kRot[1, 0];
            xaxis.z = kRot[2, 0];

            yaxis.x = kRot[0, 1];
            yaxis.y = kRot[1, 1];
            yaxis.z = kRot[2, 1];

            zaxis.x = kRot[0, 2];
            zaxis.y = kRot[1, 2];
            zaxis.z = kRot[2, 2];
        }

        public MVector3 xAxis()
        {
            float fTy = 2.0f * y;
            float fTz = 2.0f * z;
            float fTwy = fTy * w;
            float fTwz = fTz * w;
            float fTxy = fTy * x;
            float fTxz = fTz * x;
            float fTyy = fTy * y;
            float fTzz = fTz * z;

            return new MVector3(1.0f - (fTyy + fTzz), fTxy + fTwz, fTxz - fTwy);
        }

        public MVector3 yAxis()
        {
            float fTx = 2.0f * x;
            float fTy = 2.0f * y;
            float fTz = 2.0f * z;
            float fTwx = fTx * w;
            float fTwz = fTz * w;
            float fTxx = fTx * x;
            float fTxy = fTy * x;
            float fTyz = fTz * y;
            float fTzz = fTz * z;

            return new MVector3(fTxy - fTwz, 1.0f - (fTxx + fTzz), fTyz + fTwx);
        }

        public MVector3 zAxis()
        {
            float fTx = 2.0f * x;
            float fTy = 2.0f * y;
            float fTz = 2.0f * z;
            float fTwx = fTx * w;
            float fTwy = fTy * w;
            float fTxx = fTx * x;
            float fTxz = fTz * x;
            float fTyy = fTy * y;
            float fTyz = fTz * y;

            return new MVector3(fTxz + fTwy, fTyz - fTwx, 1.0f - (fTxx + fTyy));
        }

        public MQuaternion assignFrom(ref MQuaternion rkQ)
        {
            this.w = rkQ.w;
            this.x = rkQ.x;
            this.y = rkQ.y;
            this.z = rkQ.z;
            return this;
        }

        static public MQuaternion operator +(MQuaternion lhs, MQuaternion rkQ)
        {
            return new MQuaternion(lhs.w + rkQ.w, lhs.x + rkQ.x, lhs.y + rkQ.y, lhs.z + rkQ.z);
        }

        static public MQuaternion operator -(MQuaternion lhs, MQuaternion rkQ)
        {
            return new MQuaternion(lhs.w - rkQ.w, lhs.x - rkQ.x, lhs.y - rkQ.y, lhs.z - rkQ.z);
        }

        static public MQuaternion operator *(MQuaternion lhs, MQuaternion rkQ)
        {
            return new MQuaternion
                (
                    lhs.w * rkQ.w - lhs.x * rkQ.x - lhs.y * rkQ.y - lhs.z * rkQ.z,
                    lhs.w * rkQ.x + lhs.x * rkQ.w + lhs.y * rkQ.z - lhs.z * rkQ.y,
                    lhs.w * rkQ.y + lhs.y * rkQ.w + lhs.z * rkQ.x - lhs.x * rkQ.z,
                    lhs.w * rkQ.z + lhs.z * rkQ.w + lhs.x * rkQ.y - lhs.y * rkQ.x
                );
        }

        static public MQuaternion operator *(MQuaternion lhs, float fScalar)
        {
            return new MQuaternion(fScalar * lhs.w, fScalar * lhs.x, fScalar * lhs.y, fScalar * lhs.z);
        }

        static public MQuaternion operator *(float fScalar, MQuaternion rkQ)
        {
            return new MQuaternion(fScalar * rkQ.w, fScalar * rkQ.x, fScalar * rkQ.y,
                    fScalar * rkQ.z);
        }

        static public MQuaternion operator -(MQuaternion lhs)
        {
            return new MQuaternion(-lhs.w, -lhs.x, -lhs.y, -lhs.z);
        }

        static public bool operator ==(MQuaternion lhs, MQuaternion rhs)
        {
            return (rhs.x == lhs.x) && (rhs.y == lhs.y) &&
                (rhs.z == lhs.z) && (rhs.w == lhs.w);
        }

        static public bool operator !=(MQuaternion lhs, MQuaternion rhs)
        {
            return !(lhs ==rhs);
        }

        public float Dot(ref MQuaternion rkQ)
        {
            return rkQ.w * rkQ.w + rkQ.x * rkQ.x + rkQ.y * rkQ.y + rkQ.z * rkQ.z;
        }

        public float Norm()
        {
            return w * w + x * x + y * y + z * z;
        }

        public float normalise()
        {
            float len = Norm();
            float factor = 1.0f / UtilMath.Sqrt(len);
            this = this * factor;
            return len;
        }

        public MQuaternion Inverse()
        {
            float fNorm = w * w + x * x + y * y + z * z;
            if (fNorm > 0.0)
            {
                float fInvNorm = 1.0f / fNorm;
                return new MQuaternion(w * fInvNorm, -x * fInvNorm, -y * fInvNorm, -z * fInvNorm);
            }
            else
            {
                return ZERO;
            }
        }

        public MQuaternion UnitInverse()
        {
            return new MQuaternion(w, -x, -y, -z);
        }

        public MQuaternion Exp()
        {
            MRadian fAngle = new MRadian(UtilMath.Sqrt(x * x + y * y + z * z));
            float fSin = UtilMath.Sin(fAngle);

            MQuaternion kResult = new MQuaternion(1);
            kResult.w = UtilMath.Cos(fAngle);

            if (UtilMath.Abs(fSin) >= msEpsilon)
            {
                float fCoeff = fSin / (fAngle.valueRadians());
                kResult.x = fCoeff * x;
                kResult.y = fCoeff * y;
                kResult.z = fCoeff * z;
            }
            else
            {
                kResult.x = x;
                kResult.y = y;
                kResult.z = z;
            }

            return kResult;
        }

        public MQuaternion Log()
        {
            MQuaternion kResult = new MQuaternion(1);
            kResult.w = 0.0f;

            if (UtilMath.Abs(w) < 1.0)
            {
                MRadian fAngle = new MRadian(UtilMath.ACos(w));
                float fSin = UtilMath.Sin(fAngle);
                if (UtilMath.Abs(fSin) >= msEpsilon)
                {
                    float fCoeff = fAngle.valueRadians() / fSin;
                    kResult.x = fCoeff * x;
                    kResult.y = fCoeff * y;
                    kResult.z = fCoeff * z;
                    return kResult;
                }
            }

            kResult.x = x;
            kResult.y = y;
            kResult.z = z;

            return kResult;
        }

        static public MVector3 operator *(MQuaternion lhs, MVector3 v)
        {
            MVector3 uv = new MVector3(0, 0, 0), uuv = new MVector3(0, 0, 0);
            MVector3 qvec = new MVector3(lhs.x, lhs.y, lhs.z);
            uv = qvec.crossProduct(ref v);
            uuv = qvec.crossProduct(ref uv);
            uv *= (2.0f * lhs.w);
            uuv *= 2.0f;

            return v + uv + uuv;
        }

        public MRadian getRoll(bool reprojectAxis = true)
        {
            if (reprojectAxis)
            {
                float fTy = 2.0f * y;
                float fTz = 2.0f * z;
                float fTwz = fTz * w;
                float fTxy = fTy * x;
                float fTyy = fTy * y;
                float fTzz = fTz * z;

                return new MRadian(UtilMath.ATan2(fTxy + fTwz, 1.0f - (fTyy + fTzz)));
            }
            else
            {
                return new MRadian(UtilMath.ATan2(2 * (x * y + w * z), w * w + x * x - y * y - z * z));
            }
        }

        public MRadian getPitch(bool reprojectAxis = true)
        {
            if (reprojectAxis)
            {
                float fTx = 2.0f * x;
                float fTz = 2.0f * z;
                float fTwx = fTx * w;
                float fTxx = fTx * x;
                float fTyz = fTz * y;
                float fTzz = fTz * z;

                return new MRadian(UtilMath.ATan2(fTyz + fTwx, 1.0f - (fTxx + fTzz)));
            }
            else
            {
                return new MRadian(UtilMath.ATan2(2 * (y * z + w * x), w * w - x * x - y * y + z * z));
            }
        }

        public MRadian getYaw(bool reprojectAxis = true)
        {
            if (reprojectAxis)
            {
                float fTx = 2.0f * x;
                float fTy = 2.0f * y;
                float fTz = 2.0f * z;
                float fTwy = fTy * w;
                float fTxx = fTx * x;
                float fTxz = fTz * x;
                float fTyy = fTy * y;

                return new MRadian(UtilMath.ATan2(fTxz + fTwy, 1.0f - (fTxx + fTyy)));

            }
            else
            {
                return new MRadian(UtilMath.ASin(-2 * (x * z - w * y)));
            }
        }

        public bool equals(MQuaternion rhs, MRadian tolerance)
        {
            float d = Dot(ref rhs);
            MRadian angle = new MRadian(UtilMath.ACos(2.0f * d * d - 1.0f));

            return UtilMath.Abs(angle.valueRadians()) <= tolerance.valueRadians();
        }

        public bool orientationEquals(MQuaternion other, float tolerance = 1e-3f)
        {
            float d = this.Dot(ref other);
            return 1 - d* d<tolerance;
        }
        
        static public MQuaternion Slerp(float fT, ref MQuaternion rkP,
            ref MQuaternion rkQ, bool shortestPath = false)
        {
            float fCos = rkP.Dot(ref rkQ);
            MQuaternion rkT;

            if (fCos < 0.0f && shortestPath)
            {
                fCos = -fCos;
                rkT = -rkQ;
            }
            else
            {
                rkT = rkQ;
            }

            if (UtilMath.Abs(fCos) < 1 - msEpsilon)
            {
                float fSin = UtilMath.Sqrt(1 - UtilMath.Sqr(fCos));
                MRadian fAngle = new MRadian(UtilMath.ATan2(fSin, fCos));
                float fInvSin = 1.0f / fSin;
                float fCoeff0 = UtilMath.Sin((1.0f - fT) * fAngle) * fInvSin;
                float fCoeff1 = UtilMath.Sin(fT * fAngle) * fInvSin;
                return fCoeff0 * rkP + fCoeff1 * rkT;
            }
            else
            {
                MQuaternion t = (1.0f - fT) * rkP + fT * rkT;
                t.normalise();
                return t;
            }
        }

        static public MQuaternion SlerpExtraSpins(float fT,
                    ref MQuaternion rkP, ref MQuaternion rkQ,
            int iExtraSpins)
        {
            float fCos = rkP.Dot(ref rkQ);
            MRadian fAngle = new MRadian(UtilMath.ACos(fCos));

            if (UtilMath.Abs(fAngle.valueRadians()) < msEpsilon)
                return rkP;

            float fSin = UtilMath.Sin(fAngle);
            MRadian fPhase = new MRadian((float)UtilMath.PI * iExtraSpins * fT);
            float fInvSin = 1.0f / fSin;
            float fCoeff0 = UtilMath.Sin((1.0f - fT) * fAngle - fPhase) * fInvSin;
            float fCoeff1 = UtilMath.Sin(fT * fAngle + fPhase) * fInvSin;
            return fCoeff0 * rkP + fCoeff1 * rkQ;
        }

        static public void Intermediate(ref MQuaternion rkQ0,
                    ref MQuaternion rkQ1, ref MQuaternion rkQ2,
            ref MQuaternion rkA, ref MQuaternion rkB)
        {
            MQuaternion kQ0inv = rkQ0.UnitInverse();
            MQuaternion kQ1inv = rkQ1.UnitInverse();
            MQuaternion rkP0 = kQ0inv * rkQ1;
            MQuaternion rkP1 = kQ1inv * rkQ2;
            MQuaternion kArg = 0.25f * (rkP0.Log() - rkP1.Log());
            MQuaternion kMinusArg = -kArg;

            rkA = rkQ1 * kArg.Exp();
            rkB = rkQ1 * kMinusArg.Exp();
        }

        static MQuaternion Squad(float fT, ref MQuaternion rkP,
                    ref MQuaternion rkA, ref MQuaternion rkB,
                    ref MQuaternion rkQ, bool shortestPath = false)
        {
            float fSlerpT = 2.0f * fT * (1.0f - fT);
            MQuaternion kSlerpP = Slerp(fT, ref rkP, ref rkQ, shortestPath);
            MQuaternion kSlerpQ = Slerp(fT, ref rkA, ref rkB);
            return Slerp(fSlerpT, ref kSlerpP, ref kSlerpQ);
        }

        static public MQuaternion nlerp(float fT, ref MQuaternion rkP,
                    ref MQuaternion rkQ, bool shortestPath = false)
        {
            MQuaternion result;
            float fCos = rkP.Dot(ref rkQ);
            if (fCos < 0.0f && shortestPath)
            {
                result = rkP + fT * ((-rkQ) - rkP);
            }
            else
            {
                result = rkP + fT * (rkQ - rkP);
            }
            result.normalise();
            return result;
        }

        public bool isNaN()
        {
            return double.IsNaN(x) || double.IsNaN(y) || double.IsNaN(z) || double.IsNaN(w);
        }

        public string ToString(MQuaternion q )
        {
            string o = "Quaternion(" + q.w + ", " + q.x + ", " + q.y + ", " + q.z + ")";
            return o;
        }

        static public MQuaternion fromNative(Quaternion native)
        {
            return new MQuaternion(native.w, native.x, native.y, native.z);
        }

        public Quaternion toNative()
        {
            return new Quaternion(x, y, z, w);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
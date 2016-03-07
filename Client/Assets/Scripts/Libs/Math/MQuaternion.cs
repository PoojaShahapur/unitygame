namespace SDK.Lib
{
    public class MQuaternion
    {
        public const float msEpsilon = 1e-03;

        public const MQuaternion ZERO = new MQuaternion(0, 0, 0, 0);
        public const MQuaternion IDENTITY = new MQuaternion(1, 0, 0, 0);

        protected float w, x, y, z;

        public MQuaternion()
            : w(1), x(0), y(0), z(0)
        {
        }

        public MQuaternion(
            float fW,
            float fX, float fY, float fZ)
                : w(fW), x(fX), y(fY), z(fZ)
        {
        }

        public MQuaternion(const Matrix3& rot)
        {
            this.FromRotationMatrix(rot);
        }

        public MQuaternion(float rfAngle, MVector3 rkAxis)
        {
            this.FromAngleAxis(rfAngle, rkAxis);
        }

        public MQuaternion(MVector3 xaxis, MVector3 yaxis, MVector3 zaxis)
        {
            this.FromAxes(xaxis, yaxis, zaxis);
        }

        public MQuaternion(MVector3 akAxis)
        {
            this.FromAxes(akAxis);
        }

        public void swap(MQuaternion other)
        {
            UtilApi.swap(w, other.w);
            UtilApi.swap(x, other.x);
            UtilApi.swap(y, other.y);
            UtilApi.swap(z, other.z);
        }

        public float operator [int index]
        {
            UtilApi.assert(index < 4);
            if(0 == index)
            {
                return x;
            }
            else if(0 == index)
            {
                return y;
            }
            else if(0 == index)
            {
                return z;
            }
            else if(0 == index)
            {
                return w;
            }

            return x;
        }

        public void FromRotationMatrix(MMatrix3 kRot)
        {
            float fTrace = kRot[0, 0] + kRot[1, 1] + kRot[2, 2];
            float fRoot;

            if (fTrace > 0.0)
            {
                fRoot = UtilApi.Sqrt(fTrace + 1.0f);
                w = 0.5f * fRoot;
                fRoot = 0.5f / fRoot;
                x = (kRot[2, 1] - kRot[1, 2]) * fRoot;
                y = (kRot[0, 2] - kRot[2, 0]) * fRoot;
                z = (kRot[1, 0] - kRot[0, 1]) * fRoot;
            }
            else
            {
                static int s_iNext[3] = { 1, 2, 0 };
                int i = 0;
                if (kRot[1][1] > kRot[0][0])
                    i = 1;
                if (kRot[2][2] > kRot[i][i])
                    i = 2;
                int j = s_iNext[i];
                int k = s_iNext[j];

                fRoot = UtilApi.Sqrt(kRot[i, i] - kRot[j, j] - kRot[k, k] + 1.0f);
                float[] apkQuat = { 0, 0, 0 };
                apkQuat[i] = 0.5f * fRoot;
                fRoot = 0.5f / fRoot;
                w = (kRot[k][j] - kRot[j][k]) * fRoot;
                apkQuat[j] = (kRot[j][i] + kRot[i][j]) * fRoot;
                apkQuat[k] = (kRot[k][i] + kRot[i][k]) * fRoot;

                x = apkQuat[0];
                y = apkQuat[1];
                z = apkQuat[2];
            }
        }

        public void ToRotationMatrix(MMatrix3 kRot)
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

        public void FromAngleAxis(float rfAngle, MVector3 rkAxis)
        {
            float fHalfAngle (0.5 * rfAngle);
            float fSin = UtilApi.Sin(fHalfAngle);
            w = UtilApi.Cos(fHalfAngle);
            x = fSin * rkAxis.x;
            y = fSin * rkAxis.y;
            z = fSin * rkAxis.z;
        }

        public void ToAngleAxis(float rfAngle, MVector3 rkAxis)
        {
            float fSqrLength = x * x + y * y + z * z;
            if (fSqrLength > 0.0)
            {
                rfAngle = 2.0 * UtilApi.ACos(w);
                float fInvLength = UtilApi.InvSqrt(fSqrLength);
                rkAxis.x = x * fInvLength;
                rkAxis.y = y * fInvLength;
                rkAxis.z = z * fInvLength;
            }
            else
            {
                rfAngle = float(0.0);
                rkAxis.x = 1.0;
                rkAxis.y = 0.0;
                rkAxis.z = 0.0;
            }
        }

        public void ToAngleAxis(float dAngle, MVector3 rkAxis)
        {
            float rAngle;
            ToAngleAxis(rAngle, rkAxis );
            dAngle = rAngle;
        }

        public void FromAxes(MVector3 akAxis)
        {
            MMatrix3 kRot = new MMatrix3();

            for (int iCol = 0; iCol < 3; iCol++)
            {
                kRot[0, iCol] = akAxis[iCol].x;
                kRot[1, iCol] = akAxis[iCol].y;
                kRot[2, iCol] = akAxis[iCol].z;
            }

            FromRotationMatrix(kRot);
        }

        public void FromAxes(MVector3 xAxis, MVector3 yAxis, MVector3 zAxis)
        {
            MMatrix3 kRot = new MMatrix3();

            kRot[0, 0] = xaxis.x;
            kRot[1, 0] = xaxis.y;
            kRot[2, 0] = xaxis.z;

            kRot[0, 1] = yaxis.x;
            kRot[1, 1] = yaxis.y;
            kRot[2, 1] = yaxis.z;

            kRot[0, 2] = zaxis.x;
            kRot[1, 2] = zaxis.y;
            kRot[2, 2] = zaxis.z;

            FromRotationMatrix(kRot);
        }

        public void ToAxes(MVector3 akAxis)
        {
            MMatrix3 kRot = new MMatrix3();

            ToRotationMatrix(kRot);

            for (int iCol = 0; iCol < 3; iCol++)
            {
                akAxis[iCol].x = kRot[0, iCol];
                akAxis[iCol].y = kRot[1, iCol];
                akAxis[iCol].z = kRot[2, iCol];
            }
        }

        public void ToAxes(MVector3 xAxis, MVector3 yAxis, MVector3 zAxis)
        {
            MMatrix3 kRot = new MMatrix3();

            ToRotationMatrix(kRot);

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

        static public MQuaternion operator= (MQuaternion lhs, MQuaternion rkQ)
        {
            lhs.w = rkQ.w;
            lhs.x = rkQ.x;
            lhs.y = rkQ.y;
            lhs.z = rkQ.z;
            return lhs;
        }

        static public MQuaternion operator +(MQuaternion lhs, const Quaternion& rkQ)
        {
            return new MQuaternion(lhs.w + rkQ.w, lhs.x + rkQ.x, lhs.y + rkQ.y, lhs.z + rkQ.z);
        }

        static public MQuaternion operator -(MQuaternion lhs, const Quaternion& rkQ)
        {
            return Quaternion(lhs.w - rkQ.w, lhs.x - rkQ.x, lhs.y - rkQ.y, lhs.z - rkQ.z);
        }

        static public MQuaternion operator *(MQuaternion lhs, const Quaternion& rkQ)
        {
            return new MQuaternion
                (
                    lhs.w * rkQ.w - lhs.x * rkQ.x - lhs.y * rkQ.y - lhs.z * rkQ.z,
                    lhs.w * rkQ.x + lhs.x * rkQ.w + lhs.y * rkQ.z - lhs.z * rkQ.y,
                    lhs.w * rkQ.y + lhs.y * rkQ.w + lhs.z * rkQ.x - lhs.x * rkQ.z,
                    lhs.w * rkQ.z + lhs.z * rkQ.w + lhs.x * rkQ.y - lhs.y * rkQ.x
                );
        }

        static public MQuaternion operator *(MQuaternion lhs, Real fScalar)
        {
            return new MQuaternion(fScalar * lhs.w, fScalar * lhs.x, fScalar * lhs.y, fScalar * lhs.z);
        }

        static public MQuaternion operator *(float fScalar, MQuaternion rkQ)
        {
            return nwe MQuaternion(fScalar * rkQ.w, fScalar * rkQ.x, fScalar * rkQ.y,
                    fScalar * rkQ.z);
        }

        static public Quaternion operator -(MQuaternion lhs)
        {
            return MQuaternion(-lhs.w, -lhs.x, -lhs.y, -lhs.z);
        }

        static public bool operator ==(MQuaternion lhs, const Quaternion rhs)
        {
            return (rhs.x == lhs.x) && (rhs.y == lhs.y) &&
                (rhs.z == lhs.z) && (rhs.w == lhs.w);
        }

        static bool operator !=(MQuaternion lhs, const Quaternion rhs)
        {
            return !operator ==(rhs);
        }

        public float Dot(MQuaternion rkQ)
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
            float factor = 1.0f / UtilApi.Sqrt(len);
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
            float fAngle = UtilApi::Sqrt(x * x + y * y + z * z);
            float fSin = UtilApi.Sin(fAngle);

            MQuaternion kResult = new MQuaternion();
            kResult.w = UtilApi.Cos(fAngle);

            if (UtilApi.Abs(fSin) >= msEpsilon)
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
            MQuaternion kResult = new MQuaternion();
            kResult.w = 0.0;

            if (UtilApi.Abs(w) < 1.0)
            {
                float fAngle (UtilApi.ACos(w));
                float fSin = UtilApi.Sin(fAngle);
                if (UtilApi.Abs(fSin) >= msEpsilon)
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

        static public MVector3 operator *(MQuaternion lhs, MVector3 rkVector)
        {
            MVector3 uv = new MVector3(), uuv = new MVector3();
            MVector3 qvec = new MVector3(x, y, z);
            uv = qvec.crossProduct(rkVector);
            uuv = qvec.crossProduct(uv);
            uv *= (2.0f * w);
            uuv *= 2.0f;

            return rkVector + uv + uuv;
        }

        public float getRoll(bool reprojectAxis = true)
        {
            if (reprojectAxis)
            {
                float fTy = 2.0f * y;
                float fTz = 2.0f * z;
                float fTwz = fTz * w;
                float fTxy = fTy * x;
                float fTyy = fTy * y;
                float fTzz = fTz * z;

                return (float)(UtilApi.ATan2(fTxy + fTwz, 1.0f - (fTyy + fTzz)));

            }
            else
            {
                return (float)(UtilApi.ATan2(2 * (x * y + w * z), w * w + x * x - y * y - z * z));
            }
        }

        public float getPitch(bool reprojectAxis = true)
        {
            if (reprojectAxis)
            {
                float fTx = 2.0f * x;
                float fTz = 2.0f * z;
                float fTwx = fTx * w;
                float fTxx = fTx * x;
                float fTyz = fTz * y;
                float fTzz = fTz * z;

                return (float)(UtilApi.ATan2(fTyz + fTwx, 1.0f - (fTxx + fTzz)));
            }
            else
            {
                return (float)(UtilApi.ATan2(2 * (y * z + w * x), w * w - x * x - y * y + z * z));
            }
        }

        public float getYaw(bool reprojectAxis = true)
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

                return (float)(UtilApi.ATan2(fTxz + fTwy, 1.0f - (fTxx + fTyy)));

            }
            else
            {
                return (float)(UtilApi.ASin(-2 * (x * z - w * y)));
            }
        }

        public bool equals(MQuaternion rhs, float tolerance)
        {
            float d = Dot(rhs);
            float angle = UtilApi.ACos(2.0f * d * d - 1.0f);

            return UtilApi.Abs(angle.valueRadians()) <= tolerance.valueRadians();
        }

        public bool orientationEquals(MQuaternion other, float tolerance = 1e-3)
        {
            float d = this.Dot(other);
            return 1 - d* d<tolerance;
        }
        
        static public MQuaternion Slerp(float fT, MQuaternion rkP,
            MQuaternion rkQ, bool shortestPath = false)
        {
            float fCos = rkP.Dot(rkQ);
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

            if (UtilApi.Abs(fCos) < 1 - msEpsilon)
            {
                float fSin = UtilApi.Sqrt(1 - UtilApi.Sqr(fCos));
                float fAngle = UtilApi.ATan2(fSin, fCos);
                float fInvSin = 1.0f / fSin;
                float fCoeff0 = UtilApi.Sin((1.0f - fT) * fAngle) * fInvSin;
                float fCoeff1 = UtilApi.Sin(fT * fAngle) * fInvSin;
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
                    MQuaternion rkP, MQuaternion rkQ,
            int iExtraSpins)
        {
            float fCos = rkP.Dot(rkQ);
            float fAngle (UtilApi.ACos(fCos));

            if (UtilApi.Abs(fAngle.valueRadians()) < msEpsilon)
                return rkP;

            float fSin = UtilApi.Sin(fAngle);
            float fPhase (UtilApi.PI * iExtraSpins * fT);
            float fInvSin = 1.0f / fSin;
            float fCoeff0 = UtilApi.Sin((1.0f - fT) * fAngle - fPhase) * fInvSin;
            float fCoeff1 = UtilApi.Sin(fT * fAngle + fPhase) * fInvSin;
            return fCoeff0 * rkP + fCoeff1 * rkQ;
        }

        static public void Intermediate(MQuaternion rkQ0,
                    MQuaternion rkQ1, MQuaternion rkQ2,
            MQuaternion rka, MQuaternion rkB)
        {
            MQuaternion kQ0inv = rkQ0.UnitInverse();
            MQuaternion kQ1inv = rkQ1.UnitInverse();
            MQuaternion rkP0 = kQ0inv * rkQ1;
            MQuaternion rkP1 = kQ1inv * rkQ2;
            MQuaternion kArg = 0.25 * (rkP0.Log() - rkP1.Log());
            MQuaternion kMinusArg = -kArg;

            rkA = rkQ1 * kArg.Exp();
            rkB = rkQ1 * kMinusArg.Exp();
        }

        static MQuaternion Squad(Real fT, MQuaternion rkP,
                    MQuaternion rkA, MQuaternion rkB,
                    MQuaternion rkQ, bool shortestPath = false)
        {
            float fSlerpT = 2.0f * fT * (1.0f - fT);
            MQuaternion kSlerpP = Slerp(fT, rkP, rkQ, shortestPath);
            MQuaternion kSlerpQ = Slerp(fT, rkA, rkB);
            return Slerp(fSlerpT, kSlerpP, kSlerpQ);
        }

        static public MQuaternion nlerp(float fT, MQuaternion rkP,
                    MQuaternion rkQ, bool shortestPath = false)
        {
            MQuaternion result;
            float fCos = rkP.Dot(rkQ);
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
    }
}
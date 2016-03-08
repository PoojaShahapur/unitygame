namespace SDK.Lib
{
    public class MMatrix3
    {
        public const float EPSILON = 1e-06f;
        public const MMatrix3 ZERO = new MMatrix3(0, 0, 0, 0, 0, 0, 0, 0, 0);
        public const MMatrix3 IDENTITY = new MMatrix3(1, 0, 0, 0, 1, 0, 0, 0, 1);

        public const float msSvdEpsilon = 1e-04f;
        public const int msSvdMaxIterations = 32;

        public float[, ] m;

        public MMatrix3()
        {
            m = new float[3, 3];
        }

        public MMatrix3(float[, ] arr)
        {
            m[0, 0] = arr[0, 0];
            m[0, 1] = arr[0, 1];
            m[0, 2] = arr[0, 2];

            m[1, 0] = arr[1, 0];
            m[1, 1] = arr[1, 1];
            m[1, 2] = arr[1, 2];

            m[2, 0] = arr[2, 0];
            m[2, 1] = arr[2, 1];
            m[2, 2] = arr[2, 2];
        }

        public MMatrix3(MMatrix3 rkMatrix)
        {
            m[0, 0] = rkMatrix.m[0, 0];
            m[0, 1] = rkMatrix.m[0, 1];
            m[0, 2] = rkMatrix.m[0, 2];

            m[1, 0] = rkMatrix.m[1, 0];
            m[1, 1] = rkMatrix.m[1, 1];
            m[1, 2] = rkMatrix.m[1, 2];

            m[2, 0] = rkMatrix.m[2, 0];
            m[2, 1] = rkMatrix.m[2, 1];
            m[2, 2] = rkMatrix.m[2, 2];
        }

        public MMatrix3(float fEntry00, float fEntry01, float fEntry02,
                    float fEntry10, float fEntry11, float fEntry12,
                    float fEntry20, float fEntry21, float fEntry22)
        {
            m[0, 0] = fEntry00;
            m[0, 1] = fEntry01;
            m[0, 2] = fEntry02;
            m[1, 0] = fEntry10;
            m[1, 1] = fEntry11;
            m[1, 2] = fEntry12;
            m[2, 0] = fEntry20;
            m[2, 1] = fEntry21;
            m[2, 2] = fEntry22;
        }

        public void swap(MMatrix3 other)
        {
            UtilApi.swap(m[0, 0], other.m[0, 0]);
            UtilApi.swap(m[0, 1], other.m[0, 1]);
            UtilApi.swap(m[0, 2], other.m[0, 2]);
            UtilApi.swap(m[1, 0], other.m[1, 0]);
            UtilApi.swap(m[1, 1], other.m[1, 1]);
            UtilApi.swap(m[1, 2], other.m[1, 2]);
            UtilApi.swap(m[2, 0], other.m[2, 0]);
            UtilApi.swap(m[2, 1], other.m[2, 1]);
            UtilApi.swap(m[2, 2], other.m[2, 2]);
        }

        public float[] this[int iRow]
        {
            return m[iRow];
        }

        public MVector3 GetColumn(int iCol)
        {
            UtilApi.assert(iCol < 3);
            return new MVector3(m[0, iCol], m[1, iCol],
                m[2, iCol]);
        }

        public void SetColumn(int iCol, MVector3 vec)
        {
            UtilApi.assert(iCol < 3);
            m[0, iCol] = vec.x;
            m[1, iCol] = vec.y;
            m[2, iCol] = vec.z;
        }

        public void FromAxes(MVector3 xAxis, MVector3 yAxis, MVector3 zAxis)
        {
            SetColumn(0, xAxis);
            SetColumn(1, yAxis);
            SetColumn(2, zAxis);
        }

        static public MMatrix3 operator= (MMatrix3 lhs, MMatrix3 rkMatrix)
        {
            lhs.m[0, 0] = rkMatrix.m[0, 0];
            lhs.m[0, 1] = rkMatrix.m[0, 1];
            lhs.m[0, 2] = rkMatrix.m[0, 2];

            lhs.m[1, 0] = rkMatrix.m[1, 0];
            lhs.m[1, 1] = rkMatrix.m[1, 1];
            lhs.m[1, 2] = rkMatrix.m[1, 2];

            lhs.m[2, 0] = rkMatrix.m[2, 0];
            lhs.m[2, 1] = rkMatrix.m[2, 1];
            lhs.m[2, 2] = rkMatrix.m[2, 2];
            return lhs;
        }

        static public bool operator ==(MMatrix3 lhs, MMatrix3 rkMatrix)
        {
            for (int iRow = 0; iRow < 3; iRow++)
            {
                for (int iCol = 0; iCol < 3; iCol++)
                {
                    if (m[iRow][iCol] != rkMatrix.m[iRow, iCol])
                        return false;
                }
            }

            return true;
        }

        static public bool operator !=(MMatrix3 lhs, MMatrix3 rkMatrix)
        {
            return !operator ==(rkMatrix);
        }

        static public MMatrix3 operator +(MMatrix3 lhs, MMatrix3 rkMatrix)
        {
            MMatrix3 kSum = new MMatrix3();
            for (int iRow = 0; iRow < 3; iRow++)
            {
                for (int iCol = 0; iCol < 3; iCol++)
                {
                    kSum.m[iRow, iCol] = m[iRow, iCol] +
                        rkMatrix.m[iRow, iCol];
                }
            }
            return kSum;
        }

        static public MMatrix3 operator -(MMatrix3 lhs, MMatrix3 rkMatrix)
        {
            MMatrix3 kDiff = new MMatrix3();
            for (int iRow = 0; iRow < 3; iRow++)
            {
                for (int iCol = 0; iCol < 3; iCol++)
                {
                    kDiff.m[iRow, iCol] = m[iRow, iCol] -
                        rkMatrix.m[iRow, iCol];
                }
            }
            return kDiff;
        }

        static public MMatrix3 operator *(MMatrix3 lhs, MMatrix3 rkMatrix)
        {
            MMatrix3 kProd = new MMatrix3();
            for (int iRow = 0; iRow < 3; iRow++)
            {
                for (int iCol = 0; iCol < 3; iCol++)
                {
                    kProd.m[iRow, iCol] =
                        m[iRow, 0] * rkMatrix.m[0, iCol] +
                        m[iRow, 1] * rkMatrix.m[1, iCol] +
                        m[iRow, 2] * rkMatrix.m[2, iCol];
                }
            }
            return kProd;
        }

        static public MMatrix3 operator -(MMatrix3 lhs)
        {
            MMatrix3 kNeg = new MMatrix3();
            for (int iRow = 0; iRow < 3; iRow++)
            {
                for (int iCol = 0; iCol < 3; iCol++)
                    kNeg[iRow, iCol] = -m[iRow, iCol];
            }
            return kNeg;
        }

        static public MVector3 operator *(MMatrix3 lhs, MVector3 rkPoint)
        {
            MVector3 kProd = new MVector3();
            for (int iRow = 0; iRow < 3; iRow++)
            {
                kProd[iRow] =
                    m[iRow, 0] * rkPoint[0] +
                    m[iRow, 1] * rkPoint[1] +
                    m[iRow, 2] * rkPoint[2];
            }
            return kProd;
        }

        static public MVector3 operator *(MVector3 rkPoint,
            MMatrix3 rkMatrix)
        {
            MVector3 kProd = new MVector3();
            for (int iRow = 0; iRow < 3; iRow++)
            {
                kProd[iRow] =
                    rkPoint[0] * rkMatrix.m[0, iRow] +
                    rkPoint[1] * rkMatrix.m[1, iRow] +
                    rkPoint[2] * rkMatrix.m[2, iRow];
            }
            return kProd;
        }

        static public MMatrix3 operator *(MMatrix3 lhs, float fScalar)
        {
            MMatrix3 kProd = new MMatrix3();
            for (int iRow = 0; iRow < 3; iRow++)
            {
                for (int iCol = 0; iCol < 3; iCol++)
                    kProd[iRow, iCol] = fScalar * m[iRow, iCol];
            }
            return kProd;
        }

        static public MMatrix3 operator *(float fScalar, MMatrix3 rkMatrix)
        {
            MMatrix3 kProd = new MMatrix3();
            for (int iRow = 0; iRow< 3; iRow++)
            {
                for (int iCol = 0; iCol< 3; iCol++)
                    kProd[iRow, iCol] = fScalar* rkMatrix.m[iRow, iCol];
            }
            return kProd;
        }

        public MMatrix3 Transpose()
        {
            MMatrix3 kTranspose = new MMatrix3();
            for (int iRow = 0; iRow < 3; iRow++)
            {
                for (int iCol = 0; iCol < 3; iCol++)
                    kTranspose[iRow, iCol] = m[iCol, iRow];
            }
            return kTranspose;
        }
        public bool Inverse(MMatrix3 rkInverse, float fTolerance = 1e-06)
        {
            rkInverse[0, 0] = m[1, 1] * m[2, 2] -
                    m[1, 2] * m[2, 1];
            rkInverse[0, 1] = m[0, 2] * m[2, 1] -
                m[0, 1] * m[2, 2];
            rkInverse[0, 2] = m[0, 1] * m[1, 2] -
                m[0, 2] * m[1, 1];
            rkInverse[1, 0] = m[1, 2] * m[2, 0] -
                m[1, 0] * m[2, 2];
            rkInverse[1, 1] = m[0, 0] * m[2, 2] -
                m[0, 2] * m[2, 0];
            rkInverse[1, 2] = m[0, 2] * m[1, 0] -
                m[0, 0] * m[1, 2];
            rkInverse[2, 0] = m[1, 0] * m[2, 1] -
                m[1, 1] * m[2, 0];
            rkInverse[2, 1] = m[0, 1] * m[2, 0] -
                m[0, 0] * m[2, 1];
            rkInverse[2, 2] = m[0, 0] * m[1, 1] -
                m[0, 1] * m[1, 0];

            Real fDet =
                m[0, 0] * rkInverse[0, 0] +
                m[0, 1] * rkInverse[1, 0] +
                m[0, 2] * rkInverse[2, 0];

            if (UtilApi.Abs(fDet) <= fTolerance)
                return false;

            float fInvDet = 1.0f / fDet;
            for (int iRow = 0; iRow < 3; iRow++)
            {
                for (int iCol = 0; iCol < 3; iCol++)
                    rkInverse[iRow, iCol] *= fInvDet;
            }

            return true;
        }
        public MMatrix3 Inverse(float fTolerance = 1e-06)
        {
            MMatrix3 kInverse = MMatrix3.ZERO;
            Inverse(kInverse, fTolerance);
            return kInverse;
        }
        public float Determinant()
        {
            float fCofactor00 = m[1, 1] * m[2, 2] -
                    m[1, 2] * m[2, 1];
            float fCofactor10 = m[1, 2] * m[2, 0] -
                m[1, 0] * m[2, 2];
            float fCofactor20 = m[1, 0] * m[2, 1] -
                m[1, 1] * m[2, 0];

            float fDet =
                m[0, 0] * fCofactor00 +
                m[0, 1] * fCofactor10 +
                m[0, 2] * fCofactor20;

            return fDet;
        }

        public void SingularValueDecomposition(MMatrix3 rkL, MVector3 rkS,
            MMatrix3 rkR)
        {
            int iRow, iCol;

            MMatrix3 kA = this;
            Bidiagonalize(kA, kL, kR);

            for (int i = 0; i < msSvdMaxIterations; i++)
            {
                float fTmp, fTmp0, fTmp1;
                float fSin0, fCos0, fTan0;
                float fSin1, fCos1, fTan1;

                bool bTest1 = (UtilApi.Abs(kA[0, 1]) <=
                    msSvdEpsilon * (UtilApi::Abs(kA[0, 0]) + UtilApi::Abs(kA[1, 1])));
                bool bTest2 = (UtilApi::Abs(kA[1, 2]) <=
                    msSvdEpsilon * (UtilApi::Abs(kA[1, 1]) + UtilApi::Abs(kA[2, 2])));
                if (bTest1)
                {
                    if (bTest2)
                    {
                        kS[0] = kA[0, 0];
                        kS[1] = kA[1, 1];
                        kS[2] = kA[2, 2];
                        break;
                    }
                    else
                    {
                        fTmp = (kA[1, 1] * kA[1, 1] - kA[2, 2] * kA[2, 2] +
                            kA[1, 2] * kA[1, 2]) / (kA[1, 2] * kA[2, 2]);
                        fTan0 = 0.5f * (fTmp + UtilApi.Sqrt(fTmp * fTmp + 4.0f));
                        fCos0 = UtilApi.InvSqrt(1.0f + fTan0 * fTan0);
                        fSin0 = fTan0 * fCos0;

                        for (iCol = 0; iCol < 3; iCol++)
                        {
                            fTmp0 = kL[iCol, 1];
                            fTmp1 = kL[iCol, 2];
                            kL[iCol, 1] = fCos0 * fTmp0 - fSin0 * fTmp1;
                            kL[iCol, 2] = fSin0 * fTmp0 + fCos0 * fTmp1;
                        }

                        fTan1 = (kA[1, 2] - kA[2, 2] * fTan0) / kA[1, 1];
                        fCos1 = UtilApi.InvSqrt(1.0f + fTan1 * fTan1);
                        fSin1 = -fTan1 * fCos1;

                        for (iRow = 0; iRow < 3; iRow++)
                        {
                            fTmp0 = kR[1, iRow];
                            fTmp1 = kR[2, iRow];
                            kR[1, iRow] = fCos1 * fTmp0 - fSin1 * fTmp1;
                            kR[2, iRow] = fSin1 * fTmp0 + fCos1 * fTmp1;
                        }

                        kS[0] = kA[0, 0];
                        kS[1] = fCos0 * fCos1 * kA[1, 1] -
                            fSin1 * (fCos0 * kA[1, 2] - fSin0 * kA[2, 2]);
                        kS[2] = fSin0 * fSin1 * kA[1][1] +
                            fCos1 * (fSin0 * kA[1, 2] + fCos0 * kA[2, 2]);
                        break;
                    }
                }
                else
                {
                    if (bTest2)
                    {
                        fTmp = (kA[0, 0] * kA[0, 0] + kA[1, 1] * kA[1, 1] -
                            kA[0, 1] * kA[0, 1]) / (kA[0, 1] * kA[1, 1]);
                        fTan0 = 0.5f * (-fTmp + UtilApi.Sqrt(fTmp * fTmp + 4.0f));
                        fCos0 = UtilApi.InvSqrt(1.0f + fTan0 * fTan0);
                        fSin0 = fTan0 * fCos0;

                        for (iCol = 0; iCol < 3; iCol++)
                        {
                            fTmp0 = kL[iCol, 0];
                            fTmp1 = kL[iCol, 1];
                            kL[iCol, 0] = fCos0 * fTmp0 - fSin0 * fTmp1;
                            kL[iCol, 1] = fSin0 * fTmp0 + fCos0 * fTmp1;
                        }

                        fTan1 = (kA[0, 1] - kA[1, 1] * fTan0) / kA[0, 0];
                        fCos1 = UtilApi.InvSqrt(1.0f + fTan1 * fTan1);
                        fSin1 = -fTan1 * fCos1;

                        for (iRow = 0; iRow < 3; iRow++)
                        {
                            fTmp0 = kR[0, iRow];
                            fTmp1 = kR[1, iRow];
                            kR[0, iRow] = fCos1 * fTmp0 - fSin1 * fTmp1;
                            kR[1, iRow] = fSin1 * fTmp0 + fCos1 * fTmp1;
                        }

                        kS[0] = fCos0 * fCos1 * kA[0, 0] -
                            fSin1 * (fCos0 * kA[0, 1] - fSin0 * kA[1, 1]);
                        kS[1] = fSin0 * fSin1 * kA[0, 0] +
                            fCos1 * (fSin0 * kA[0, 1] + fCos0 * kA[1, 1]);
                        kS[2] = kA[2][2];
                        break;
                    }
                    else
                    {
                        GolubKahanStep(kA, kL, kR);
                    }
                }
            }

            for (iRow = 0; iRow < 3; iRow++)
            {
                if (kS[iRow] < 0.0)
                {
                    kS[iRow] = -kS[iRow];
                    for (iCol = 0; iCol < 3; iCol++)
                        kR[iRow, iCol] = -kR[iRow, iCol];
                }
            }
        }

        public void SingularValueComposition(MMatrix3 rkL,
                    MVector3 rkS, MMatrix3 rkR)
        {
            int iRow, iCol;
            MMatrix3 kTmp = new MMatrix3();

            for (iRow = 0; iRow < 3; iRow++)
            {
                for (iCol = 0; iCol < 3; iCol++)
                    kTmp[iRow, iCol] = kS[iRow] * kR[iRow, iCol];
            }

            for (iRow = 0; iRow < 3; iRow++)
            {
                for (iCol = 0; iCol < 3; iCol++)
                {
                    m[iRow, iCol] = 0.0;
                    for (int iMid = 0; iMid < 3; iMid++)
                        m[iRow, iCol] += kL[iRow, iMid] * kTmp[iMid, iCol];
                }
            }
        }

        public void Orthonormalize()
        {
            float fInvLength = UtilApi.InvSqrt(m[0, 0] * m[0, 0]
                    + m[1, 0] * m[1, 0] +
                    m[2, 0] * m[2, 0]);

            m[0, 0] *= fInvLength;
            m[1, 0] *= fInvLength;
            m[2, 0] *= fInvLength;

            float fDot0 =
                m[0, 0] * m[0, 1] +
                m[1, 0] * m[1, 1] +
                m[2, 0] * m[2, 1];

            m[0, 1] -= fDot0 * m[0, 0];
            m[1, 1] -= fDot0 * m[1, 0];
            m[2, 1] -= fDot0 * m[2, 0];

            fInvLength = UtilApi::InvSqrt(m[0, 1] * m[0, 1] +
                m[1, 1] * m[1, 1] +
                m[2, 1] * m[2, 1]);

            m[0, 1] *= fInvLength;
            m[1, 1] *= fInvLength;
            m[2, 1] *= fInvLength;

            float fDot1 =
                m[0, 1] * m[0, 2] +
                m[1, 1] * m[1, 2] +
                m[2, 1] * m[2, 2];

            fDot0 =
                m[0, 0] * m[0, 2] +
                m[1, 0] * m[1, 2] +
                m[2, 0] * m[2, 2];

            m[0, 2] -= fDot0 * m[0, 0] + fDot1 * m[0, 1];
            m[1, 2] -= fDot0 * m[1, 0] + fDot1 * m[1, 1];
            m[2, 2] -= fDot0 * m[2, 0] + fDot1 * m[2, 1];

            fInvLength = UtilApi.InvSqrt(m[0, 2] * m[0, 2] +
                m[1, 2] * m[1, 2] +
                m[2, 2] * m[2, 2]);

            m[0, 2] *= fInvLength;
            m[1, 2] *= fInvLength;
            m[2, 2] *= fInvLength;
        }

        public void QDUDecomposition(MMatrix3 rkQ, MVector3 rkD,
            MVector3 rkU)
        {
            float fInvLength = UtilApi.InvSqrt(m[0, 0] * m[0, 0] + m[1, 0] * m[1, 0] + m[2, 0] * m[2, 0]);

            kQ[0, 0] = m[0, 0] * fInvLength;
            kQ[1, 0] = m[1, 0] * fInvLength;
            kQ[2, 0] = m[2, 0] * fInvLength;

            Real fDot = kQ[0, 0] * m[0, 1] + kQ[1, 0] * m[1, 1] +
                kQ[2, 0] * m[2, 1];
            kQ[0, 1] = m[0, 1] - fDot * kQ[0, 0];
            kQ[1, 1] = m[1, 1] - fDot * kQ[1, 0];
            kQ[2, 1] = m[2, 1] - fDot * kQ[2, 0];
            fInvLength = UtilApi.InvSqrt(kQ[0][1] * kQ[0][1] + kQ[1][1] * kQ[1][1] + kQ[2][1] * kQ[2][1]);

            kQ[0, 1] *= fInvLength;
            kQ[1, 1] *= fInvLength;
            kQ[2, 1] *= fInvLength;

            fDot = kQ[0, 0] * m[0, 2] + kQ[1, 0] * m[1, 2] +
                kQ[2, 0] * m[2, 2];
            kQ[0][2] = m[0][2] - fDot * kQ[0, 0];
            kQ[1][2] = m[1][2] - fDot * kQ[1, 0];
            kQ[2][2] = m[2][2] - fDot * kQ[2, 0];
            fDot = kQ[0, 1] * m[0, 2] + kQ[1, 1] * m[1, 2] +
                kQ[2, 1] * m[2, 2];
            kQ[0, 2] -= fDot * kQ[0, 1];
            kQ[1, 2] -= fDot * kQ[1, 1];
            kQ[2, 2] -= fDot * kQ[2, 1];
            fInvLength = UtilApi.InvSqrt(kQ[0, 2] * kQ[0, 2] + kQ[1, 2] * kQ[1, 2] + kQ[2, 2] * kQ[2, 2]);

            kQ[0, 2] *= fInvLength;
            kQ[1, 2] *= fInvLength;
            kQ[2, 2] *= fInvLength;

            float fDet = kQ[0, 0] * kQ[1, 1] * kQ[2, 2] + kQ[0, 1] * kQ[1, 2] * kQ[2, 0] +
                kQ[0, 2] * kQ[1, 0] * kQ[2, 1] - kQ[0, 2] * kQ[1, 1] * kQ[2, 0] -
                kQ[0, 1] * kQ[1, 0] * kQ[2, 2] - kQ[0, 0] * kQ[1, 2] * kQ[2, 1];

            if (fDet < 0.0)
            {
                for (int iRow = 0; iRow < 3; iRow++)
                    for (int iCol = 0; iCol < 3; iCol++)
                        kQ[iRow, iCol] = -kQ[iRow, iCol];
            }

            MMatrix3 kR = new MMatrix3();
            kR[0, 0] = kQ[0, 0] * m[0, 0] + kQ[1, 0] * m[1, 0] +
                kQ[2, 0] * m[2, 0];
            kR[0, 1] = kQ[0, 0] * m[0, 1] + kQ[1, 0] * m[1, 1] +
                kQ[2, 0] * m[2, 1];
            kR[1, 1] = kQ[0, 1] * m[0, 1] + kQ[1, 1] * m[1, 1] +
                kQ[2, 1] * m[2, 1];
            kR[0, 2] = kQ[0, 0] * m[0, 2] + kQ[1, 0] * m[1, 2] +
                kQ[2, 0] * m[2, 2];
            kR[1, 2] = kQ[0, 1] * m[0, 2] + kQ[1, 1] * m[1, 2] +
                kQ[2, 1] * m[2, 2];
            kR[2, 2] = kQ[0, 2] * m[0, 2] + kQ[1, 2] * m[1, 2] +
                kQ[2, 2] * m[2, 2];

            kD[0] = kR[0, 0];
            kD[1] = kR[1, 1];
            kD[2] = kR[2, 2];

            float fInvD0 = 1.0f / kD[0];
            kU[0] = kR[0, 1] * fInvD0;
            kU[1] = kR[0, 2] * fInvD0;
            kU[2] = kR[1, 2] / kD[1];
        }

        public float SpectralNorm()
        {
            MMatrix3 kP = new MMatrix3();
            int iRow, iCol;
            float fPmax = 0.0f;
            for (iRow = 0; iRow < 3; iRow++)
            {
                for (iCol = 0; iCol < 3; iCol++)
                {
                    kP[iRow, iCol] = 0.0;
                    for (int iMid = 0; iMid < 3; iMid++)
                    {
                        kP[iRow, iCol] +=
                            m[iMid, iRow] * m[iMid, iCol];
                    }
                    if (kP[iRow, iCol] > fPmax)
                        fPmax = kP[iRow, iCol];
                }
            }

            float fInvPmax = 1.0f / fPmax;
            for (iRow = 0; iRow < 3; iRow++)
            {
                for (iCol = 0; iCol < 3; iCol++)
                    kP[iRow][iCol] *= fInvPmax;
            }

            float[] afCoeff = new float[3];
            afCoeff[0] = -(kP[0, 0] * (kP[1, 1] * kP[2, 2] - kP[1, 2] * kP[2, 1]) +
                kP[0, 1] * (kP[2, 0] * kP[1, 2] - kP[1, 0] * kP[2, 2]) +
                kP[0, 2] * (kP[1, 0] * kP[2, 1] - kP[2, 0] * kP[1, 1]));
            afCoeff[1] = kP[0][0] * kP[11] - kP[0, 1] * kP[1, 0] +
                kP[0, 0] * kP[2, 2] - kP[0, 2] * kP[2, 0] +
                kP[1, 1] * kP[2, 2] - kP[1, 2] * kP[2, 1];
            afCoeff[2] = -(kP[0, 0] + kP[1, 1] + kP[2, 2]);

            float fRoot = MaxCubicRoot(afCoeff);
            float fNorm = UtilApi.Sqrt(fPmax * fRoot);
            return fNorm;
        }

        public void ToAngleAxis(MVector3 rkAxis, ref float rfRadians)
        {
            float fTrace = m[0, 0] + m[1, 1] + m[2, 2];
            float fCos = 0.5f * (fTrace - 1.0f);
            rfRadians = UtilApi.ACos(fCos);

            if (rfRadians > (float)(0.0f))
            {
                if (rfRadians < (float)(UtilApi.PI))
                {
                    rkAxis.x = m[2, 1] - m[1, 2];
                    rkAxis.y = m[0, 2] - m[2, 0];
                    rkAxis.z = m[1, 0] - m[0, 1];
                    rkAxis.normalise();
                }
                else
                {
                    float fHalfInverse;
                    if (m[0, 0] >= m[1, 1])
                    {
                        if (m[0, 0] >= m[2, 2])
                        {
                            rkAxis.x = 0.5f * UtilApi.Sqrt(m[0, 0] -
                                m[1, 1] - m[2, 2] + 1.0f);
                            fHalfInverse = 0.5f / rkAxis.x;
                            rkAxis.y = fHalfInverse * m[0, 1];
                            rkAxis.z = fHalfInverse * m[0, 2];
                        }
                        else
                        {
                            rkAxis.z = 0.5f * UtilApi.Sqrt(m[2, 2] -
                                m[0, 0] - m[1, 1] + 1.0f);
                            fHalfInverse = 0.5f / rkAxis.z;
                            rkAxis.x = fHalfInverse * m[0, 2];
                            rkAxis.y = fHalfInverse * m[1, 2];
                        }
                    }
                    else
                    {
                        if (m[1, 1] >= m[2, 2])
                        {
                            rkAxis.y = 0.5f * UtilApi::Sqrt(m[1, 1] -
                                m[0, 0] - m[2, 2] + 1.0f);
                            fHalfInverse = 0.5f / rkAxis.y;
                            rkAxis.x = fHalfInverse * m[0, 1];
                            rkAxis.z = fHalfInverse * m[1, 2];
                        }
                        else
                        {
                            rkAxis.z = 0.5f * UtilApi.Sqrt(m[2, 2] -
                                m[0, 0] - m[1, 1] + 1.0f);
                            fHalfInverse = 0.5f / rkAxis.z;
                            rkAxis.x = fHalfInverse * m[0, 2];
                            rkAxis.y = fHalfInverse * m[1, 2];
                        }
                    }
                }
            }
            else
            {
                rkAxis.x = 1.0f;
                rkAxis.y = 0.0f;
                rkAxis.z = 0.0f;
            }
        }

        public void ToAngleAxis(MVector3 rkAxis, ref float rfAngle)
        {
            float r;
            ToAngleAxis(rkAxis, ref r);
            rfAngle = r;
        }

        public void FromAngleAxis(MVector3 rkAxis, float fRadians)
        {
            float fCos = UtilApi.Cos(fRadians);
            float fSin = UtilApi.Sin(fRadians);
            float fOneMinusCos = 1.0f - fCos;
            float fX2 = rkAxis.x * rkAxis.x;
            float fY2 = rkAxis.y * rkAxis.y;
            float fZ2 = rkAxis.z * rkAxis.z;
            float fXYM = rkAxis.x * rkAxis.y * fOneMinusCos;
            float fXZM = rkAxis.x * rkAxis.z * fOneMinusCos;
            float fYZM = rkAxis.y * rkAxis.z * fOneMinusCos;
            float fXSin = rkAxis.x * fSin;
            float fYSin = rkAxis.y * fSin;
            float fZSin = rkAxis.z * fSin;

            m[0, 0] = fX2 * fOneMinusCos + fCos;
            m[0, 1] = fXYM - fZSin;
            m[0, 2] = fXZM + fYSin;
            m[1, 0] = fXYM + fZSin;
            m[1, 1] = fY2 * fOneMinusCos + fCos;
            m[1, 2] = fYZM - fXSin;
            m[2, 0] = fXZM - fYSin;
            m[2, 1] = fYZM + fXSin;
            m[2, 2] = fZ2 * fOneMinusCos + fCos;
        }

        public bool ToEulerAnglesXYZ(ref float rfYAngle, ref float rfPAngle,
            ref float rfRAngle)
        {
            rfPAngle = (float)(UtilApi.ASin(m[0, 2]));
            if (rfPAngle < (float)(UtilApi.HALF_PI))
            {
                if (rfPAngle > (float)(-UtilApi.HALF_PI))
                {
                    rfYAngle = UtilApi.ATan2(-m[1, 2], m[2, 2]);
                    rfRAngle = UtilApi.ATan2(-m[0, 1], m[0, 0]);
                    return true;
                }
                else
                {
                    float fRmY = UtilApi.ATan2(m[1, 0], m[1, 1]);
                    rfRAngle = (float)(0.0f);
                    rfYAngle = rfRAngle - fRmY;
                    return false;
                }
            }
            else
            {
                float fRpY = UtilApi.ATan2(m[1, 0], m[1, 1]);
                rfRAngle = (float)(0.0f);
                rfYAngle = fRpY - rfRAngle;
                return false;
            }
        }

        public bool ToEulerAnglesXZY(ref float rfYAngle, ref float rfPAngle,
            ref float rfRAngle)
        {
            rfPAngle = UtilApi.ASin(-m[0][1]);
            if (rfPAngle < (float)(UtilApi.HALF_PI))
            {
                if (rfPAngle > (float)(-UtilApi.HALF_PI))
                {
                    rfYAngle = UtilApi.ATan2(m[2, 1], m[1, 1]);
                    rfRAngle = UtilApi.ATan2(m[0, 2], m[0, 0]);
                    return true;
                }
                else
                {
                    float fRmY = UtilApi.ATan2(-m[2, 0], m[2, 2]);
                    rfRAngle = (float)(0.0f);
                    rfYAngle = rfRAngle - fRmY;
                    return false;
                }
            }
            else
            {
                float fRpY = UtilApi.ATan2(-m[2, 0], m[2, 2]);
                rfRAngle = (float)(0.0);
                rfYAngle = fRpY - rfRAngle;
                return false;
            }
        }

        public bool ToEulerAnglesYXZ(ref float rfYAngle, ref float rfPAngle,
            ref float rfRAngle)
        {
            rfPAngle = UtilApi.ASin(-m[1, 2]);
            if (rfPAngle < (float)(UtilApi.HALF_PI))
            {
                if (rfPAngle > Radian(-UtilApi.HALF_PI))
                {
                    rfYAngle = UtilApi.ATan2(m[0, 2], m[2, 2]);
                    rfRAngle = UtilApi.ATan2(m[1, 0], m[1, 1]);
                    return true;
                }
                else
                {
                    float fRmY = UtilApi.ATan2(-m[0, 1], m[0, 0]);
                    rfRAngle = (float)(0.0f);
                    rfYAngle = rfRAngle - fRmY;
                    return false;
                }
            }
            else
            {
                float fRpY = UtilApi.ATan2(-m[0, 1], m[0, 0]);
                rfRAngle = (float)(0.0);
                rfYAngle = fRpY - rfRAngle;
                return false;
            }
        }

        public bool ToEulerAnglesYZX(ref float rfYAngle, ref float rfPAngle,
            ref float rfRAngle)
        {
            rfPAngle = UtilApi.ASin(m[1, 0]);
            if (rfPAngle < (float)(UtilApi.HALF_PI))
            {
                if (rfPAngle > (float)(-UtilApi.HALF_PI))
                {
                    rfYAngle = UtilApi.ATan2(-m[2, 0], m[0, 0]);
                    rfRAngle = UtilApi.ATan2(-m[1, 2], m[1, 1]);
                    return true;
                }
                else
                {
                    float fRmY = UtilApi.ATan2(m[2, 1], m[2, 2]);
                    rfRAngle = (float)(0.0);
                    rfYAngle = rfRAngle - fRmY;
                    return false;
                }
            }
            else
            {
                float fRpY = UtilApi.ATan2(m[2, 1], m[2, 2]);
                rfRAngle = (float)(0.0);
                rfYAngle = fRpY - rfRAngle;
                return false;
            }
        }

        public bool ToEulerAnglesZXY(ref float rfYAngle, ref float rfPAngle,
            ref float rfRAngle)
        {
            rfPAngle = UtilApi.ASin(m[2, 1]);
            if (rfPAngle < (float)(UtilApi.HALF_PI))
            {
                if (rfPAngle > (float)(-UtilApi.HALF_PI))
                {
                    rfYAngle = UtilApi.ATan2(-m[0, 1], m[1, 1]);
                    rfRAngle = UtilApi.ATan2(-m[2, 0], m[2, 2]);
                    return true;
                }
                else
                {
                    float fRmY = UtilApi.ATan2(m[0, 2], m[0, 0]);
                    rfRAngle = (float)(0.0);
                    rfYAngle = rfRAngle - fRmY;
                    return false;
                }
            }
            else
            {
                float fRpY = UtilApi.ATan2(m[0, 2], m[0, 0]);
                rfRAngle = (float)(0.0);
                rfYAngle = fRpY - rfRAngle;
                return false;
            }
        }

        public bool ToEulerAnglesZYX(ref float rfYAngle, ref float rfPAngle,
            ref float rfRAngle)
        {
            rfPAngle = UtilApi.ASin(-m[2, 0]);
            if (rfPAngle < (float)(UtilApi.HALF_PI))
            {
                if (rfPAngle > (float)(-UtilApi.HALF_PI))
                {
                    rfYAngle = UtilApi.ATan2(m[1, 0], m[0, 0]);
                    rfRAngle = UtilApi.ATan2(m[2, 1], m[2, 2]);
                    return true;
                }
                else
                {
                    float fRmY = UtilApi.ATan2(-m[0, 1], m[0, 2]);
                    rfRAngle = (float)(0.0);
                    rfYAngle = rfRAngle - fRmY;
                    return false;
                }
            }
            else
            {
                float fRpY = UtilApi.ATan2(-m[0, 1], m[0, 2]);
                rfRAngle = (float)(0.0);
                rfYAngle = fRpY - rfRAngle;
                return false;
            }
        }

        public void FromEulerAnglesXYZ(float fYAngle, float fPAngle, float fRAngle)
        {
            float fCos, fSin;

            fCos = UtilApi.Cos(fYAngle);
            fSin = UtilApi.Sin(fYAngle);
            MMatrix3 kXMat = new MMatrix3(1.0, 0.0, 0.0, 0.0, fCos, -fSin, 0.0, fSin, fCos);

            fCos = UtilApi.Cos(fPAngle);
            fSin = UtilApi.Sin(fPAngle);
            MMatrix3 kYMat = new MMatrix3(fCos,0.0, fSin, 0.0, 1.0, 0.0, -fSin, 0.0, fCos);

            fCos = UtilApi.Cos(fRAngle);
            fSin = UtilApi.Sin(fRAngle);
            MMatrix3 kZMat = new MMatrix3(fCos,-fSin, 0.0, fSin, fCos, 0.0, 0.0, 0.0, 1.0);

            this = kXMat * (kYMat * kZMat);
        }

        public void FromEulerAnglesXZY(float fYAngle, float fPAngle, float fRAngle)
        {
            float fCos, fSin;

            fCos = UtilApi.Cos(fYAngle);
            fSin = UtilApi.Sin(fYAngle);
            MMatrix3 kXMat = new MMatrix3(1.0, 0.0, 0.0, 0.0, fCos, -fSin, 0.0, fSin, fCos);

            fCos = UtilApi.Cos(fPAngle);
            fSin = UtilApi.Sin(fPAngle);
            MMatrix3 kZMat = new MMatrix3(fCos,-fSin, 0.0, fSin, fCos, 0.0, 0.0, 0.0, 1.0);

            fCos = UtilApi.Cos(fRAngle);
            fSin = UtilApi.Sin(fRAngle);
            MMatrix3 kYMat = new MMatrix3(fCos,0.0, fSin, 0.0, 1.0, 0.0, -fSin, 0.0, fCos);

            this = kXMat * (kZMat * kYMat);
        }

        public void FromEulerAnglesYXZ(float fYAngle, float fPAngle, float fRAngle)
        {
            float fCos, fSin;

            fCos = UtilApi.Cos(fYAngle);
            fSin = UtilApi.Sin(fYAngle);
            Matrix3 kYMat(fCos,0.0, fSin, 0.0, 1.0, 0.0, -fSin, 0.0, fCos);

            fCos = UtilApi.Cos(fPAngle);
            fSin = UtilApi.Sin(fPAngle);
            Matrix3 kXMat(1.0, 0.0, 0.0, 0.0, fCos, -fSin, 0.0, fSin, fCos);

            fCos = UtilApi.Cos(fRAngle);
            fSin = UtilApi.Sin(fRAngle);
            Matrix3 kZMat(fCos,-fSin, 0.0, fSin, fCos, 0.0, 0.0, 0.0, 1.0);

            this = kYMat * (kXMat * kZMat);
        }

        public void FromEulerAnglesYZX(float fYAngle, float fPAngle, float fRAngle)
        {
            float fCos, fSin;

            fCos = UtilApi.Cos(fYAngle);
            fSin = UtilApi.Sin(fYAngle);
            MMatrix3 kYMat = new MMatrix3(fCos,0.0, fSin, 0.0, 1.0, 0.0, -fSin, 0.0, fCos);

            fCos = UtilApi.Cos(fPAngle);
            fSin = UtilApi.Sin(fPAngle);
            MMatrix3 kZMat = new MMatrix3(fCos,-fSin, 0.0, fSin, fCos, 0.0, 0.0, 0.0, 1.0);

            fCos = UtilApi.Cos(fRAngle);
            fSin = UtilApi.Sin(fRAngle);
            MMatrix3 kXMat = new MMatrix3(1.0, 0.0, 0.0, 0.0, fCos, -fSin, 0.0, fSin, fCos);

            this = kYMat * (kZMat * kXMat);
        }

        public void FromEulerAnglesZXY(float fYAngle, float fPAngle, float fRAngle)
        {
            float fCos, fSin;

            fCos = UtilApi.Cos(fYAngle);
            fSin = UtilApi.Sin(fYAngle);
            MMatrix3 kZMat = new MMatrix3(fCos,-fSin, 0.0, fSin, fCos, 0.0, 0.0, 0.0, 1.0);

            fCos = UtilApi.Cos(fPAngle);
            fSin = UtilApi.Sin(fPAngle);
            MMatrix3 kXMat = new MMatrix3(1.0, 0.0, 0.0, 0.0, fCos, -fSin, 0.0, fSin, fCos);

            fCos = UtilApi.Cos(fRAngle);
            fSin = UtilApi.Sin(fRAngle);
            MMatrix3 kYMat = new MMatrix3(fCos,0.0, fSin, 0.0, 1.0, 0.0, -fSin, 0.0, fCos);

            this = kZMat * (kXMat * kYMat);
        }

        public void FromEulerAnglesZYX(float fYAngle, float fPAngle, float fRAngle)
        {
            float fCos, fSin;

            fCos = UtilApi.Cos(fYAngle);
            fSin = UtilApi.Sin(fYAngle);
            MMatrix3 kZMat(fCos,-fSin, 0.0, fSin, fCos, 0.0, 0.0, 0.0, 1.0);

            fCos = UtilApi.Cos(fPAngle);
            fSin = UtilApi.Sin(fPAngle);
            MMatrix3 kYMat(fCos,0.0, fSin, 0.0, 1.0, 0.0, -fSin, 0.0, fCos);

            fCos = UtilApi.Cos(fRAngle);
            fSin = UtilApi.Sin(fRAngle);
            MMatrix3 kXMat = new MMatrix3(1.0, 0.0, 0.0, 0.0, fCos, -fSin, 0.0, fSin, fCos);

            this = kZMat * (kYMat * kXMat);
        }

        public void EigenSolveSymmetric(float[] afEigenvalue,
            MVector3[] akEigenvector)
        {
            MMatrix3 kMatrix = this;
            float[] afSubDiag = new float[3];
            kMatrix.Tridiagonal(afEigenvalue, afSubDiag);
            kMatrix.QLAlgorithm(afEigenvalue, afSubDiag);

            for (int i = 0; i < 3; i++)
            {
                akEigenvector[i][0] = kMatrix[0][i];
                akEigenvector[i][1] = kMatrix[1][i];
                akEigenvector[i][2] = kMatrix[2][i];
            }

            MVector3 kCross = akEigenvector[1].crossProduct(akEigenvector[2]);
            float fDet = akEigenvector[0].dotProduct(kCross);
            if (fDet < 0.0f)
            {
                akEigenvector[2][0] = -akEigenvector[2][0];
                akEigenvector[2][1] = -akEigenvector[2][1];
                akEigenvector[2][2] = -akEigenvector[2][2];
            }
        }

        static public void TensorProduct(MVector3 rkU, MVector3 rkV,
            MMatrix3 rkProduct)
        {
            for (int iRow = 0; iRow < 3; iRow++)
            {
                for (int iCol = 0; iCol < 3; iCol++)
                    rkProduct[iRow, iCol] = rkU[iRow] * rkV[iCol];
            }
        }

        public bool hasScale()
        {
            float t = m[0, 0] * m[0, 0] + m[1, 0] * m[1, 0] + m[2, 0] * m[2, 0];
            if (!UtilApi.RealEqual(t, 1.0, (float)1e-04))
                return true;
            t = m[0, 1] * m[0, 1] + m[1, 1] * m[1, 1] + m[2, 1] * m[2, 1];
            if (!UtilApi.RealEqual(t, 1.0, (float)1e-04))
                return true;
            t = m[0, 2] * m[0, 2] + m[1, 2] * m[1, 2] + m[2, 2] * m[2, 2];
            if (!UtilApi.RealEqual(t, 1.0, (float)1e-04))
                return true;

            return false;
        }

        static public string ToString(MMatrix3 mat )
        {
            string o = "Matrix3(" + mat[0, 0] + ", " + mat[0, 1] + ", " + mat[0, 2] + ", "
                            + mat[1, 0] + ", " + mat[1, 1] + ", " + mat[1, 2] + ", "
                            + mat[2, 0] + ", " + mat[2, 1] + ", " + mat[2, 2] + ")";
            return o;
        }



        public void Tridiagonal(float[] afDiag, float[] afSubDiag)
        {
            float fA = m[0, 0];
            float fB = m[0, 1];
            float fC = m[0, 2];
            float fD = m[1, 1];
            float fE = m[1, 2];
            float fF = m[2, 2];

            afDiag[0] = fA;
            afSubDiag[2] = 0.0f;
            if (UtilApi.Abs(fC) >= EPSILON)
            {
                float fLength = UtilApi.Sqrt(fB * fB + fC * fC);
                float fInvLength = 1.0f / fLength;
                fB *= fInvLength;
                fC *= fInvLength;
                float fQ = 2.0f * fB * fE + fC * (fF - fD);
                afDiag[1] = fD + fC * fQ;
                afDiag[2] = fF - fC * fQ;
                afSubDiag[0] = fLength;
                afSubDiag[1] = fE - fB * fQ;
                m[0, 0] = 1.0;
                m[0, 1] = 0.0;
                m[0, 2] = 0.0;
                m[1, 0] = 0.0;
                m[1, 1] = fB;
                m[1, 2] = fC;
                m[2, 0] = 0.0;
                m[2, 1] = fC;
                m[2, 2] = -fB;
            }
            else
            {
                afDiag[1] = fD;
                afDiag[2] = fF;
                afSubDiag[0] = fB;
                afSubDiag[1] = fE;
                m[0, 0] = 1.0f;
                m[0, 1] = 0.0f;
                m[0, 2] = 0.0f;
                m[1, 0] = 0.0f;
                m[1, 1] = 1.0f;
                m[1, 2] = 0.0f;
                m[2, 0] = 0.0f;
                m[2, 1] = 0.0f;
                m[2, 2] = 1.0f;
            }
        }

        public bool QLAlgorithm(float[] afDiag, float[] afSubDiag)
        {
            for (int i0 = 0; i0 < 3; i0++)
            {
                const int iMaxIter = 32;
                int iIter;
                for (iIter = 0; iIter < iMaxIter; iIter++)
                {
                    int i1;
                    for (i1 = i0; i1 <= 1; i1++)
                    {
                        float fSum = UtilApi.Abs(afDiag[i1]) +
                            UtilApi.Abs(afDiag[i1 + 1]);
                        if (UtilApi.Abs(afSubDiag[i1]) + fSum == fSum)
                            break;
                    }
                    if (i1 == i0)
                        break;

                    float fTmp0 = (afDiag[i0 + 1] - afDiag[i0]) / (2.0f * afSubDiag[i0]);
                    float fTmp1 = UtilAPi.Sqrt(fTmp0 * fTmp0 + 1.0f);
                    if (fTmp0 < 0.0)
                        fTmp0 = afDiag[i1] - afDiag[i0] + afSubDiag[i0] / (fTmp0 - fTmp1);
                    else
                        fTmp0 = afDiag[i1] - afDiag[i0] + afSubDiag[i0] / (fTmp0 + fTmp1);
                    float fSin = 1.0f;
                    float fCos = 1.0f;
                    float fTmp2 = 0.0f;
                    for (int i2 = i1 - 1; i2 >= i0; i2--)
                    {
                        float fTmp3 = fSin * afSubDiag[i2];
                        float fTmp4 = fCos * afSubDiag[i2];
                        if (UtilApi.Abs(fTmp3) >= UtilApi.Abs(fTmp0))
                        {
                            fCos = fTmp0 / fTmp3;
                            fTmp1 = UtilApi.Sqrt(fCos * fCos + 1.0f);
                            afSubDiag[i2 + 1] = fTmp3 * fTmp1;
                            fSin = 1.0f / fTmp1;
                            fCos *= fSin;
                        }
                        else
                        {
                            fSin = fTmp3 / fTmp0;
                            fTmp1 = UtilApi.Sqrt(fSin * fSin + 1.0f);
                            afSubDiag[i2 + 1] = fTmp0 * fTmp1;
                            fCos = 1.0f / fTmp1;
                            fSin *= fCos;
                        }
                        fTmp0 = afDiag[i2 + 1] - fTmp2;
                        fTmp1 = (afDiag[i2] - fTmp0) * fSin + 2.0f * fTmp4 * fCos;
                        fTmp2 = fSin * fTmp1;
                        afDiag[i2 + 1] = fTmp0 + fTmp2;
                        fTmp0 = fCos * fTmp1 - fTmp4;

                        for (int iRow = 0; iRow < 3; iRow++)
                        {
                            fTmp3 = m[iRow, i2 + 1];
                            m[iRow, i2 + 1] = fSin * m[iRow, i2] +
                                fCos * fTmp3;
                            m[iRow, i2] = fCos * m[iRow, i2] -
                                fSin * fTmp3;
                        }
                    }
                    afDiag[i0] -= fTmp2;
                    afSubDiag[i0] = fTmp0;
                    afSubDiag[i1] = 0.0f;
                }

                if (iIter == iMaxIter)
                {
                    return false;
                }
            }

            return true;
        }

        static public void Bidiagonalize(MMatrix3 kA, MMatrix3 kL,
            MMatrix3 kR)
        {
            float[] afV = new float[3];
            float[] afW = new float[3];
            float fLength, fSign, fT1, fInvT1, fT2;
            bool bIdentity;

            fLength = UtilApi.Sqrt(kA[0, 0] * kA[0, 0] + kA[1, 0] * kA[1, 0] +
                kA[2, 0] * kA[2, 0]);
            if (fLength > 0.0)
            {
                fSign = (kA[0, 0] > 0.0f ? 1.0f : -1.0f);
                fT1 = kA[0, 0] + fSign * fLength;
                fInvT1 = 1.0f / fT1;
                afV[1] = kA[1, 0] * fInvT1;
                afV[2] = kA[2, 0] * fInvT1;

                fT2 = -2.0f / (1.0f + afV[1] * afV[1] + afV[2] * afV[2]);
                afW[0] = fT2 * (kA[0, 0] + kA[1, 0] * afV[1] + kA[2, 0] * afV[2]);
                afW[1] = fT2 * (kA[0, 1] + kA[1, 1] * afV[1] + kA[2, 1] * afV[2]);
                afW[2] = fT2 * (kA[0, 2] + kA[1, 2] * afV[1] + kA[2, 2] * afV[2]);
                kA[0, 0] += afW[0];
                kA[0, 1] += afW[1];
                kA[0, 2] += afW[2];
                kA[1, 1] += afV[1] * afW[1];
                kA[1, 2] += afV[1] * afW[2];
                kA[2, 1] += afV[2] * afW[1];
                kA[2, 2] += afV[2] * afW[2];

                kL[0, 0] = 1.0f + fT2;
                kL[0, 1] = kL[1, 0] = fT2 * afV[1];
                kL[0, 2] = kL[2, 0] = fT2 * afV[2];
                kL[1, 1] = 1.0f + fT2 * afV[1] * afV[1];
                kL[1, 2] = kL[2, 1] = fT2 * afV[1] * afV[2];
                kL[2, 2] = 1.0f + fT2 * afV[2] * afV[2];
                bIdentity = false;
            }
            else
            {
                kL = MMatrix3.IDENTITY;
                bIdentity = true;
            }

            fLength = UtilApi.Sqrt(kA[0, 1] * kA[0, 1] + kA[0, 2] * kA[0, 2]);
            if (fLength > 0.0)
            {
                fSign = (kA[0, 1] > 0.0f ? 1.0f : -1.0f);
                fT1 = kA[0, 1] + fSign * fLength;
                afV[2] = kA[0, 2] / fT1;

                fT2 = -2.0f / (1.0f + afV[2] * afV[2]);
                afW[0] = fT2 * (kA[0, 1] + kA[0, 2] * afV[2]);
                afW[1] = fT2 * (kA[1, 1] + kA[1, 2] * afV[2]);
                afW[2] = fT2 * (kA[2, 1] + kA[2, 2] * afV[2]);
                kA[0, 1] += afW[0];
                kA[1, 1] += afW[1];
                kA[1, 2] += afW[1] * afV[2];
                kA[2, 1] += afW[2];
                kA[2, 2] += afW[2] * afV[2];

                kR[0, 0] = 1.0;
                kR[0, 1] = kR[1, 0] = 0.0;
                kR[0, 2] = kR[2, 0] = 0.0;
                kR[1, 1] = 1.0f + fT2;
                kR[1, 2] = kR[2, 1] = fT2 * afV[2];
                kR[2, 2] = 1.0f + fT2 * afV[2] * afV[2];
            }
            else
            {
                kR = MMatrix3.IDENTITY;
            }

            fLength = UtilApi.Sqrt(kA[1][1] * kA[1][1] + kA[2][1] * kA[2][1]);
            if (fLength > 0.0)
            {
                fSign = (kA[1, 1] > 0.0f ? 1.0f : -1.0f);
                fT1 = kA[1, 1] + fSign * fLength;
                afV[2] = kA[2, 1] / fT1;

                fT2 = -2.0f / (1.0f + afV[2] * afV[2]);
                afW[1] = fT2 * (kA[1, 1] + kA[2, 1] * afV[2]);
                afW[2] = fT2 * (kA[1, 2] + kA[2, 2] * afV[2]);
                kA[1, 1] += afW[1];
                kA[1, 2] += afW[2];
                kA[2, 2] += afV[2] * afW[2];

                float fA = 1.0f + fT2;
                float fB = fT2 * afV[2];
                float fC = 1.0f + fB * afV[2];

                if (bIdentity)
                {
                    kL[0, 0] = 1.0;
                    kL[0, 1] = kL[1, 0] = 0.0;
                    kL[0, 2] = kL[2, 0] = 0.0;
                    kL[1, 1] = fA;
                    kL[1, 2] = kL[2, 1] = fB;
                    kL[2, 2] = fC;
                }
                else
                {
                    for (int iRow = 0; iRow < 3; iRow++)
                    {
                        float fTmp0 = kL[iRow][1];
                        float fTmp1 = kL[iRow][2];
                        kL[iRow, 1] = fA * fTmp0 + fB * fTmp1;
                        kL[iRow, 2] = fB * fTmp0 + fC * fTmp1;
                    }
                }
            }
        }

        static public void GolubKahanStep(MMatrix3 kA, MMatrix3 kL,
            MMatrix3 kR)
        {
            float fT11 = kA[0][1] * kA[0][1] + kA[1][1] * kA[1][1];
            float fT22 = kA[1][2] * kA[1][2] + kA[2][2] * kA[2][2];
            float fT12 = kA[1][1] * kA[1][2];
            float fTrace = fT11 + fT22;
            float fDiff = fT11 - fT22;
            float fDiscr = UtilApi.Sqrt(fDiff * fDiff + 4.0f * fT12 * fT12);
            float fRoot1 = 0.5f * (fTrace + fDiscr);
            float fRoot2 = 0.5f * (fTrace - fDiscr);

            float fY = kA[0][0] - (UtilApi.Abs(fRoot1 - fT22) <=
                UtilApi.Abs(fRoot2 - fT22) ? fRoot1 : fRoot2);
            float fZ = kA[0][1];
            float fInvLength = UtilApi.InvSqrt(fY * fY + fZ * fZ);
            float fSin = fZ * fInvLength;
            float fCos = -fY * fInvLength;

            float fTmp0 = kA[0][0];
            float fTmp1 = kA[0][1];
            kA[0][0] = fCos * fTmp0 - fSin * fTmp1;
            kA[0][1] = fSin * fTmp0 + fCos * fTmp1;
            kA[1][0] = -fSin * kA[1][1];
            kA[1][1] *= fCos;

            int iRow;
            for (iRow = 0; iRow < 3; iRow++)
            {
                fTmp0 = kR[0, iRow];
                fTmp1 = kR[1, iRow];
                kR[0, iRow] = fCos * fTmp0 - fSin * fTmp1;
                kR[1, iRow] = fSin * fTmp0 + fCos * fTmp1;
            }

            fY = kA[0][0];
            fZ = kA[1][0];
            fInvLength = UtilApi.InvSqrt(fY * fY + fZ * fZ);
            fSin = fZ * fInvLength;
            fCos = -fY * fInvLength;

            kA[0, 0] = fCos * kA[0, 0] - fSin * kA[1, 0];
            fTmp0 = kA[0, 1];
            fTmp1 = kA[1, 1];
            kA[0, 1] = fCos * fTmp0 - fSin * fTmp1;
            kA[1, 1] = fSin * fTmp0 + fCos * fTmp1;
            kA[0, 2] = -fSin * kA[1, 2];
            kA[1, 2] *= fCos;

            int iCol;
            for (iCol = 0; iCol < 3; iCol++)
            {
                fTmp0 = kL[iCol, 0];
                fTmp1 = kL[iCol, 1];
                kL[iCol, 0] = fCos * fTmp0 - fSin * fTmp1;
                kL[iCol, 1] = fSin * fTmp0 + fCos * fTmp1;
            }

            fY = kA[0, 1];
            fZ = kA[0, 2];
            fInvLength = UtilApi.InvSqrt(fY * fY + fZ * fZ);
            fSin = fZ * fInvLength;
            fCos = -fY * fInvLength;

            kA[0, 1] = fCos * kA[0, 1] - fSin * kA[0, 2];
            fTmp0 = kA[1, 1];
            fTmp1 = kA[1, 2];
            kA[1, 1] = fCos * fTmp0 - fSin * fTmp1;
            kA[1, 2] = fSin * fTmp0 + fCos * fTmp1;
            kA[2, 1] = -fSin * kA[2][2];
            kA[2, 2] *= fCos;

            for (iRow = 0; iRow < 3; iRow++)
            {
                fTmp0 = kR[1, iRow];
                fTmp1 = kR[2, iRow];
                kR[1, iRow] = fCos * fTmp0 - fSin * fTmp1;
                kR[2, iRow] = fSin * fTmp0 + fCos * fTmp1;
            }

            fY = kA[1, 1];
            fZ = kA[2, 1];
            fInvLength = UtilApi.InvSqrt(fY * fY + fZ * fZ);
            fSin = fZ * fInvLength;
            fCos = -fY * fInvLength;

            kA[1, 1] = fCos * kA[1, 1] - fSin * kA[2, 1];
            fTmp0 = kA[1, 2];
            fTmp1 = kA[2, 2];
            kA[1, 2] = fCos * fTmp0 - fSin * fTmp1;
            kA[2, 2] = fSin * fTmp0 + fCos * fTmp1;

            for (iCol = 0; iCol < 3; iCol++)
            {
                fTmp0 = kL[iCol, 1];
                fTmp1 = kL[iCol, 2];
                kL[iCol, 1] = fCos * fTmp0 - fSin * fTmp1;
                kL[iCol, 2] = fSin * fTmp0 + fCos * fTmp1;
            }
        }

        static public float MaxCubicRoot(float[] afCoeff)
        {
            const float fOneThird = 1.0f / 3.0f;
            const float fEpsilon = 1e-06f;
            float fDiscr = afCoeff[2] * afCoeff[2] - 3.0f * afCoeff[1];
            if (fDiscr <= fEpsilon)
                return -fOneThird * afCoeff[2];

            float fX = 1.0f;
            float fPoly = afCoeff[0] + fX * (afCoeff[1] + fX * (afCoeff[2] + fX));
            if (fPoly < 0.0)
            {
                fX = UtilApi.Abs(afCoeff[0]);
                float fTmp = 1.0f + UtilAPi.Abs(afCoeff[1]);
                if (fTmp > fX)
                    fX = fTmp;
                fTmp = 1.0f + UtilApi.Abs(afCoeff[2]);
                if (fTmp > fX)
                    fX = fTmp;
            }

            float fTwoC2 = 2.0f * afCoeff[2];
            for (int i = 0; i < 16; i++)
            {
                fPoly = afCoeff[0] + fX * (afCoeff[1] + fX * (afCoeff[2] + fX));
                if (UtilApi.Abs(fPoly) <= fEpsilon)
                    return fX;

                float fDeriv = afCoeff[1] + fX * (fTwoC2 + 3.0f * fX);
                fX -= fPoly / fDeriv;
            }

            return fX;
        }
    }
}
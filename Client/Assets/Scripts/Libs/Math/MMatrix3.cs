namespace SDK.Lib
{
    public class MMatrix3
    {
        public const Real EPSILON;
        public const Matrix3 ZERO;
        public const Matrix3 IDENTITY;

        public const float msSvdEpsilon;
        public const int msSvdMaxIterations;

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
            for (size_t iRow = 0; iRow < 3; iRow++)
            {
                for (size_t iCol = 0; iCol < 3; iCol++)
                {
                    if (m[iRow][iCol] != rkMatrix.m[iRow][iCol])
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
            Matrix3 kSum;
            for (size_t iRow = 0; iRow < 3; iRow++)
            {
                for (size_t iCol = 0; iCol < 3; iCol++)
                {
                    kSum.m[iRow][iCol] = m[iRow][iCol] +
                        rkMatrix.m[iRow][iCol];
                }
            }
            return kSum;
        }

        static public MMatrix3 operator -(MMatrix3 lhs, MMatrix3 rkMatrix)
        {
            Matrix3 kDiff;
            for (size_t iRow = 0; iRow < 3; iRow++)
            {
                for (size_t iCol = 0; iCol < 3; iCol++)
                {
                    kDiff.m[iRow][iCol] = m[iRow][iCol] -
                        rkMatrix.m[iRow][iCol];
                }
            }
            return kDiff;
        }

        /** Matrix concatenation using '*'.
         */
        static public Matrix3 operator *(MMatrix3 lhs, MMatrix3 rkMatrix)
        {
            Matrix3 kProd;
            for (size_t iRow = 0; iRow < 3; iRow++)
            {
                for (size_t iCol = 0; iCol < 3; iCol++)
                {
                    kProd.m[iRow][iCol] =
                        m[iRow][0] * rkMatrix.m[0][iCol] +
                        m[iRow][1] * rkMatrix.m[1][iCol] +
                        m[iRow][2] * rkMatrix.m[2][iCol];
                }
            }
            return kProd;
        }

        static public Matrix3 operator -(MMatrix3 lhs)
        {
            Matrix3 kNeg;
            for (size_t iRow = 0; iRow < 3; iRow++)
            {
                for (size_t iCol = 0; iCol < 3; iCol++)
                    kNeg[iRow][iCol] = -m[iRow][iCol];
            }
            return kNeg;
        }

        static public Vector3 operator *(MMatrix3 lhs, MVector3 rkVector)
        {
            Vector3 kProd;
            for (size_t iRow = 0; iRow < 3; iRow++)
            {
                kProd[iRow] =
                    m[iRow][0] * rkPoint[0] +
                    m[iRow][1] * rkPoint[1] +
                    m[iRow][2] * rkPoint[2];
            }
            return kProd;
        }

        static public Vector3 operator *(Vector3 rkVector,
            Matrix3 rkMatrix)
        {
            Vector3 kProd;
            for (size_t iRow = 0; iRow < 3; iRow++)
            {
                kProd[iRow] =
                    rkPoint[0] * rkMatrix.m[0][iRow] +
                    rkPoint[1] * rkMatrix.m[1][iRow] +
                    rkPoint[2] * rkMatrix.m[2][iRow];
            }
            return kProd;
        }

        static public Matrix3 operator *(MMatrix3 lhs, Real fScalar)
        {
            Matrix3 kProd;
            for (size_t iRow = 0; iRow < 3; iRow++)
            {
                for (size_t iCol = 0; iCol < 3; iCol++)
                    kProd[iRow][iCol] = fScalar * m[iRow][iCol];
            }
            return kProd;
        }

        static public Matrix3 operator *(Real fScalar, const Matrix3& rkMatrix)
        {
            Matrix3 kProd;
            for (size_t iRow = 0; iRow< 3; iRow++)
            {
                for (size_t iCol = 0; iCol< 3; iCol++)
                    kProd[iRow][iCol] = fScalar* rkMatrix.m[iRow][iCol];
            }
            return kProd;
        }

        public Matrix3 Transpose()
        {
            Matrix3 kTranspose;
            for (size_t iRow = 0; iRow < 3; iRow++)
            {
                for (size_t iCol = 0; iCol < 3; iCol++)
                    kTranspose[iRow][iCol] = m[iCol][iRow];
            }
            return kTranspose;
        }
        public bool Inverse(Matrix3& rkInverse, Real fTolerance = 1e-06)
        {
            rkInverse[0][0] = m[1][1] * m[2][2] -
                    m[1][2] * m[2][1];
            rkInverse[0][1] = m[0][2] * m[2][1] -
                m[0][1] * m[2][2];
            rkInverse[0][2] = m[0][1] * m[1][2] -
                m[0][2] * m[1][1];
            rkInverse[1][0] = m[1][2] * m[2][0] -
                m[1][0] * m[2][2];
            rkInverse[1][1] = m[0][0] * m[2][2] -
                m[0][2] * m[2][0];
            rkInverse[1][2] = m[0][2] * m[1][0] -
                m[0][0] * m[1][2];
            rkInverse[2][0] = m[1][0] * m[2][1] -
                m[1][1] * m[2][0];
            rkInverse[2][1] = m[0][1] * m[2][0] -
                m[0][0] * m[2][1];
            rkInverse[2][2] = m[0][0] * m[1][1] -
                m[0][1] * m[1][0];

            Real fDet =
                m[0][0] * rkInverse[0][0] +
                m[0][1] * rkInverse[1][0] +
                m[0][2] * rkInverse[2][0];

            if (Math::Abs(fDet) <= fTolerance)
                return false;

            Real fInvDet = 1.0f / fDet;
            for (size_t iRow = 0; iRow < 3; iRow++)
            {
                for (size_t iCol = 0; iCol < 3; iCol++)
                    rkInverse[iRow][iCol] *= fInvDet;
            }

            return true;
        }
        public Matrix3 Inverse(Real fTolerance = 1e-06)
        {
            Matrix3 kInverse = Matrix3::ZERO;
            Inverse(kInverse, fTolerance);
            return kInverse;
        }
        public Real Determinant()
        {
            Real fCofactor00 = m[1][1] * m[2][2] -
                    m[1][2] * m[2][1];
            Real fCofactor10 = m[1][2] * m[2][0] -
                m[1][0] * m[2][2];
            Real fCofactor20 = m[1][0] * m[2][1] -
                m[1][1] * m[2][0];

            Real fDet =
                m[0][0] * fCofactor00 +
                m[0][1] * fCofactor10 +
                m[0][2] * fCofactor20;

            return fDet;
        }

public void SingularValueDecomposition(Matrix3& rkL, Vector3& rkS,
    Matrix3& rkR)
{
    size_t iRow, iCol;

    Matrix3 kA = *this;
    Bidiagonalize(kA, kL, kR);

    for (unsigned int i = 0; i < msSvdMaxIterations; i++)
    {
        Real fTmp, fTmp0, fTmp1;
        Real fSin0, fCos0, fTan0;
        Real fSin1, fCos1, fTan1;

        bool bTest1 = (Math::Abs(kA[0][1]) <=
            msSvdEpsilon * (Math::Abs(kA[0][0]) + Math::Abs(kA[1][1])));
        bool bTest2 = (Math::Abs(kA[1][2]) <=
            msSvdEpsilon * (Math::Abs(kA[1][1]) + Math::Abs(kA[2][2])));
        if (bTest1)
        {
            if (bTest2)
            {
                kS[0] = kA[0][0];
                kS[1] = kA[1][1];
                kS[2] = kA[2][2];
                break;
            }
            else
            {
                // 2x2 closed form factorization
                fTmp = (kA[1][1] * kA[1][1] - kA[2][2] * kA[2][2] +
                    kA[1][2] * kA[1][2]) / (kA[1][2] * kA[2][2]);
                fTan0 = 0.5f * (fTmp + Math::Sqrt(fTmp * fTmp + 4.0f));
                fCos0 = Math::InvSqrt(1.0f + fTan0 * fTan0);
                fSin0 = fTan0 * fCos0;

                for (iCol = 0; iCol < 3; iCol++)
                {
                    fTmp0 = kL[iCol][1];
                    fTmp1 = kL[iCol][2];
                    kL[iCol][1] = fCos0 * fTmp0 - fSin0 * fTmp1;
                    kL[iCol][2] = fSin0 * fTmp0 + fCos0 * fTmp1;
                }

                fTan1 = (kA[1][2] - kA[2][2] * fTan0) / kA[1][1];
                fCos1 = Math::InvSqrt(1.0f + fTan1 * fTan1);
                fSin1 = -fTan1 * fCos1;

                for (iRow = 0; iRow < 3; iRow++)
                {
                    fTmp0 = kR[1][iRow];
                    fTmp1 = kR[2][iRow];
                    kR[1][iRow] = fCos1 * fTmp0 - fSin1 * fTmp1;
                    kR[2][iRow] = fSin1 * fTmp0 + fCos1 * fTmp1;
                }

                kS[0] = kA[0][0];
                kS[1] = fCos0 * fCos1 * kA[1][1] -
                    fSin1 * (fCos0 * kA[1][2] - fSin0 * kA[2][2]);
                kS[2] = fSin0 * fSin1 * kA[1][1] +
                    fCos1 * (fSin0 * kA[1][2] + fCos0 * kA[2][2]);
                break;
            }
        }
        else
        {
            if (bTest2)
            {
                // 2x2 closed form factorization
                fTmp = (kA[0][0] * kA[0][0] + kA[1][1] * kA[1][1] -
                    kA[0][1] * kA[0][1]) / (kA[0][1] * kA[1][1]);
                fTan0 = 0.5f * (-fTmp + Math::Sqrt(fTmp * fTmp + 4.0f));
                fCos0 = Math::InvSqrt(1.0f + fTan0 * fTan0);
                fSin0 = fTan0 * fCos0;

                for (iCol = 0; iCol < 3; iCol++)
                {
                    fTmp0 = kL[iCol][0];
                    fTmp1 = kL[iCol][1];
                    kL[iCol][0] = fCos0 * fTmp0 - fSin0 * fTmp1;
                    kL[iCol][1] = fSin0 * fTmp0 + fCos0 * fTmp1;
                }

                fTan1 = (kA[0][1] - kA[1][1] * fTan0) / kA[0][0];
                fCos1 = Math::InvSqrt(1.0f + fTan1 * fTan1);
                fSin1 = -fTan1 * fCos1;

                for (iRow = 0; iRow < 3; iRow++)
                {
                    fTmp0 = kR[0][iRow];
                    fTmp1 = kR[1][iRow];
                    kR[0][iRow] = fCos1 * fTmp0 - fSin1 * fTmp1;
                    kR[1][iRow] = fSin1 * fTmp0 + fCos1 * fTmp1;
                }

                kS[0] = fCos0 * fCos1 * kA[0][0] -
                    fSin1 * (fCos0 * kA[0][1] - fSin0 * kA[1][1]);
                kS[1] = fSin0 * fSin1 * kA[0][0] +
                    fCos1 * (fSin0 * kA[0][1] + fCos0 * kA[1][1]);
                kS[2] = kA[2][2];
                break;
            }
            else
            {
                GolubKahanStep(kA, kL, kR);
            }
        }
    }

    // positize diagonal
    for (iRow = 0; iRow < 3; iRow++)
    {
        if (kS[iRow] < 0.0)
        {
            kS[iRow] = -kS[iRow];
            for (iCol = 0; iCol < 3; iCol++)
                kR[iRow][iCol] = -kR[iRow][iCol];
        }
    }
}

public void SingularValueComposition(const Matrix3& rkL,
            const Vector3& rkS, const Matrix3& rkR)
{
    size_t iRow, iCol;
    Matrix3 kTmp;

    // product S*R
    for (iRow = 0; iRow < 3; iRow++)
    {
        for (iCol = 0; iCol < 3; iCol++)
            kTmp[iRow][iCol] = kS[iRow] * kR[iRow][iCol];
    }

    // product L*S*R
    for (iRow = 0; iRow < 3; iRow++)
    {
        for (iCol = 0; iCol < 3; iCol++)
        {
            m[iRow][iCol] = 0.0;
            for (int iMid = 0; iMid < 3; iMid++)
                m[iRow][iCol] += kL[iRow][iMid] * kTmp[iMid][iCol];
        }
    }
}

public void Orthonormalize()
{
    Real fInvLength = Math::InvSqrt(m[0][0] * m[0][0]
            + m[1][0] * m[1][0] +
            m[2][0] * m[2][0]);

    m[0][0] *= fInvLength;
    m[1][0] *= fInvLength;
    m[2][0] *= fInvLength;

    // compute q1
    Real fDot0 =
        m[0][0] * m[0][1] +
        m[1][0] * m[1][1] +
        m[2][0] * m[2][1];

    m[0][1] -= fDot0 * m[0][0];
    m[1][1] -= fDot0 * m[1][0];
    m[2][1] -= fDot0 * m[2][0];

    fInvLength = Math::InvSqrt(m[0][1] * m[0][1] +
        m[1][1] * m[1][1] +
        m[2][1] * m[2][1]);

    m[0][1] *= fInvLength;
    m[1][1] *= fInvLength;
    m[2][1] *= fInvLength;

    // compute q2
    Real fDot1 =
        m[0][1] * m[0][2] +
        m[1][1] * m[1][2] +
        m[2][1] * m[2][2];

    fDot0 =
        m[0][0] * m[0][2] +
        m[1][0] * m[1][2] +
        m[2][0] * m[2][2];

    m[0][2] -= fDot0 * m[0][0] + fDot1 * m[0][1];
    m[1][2] -= fDot0 * m[1][0] + fDot1 * m[1][1];
    m[2][2] -= fDot0 * m[2][0] + fDot1 * m[2][1];

    fInvLength = Math::InvSqrt(m[0][2] * m[0][2] +
        m[1][2] * m[1][2] +
        m[2][2] * m[2][2]);

    m[0][2] *= fInvLength;
    m[1][2] *= fInvLength;
    m[2][2] *= fInvLength;
}

public void QDUDecomposition(Matrix3& rkQ, Vector3& rkD,
    Vector3& rkU)
{
    Real fInvLength = Math::InvSqrt(m[0][0] * m[0][0] + m[1][0] * m[1][0] + m[2][0] * m[2][0]);

    kQ[0][0] = m[0][0] * fInvLength;
    kQ[1][0] = m[1][0] * fInvLength;
    kQ[2][0] = m[2][0] * fInvLength;

    Real fDot = kQ[0][0] * m[0][1] + kQ[1][0] * m[1][1] +
        kQ[2][0] * m[2][1];
    kQ[0][1] = m[0][1] - fDot * kQ[0][0];
    kQ[1][1] = m[1][1] - fDot * kQ[1][0];
    kQ[2][1] = m[2][1] - fDot * kQ[2][0];
    fInvLength = Math::InvSqrt(kQ[0][1] * kQ[0][1] + kQ[1][1] * kQ[1][1] + kQ[2][1] * kQ[2][1]);

    kQ[0][1] *= fInvLength;
    kQ[1][1] *= fInvLength;
    kQ[2][1] *= fInvLength;

    fDot = kQ[0][0] * m[0][2] + kQ[1][0] * m[1][2] +
        kQ[2][0] * m[2][2];
    kQ[0][2] = m[0][2] - fDot * kQ[0][0];
    kQ[1][2] = m[1][2] - fDot * kQ[1][0];
    kQ[2][2] = m[2][2] - fDot * kQ[2][0];
    fDot = kQ[0][1] * m[0][2] + kQ[1][1] * m[1][2] +
        kQ[2][1] * m[2][2];
    kQ[0][2] -= fDot * kQ[0][1];
    kQ[1][2] -= fDot * kQ[1][1];
    kQ[2][2] -= fDot * kQ[2][1];
    fInvLength = Math::InvSqrt(kQ[0][2] * kQ[0][2] + kQ[1][2] * kQ[1][2] + kQ[2][2] * kQ[2][2]);

    kQ[0][2] *= fInvLength;
    kQ[1][2] *= fInvLength;
    kQ[2][2] *= fInvLength;

    // guarantee that orthogonal matrix has determinant 1 (no reflections)
    Real fDet = kQ[0][0] * kQ[1][1] * kQ[2][2] + kQ[0][1] * kQ[1][2] * kQ[2][0] +
        kQ[0][2] * kQ[1][0] * kQ[2][1] - kQ[0][2] * kQ[1][1] * kQ[2][0] -
        kQ[0][1] * kQ[1][0] * kQ[2][2] - kQ[0][0] * kQ[1][2] * kQ[2][1];

    if (fDet < 0.0)
    {
        for (size_t iRow = 0; iRow < 3; iRow++)
            for (size_t iCol = 0; iCol < 3; iCol++)
                kQ[iRow][iCol] = -kQ[iRow][iCol];
    }

    // build "right" matrix R
    Matrix3 kR;
    kR[0][0] = kQ[0][0] * m[0][0] + kQ[1][0] * m[1][0] +
        kQ[2][0] * m[2][0];
    kR[0][1] = kQ[0][0] * m[0][1] + kQ[1][0] * m[1][1] +
        kQ[2][0] * m[2][1];
    kR[1][1] = kQ[0][1] * m[0][1] + kQ[1][1] * m[1][1] +
        kQ[2][1] * m[2][1];
    kR[0][2] = kQ[0][0] * m[0][2] + kQ[1][0] * m[1][2] +
        kQ[2][0] * m[2][2];
    kR[1][2] = kQ[0][1] * m[0][2] + kQ[1][1] * m[1][2] +
        kQ[2][1] * m[2][2];
    kR[2][2] = kQ[0][2] * m[0][2] + kQ[1][2] * m[1][2] +
        kQ[2][2] * m[2][2];

    // the scaling component
    kD[0] = kR[0][0];
    kD[1] = kR[1][1];
    kD[2] = kR[2][2];

    // the shear component
    Real fInvD0 = 1.0f / kD[0];
    kU[0] = kR[0][1] * fInvD0;
    kU[1] = kR[0][2] * fInvD0;
    kU[2] = kR[1][2] / kD[1];
}

public Real SpectralNorm()
{
    Matrix3 kP;
    size_t iRow, iCol;
    Real fPmax = 0.0;
    for (iRow = 0; iRow < 3; iRow++)
    {
        for (iCol = 0; iCol < 3; iCol++)
        {
            kP[iRow][iCol] = 0.0;
            for (int iMid = 0; iMid < 3; iMid++)
            {
                kP[iRow][iCol] +=
                    m[iMid][iRow] * m[iMid][iCol];
            }
            if (kP[iRow][iCol] > fPmax)
                fPmax = kP[iRow][iCol];
        }
    }

    Real fInvPmax = 1.0f / fPmax;
    for (iRow = 0; iRow < 3; iRow++)
    {
        for (iCol = 0; iCol < 3; iCol++)
            kP[iRow][iCol] *= fInvPmax;
    }

    Real afCoeff[3];
    afCoeff[0] = -(kP[0][0] * (kP[1][1] * kP[2][2] - kP[1][2] * kP[2][1]) +
        kP[0][1] * (kP[2][0] * kP[1][2] - kP[1][0] * kP[2][2]) +
        kP[0][2] * (kP[1][0] * kP[2][1] - kP[2][0] * kP[1][1]));
    afCoeff[1] = kP[0][0] * kP[1][1] - kP[0][1] * kP[1][0] +
        kP[0][0] * kP[2][2] - kP[0][2] * kP[2][0] +
        kP[1][1] * kP[2][2] - kP[1][2] * kP[2][1];
    afCoeff[2] = -(kP[0][0] + kP[1][1] + kP[2][2]);

    Real fRoot = MaxCubicRoot(afCoeff);
    Real fNorm = Math::Sqrt(fPmax * fRoot);
    return fNorm;
}

/// Note: Matrix must be orthonormal
public void ToAngleAxis(Vector3& rkAxis, Radian& rfAngle)
{
    Real fTrace = m[0][0] + m[1][1] + m[2][2];
    Real fCos = 0.5f * (fTrace - 1.0f);
    rfRadians = Math::ACos(fCos);  // in [0,PI]

    if (rfRadians > Radian(0.0))
    {
        if (rfRadians < Radian(Math::PI))
        {
            rkAxis.x = m[2][1] - m[1][2];
            rkAxis.y = m[0][2] - m[2][0];
            rkAxis.z = m[1][0] - m[0][1];
            rkAxis.normalise();
        }
        else
        {
            // angle is PI
            float fHalfInverse;
            if (m[0][0] >= m[1][1])
            {
                // r00 >= r11
                if (m[0][0] >= m[2][2])
                {
                    // r00 is maximum diagonal term
                    rkAxis.x = 0.5f * Math::Sqrt(m[0][0] -
                        m[1][1] - m[2][2] + 1.0f);
                    fHalfInverse = 0.5f / rkAxis.x;
                    rkAxis.y = fHalfInverse * m[0][1];
                    rkAxis.z = fHalfInverse * m[0][2];
                }
                else
                {
                    // r22 is maximum diagonal term
                    rkAxis.z = 0.5f * Math::Sqrt(m[2][2] -
                        m[0][0] - m[1][1] + 1.0f);
                    fHalfInverse = 0.5f / rkAxis.z;
                    rkAxis.x = fHalfInverse * m[0][2];
                    rkAxis.y = fHalfInverse * m[1][2];
                }
            }
            else
            {
                // r11 > r00
                if (m[1][1] >= m[2][2])
                {
                    // r11 is maximum diagonal term
                    rkAxis.y = 0.5f * Math::Sqrt(m[1][1] -
                        m[0][0] - m[2][2] + 1.0f);
                    fHalfInverse = 0.5f / rkAxis.y;
                    rkAxis.x = fHalfInverse * m[0][1];
                    rkAxis.z = fHalfInverse * m[1][2];
                }
                else
                {
                    // r22 is maximum diagonal term
                    rkAxis.z = 0.5f * Math::Sqrt(m[2][2] -
                        m[0][0] - m[1][1] + 1.0f);
                    fHalfInverse = 0.5f / rkAxis.z;
                    rkAxis.x = fHalfInverse * m[0][2];
                    rkAxis.y = fHalfInverse * m[1][2];
                }
            }
        }
    }
    else
    {
        // The angle is 0 and the matrix is the identity.  Any axis will
        // work, so just use the x-axis.
        rkAxis.x = 1.0;
        rkAxis.y = 0.0;
        rkAxis.z = 0.0;
    }
}
    public void ToAngleAxis(Vector3& rkAxis, Degree& rfAngle) const 
    {
        Radian r;
        ToAngleAxis(rkAxis, r );
        rfAngle = r;
    }
       public void FromAngleAxis(const Vector3& rkAxis, const Radian& fRadians)
{
    Real fCos = Math::Cos(fRadians);
    Real fSin = Math::Sin(fRadians);
    Real fOneMinusCos = 1.0f - fCos;
    Real fX2 = rkAxis.x * rkAxis.x;
    Real fY2 = rkAxis.y * rkAxis.y;
    Real fZ2 = rkAxis.z * rkAxis.z;
    Real fXYM = rkAxis.x * rkAxis.y * fOneMinusCos;
    Real fXZM = rkAxis.x * rkAxis.z * fOneMinusCos;
    Real fYZM = rkAxis.y * rkAxis.z * fOneMinusCos;
    Real fXSin = rkAxis.x * fSin;
    Real fYSin = rkAxis.y * fSin;
    Real fZSin = rkAxis.z * fSin;

    m[0][0] = fX2 * fOneMinusCos + fCos;
    m[0][1] = fXYM - fZSin;
    m[0][2] = fXZM + fYSin;
    m[1][0] = fXYM + fZSin;
    m[1][1] = fY2 * fOneMinusCos + fCos;
    m[1][2] = fYZM - fXSin;
    m[2][0] = fXZM - fYSin;
    m[2][1] = fYZM + fXSin;
    m[2][2] = fZ2 * fOneMinusCos + fCos;
}


public bool ToEulerAnglesXYZ(Radian& rfYAngle, Radian& rfPAngle,
    Radian& rfRAngle)
{
    rfPAngle = Radian(Math::ASin(m[0][2]));
    if (rfPAngle < Radian(Math::HALF_PI))
    {
        if (rfPAngle > Radian(-Math::HALF_PI))
        {
            rfYAngle = Math::ATan2(-m[1][2], m[2][2]);
            rfRAngle = Math::ATan2(-m[0][1], m[0][0]);
            return true;
        }
        else
        {
            // WARNING.  Not a unique solution.
            Radian fRmY = Math::ATan2(m[1][0], m[1][1]);
            rfRAngle = Radian(0.0);  // any angle works
            rfYAngle = rfRAngle - fRmY;
            return false;
        }
    }
    else
    {
        // WARNING.  Not a unique solution.
        Radian fRpY = Math::ATan2(m[1][0], m[1][1]);
        rfRAngle = Radian(0.0);  // any angle works
        rfYAngle = fRpY - rfRAngle;
        return false;
    }
}
public bool ToEulerAnglesXZY(Radian& rfYAngle, Radian& rfPAngle,
    Radian& rfRAngle)
{
    rfPAngle = Math::ASin(-m[0][1]);
    if (rfPAngle < Radian(Math::HALF_PI))
    {
        if (rfPAngle > Radian(-Math::HALF_PI))
        {
            rfYAngle = Math::ATan2(m[2][1], m[1][1]);
            rfRAngle = Math::ATan2(m[0][2], m[0][0]);
            return true;
        }
        else
        {
            // WARNING.  Not a unique solution.
            Radian fRmY = Math::ATan2(-m[2][0], m[2][2]);
            rfRAngle = Radian(0.0);  // any angle works
            rfYAngle = rfRAngle - fRmY;
            return false;
        }
    }
    else
    {
        // WARNING.  Not a unique solution.
        Radian fRpY = Math::ATan2(-m[2][0], m[2][2]);
        rfRAngle = Radian(0.0);  // any angle works
        rfYAngle = fRpY - rfRAngle;
        return false;
    }
}
public bool ToEulerAnglesYXZ(Radian& rfYAngle, Radian& rfPAngle,
    Radian& rfRAngle)
{
    rfPAngle = Math::ASin(-m[1][2]);
    if (rfPAngle < Radian(Math::HALF_PI))
    {
        if (rfPAngle > Radian(-Math::HALF_PI))
        {
            rfYAngle = Math::ATan2(m[0][2], m[2][2]);
            rfRAngle = Math::ATan2(m[1][0], m[1][1]);
            return true;
        }
        else
        {
            // WARNING.  Not a unique solution.
            Radian fRmY = Math::ATan2(-m[0][1], m[0][0]);
            rfRAngle = Radian(0.0);  // any angle works
            rfYAngle = rfRAngle - fRmY;
            return false;
        }
    }
    else
    {
        // WARNING.  Not a unique solution.
        Radian fRpY = Math::ATan2(-m[0][1], m[0][0]);
        rfRAngle = Radian(0.0);  // any angle works
        rfYAngle = fRpY - rfRAngle;
        return false;
    }
}
public bool ToEulerAnglesYZX(Radian& rfYAngle, Radian& rfPAngle,
    Radian& rfRAngle)
{
    rfPAngle = Math::ASin(m[1][0]);
    if (rfPAngle < Radian(Math::HALF_PI))
    {
        if (rfPAngle > Radian(-Math::HALF_PI))
        {
            rfYAngle = Math::ATan2(-m[2][0], m[0][0]);
            rfRAngle = Math::ATan2(-m[1][2], m[1][1]);
            return true;
        }
        else
        {
            // WARNING.  Not a unique solution.
            Radian fRmY = Math::ATan2(m[2][1], m[2][2]);
            rfRAngle = Radian(0.0);  // any angle works
            rfYAngle = rfRAngle - fRmY;
            return false;
        }
    }
    else
    {
        // WARNING.  Not a unique solution.
        Radian fRpY = Math::ATan2(m[2][1], m[2][2]);
        rfRAngle = Radian(0.0);  // any angle works
        rfYAngle = fRpY - rfRAngle;
        return false;
    }
}
public bool ToEulerAnglesZXY(Radian& rfYAngle, Radian& rfPAngle,
    Radian& rfRAngle)
{
    rfPAngle = Math::ASin(m[2][1]);
    if (rfPAngle < Radian(Math::HALF_PI))
    {
        if (rfPAngle > Radian(-Math::HALF_PI))
        {
            rfYAngle = Math::ATan2(-m[0][1], m[1][1]);
            rfRAngle = Math::ATan2(-m[2][0], m[2][2]);
            return true;
        }
        else
        {
            // WARNING.  Not a unique solution.
            Radian fRmY = Math::ATan2(m[0][2], m[0][0]);
            rfRAngle = Radian(0.0);  // any angle works
            rfYAngle = rfRAngle - fRmY;
            return false;
        }
    }
    else
    {
        // WARNING.  Not a unique solution.
        Radian fRpY = Math::ATan2(m[0][2], m[0][0]);
        rfRAngle = Radian(0.0);  // any angle works
        rfYAngle = fRpY - rfRAngle;
        return false;
    }
}
public bool ToEulerAnglesZYX(Radian& rfYAngle, Radian& rfPAngle,
    Radian& rfRAngle)
{
    rfPAngle = Math::ASin(-m[2][0]);
    if (rfPAngle < Radian(Math::HALF_PI))
    {
        if (rfPAngle > Radian(-Math::HALF_PI))
        {
            rfYAngle = Math::ATan2(m[1][0], m[0][0]);
            rfRAngle = Math::ATan2(m[2][1], m[2][2]);
            return true;
        }
        else
        {
            // WARNING.  Not a unique solution.
            Radian fRmY = Math::ATan2(-m[0][1], m[0][2]);
            rfRAngle = Radian(0.0);  // any angle works
            rfYAngle = rfRAngle - fRmY;
            return false;
        }
    }
    else
    {
        // WARNING.  Not a unique solution.
        Radian fRpY = Math::ATan2(-m[0][1], m[0][2]);
        rfRAngle = Radian(0.0);  // any angle works
        rfYAngle = fRpY - rfRAngle;
        return false;
    }
}
public void FromEulerAnglesXYZ(const Radian& fYAngle, const Radian& fPAngle, const Radian& fRAngle)
{
    Real fCos, fSin;

    fCos = Math::Cos(fYAngle);
    fSin = Math::Sin(fYAngle);
    Matrix3 kXMat(1.0, 0.0, 0.0, 0.0, fCos, -fSin, 0.0, fSin, fCos);

    fCos = Math::Cos(fPAngle);
    fSin = Math::Sin(fPAngle);
    Matrix3 kYMat(fCos,0.0, fSin, 0.0, 1.0, 0.0, -fSin, 0.0, fCos);

    fCos = Math::Cos(fRAngle);
    fSin = Math::Sin(fRAngle);
    Matrix3 kZMat(fCos,-fSin, 0.0, fSin, fCos, 0.0, 0.0, 0.0, 1.0);

    *this = kXMat * (kYMat * kZMat);
}
public void FromEulerAnglesXZY(const Radian& fYAngle, const Radian& fPAngle, const Radian& fRAngle)
{
    Real fCos, fSin;

    fCos = Math::Cos(fYAngle);
    fSin = Math::Sin(fYAngle);
    Matrix3 kXMat(1.0, 0.0, 0.0, 0.0, fCos, -fSin, 0.0, fSin, fCos);

    fCos = Math::Cos(fPAngle);
    fSin = Math::Sin(fPAngle);
    Matrix3 kZMat(fCos,-fSin, 0.0, fSin, fCos, 0.0, 0.0, 0.0, 1.0);

    fCos = Math::Cos(fRAngle);
    fSin = Math::Sin(fRAngle);
    Matrix3 kYMat(fCos,0.0, fSin, 0.0, 1.0, 0.0, -fSin, 0.0, fCos);

    *this = kXMat * (kZMat * kYMat);
}
public void FromEulerAnglesYXZ(const Radian& fYAngle, const Radian& fPAngle, const Radian& fRAngle)
{
    Real fCos, fSin;

    fCos = Math::Cos(fYAngle);
    fSin = Math::Sin(fYAngle);
    Matrix3 kYMat(fCos,0.0, fSin, 0.0, 1.0, 0.0, -fSin, 0.0, fCos);

    fCos = Math::Cos(fPAngle);
    fSin = Math::Sin(fPAngle);
    Matrix3 kXMat(1.0, 0.0, 0.0, 0.0, fCos, -fSin, 0.0, fSin, fCos);

    fCos = Math::Cos(fRAngle);
    fSin = Math::Sin(fRAngle);
    Matrix3 kZMat(fCos,-fSin, 0.0, fSin, fCos, 0.0, 0.0, 0.0, 1.0);

    *this = kYMat * (kXMat * kZMat);
}
public void FromEulerAnglesYZX(const Radian& fYAngle, const Radian& fPAngle, const Radian& fRAngle)
{
    Real fCos, fSin;

    fCos = Math::Cos(fYAngle);
    fSin = Math::Sin(fYAngle);
    Matrix3 kYMat(fCos,0.0, fSin, 0.0, 1.0, 0.0, -fSin, 0.0, fCos);

    fCos = Math::Cos(fPAngle);
    fSin = Math::Sin(fPAngle);
    Matrix3 kZMat(fCos,-fSin, 0.0, fSin, fCos, 0.0, 0.0, 0.0, 1.0);

    fCos = Math::Cos(fRAngle);
    fSin = Math::Sin(fRAngle);
    Matrix3 kXMat(1.0, 0.0, 0.0, 0.0, fCos, -fSin, 0.0, fSin, fCos);

    *this = kYMat * (kZMat * kXMat);
}
public void FromEulerAnglesZXY(const Radian& fYAngle, const Radian& fPAngle, const Radian& fRAngle)
{
    Real fCos, fSin;

    fCos = Math::Cos(fYAngle);
    fSin = Math::Sin(fYAngle);
    Matrix3 kZMat(fCos,-fSin, 0.0, fSin, fCos, 0.0, 0.0, 0.0, 1.0);

    fCos = Math::Cos(fPAngle);
    fSin = Math::Sin(fPAngle);
    Matrix3 kXMat(1.0, 0.0, 0.0, 0.0, fCos, -fSin, 0.0, fSin, fCos);

    fCos = Math::Cos(fRAngle);
    fSin = Math::Sin(fRAngle);
    Matrix3 kYMat(fCos,0.0, fSin, 0.0, 1.0, 0.0, -fSin, 0.0, fCos);

    *this = kZMat * (kXMat * kYMat);
}
public void FromEulerAnglesZYX(const Radian& fYAngle, const Radian& fPAngle, const Radian& fRAngle)
{
    Real fCos, fSin;

    fCos = Math::Cos(fYAngle);
    fSin = Math::Sin(fYAngle);
    Matrix3 kZMat(fCos,-fSin, 0.0, fSin, fCos, 0.0, 0.0, 0.0, 1.0);

    fCos = Math::Cos(fPAngle);
    fSin = Math::Sin(fPAngle);
    Matrix3 kYMat(fCos,0.0, fSin, 0.0, 1.0, 0.0, -fSin, 0.0, fCos);

    fCos = Math::Cos(fRAngle);
    fSin = Math::Sin(fRAngle);
    Matrix3 kXMat(1.0, 0.0, 0.0, 0.0, fCos, -fSin, 0.0, fSin, fCos);

    *this = kZMat * (kYMat * kXMat);
}

public void EigenSolveSymmetric(Real afEigenvalue[3],
    Vector3 akEigenvector[3])
{
    Matrix3 kMatrix = *this;
    Real afSubDiag[3];
    kMatrix.Tridiagonal(afEigenvalue, afSubDiag);
    kMatrix.QLAlgorithm(afEigenvalue, afSubDiag);

    for (size_t i = 0; i < 3; i++)
    {
        akEigenvector[i][0] = kMatrix[0][i];
        akEigenvector[i][1] = kMatrix[1][i];
        akEigenvector[i][2] = kMatrix[2][i];
    }

    // make eigenvectors form a right--handed system
    Vector3 kCross = akEigenvector[1].crossProduct(akEigenvector[2]);
    Real fDet = akEigenvector[0].dotProduct(kCross);
    if (fDet < 0.0)
    {
        akEigenvector[2][0] = -akEigenvector[2][0];
        akEigenvector[2][1] = -akEigenvector[2][1];
        akEigenvector[2][2] = -akEigenvector[2][2];
    }
}

static public void TensorProduct(const Vector3& rkU, const Vector3& rkV,
    Matrix3& rkProduct)
{
    for (size_t iRow = 0; iRow < 3; iRow++)
    {
        for (size_t iCol = 0; iCol < 3; iCol++)
            rkProduct[iRow][iCol] = rkU[iRow] * rkV[iCol];
    }
}

/** Determines if this matrix involves a scaling. */
public bool hasScale() const
        {
            // check magnitude of column vectors (==local axes)
            Real t = m[0][0] * m[0][0] + m[1][0] * m[1][0] + m[2][0] * m[2][0];
            if (!Math::RealEqual(t, 1.0, (Real)1e-04))
                return true;
            t = m[0][1] * m[0][1] + m[1][1] * m[1][1] + m[2][1] * m[2][1];
            if (!Math::RealEqual(t, 1.0, (Real)1e-04))
                return true;
            t = m[0][2] * m[0][2] + m[1][2] * m[1][2] + m[2][2] * m[2][2];
            if (!Math::RealEqual(t, 1.0, (Real)1e-04))
                return true;

            return false;
        }

        /** Function for writing to a stream.
        */
        inline _OgreExport friend std::ostream& operator <<
            ( std::ostream& o, const Matrix3& mat )
{
    o << "Matrix3(" << mat[0][0] << ", " << mat[0][1] << ", " << mat[0][2] << ", "
                    << mat[1][0] << ", " << mat[1][1] << ", " << mat[1][2] << ", "
                    << mat[2][0] << ", " << mat[2][1] << ", " << mat[2][2] << ")";
    return o;
}



protected:
        // support for eigensolver
        void Tridiagonal(Real afDiag[3], Real afSubDiag[3])
{
    Real fA = m[0][0];
    Real fB = m[0][1];
    Real fC = m[0][2];
    Real fD = m[1][1];
    Real fE = m[1][2];
    Real fF = m[2][2];

    afDiag[0] = fA;
    afSubDiag[2] = 0.0;
    if (Math::Abs(fC) >= EPSILON)
    {
        Real fLength = Math::Sqrt(fB * fB + fC * fC);
        Real fInvLength = 1.0f / fLength;
        fB *= fInvLength;
        fC *= fInvLength;
        Real fQ = 2.0f * fB * fE + fC * (fF - fD);
        afDiag[1] = fD + fC * fQ;
        afDiag[2] = fF - fC * fQ;
        afSubDiag[0] = fLength;
        afSubDiag[1] = fE - fB * fQ;
        m[0][0] = 1.0;
        m[0][1] = 0.0;
        m[0][2] = 0.0;
        m[1][0] = 0.0;
        m[1][1] = fB;
        m[1][2] = fC;
        m[2][0] = 0.0;
        m[2][1] = fC;
        m[2][2] = -fB;
    }
    else
    {
        afDiag[1] = fD;
        afDiag[2] = fF;
        afSubDiag[0] = fB;
        afSubDiag[1] = fE;
        m[0][0] = 1.0;
        m[0][1] = 0.0;
        m[0][2] = 0.0;
        m[1][0] = 0.0;
        m[1][1] = 1.0;
        m[1][2] = 0.0;
        m[2][0] = 0.0;
        m[2][1] = 0.0;
        m[2][2] = 1.0;
    }
}
bool QLAlgorithm(Real afDiag[3], Real afSubDiag[3])
{
    for (int i0 = 0; i0 < 3; i0++)
    {
        const unsigned int iMaxIter = 32;
        unsigned int iIter;
        for (iIter = 0; iIter < iMaxIter; iIter++)
        {
            int i1;
            for (i1 = i0; i1 <= 1; i1++)
            {
                Real fSum = Math::Abs(afDiag[i1]) +
                    Math::Abs(afDiag[i1 + 1]);
                if (Math::Abs(afSubDiag[i1]) + fSum == fSum)
                    break;
            }
            if (i1 == i0)
                break;

            Real fTmp0 = (afDiag[i0 + 1] - afDiag[i0]) / (2.0f * afSubDiag[i0]);
            Real fTmp1 = Math::Sqrt(fTmp0 * fTmp0 + 1.0f);
            if (fTmp0 < 0.0)
                fTmp0 = afDiag[i1] - afDiag[i0] + afSubDiag[i0] / (fTmp0 - fTmp1);
            else
                fTmp0 = afDiag[i1] - afDiag[i0] + afSubDiag[i0] / (fTmp0 + fTmp1);
            Real fSin = 1.0;
            Real fCos = 1.0;
            Real fTmp2 = 0.0;
            for (int i2 = i1 - 1; i2 >= i0; i2--)
            {
                Real fTmp3 = fSin * afSubDiag[i2];
                Real fTmp4 = fCos * afSubDiag[i2];
                if (Math::Abs(fTmp3) >= Math::Abs(fTmp0))
                {
                    fCos = fTmp0 / fTmp3;
                    fTmp1 = Math::Sqrt(fCos * fCos + 1.0f);
                    afSubDiag[i2 + 1] = fTmp3 * fTmp1;
                    fSin = 1.0f / fTmp1;
                    fCos *= fSin;
                }
                else
                {
                    fSin = fTmp3 / fTmp0;
                    fTmp1 = Math::Sqrt(fSin * fSin + 1.0f);
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
                    fTmp3 = m[iRow][i2 + 1];
                    m[iRow][i2 + 1] = fSin * m[iRow][i2] +
                        fCos * fTmp3;
                    m[iRow][i2] = fCos * m[iRow][i2] -
                        fSin * fTmp3;
                }
            }
            afDiag[i0] -= fTmp2;
            afSubDiag[i0] = fTmp0;
            afSubDiag[i1] = 0.0;
        }

        if (iIter == iMaxIter)
        {
            // should not get here under normal circumstances
            return false;
        }
    }

    return true;
}

static public void Bidiagonalize(Matrix3& kA, Matrix3& kL,
    Matrix3& kR)
{
    Real afV[3], afW[3];
    Real fLength, fSign, fT1, fInvT1, fT2;
    bool bIdentity;

    // map first column to (*,0,0)
    fLength = Math::Sqrt(kA[0][0] * kA[0][0] + kA[1][0] * kA[1][0] +
        kA[2][0] * kA[2][0]);
    if (fLength > 0.0)
    {
        fSign = (kA[0][0] > 0.0f ? 1.0f : -1.0f);
        fT1 = kA[0][0] + fSign * fLength;
        fInvT1 = 1.0f / fT1;
        afV[1] = kA[1][0] * fInvT1;
        afV[2] = kA[2][0] * fInvT1;

        fT2 = -2.0f / (1.0f + afV[1] * afV[1] + afV[2] * afV[2]);
        afW[0] = fT2 * (kA[0][0] + kA[1][0] * afV[1] + kA[2][0] * afV[2]);
        afW[1] = fT2 * (kA[0][1] + kA[1][1] * afV[1] + kA[2][1] * afV[2]);
        afW[2] = fT2 * (kA[0][2] + kA[1][2] * afV[1] + kA[2][2] * afV[2]);
        kA[0][0] += afW[0];
        kA[0][1] += afW[1];
        kA[0][2] += afW[2];
        kA[1][1] += afV[1] * afW[1];
        kA[1][2] += afV[1] * afW[2];
        kA[2][1] += afV[2] * afW[1];
        kA[2][2] += afV[2] * afW[2];

        kL[0][0] = 1.0f + fT2;
        kL[0][1] = kL[1][0] = fT2 * afV[1];
        kL[0][2] = kL[2][0] = fT2 * afV[2];
        kL[1][1] = 1.0f + fT2 * afV[1] * afV[1];
        kL[1][2] = kL[2][1] = fT2 * afV[1] * afV[2];
        kL[2][2] = 1.0f + fT2 * afV[2] * afV[2];
        bIdentity = false;
    }
    else
    {
        kL = Matrix3::IDENTITY;
        bIdentity = true;
    }

    // map first row to (*,*,0)
    fLength = Math::Sqrt(kA[0][1] * kA[0][1] + kA[0][2] * kA[0][2]);
    if (fLength > 0.0)
    {
        fSign = (kA[0][1] > 0.0f ? 1.0f : -1.0f);
        fT1 = kA[0][1] + fSign * fLength;
        afV[2] = kA[0][2] / fT1;

        fT2 = -2.0f / (1.0f + afV[2] * afV[2]);
        afW[0] = fT2 * (kA[0][1] + kA[0][2] * afV[2]);
        afW[1] = fT2 * (kA[1][1] + kA[1][2] * afV[2]);
        afW[2] = fT2 * (kA[2][1] + kA[2][2] * afV[2]);
        kA[0][1] += afW[0];
        kA[1][1] += afW[1];
        kA[1][2] += afW[1] * afV[2];
        kA[2][1] += afW[2];
        kA[2][2] += afW[2] * afV[2];

        kR[0][0] = 1.0;
        kR[0][1] = kR[1][0] = 0.0;
        kR[0][2] = kR[2][0] = 0.0;
        kR[1][1] = 1.0f + fT2;
        kR[1][2] = kR[2][1] = fT2 * afV[2];
        kR[2][2] = 1.0f + fT2 * afV[2] * afV[2];
    }
    else
    {
        kR = Matrix3::IDENTITY;
    }

    // map second column to (*,*,0)
    fLength = Math::Sqrt(kA[1][1] * kA[1][1] + kA[2][1] * kA[2][1]);
    if (fLength > 0.0)
    {
        fSign = (kA[1][1] > 0.0f ? 1.0f : -1.0f);
        fT1 = kA[1][1] + fSign * fLength;
        afV[2] = kA[2][1] / fT1;

        fT2 = -2.0f / (1.0f + afV[2] * afV[2]);
        afW[1] = fT2 * (kA[1][1] + kA[2][1] * afV[2]);
        afW[2] = fT2 * (kA[1][2] + kA[2][2] * afV[2]);
        kA[1][1] += afW[1];
        kA[1][2] += afW[2];
        kA[2][2] += afV[2] * afW[2];

        Real fA = 1.0f + fT2;
        Real fB = fT2 * afV[2];
        Real fC = 1.0f + fB * afV[2];

        if (bIdentity)
        {
            kL[0][0] = 1.0;
            kL[0][1] = kL[1][0] = 0.0;
            kL[0][2] = kL[2][0] = 0.0;
            kL[1][1] = fA;
            kL[1][2] = kL[2][1] = fB;
            kL[2][2] = fC;
        }
        else
        {
            for (int iRow = 0; iRow < 3; iRow++)
            {
                Real fTmp0 = kL[iRow][1];
                Real fTmp1 = kL[iRow][2];
                kL[iRow][1] = fA * fTmp0 + fB * fTmp1;
                kL[iRow][2] = fB * fTmp0 + fC * fTmp1;
            }
        }
    }
}
static public void GolubKahanStep(Matrix3& kA, Matrix3& kL,
    Matrix3& kR)
{
    Real fT11 = kA[0][1] * kA[0][1] + kA[1][1] * kA[1][1];
    Real fT22 = kA[1][2] * kA[1][2] + kA[2][2] * kA[2][2];
    Real fT12 = kA[1][1] * kA[1][2];
    Real fTrace = fT11 + fT22;
    Real fDiff = fT11 - fT22;
    Real fDiscr = Math::Sqrt(fDiff * fDiff + 4.0f * fT12 * fT12);
    Real fRoot1 = 0.5f * (fTrace + fDiscr);
    Real fRoot2 = 0.5f * (fTrace - fDiscr);

    // adjust right
    Real fY = kA[0][0] - (Math::Abs(fRoot1 - fT22) <=
        Math::Abs(fRoot2 - fT22) ? fRoot1 : fRoot2);
    Real fZ = kA[0][1];
    Real fInvLength = Math::InvSqrt(fY * fY + fZ * fZ);
    Real fSin = fZ * fInvLength;
    Real fCos = -fY * fInvLength;

    Real fTmp0 = kA[0][0];
    Real fTmp1 = kA[0][1];
    kA[0][0] = fCos * fTmp0 - fSin * fTmp1;
    kA[0][1] = fSin * fTmp0 + fCos * fTmp1;
    kA[1][0] = -fSin * kA[1][1];
    kA[1][1] *= fCos;

    size_t iRow;
    for (iRow = 0; iRow < 3; iRow++)
    {
        fTmp0 = kR[0][iRow];
        fTmp1 = kR[1][iRow];
        kR[0][iRow] = fCos * fTmp0 - fSin * fTmp1;
        kR[1][iRow] = fSin * fTmp0 + fCos * fTmp1;
    }

    // adjust left
    fY = kA[0][0];
    fZ = kA[1][0];
    fInvLength = Math::InvSqrt(fY * fY + fZ * fZ);
    fSin = fZ * fInvLength;
    fCos = -fY * fInvLength;

    kA[0][0] = fCos * kA[0][0] - fSin * kA[1][0];
    fTmp0 = kA[0][1];
    fTmp1 = kA[1][1];
    kA[0][1] = fCos * fTmp0 - fSin * fTmp1;
    kA[1][1] = fSin * fTmp0 + fCos * fTmp1;
    kA[0][2] = -fSin * kA[1][2];
    kA[1][2] *= fCos;

    size_t iCol;
    for (iCol = 0; iCol < 3; iCol++)
    {
        fTmp0 = kL[iCol][0];
        fTmp1 = kL[iCol][1];
        kL[iCol][0] = fCos * fTmp0 - fSin * fTmp1;
        kL[iCol][1] = fSin * fTmp0 + fCos * fTmp1;
    }

    // adjust right
    fY = kA[0][1];
    fZ = kA[0][2];
    fInvLength = Math::InvSqrt(fY * fY + fZ * fZ);
    fSin = fZ * fInvLength;
    fCos = -fY * fInvLength;

    kA[0][1] = fCos * kA[0][1] - fSin * kA[0][2];
    fTmp0 = kA[1][1];
    fTmp1 = kA[1][2];
    kA[1][1] = fCos * fTmp0 - fSin * fTmp1;
    kA[1][2] = fSin * fTmp0 + fCos * fTmp1;
    kA[2][1] = -fSin * kA[2][2];
    kA[2][2] *= fCos;

    for (iRow = 0; iRow < 3; iRow++)
    {
        fTmp0 = kR[1][iRow];
        fTmp1 = kR[2][iRow];
        kR[1][iRow] = fCos * fTmp0 - fSin * fTmp1;
        kR[2][iRow] = fSin * fTmp0 + fCos * fTmp1;
    }

    // adjust left
    fY = kA[1][1];
    fZ = kA[2][1];
    fInvLength = Math::InvSqrt(fY * fY + fZ * fZ);
    fSin = fZ * fInvLength;
    fCos = -fY * fInvLength;

    kA[1][1] = fCos * kA[1][1] - fSin * kA[2][1];
    fTmp0 = kA[1][2];
    fTmp1 = kA[2][2];
    kA[1][2] = fCos * fTmp0 - fSin * fTmp1;
    kA[2][2] = fSin * fTmp0 + fCos * fTmp1;

    for (iCol = 0; iCol < 3; iCol++)
    {
        fTmp0 = kL[iCol][1];
        fTmp1 = kL[iCol][2];
        kL[iCol][1] = fCos * fTmp0 - fSin * fTmp1;
        kL[iCol][2] = fSin * fTmp0 + fCos * fTmp1;
    }
}

// support for spectral norm
static public Real MaxCubicRoot(Real afCoeff[3])
{
    const Real fOneThird = 1.0 / 3.0;
    const Real fEpsilon = 1e-06;
    Real fDiscr = afCoeff[2] * afCoeff[2] - 3.0f * afCoeff[1];
    if (fDiscr <= fEpsilon)
        return -fOneThird * afCoeff[2];

    // Compute an upper bound on roots of P(x).  This assumes that A^T*A
    // has been scaled by its largest entry.
    Real fX = 1.0;
    Real fPoly = afCoeff[0] + fX * (afCoeff[1] + fX * (afCoeff[2] + fX));
    if (fPoly < 0.0)
    {
        // uses a matrix norm to find an upper bound on maximum root
        fX = Math::Abs(afCoeff[0]);
        Real fTmp = 1.0f + Math::Abs(afCoeff[1]);
        if (fTmp > fX)
            fX = fTmp;
        fTmp = 1.0f + Math::Abs(afCoeff[2]);
        if (fTmp > fX)
            fX = fTmp;
    }

    // Newton's method to find root
    Real fTwoC2 = 2.0f * afCoeff[2];
    for (int i = 0; i < 16; i++)
    {
        fPoly = afCoeff[0] + fX * (afCoeff[1] + fX * (afCoeff[2] + fX));
        if (Math::Abs(fPoly) <= fEpsilon)
            return fX;

        Real fDeriv = afCoeff[1] + fX * (fTwoC2 + 3.0f * fX);
        fX -= fPoly / fDeriv;
    }

    return fX;
}
    }
}
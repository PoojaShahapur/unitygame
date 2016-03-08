namespace SDK.Lib
{
    public struct MMatrix4
    {
        public static MMatrix4 ZERO = new MMatrix4(
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0);

        public static MMatrix4 ZEROAFFINE = new MMatrix4(
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 0,
        0, 0, 0, 1);

        public static MMatrix4 IDENTITY = new MMatrix4(
        1, 0, 0, 0,
        0, 1, 0, 0,
        0, 0, 1, 0,
        0, 0, 0, 1);

        public static MMatrix4 CLIPSPACE2DTOIMAGESPACE = new MMatrix4(
        0.5f, 0, 0, 0.5f,
        0, -0.5f, 0, 0.5f,
        0, 0, 1, 0,
        0, 0, 0, 1);

        public float[,] m;

        public MMatrix4(
            float m00 = 0, float m01 = 0, float m02 = 0, float m03 = 1,
            float m10 = 0, float m11 = 0, float m12 = 0, float m13 = 1,
            float m20 = 0, float m21 = 0, float m22 = 0, float m23 = 1,
            float m30 = 0, float m31 = 0, float m32 = 0, float m33 = 1)
        {
            m = new float[4, 4];

            m[0, 0] = m00;
            m[0, 1] = m01;
            m[0, 2] = m02;
            m[0, 3] = m03;
            m[1, 0] = m10;
            m[1, 1] = m11;
            m[1, 2] = m12;
            m[1, 3] = m13;
            m[2, 0] = m20;
            m[2, 1] = m21;
            m[2, 2] = m22;
            m[2, 3] = m23;
            m[3, 0] = m30;
            m[3, 1] = m31;
            m[3, 2] = m32;
            m[3, 3] = m33;
        }

        public MMatrix4(MMatrix3 m3x3)
        {
            this = IDENTITY;
            this.assignForm(m3x3);
        }

        public MMatrix4(MQuaternion rot)
        {
            MMatrix3 m3x3 = new MMatrix3();
            rot.ToRotationMatrix(m3x3);
            this = IDENTITY;
            this.assignForm(m3x3);
        }

        public void swap(MMatrix4 other)
        {
            UtilApi.swap(ref m[0, 0], ref other.m[0, 0]);
            UtilApi.swap(ref m[0, 1], ref other.m[0, 1]);
            UtilApi.swap(ref m[0, 2], ref other.m[0, 2]);
            UtilApi.swap(ref m[0, 3], ref other.m[0, 3]);
            UtilApi.swap(ref m[1, 0], ref other.m[1, 0]);
            UtilApi.swap(ref m[1, 1], ref other.m[1, 1]);
            UtilApi.swap(ref m[1, 2], ref other.m[1, 2]);
            UtilApi.swap(ref m[1, 3], ref other.m[1, 3]);
            UtilApi.swap(ref m[2, 0], ref other.m[2, 0]);
            UtilApi.swap(ref m[2, 1], ref other.m[2, 1]);
            UtilApi.swap(ref m[2, 2], ref other.m[2, 2]);
            UtilApi.swap(ref m[2, 3], ref other.m[2, 3]);
            UtilApi.swap(ref m[3, 0], ref other.m[3, 0]);
            UtilApi.swap(ref m[3, 1], ref other.m[3, 1]);
            UtilApi.swap(ref m[3, 2], ref other.m[3, 2]);
            UtilApi.swap(ref m[3, 3], ref other.m[3, 3]);
        }

        public float this[int row, int column]
        {
            get
            {
                UtilApi.assert(row < 4 && column < 4);
                return m[row, column];
            }
        }

        public MMatrix4 concatenate(MMatrix4 m2)
        {
            MMatrix4 r = new MMatrix4();
            r.m[0, 0] = m[0, 0] * m2.m[0, 0] + m[0, 1] * m2.m[1, 0] + m[0, 2] * m2.m[2, 0] + m[0, 3] * m2.m[3, 0];
            r.m[0, 1] = m[0, 0] * m2.m[0, 1] + m[0, 1] * m2.m[1, 1] + m[0, 2] * m2.m[2, 1] + m[0, 3] * m2.m[3, 1];
            r.m[0, 2] = m[0, 0] * m2.m[0, 2] + m[0, 1] * m2.m[1, 2] + m[0, 2] * m2.m[2, 2] + m[0, 3] * m2.m[3, 2];
            r.m[0, 3] = m[0, 0] * m2.m[0, 3] + m[0, 1] * m2.m[1, 3] + m[0, 2] * m2.m[2, 3] + m[0, 3] * m2.m[3, 3];

            r.m[1, 0] = m[1, 0] * m2.m[0, 0] + m[1, 1] * m2.m[1, 0] + m[1, 2] * m2.m[2, 0] + m[1, 3] * m2.m[3, 0];
            r.m[1, 1] = m[1, 0] * m2.m[0, 1] + m[1, 1] * m2.m[1, 1] + m[1, 2] * m2.m[2, 1] + m[1, 3] * m2.m[3, 1];
            r.m[1, 2] = m[1, 0] * m2.m[0, 2] + m[1, 1] * m2.m[1, 2] + m[1, 2] * m2.m[2, 2] + m[1, 3] * m2.m[3, 2];
            r.m[1, 3] = m[1, 0] * m2.m[0, 3] + m[1, 1] * m2.m[1, 3] + m[1, 2] * m2.m[2, 3] + m[1, 3] * m2.m[3, 3];

            r.m[2, 0] = m[2, 0] * m2.m[0, 0] + m[2, 1] * m2.m[1, 0] + m[2, 2] * m2.m[2, 0] + m[2, 3] * m2.m[3, 0];
            r.m[2, 1] = m[2, 0] * m2.m[0, 1] + m[2, 1] * m2.m[1, 1] + m[2, 2] * m2.m[2, 1] + m[2, 3] * m2.m[3, 1];
            r.m[2, 2] = m[2, 0] * m2.m[0, 2] + m[2, 1] * m2.m[1, 2] + m[2, 2] * m2.m[2, 2] + m[2, 3] * m2.m[3, 2];
            r.m[2, 3] = m[2, 0] * m2.m[0, 3] + m[2, 1] * m2.m[1, 3] + m[2, 2] * m2.m[2, 3] + m[2, 3] * m2.m[3, 3];

            r.m[3, 0] = m[3, 0] * m2.m[0, 0] + m[3, 1] * m2.m[1, 0] + m[3, 2] * m2.m[2, 0] + m[3, 3] * m2.m[3, 0];
            r.m[3, 1] = m[3, 0] * m2.m[0, 1] + m[3, 1] * m2.m[1, 1] + m[3, 2] * m2.m[2, 1] + m[3, 3] * m2.m[3, 1];
            r.m[3, 2] = m[3, 0] * m2.m[0, 2] + m[3, 1] * m2.m[1, 2] + m[3, 2] * m2.m[2, 2] + m[3, 3] * m2.m[3, 2];
            r.m[3, 3] = m[3, 0] * m2.m[0, 3] + m[3, 1] * m2.m[1, 3] + m[3, 2] * m2.m[2, 3] + m[3, 3] * m2.m[3, 3];

            return r;
        }

        static public MMatrix4 operator *(MMatrix4 lhs, MMatrix4 m2)
        {
            return lhs.concatenate(m2);
        }

        static public MVector3 operator *(MMatrix4 lhs, MVector3 v)
        {
            MVector3 r = new MVector3();

            float fInvW = 1.0f / (lhs[3, 0] * v.x + lhs[3, 1] * v.y + lhs[3, 2] * v.z + lhs[3, 3]);

            r.x = (lhs[0, 0] * v.x + lhs[0, 1] * v.y + lhs[0, 2] * v.z + lhs[0, 3]) * fInvW;
            r.y = (lhs[1, 0] * v.x + lhs[1, 1] * v.y + lhs[1, 2] * v.z + lhs[1, 3]) * fInvW;
            r.z = (lhs[2, 0] * v.x + lhs[2, 1] * v.y + lhs[2, 2] * v.z + lhs[2, 3]) * fInvW;

            return r;
        }

        static public MVector4 operator *(MMatrix4 lhs, MVector4 v)
        {
            return new MVector4(
                lhs[0, 0] * v.x + lhs[0, 1] * v.y + lhs[0, 2] * v.z + lhs[0, 3] * v.w,
                lhs[1, 0] * v.x + lhs[1, 1] * v.y + lhs[1, 2] * v.z + lhs[1, 3] * v.w,
                lhs[2, 0] * v.x + lhs[2, 1] * v.y + lhs[2, 2] * v.z + lhs[2, 3] * v.w,
                lhs[3, 0] * v.x + lhs[3, 1] * v.y + lhs[3, 2] * v.z + lhs[3, 3] * v.w
                );
        }

        public MPlane operator *(MPlane p)
        {
            MPlane ret;
            MMatrix4 invTrans = inverse().transpose();
            MVector4 v4(p.normal.x, p.normal.y, p.normal.z, p.d );
            v4 = invTrans * v4;
            ret.normal.x = v4.x;
            ret.normal.y = v4.y;
            ret.normal.z = v4.z;
            ret.d = v4.w / ret.normal.normalise();

            return ret;
        }

        static public MMatrix4 operator +(MMatrix4 lhs, MMatrix4 m2)
        {
            MMatrix4 r = new MMatrix4();

            r.m[0, 0] = lhs[0, 0] + m2.m[0, 0];
            r.m[0, 1] = lhs[0, 1] + m2.m[0, 1];
            r.m[0, 2] = lhs[0, 2] + m2.m[0, 2];
            r.m[0, 3] = lhs[0, 3] + m2.m[0, 3];

            r.m[1, 0] = lhs[1, 0] + m2.m[1, 0];
            r.m[1, 1] = lhs[1, 1] + m2.m[1, 1];
            r.m[1, 2] = lhs[1, 2] + m2.m[1, 2];
            r.m[1, 3] = lhs[1, 3] + m2.m[1, 3];

            r.m[2, 0] = lhs[2, 0] + m2.m[2, 0];
            r.m[2, 1] = lhs[2, 1] + m2.m[2, 1];
            r.m[2, 2] = lhs[2, 2] + m2.m[2, 2];
            r.m[2, 3] = lhs[2, 3] + m2.m[2, 3];

            r.m[3, 0] = lhs[3, 0] + m2.m[3, 0];
            r.m[3, 1] = lhs[3, 1] + m2.m[3, 1];
            r.m[3, 2] = lhs[3, 2] + m2.m[3, 2];
            r.m[3, 3] = lhs[3, 3] + m2.m[3, 3];

            return r;
        }

        static public MMatrix4 operator -(MMatrix4 lhs, MMatrix4 m2)
        {
            MMatrix4 r = new MMatrix4();
            r.m[0, 0] = lhs.m[0, 0] - m2.m[0, 0];
            r.m[0, 1] = lhs.m[0, 1] - m2.m[0, 1];
            r.m[0, 2] = lhs.m[0, 2] - m2.m[0, 2];
            r.m[0, 3] = lhs.m[0, 3] - m2.m[0, 3];

            r.m[1, 0] = lhs.m[1, 0] - m2.m[1, 0];
            r.m[1, 1] = lhs.m[1, 1] - m2.m[1, 1];
            r.m[1, 2] = lhs.m[1, 2] - m2.m[1, 2];
            r.m[1, 3] = lhs.m[1, 3] - m2.m[1, 3];

            r.m[2, 0] = lhs.m[2, 0] - m2.m[2, 0];
            r.m[2, 1] = lhs.m[2, 1] - m2.m[2, 1];
            r.m[2, 2] = lhs.m[2, 2] - m2.m[2, 2];
            r.m[2, 3] = lhs.m[2, 3] - m2.m[2, 3];

            r.m[3, 0] = lhs.m[3, 0] - m2.m[3, 0];
            r.m[3, 1] = lhs.m[3, 1] - m2.m[3, 1];
            r.m[3, 2] = lhs.m[3, 2] - m2.m[3, 2];
            r.m[3, 3] = lhs.m[3, 3] - m2.m[3, 3];

            return r;
        }

        static public bool operator ==(MMatrix4 lhs, MMatrix4 m2)
        {
            if (
                lhs.m[0, 0] != m2.m[0, 0] || lhs.m[0, 1] != m2.m[0, 1] || lhs.m[0, 2] != m2.m[0, 2] || lhs.m[0, 3] != m2.m[0, 3] ||
                lhs.m[1, 0] != m2.m[1, 0] || lhs.m[1, 1] != m2.m[1, 1] || lhs.m[1, 2] != m2.m[1, 2] || lhs.m[1, 3] != m2.m[1, 3] ||
                lhs.m[2, 0] != m2.m[2, 0] || lhs.m[2, 1] != m2.m[2, 1] || lhs.m[2, 2] != m2.m[2, 2] || lhs.m[2, 3] != m2.m[2, 3] ||
                lhs.m[3, 0] != m2.m[3, 0] || lhs.m[3, 1] != m2.m[3, 1] || lhs.m[3, 2] != m2.m[3, 2] || lhs.m[3, 3] != m2.m[3, 3])
                return false;
            return true;
        }

        static public bool operator !=(MMatrix4 lhs, MMatrix4 m2)
        {
            if (
                lhs.m[0, 0] != m2.m[0, 0] || lhs.m[0, 1] != m2.m[0, 1] || lhs.m[0, 2] != m2.m[0, 2] || lhs.m[0, 3] != m2.m[0, 3] ||
                lhs.m[1, 0] != m2.m[1, 0] || lhs.m[1, 1] != m2.m[1, 1] || lhs.m[1, 2] != m2.m[1, 2] || lhs.m[1, 3] != m2.m[1, 3] ||
                lhs.m[2, 0] != m2.m[2, 0] || lhs.m[2, 1] != m2.m[2, 1] || lhs.m[2, 2] != m2.m[2, 2] || lhs.m[2, 3] != m2.m[2, 3] ||
                lhs.m[3, 0] != m2.m[3, 0] || lhs.m[3, 1] != m2.m[3, 1] || lhs.m[3, 2] != m2.m[3, 2] || lhs.m[3, 3] != m2.m[3, 3])
                return true;
            return false;
        }

        public void assignForm(MMatrix3 mat3)
        {
            m[0, 0] = mat3.m[0, 0]; m[0, 1] = mat3.m[0, 1]; m[0, 2] = mat3.m[0, 2];
            m[1, 0] = mat3.m[1, 0]; m[1, 1] = mat3.m[1, 1]; m[1, 2] = mat3.m[1, 2];
            m[2, 0] = mat3.m[2, 0]; m[2, 1] = mat3.m[2, 1]; m[2, 2] = mat3.m[2, 2];
        }

        public MMatrix4 transpose()
        {
            return new MMatrix4(m[0, 0], m[1, 0], m[2, 0], m[3, 0],
                           m[0, 1], m[1, 1], m[2, 1], m[3, 1],
                           m[0, 2], m[1, 2], m[2, 2], m[3, 2],
                           m[0, 3], m[1, 3], m[2, 3], m[3, 3]);
        }

        public void setTrans(MVector3 v)
        {
            m[0, 3] = v.x;
            m[1, 3] = v.y;
            m[2, 3] = v.z;
        }

        public MVector3 getTrans()
        {
            return new MVector3(m[0, 3], m[1, 3], m[2, 3]);
        }

        public void makeTrans(MVector3 v)
        {
            m[0, 0] = 1.0f; m[0, 1] = 0.0f; m[0, 2] = 0.0f; m[0, 3] = v.x;
            m[1, 0] = 0.0f; m[1, 1] = 1.0f; m[1, 2] = 0.0f; m[1, 3] = v.y;
            m[2, 0] = 0.0f; m[2, 1] = 0.0f; m[2, 2] = 1.0f; m[2, 3] = v.z;
            m[3, 0] = 0.0f; m[3, 1] = 0.0f; m[3, 2] = 0.0f; m[3, 3] = 1.0f;
        }

        public void makeTrans(float tx, float ty, float tz)
        {
            m[0, 0] = 1.0f; m[0, 1] = 0.0f; m[0, 2] = 0.0f; m[0, 3] = tx;
            m[1, 0] = 0.0f; m[1, 1] = 1.0f; m[1, 2] = 0.0f; m[1, 3] = ty;
            m[2, 0] = 0.0f; m[2, 1] = 0.0f; m[2, 2] = 1.0f; m[2, 3] = tz;
            m[3, 0] = 0.0f; m[3, 1] = 0.0f; m[3, 2] = 0.0f; m[3, 3] = 1.0f;
        }

        public static MMatrix4 getTrans(MVector3 v)
        {
            MMatrix4 r = new MMatrix4();

            r.m[0, 0] = 1.0f; r.m[0, 1] = 0.0f; r.m[0, 2] = 0.0f; r.m[0, 3] = v.x;
            r.m[1, 0] = 0.0f; r.m[1, 1] = 1.0f; r.m[1, 2] = 0.0f; r.m[1, 3] = v.y;
            r.m[2, 0] = 0.0f; r.m[2, 1] = 0.0f; r.m[2, 2] = 1.0f; r.m[2, 3] = v.z;
            r.m[3, 0] = 0.0f; r.m[3, 1] = 0.0f; r.m[3, 2] = 0.0f; r.m[3, 3] = 1.0f;

            return r;
        }

        public static MMatrix4 getTrans(float t_x, float t_y, float t_z)
        {
            MMatrix4 r = new MMatrix4();

            r.m[0, 0] = 1.0f; r.m[0, 1] = 0.0f; r.m[0, 2] = 0.0f; r.m[0, 3] = t_x;
            r.m[1, 0] = 0.0f; r.m[1, 1] = 1.0f; r.m[1, 2] = 0.0f; r.m[1, 3] = t_y;
            r.m[2, 0] = 0.0f; r.m[2, 1] = 0.0f; r.m[2, 2] = 1.0f; r.m[2, 3] = t_z;
            r.m[3, 0] = 0.0f; r.m[3, 1] = 0.0f; r.m[3, 2] = 0.0f; r.m[3, 3] = 1.0f;

            return r;
        }

        public void setScale(MVector3 v)
        {
            m[0, 0] = v.x;
            m[1, 1] = v.y;
            m[2, 2] = v.z;
        }

        public static MMatrix4 getScale(MVector3 v)
        {
            MMatrix4 r = new MMatrix4();
            r.m[0, 0] = v.x; r.m[0, 1] = 0.0f; r.m[0, 2] = 0.0f; r.m[0, 3] = 0.0f;
            r.m[1, 0] = 0.0f; r.m[1, 1] = v.y; r.m[1, 2] = 0.0f; r.m[1, 3] = 0.0f;
            r.m[2, 0] = 0.0f; r.m[2, 1] = 0.0f; r.m[2, 2] = v.z; r.m[2, 3] = 0.0f;
            r.m[3, 0] = 0.0f; r.m[3, 1] = 0.0f; r.m[3, 2] = 0.0f; r.m[3, 3] = 1.0f;

            return r;
        }

        public static MMatrix4 getScale(float s_x, float s_y, float s_z)
        {
            MMatrix4 r = new MMatrix4();
            r.m[0, 0] = s_x; r.m[0, 1] = 0.0f; r.m[0, 2] = 0.0f; r.m[0, 3] = 0.0f;
            r.m[1, 0] = 0.0f; r.m[1, 1] = s_y; r.m[1, 2] = 0.0f; r.m[1, 3] = 0.0f;
            r.m[2, 0] = 0.0f; r.m[2, 1] = 0.0f; r.m[2, 2] = s_z; r.m[2, 3] = 0.0f;
            r.m[3, 0] = 0.0f; r.m[3, 1] = 0.0f; r.m[3, 2] = 0.0f; r.m[3, 3] = 1.0f;

            return r;
        }

        public void extract3x3Matrix(MMatrix3 m3x3)
        {
            m3x3.m[0, 0] = m[0, 0];
            m3x3.m[0, 1] = m[0, 1];
            m3x3.m[0, 2] = m[0, 2];
            m3x3.m[1, 0] = m[1, 0];
            m3x3.m[1, 1] = m[1, 1];
            m3x3.m[1, 2] = m[1, 2];
            m3x3.m[2, 0] = m[2, 0];
            m3x3.m[2, 1] = m[2, 1];
            m3x3.m[2, 2] = m[2, 2];

        }

        public bool hasScale()
        {
            float t = m[0, 0] * m[0, 0] + m[1, 0] * m[1, 0] + m[2, 0] * m[2, 0];
            if (!UtilApi.RealEqual(t, 1.0f, (float)1e-04))
                return true;
            t = m[0, 1] * m[0, 1] + m[1, 1] * m[1, 1] + m[2, 1] * m[2, 1];
            if (!UtilApi.RealEqual(t, 1.0f, (float)1e-04))
                return true;
            t = m[0, 2] * m[0, 2] + m[1, 2] * m[1, 2] + m[2, 2] * m[2, 2];
            if (!UtilApi.RealEqual(t, 1.0f, (float)1e-04))
                return true;

            return false;
        }

        public bool hasNegativeScale()
        {
            return determinant() < 0;
        }

        public MQuaternion extractQuaternion()
        {
            MMatrix3 m3x3 = new MMatrix3();
            extract3x3Matrix(m3x3);
            return new MQuaternion(m3x3);
        }

        static public MMatrix4 operator *(MMatrix4 lhs, float scalar)
        {
            return new MMatrix4(
                scalar * lhs.m[0, 0], scalar * lhs.m[0, 1], scalar * lhs.m[0, 2], scalar * lhs.m[0, 3],
                scalar * lhs.m[1, 0], scalar * lhs.m[1, 1], scalar * lhs.m[1, 2], scalar * lhs.m[1, 3],
                scalar * lhs.m[2, 0], scalar * lhs.m[2, 1], scalar * lhs.m[2, 2], scalar * lhs.m[2, 3],
                scalar * lhs.m[3, 0], scalar * lhs.m[3, 1], scalar * lhs.m[3, 2], scalar * lhs.m[3, 3]);
        }

        public string ToString(MMatrix4 mat)
        {
            string o = "Matrix4(";
            for (int i = 0; i < 4; ++i)
            {
                o += (" row" + i + "{");
                for (int j = 0; j < 4; ++j)
                {
                    o += (mat[i, j] + " ");
                }
                o += "}";
            }
            o += ")";
            return o;
        }

        static public float MINOR(MMatrix4 m, int r0, int r1, int r2,
                                        int c0, int c1, int c2)
        {
            return m[r0, c0] * (m[r1, c1] * m[r2, c2] - m[r2, c1] * m[r1, c2]) -
                m[r0, c1] * (m[r1, c0] * m[r2, c2] - m[r2, c0] * m[r1, c2]) +
                m[r0, c2] * (m[r1, c0] * m[r2, c1] - m[r2, c0] * m[r1, c1]);
        }

        public MMatrix4 adjoint()
        {
            return new MMatrix4(MINOR(this, 1, 2, 3, 1, 2, 3),
                    -MINOR(this, 0, 2, 3, 1, 2, 3),
                    MINOR(this, 0, 1, 3, 1, 2, 3),
                    -MINOR(this, 0, 1, 2, 1, 2, 3),

                    -MINOR(this, 1, 2, 3, 0, 2, 3),
                    MINOR(this, 0, 2, 3, 0, 2, 3),
                    -MINOR(this, 0, 1, 3, 0, 2, 3),
                    MINOR(this, 0, 1, 2, 0, 2, 3),

                    MINOR(this, 1, 2, 3, 0, 1, 3),
                    -MINOR(this, 0, 2, 3, 0, 1, 3),
                    MINOR(this, 0, 1, 3, 0, 1, 3),
                    -MINOR(this, 0, 1, 2, 0, 1, 3),

                    -MINOR(this, 1, 2, 3, 0, 1, 2),
                    MINOR(this, 0, 2, 3, 0, 1, 2),
                    -MINOR(this, 0, 1, 3, 0, 1, 2),
                    MINOR(this, 0, 1, 2, 0, 1, 2));
        }

        public float determinant()
        {
            return m[0, 0] * MINOR(this, 1, 2, 3, 1, 2, 3) -
                    m[0, 1] * MINOR(this, 1, 2, 3, 0, 2, 3) +
                    m[0, 2] * MINOR(this, 1, 2, 3, 0, 1, 3) -
                    m[0, 3] * MINOR(this, 1, 2, 3, 0, 1, 2);
        }

        public MMatrix4 inverse()
        {
            float m00 = m[0, 0], m01 = m[0, 1], m02 = m[0, 2], m03 = m[0, 3];
            float m10 = m[1, 0], m11 = m[1, 1], m12 = m[1, 2], m13 = m[1, 3];
            float m20 = m[2, 0], m21 = m[2, 1], m22 = m[2, 2], m23 = m[2, 3];
            float m30 = m[3, 0], m31 = m[3, 1], m32 = m[3, 2], m33 = m[3, 3];

            float v0 = m20 * m31 - m21 * m30;
            float v1 = m20 * m32 - m22 * m30;
            float v2 = m20 * m33 - m23 * m30;
            float v3 = m21 * m32 - m22 * m31;
            float v4 = m21 * m33 - m23 * m31;
            float v5 = m22 * m33 - m23 * m32;

            float t00 = +(v5 * m11 - v4 * m12 + v3 * m13);
            float t10 = -(v5 * m10 - v2 * m12 + v1 * m13);
            float t20 = +(v4 * m10 - v2 * m11 + v0 * m13);
            float t30 = -(v3 * m10 - v1 * m11 + v0 * m12);

            float invDet = 1 / (t00 * m00 + t10 * m01 + t20 * m02 + t30 * m03);

            float d00 = t00 * invDet;
            float d10 = t10 * invDet;
            float d20 = t20 * invDet;
            float d30 = t30 * invDet;

            float d01 = -(v5 * m01 - v4 * m02 + v3 * m03) * invDet;
            float d11 = +(v5 * m00 - v2 * m02 + v1 * m03) * invDet;
            float d21 = -(v4 * m00 - v2 * m01 + v0 * m03) * invDet;
            float d31 = +(v3 * m00 - v1 * m01 + v0 * m02) * invDet;

            v0 = m10 * m31 - m11 * m30;
            v1 = m10 * m32 - m12 * m30;
            v2 = m10 * m33 - m13 * m30;
            v3 = m11 * m32 - m12 * m31;
            v4 = m11 * m33 - m13 * m31;
            v5 = m12 * m33 - m13 * m32;

            float d02 = +(v5 * m01 - v4 * m02 + v3 * m03) * invDet;
            float d12 = -(v5 * m00 - v2 * m02 + v1 * m03) * invDet;
            float d22 = +(v4 * m00 - v2 * m01 + v0 * m03) * invDet;
            float d32 = -(v3 * m00 - v1 * m01 + v0 * m02) * invDet;

            v0 = m21 * m10 - m20 * m11;
            v1 = m22 * m10 - m20 * m12;
            v2 = m23 * m10 - m20 * m13;
            v3 = m22 * m11 - m21 * m12;
            v4 = m23 * m11 - m21 * m13;
            v5 = m23 * m12 - m22 * m13;

            float d03 = -(v5 * m01 - v4 * m02 + v3 * m03) * invDet;
            float d13 = +(v5 * m00 - v2 * m02 + v1 * m03) * invDet;
            float d23 = -(v4 * m00 - v2 * m01 + v0 * m03) * invDet;
            float d33 = +(v3 * m00 - v1 * m01 + v0 * m02) * invDet;

            return new MMatrix4(
                d00, d01, d02, d03,
                d10, d11, d12, d13,
                d20, d21, d22, d23,
                d30, d31, d32, d33);
        }

        public void makeTransform(MVector3 position, MVector3 scale, MQuaternion orientation)
        {
            MMatrix3 rot3x3 = new MMatrix3();
            orientation.ToRotationMatrix(rot3x3);

            m[0, 0] = scale.x * rot3x3.m[0, 0]; m[0, 1] = scale.y * rot3x3.m[0, 1]; m[0, 2] = scale.z * rot3x3.m[0, 2]; m[0, 3] = position.x;
            m[1, 0] = scale.x * rot3x3.m[1, 0]; m[1, 1] = scale.y * rot3x3.m[1, 1]; m[1, 2] = scale.z * rot3x3.m[1, 2]; m[1, 3] = position.y;
            m[2, 0] = scale.x * rot3x3.m[2, 0]; m[2, 1] = scale.y * rot3x3.m[2, 1]; m[2, 2] = scale.z * rot3x3.m[2, 2]; m[2, 3] = position.z;

            m[3, 0] = 0; m[3, 1] = 0; m[3, 2] = 0; m[3, 3] = 1;
        }

        public void makeInverseTransform(MVector3 position, MVector3 scale, MQuaternion orientation)
        {
            MVector3 invTranslate = -position;
            MVector3 invScale = new MVector3(1 / scale.x, 1 / scale.y, 1 / scale.z);
            MQuaternion invRot = orientation.Inverse();

            invTranslate = invRot * invTranslate;
            invTranslate *= invScale;

            MMatrix3 rot3x3 = new MMatrix3();
            invRot.ToRotationMatrix(rot3x3);

            m[0, 0] = invScale.x * rot3x3.m[0, 0]; m[0, 1] = invScale.x * rot3x3.m[0, 1]; m[0, 2] = invScale.x * rot3x3.m[0, 2]; m[0, 3] = invTranslate.x;
            m[1, 0] = invScale.y * rot3x3.m[1, 0]; m[1, 1] = invScale.y * rot3x3.m[1, 1]; m[1, 2] = invScale.y * rot3x3.m[1, 2]; m[1, 3] = invTranslate.y;
            m[2, 0] = invScale.z * rot3x3.m[2, 0]; m[2, 1] = invScale.z * rot3x3.m[2, 1]; m[2, 2] = invScale.z * rot3x3.m[2, 2]; m[2, 3] = invTranslate.z;

            m[3, 0] = 0; m[3, 1] = 0; m[3, 2] = 0; m[3, 3] = 1;
        }

        public void decomposition(ref MVector3 position, ref MVector3 scale, ref MQuaternion orientation)
        {
            UtilApi.assert(isAffine());

            MMatrix3 m3x3 = new MMatrix3();
            extract3x3Matrix(m3x3);

            MMatrix3 matQ;
            MVector3 vecU;
            m3x3.QDUDecomposition(matQ, scale, vecU);

            orientation = new MQuaternion(matQ);
            position = new MVector3(m[0][3], m[1][3], m[2][3]);
        }

        public bool isAffine()
        {
            return m[3, 0] == 0 && m[3, 1] == 0 && m[3, 2] == 0 && m[3, 3] == 1;
        }

        public MMatrix4 inverseAffine()
        {
            UtilApi.assert(isAffine());

            float m10 = m[1, 0], m11 = m[1, 1], m12 = m[1, 2];
            float m20 = m[2, 0], m21 = m[2, 1], m22 = m[2, 2];

            float t00 = m22 * m11 - m21 * m12;
            float t10 = m20 * m12 - m22 * m10;
            float t20 = m21 * m10 - m20 * m11;

            float m00 = m[0, 0], m01 = m[0, 1], m02 = m[0, 2];

            float invDet = 1 / (m00 * t00 + m01 * t10 + m02 * t20);

            t00 *= invDet; t10 *= invDet; t20 *= invDet;

            m00 *= invDet; m01 *= invDet; m02 *= invDet;

            float r00 = t00;
            float r01 = m02 * m21 - m01 * m22;
            float r02 = m01 * m12 - m02 * m11;

            float r10 = t10;
            float r11 = m00 * m22 - m02 * m20;
            float r12 = m02 * m10 - m00 * m12;

            float r20 = t20;
            float r21 = m01 * m20 - m00 * m21;
            float r22 = m00 * m11 - m01 * m10;

            float m03 = m[0, 3], m13 = m[1, 3], m23 = m[2, 3];

            float r03 = -(r00 * m03 + r01 * m13 + r02 * m23);
            float r13 = -(r10 * m03 + r11 * m13 + r12 * m23);
            float r23 = -(r20 * m03 + r21 * m13 + r22 * m23);

            return new MMatrix4(
                r00, r01, r02, r03,
                r10, r11, r12, r13,
                r20, r21, r22, r23,
                  0, 0, 0, 1);
        }

        public MMatrix4 concatenateAffine(MMatrix4 m2)
        {
            UtilApi.assert(isAffine() && m2.isAffine());

            return new MMatrix4(
                m[0, 0] * m2.m[0, 0] + m[0, 1] * m2.m[1, 0] + m[0, 2] * m2.m[2, 0],
                m[0, 0] * m2.m[0, 1] + m[0, 1] * m2.m[1, 1] + m[0, 2] * m2.m[2, 1],
                m[0, 0] * m2.m[0, 2] + m[0, 1] * m2.m[1, 2] + m[0, 2] * m2.m[2, 2],
                m[0, 0] * m2.m[0, 3] + m[0, 1] * m2.m[1, 3] + m[0, 2] * m2.m[2, 3] + m[0, 3],

                m[1, 0] * m2.m[0, 0] + m[1, 1] * m2.m[1, 0] + m[1, 2] * m2.m[2, 0],
                m[1, 0] * m2.m[0, 1] + m[1, 1] * m2.m[1, 1] + m[1, 2] * m2.m[2, 1],
                m[1, 0] * m2.m[0, 2] + m[1, 1] * m2.m[1, 2] + m[1, 2] * m2.m[2, 2],
                m[1, 0] * m2.m[0, 3] + m[1, 1] * m2.m[1, 3] + m[1, 2] * m2.m[2, 3] + m[1, 3],

                m[2, 0] * m2.m[0, 0] + m[2, 1] * m2.m[1, 0] + m[2, 2] * m2.m[2, 0],
                m[2, 0] * m2.m[0, 1] + m[2, 1] * m2.m[1, 1] + m[2, 2] * m2.m[2, 1],
                m[2, 0] * m2.m[0, 2] + m[2, 1] * m2.m[1, 2] + m[2, 2] * m2.m[2, 2],
                m[2, 0] * m2.m[0, 3] + m[2, 1] * m2.m[1, 3] + m[2, 2] * m2.m[2, 3] + m[2, 3],

                0, 0, 0, 1);
        }

        public MVector3 transformAffine(MVector3 v)
        {
            UtilApi.assert(isAffine());

            return new MVector3(
                    m[0, 0] * v.x + m[0, 1] * v.y + m[0, 2] * v.z + m[0, 3],
                    m[1, 0] * v.x + m[1, 1] * v.y + m[1, 2] * v.z + m[1, 3],
                    m[2, 0] * v.x + m[2, 1] * v.y + m[2, 2] * v.z + m[2, 3]);
        }

        public MVector4 transformAffine(MVector4 v)
        {
            UtilApi.assert(isAffine());

            return new MVector4(
                m[0, 0] * v.x + m[0, 1] * v.y + m[0, 2] * v.z + m[0, 3] * v.w,
                m[1, 0] * v.x + m[1, 1] * v.y + m[1, 2] * v.z + m[1, 3] * v.w,
                m[2, 0] * v.x + m[2, 1] * v.y + m[2, 2] * v.z + m[2, 3] * v.w,
                v.w);
        }

        static public MVector4 operator *(MVector4 v, MMatrix4 mat)
        {
            return new MVector4(
                v.x * mat[0, 0] + v.y * mat[1, 0] + v.z * mat[2, 0] + v.w * mat[3, 0],
                v.x * mat[0, 1] + v.y * mat[1, 1] + v.z * mat[2, 1] + v.w * mat[3, 1],
                v.x * mat[0, 2] + v.y * mat[1, 2] + v.z * mat[2, 2] + v.w * mat[3, 2],
                v.x * mat[0, 3] + v.y * mat[1, 3] + v.z * mat[2, 3] + v.w * mat[3, 3]
                );
        }
    }
}
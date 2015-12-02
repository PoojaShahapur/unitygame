namespace SDK.Lib
{
    /**
     * @brief 3D Matrix
     */
    public class MMatrix3D
    {
        public float[, ] m;

        public MMatrix3D(
            float m00 = 0, float m01 = 0, float m02 = 0, float m03 = 0,
            float m10 = 0, float m11 = 0, float m12 = 0, float m13 = 0,
            float m20 = 0, float m21 = 0, float m22 = 0, float m23 = 0,
            float m30 = 0, float m31 = 0, float m32 = 0, float m33 = 0)
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
    }
}
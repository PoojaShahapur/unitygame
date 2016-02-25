using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 3D 面板，点法式面板
     */
    public class MPlane3D
    {
        public enum Side
        {
            NO_SIDE,
            POSITIVE_SIDE,
            NEGATIVE_SIDE,
            BOTH_SIDE
        };

        /**
		 * 面板的 A 系数，或者面板法线的 x 维
		 */
        public float m_a;

        /**
		 * 面板的 B 系数，或者面板法线的 y 维
		 */
        public float m_b;

        /**
		 * 面板的 C 系数，或者面板法线的 z 维
		 */
        public float m_c;
		
		/**
		 * 面板的 D 系数，法线和点点乘的倒数
		 */
		public float m_d;
		
		/**
		 * 使用 ABCD 系数创建面板
		 */
		public MPlane3D(float a = 0, float b = 0, float c = 0, float d = 0)
        {
            this.m_a = a;
            this.m_b = b;
            this.m_c = c;
            this.m_d = d;
        }

        float getDistance(Vector3 rkPoint)
        {
            return (m_a * rkPoint.x + m_b * rkPoint.y + m_c * rkPoint.z) + m_d;
        }

        /**
         * 从 3d 空间中的 3 个点填充 Plane3D 系数
         * @param p0 Vector3D
         * @param p1 Vector3D
         * @param p2 Vector3D
         */
        public void fromPoints(Vector3 p0, Vector3 p1, Vector3 p2)
		{
			float d1x = p1.x - p0.x;
			float d1y = p1.y - p0.y;
			float d1z = p1.z - p0.z;

            float d2x = p2.x - p0.x;
            float d2y = p2.y - p0.y;
            float d2z = p2.z - p0.z;

            m_a = d1y* d2z - d1z* d2y;
            m_b = d1z* d2x - d1x* d2z;
            m_c = d1x* d2y - d1y* d2x;
            normalize();
            m_d = -(m_a * p0.x + m_b * p0.y + m_c * p0.z);
		}

        /**
         * 使用 3d 空间中的点和面板的法向量填充 Plane3D 系数
         * @param normal Vector3D
         * @param point  Vector3D
         */
        public void fromNormalAndPoint(Vector3 normal, Vector3 point)
		{
            m_a = normal.x;
            m_b = normal.y;
            m_c = normal.z;
            m_d = -(m_a * point.x + m_b * point.y + m_c * point.z);
		}

        /**
         * 单位化 Plane3D
         * @return Plane3D
         */
        public MPlane3D normalize()
		{
            float fLength = UtilApi.Sqrt(m_a * m_a + m_b * m_b + m_c * m_c);
            if (fLength > 0.0f)
            {
                float len = (float)(1 / fLength);
                m_a *= len;
                m_b *= len;
                m_c *= len;
                m_d *= len;
            }
			return this;
        }
		
		/**
		 * 判断一个点是在面板的前面后面还是在面板上
		 * @param p Vector3D
		 * @return int Plane3.FRONT or Plane3D.BACK or Plane3D.INTERSECT
		 */
		public Side getSide(Vector3 p, float epsilon = 0.01f)
		{
            float len = getDistance(p);

            if (len < -epsilon)
            {
                return Side.POSITIVE_SIDE;
            }
            else if (len > epsilon)
            {
                return Side.NEGATIVE_SIDE;
            }
            else
            {
                return Side.NO_SIDE;
            }
		}

        /**
         * @brief 判断一个四边形是在面板的哪一面
         */
        public Side getSide(Vector3 center, Vector3 halfSize)
        {
            float dist = getDistance(center);
            float maxAbsDist = UtilApi.Abs(m_a * halfSize.x) + UtilApi.Abs(m_b * halfSize.y) + UtilApi.Abs(m_c * halfSize.z);

            if (dist < -maxAbsDist)
            {
                return Side.NEGATIVE_SIDE;
            }

            if (dist > +maxAbsDist)
            {
                return Side.POSITIVE_SIDE;
            }

            return Side.BOTH_SIDE;
        }

        public string toString()
		{
			return "Plane3D [a:" + m_a + ", b:" + m_b + ", c:" + m_c + ", d:" + m_d + "].";
		}
    }
}
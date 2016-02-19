using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 球形坐标系
     */
    public class SphericalCoordinate : Coordinate
    {
        protected float m_radius;       // r∈[0,+∞)
        protected float m_theta;        // θ∈[0, π]
        protected float m_fai;          // φ∈[0,2π]

        protected float m_x;
        protected float m_y;
        protected float m_z;

        public SphericalCoordinate()
        {
            m_radius = 5;
            m_theta = Mathf.PI / 4;
            m_radius = Mathf.PI;
            m_x = 0;
            m_y = 0;
            m_z = 0;
        }

        public float getX()
        {
            return m_x;
        }

        public float getY()
        {
            return m_y;
        }

        public float getZ()
        {
            return m_z;
        }

        public void setParam(float radius, float theta, float fai)
        {
            m_radius = radius;
            m_theta = theta;
            m_fai = fai;

            convSpherical2Cartesian();
        }

        public void convCartesian2Spherical()
        {
            m_radius = Mathf.Sqrt(m_x * m_x + m_y * m_y + m_z * m_z);
            m_theta = Mathf.Acos(m_z / m_radius);
            m_fai = Mathf.Atan(m_y / m_x);
        }

        public void convSpherical2Cartesian()
        {
            m_x = m_radius * Mathf.Sin(m_theta) * Mathf.Cos(m_fai);
            m_y = m_radius * Mathf.Sin(m_theta) * Mathf.Sin(m_fai);
            m_z = m_radius * Mathf.Cos(m_theta);
        }

        public void syncTrans(Transform trans)
        {
            Vector3 vec = trans.localPosition;
            vec.Set(m_x, m_y, m_z);
        }
    }
}
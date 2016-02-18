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

        }

        public void setParam(float radius, float theta, float fai)
        {

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
    }
}
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 球形坐标系
     */
    public class SphericalCoordinate : Coordinate
    {
        // 球形坐标系，设置的旋转角度，最后都变成了空间中的坐标，因此不需要进行旋转
        protected float m_radius;       // r∈[0,+∞)，半径
        protected float m_theta;        // θ∈[0, π]，与 Y 夹角
        protected float m_fai;          // φ∈[0,2π]，与 X 夹角

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

        override public float getX()
        {
            return m_x;
        }

        override public float getY()
        {
            return m_y;
        }

        override public float getZ()
        {
            return m_z;
        }

        // 增加 theta
        override public void incTheta(float deltaDegree)
        {
            m_theta += deltaDegree * Mathf.Deg2Rad;
            //m_theta += deltaDegree;
            //m_theta %= Mathf.PI;
            if (m_theta > Mathf.PI)
            {
                //m_theta = Mathf.PI;
                m_theta = Mathf.PI - (m_theta - Mathf.PI);
                m_fai += Mathf.PI;
                m_fai %= (2 * Mathf.PI);
            }
        }

        // 减少 theta
        override public void decTheta(float deltaDegree)
        {
            m_theta -= deltaDegree * Mathf.Deg2Rad;
            //m_theta -= deltaDegree;
            if (m_theta < 0)
            {
                //m_theta = 0;
                m_theta = -m_theta;
                m_fai += Mathf.PI;
                m_fai %= (2 * Mathf.PI);
            }
        }

        override public void setParam(float radius, float theta, float fai)
        {
            m_radius = radius;
            m_theta = theta * Mathf.Deg2Rad;
            m_fai = fai * Mathf.Deg2Rad;
        }

        override public void convCartesian2Spherical()
        {
            m_radius = Mathf.Sqrt(m_x * m_x + m_y * m_y + m_z * m_z);
            m_theta = Mathf.Acos(m_z / m_radius);
            m_fai = Mathf.Atan(m_y / m_x);
        }

        override public void convSpherical2Cartesian()
        {
            m_x = m_radius * Mathf.Sin(m_theta) * Mathf.Cos(m_fai);
            m_y = m_radius * Mathf.Cos(m_theta);
            m_z = m_radius * Mathf.Sin(m_theta) * Mathf.Sin(m_fai);
        }

        override public void syncTrans(Transform trans)
        {
            Vector3 vec = trans.localPosition;
            vec.Set(m_x, m_y, m_z);
        }

        override public void updateCoord()
        {
            base.updateCoord();
            convSpherical2Cartesian();
        }
    }
}
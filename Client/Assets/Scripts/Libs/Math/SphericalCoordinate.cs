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

        protected bool m_bHalfMode;   // 半球模式
        protected bool m_bOrderMode; // 是否是相反的模式，就是操作摄像机的时候，是增加还是减少
        protected float m_thetaEpsilon;

        protected float m_x;
        protected float m_y;
        protected float m_z;

        public SphericalCoordinate()
        {
            m_bOrderMode = true;
            m_bHalfMode = true;
            m_thetaEpsilon = float.Epsilon;
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

        override public float getTheta()
        {
            return m_theta;
        }

        // 增加 theta
        override public void incTheta(float deltaDegree)
        {
            if(m_bOrderMode)
            {
                incThetaInternal(deltaDegree);
            }
            else
            {
                decThetaInternal(deltaDegree);
            }
        }

        protected void incThetaInternal(float deltaDegree)
        {
            m_theta += deltaDegree * Mathf.Deg2Rad;
            //m_theta += deltaDegree;
            //m_theta %= Mathf.PI;
            // 如果超过范围
            if (m_theta > Mathf.PI)
            {
                if (m_bHalfMode)
                {
                    m_theta = Mathf.PI - m_thetaEpsilon;
                }
                else
                {
                    /*
                    // 如果增加的比较大
                    m_bOrderMode = !m_bOrderMode;
                    //m_theta = Mathf.PI;
                    m_theta = Mathf.PI - (m_theta - Mathf.PI);
                    m_fai += Mathf.PI;
                    m_fai %= (2 * Mathf.PI);
                    */

                    // 如果范围取模后，最终在 (Mathf.PI, 2 * Mathf.PI)
                    float mod = m_theta % (2 * Mathf.PI);
                    if (mod > Mathf.PI)
                    {
                        m_bOrderMode = !m_bOrderMode;
                        m_theta = Mathf.PI - (m_theta - Mathf.PI);
                        m_fai += Mathf.PI;
                        m_fai %= (2 * Mathf.PI);
                    }
                    else
                    {
                        // 如果又进入 [0, Mathf.PI]
                        m_theta = mod;
                    }
                }
            }
        }

        // 减少 theta
        override public void decTheta(float deltaDegree)
        {
            if (m_bOrderMode)
            {
                decThetaInternal(deltaDegree);
            }
            else
            {
                incThetaInternal(deltaDegree);
            }
        }

        protected void decThetaInternal(float deltaDegree)
        {
            m_theta -= deltaDegree * Mathf.Deg2Rad;
            //m_theta -= deltaDegree;
            if (m_theta < 0)
            {
                if (m_bHalfMode)
                {
                    m_theta = m_thetaEpsilon;
                }
                else
                {
                    /*
                    m_bOrderMode = !m_bOrderMode;
                    //m_theta = 0;
                    m_theta = -m_theta;
                    m_fai += Mathf.PI;
                    m_fai %= (2 * Mathf.PI);
                    */

                    float modAbs = Mathf.Abs(m_theta % (2 * Mathf.PI));
                    // 如果减到另外一面
                    if (modAbs < Mathf.PI)
                    {
                        m_bOrderMode = !m_bOrderMode;
                        m_theta = -m_theta;
                        m_fai += Mathf.PI;
                        m_fai %= (2 * Mathf.PI);
                    }
                    else
                    {
                        // 还是在原来一面
                        m_theta = Mathf.PI - (modAbs - Mathf.PI);
                    }
                }
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
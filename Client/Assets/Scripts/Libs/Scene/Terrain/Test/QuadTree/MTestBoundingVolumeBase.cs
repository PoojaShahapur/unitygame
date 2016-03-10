using UnityEngine;

namespace SDK.Lib
{
    /**
     * @breif 包围盒 Base
     */
    public class MTestBoundingVolumeBase
    {
        protected MVector3 m_min;
        protected MVector3 m_max;
        protected MList<float> m_aabbPoints;
        protected bool m_aabbPointsDirty;

        public MTestBoundingVolumeBase()
        {
            m_aabbPoints = new MList<float>();
            m_aabbPointsDirty = true;
        }

        public MVector3 getMin()
        {
            return m_min;
        }

        public void setMin(MVector3 value)
        {
            m_min = value;
        }

        public MVector3 getMax()
        {
            return m_max;
        }

        public void setMax(MVector3 value)
        {
            m_max = value;
        }

        public MVector3 getHalfSize()
        {
            return (m_max - m_min) * 0.5f;
        }
    }
}
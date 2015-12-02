using UnityEngine;

namespace SDK.Lib
{
    /**
     * @breif 包围盒 Base
     */
    public class MBoundingVolumeBase
    {
        protected Vector3 m_min;
		protected Vector3 m_max;
		protected MList<float>  m_aabbPoints;
		protected bool m_aabbPointsDirty;

        public MBoundingVolumeBase()
        {
            m_aabbPoints = new MList<float>();
            m_aabbPointsDirty = true;
        }

        public Vector3 getMin()
        {
            return m_min;
        }

        public Vector3 getMax()
        {
            return m_max;
        }
    }
}
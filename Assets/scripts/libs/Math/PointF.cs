using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDK.Lib
{
    public class PointF
    {
        protected float m_x;
        protected float m_y;

        public PointF(float x, float y)
        {
            m_x = x;
            m_y = y;
        }

        public float X
        {
            get
            {
                return m_x;
            }
            set
            {
                m_x = value;
            }
        }

        public float Y
        {
            get
            {
                return m_y;
            }
            set
            {
                m_y = value;
            }
        }
    }
}

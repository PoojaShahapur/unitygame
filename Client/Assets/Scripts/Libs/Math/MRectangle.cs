using System;

namespace SDK.Lib
{
    public class MRectangleF
    {
        protected float m_x;
        protected float m_y;
        protected float m_width;
        protected float m_height;

        public MRectangleF(float x, float y, float width, float height)
        {
            m_x = x;
            m_y = y;
            m_width = width;
            m_height = height;
        }

        public MRectangleF(MPointF point, MSizeF size)
        {
            m_x = point.X;
            m_y = point.Y;
            m_width = size.Width;
            m_height = size.Height;
        }

        public float X
        {
            get
            {
                return m_x;
            }
        }

        public float x
        {
            get
            {
                return m_x;
            }
        }

        public float Y
        {
            get
            {
                return m_y;
            }
        }

        public float y
        {
            get
            {
                return m_y;
            }
        }

        public float Width
        {
            get
            {
                return m_width;
            }
        }

        public float width
        {
            get
            {
                return m_width;
            }
        }

        public float Height
        {
            get
            {
                return m_height;
            }
        }

        public float height
        {
            get
            {
                return m_height;
            }
        }

        public MPointF Location
        {
            get
            {
                return new MPointF(m_x, m_y);
            }
        }

        public float Left
        {
            get
            {
                return m_x;
            }
        }

        public float Top
        {
            get
            {
                return m_y;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return (m_width == 0 && m_height == 0);
            }
        }

        public bool Contains(MRectangleF rect)
        {
            float minx = Math.Max(m_x, rect.X);
            float miny = Math.Max(m_y, rect.Y);
            float maxx = Math.Min(m_x + m_width, rect.m_x + rect.Width);
            float maxy = Math.Min(m_y + m_height, rect.m_y + rect.Height);

            if (minx > maxx) return false;
            if (miny > maxy) return false;
            return (maxx - minx) * (maxy - miny) == rect.Width * rect.Height;
        }

        public bool IntersectsWith(MRectangleF rect)
        {
            float minx = Math.Max(m_x, rect.m_x);
            float miny = Math.Max(m_y, rect.m_y);
            float maxx = Math.Min(m_x + m_width, rect.X + rect.Width);
            float maxy = Math.Min(m_y + m_height, rect.Y + rect.Height);

            if ( minx>maxx ) return false;
            if ( miny>maxy ) return false;
            return (maxx - minx) * (maxy - miny) > 0;
        }
    }
}
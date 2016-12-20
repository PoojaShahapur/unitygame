namespace SDK.Lib
{
    public struct MTRectF
    {
        public float left, top, right, bottom;

        public MTRectF(float l, float t, float r, float b)
        {
            left = l;
            top = t;
            right = r;
            bottom = b;
        }

        public MTRectF(ref MTRectF o)
        {
            left = o.left;
            top = o.top;
            right = o.right;
            bottom = o.bottom;
        }

        public MTRectF assignFrom(ref MTRectF o)
        {
            left = o.left;
            top = o.top;
            right = o.right;
            bottom = o.bottom;
            return this;
        }

        public float width()
        {
            return right - left;
        }

        public float height()
        {
            return bottom - top;
        }

        public bool isNull()
        {
            return width() == 0 || height() == 0;
        }

        public void setNull()
        {
            left = right = top = bottom = 0;
        }

        public MTRectF merge(ref MTRectF rhs)
        {
            if (isNull())
            {
                this = rhs;
            }
            else if (!rhs.isNull())
            {
                left = UtilMath.min(left, rhs.left);
                right = UtilMath.max(right, rhs.right);
                top = UtilMath.min(top, rhs.top);
                bottom = UtilMath.max(bottom, rhs.bottom);
            }

            return this;
        }

        public MTRectF intersect(MTRectF rhs)
        {
            MTRectF ret = new MTRectF(0, 0, 0, 0);
            if (isNull() || rhs.isNull())
            {
                return ret;
            }
            else
            {
                ret.left = UtilMath.max(left, rhs.left);
                ret.right = UtilMath.min(right, rhs.right);
                ret.top = UtilMath.max(top, rhs.top);
                ret.bottom = UtilMath.min(bottom, rhs.bottom);
            }

            if (ret.left > ret.right || ret.top > ret.bottom)
            {
                ret.left = ret.top = ret.right = ret.bottom = 0;
            }

            return ret;
        }

        override public string ToString()
        {
            string o = ("TRect<>(l:" + this.left + ", t:" + this.top + ", r:" + this.right + ", b:" + this.bottom + ")");
            return o;
        }
    }
}
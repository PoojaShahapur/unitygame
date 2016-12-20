namespace SDK.Lib
{
    public struct MTRectI
    {
        public int left, top, right, bottom;

        public MTRectI(int l, int t, int r, int b)
        {
            left = l;
            top = t;
            right = r;
            bottom = b;
        }

        public MTRectI(ref MTRectI o)
        {
            left = o.left;
            top = o.top;
            right = o.right;
            bottom = o.bottom;
        }

        public MTRectI assignFrom(ref MTRectI o)
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

        public MTRectI merge(ref MTRectI rhs)
        {
            if (isNull())
            {
                this = rhs;
            }
            else if (!rhs.isNull())
            {
                left = (int)UtilMath.min(left, rhs.left);
                right = (int)UtilMath.max(right, rhs.right);
                top = (int)UtilMath.min(top, rhs.top);
                bottom = (int)UtilMath.max(bottom, rhs.bottom);
            }

            return this;
        }

        public MTRectI intersect(MTRectI rhs)
        {
            MTRectI ret = new MTRectI(0, 0, 0, 0);
            if (isNull() || rhs.isNull())
            {
                return ret;
            }
            else
            {
                ret.left = (int)UtilMath.max(left, rhs.left);
                ret.right = (int)UtilMath.min(right, rhs.right);
                ret.top = (int)UtilMath.max(top, rhs.top);
                ret.bottom = (int)UtilMath.min(bottom, rhs.bottom);
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
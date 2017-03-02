namespace SDK.Lib
{
    public class MSizeF
    {
        protected float mWidth;
        protected float mHeight;

        public MSizeF(float width, float height)
        {
            mWidth = width;
            mHeight = height;
        }

        public float Width
        {
            get
            {
                return mWidth;
            }
            set
            {
                mWidth = value;
            }
        }

        public float Height
        {
            get
            {
                return mHeight;
            }
            set
            {
                mHeight = value;
            }
        }
    }
}
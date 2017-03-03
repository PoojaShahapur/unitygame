namespace SDK.Lib
{
    public class TDClipRect
    {
        protected UnityEngine.Vector2 mCenter;
        protected float mWidth;    // ���
        protected float mHeight;    // �߶�
        protected UnityEngine.Vector2 mCurPos;  // ��ǰλ��
        protected float mTolerance;     // �����ƶ��ķ�Χ

        public TDClipRect()
        {
            this.init();
        }

        public void init()
        {
            this.mCenter = UnityEngine.Vector2.zero;
            this.mWidth = 2;    // ����һ����С���
            this.mHeight = 2;   // ����һ����С�߶�

            this.mCurPos = UnityEngine.Vector2.zero;
            this.mTolerance = 1.0f;
        }

        public void dispose()
        {

        }

        public UnityEngine.Vector2 getCurPos()
        {
            return this.mCurPos;
        }

        public float getLeft()
        {
            float pos = this.mCurPos.x - this.mWidth / 2;
            return pos;
        }

        public float getRight()
        {
            float pos = this.mCurPos.x + this.mWidth / 2;
            return pos;
        }

        public float getTop()
        {
            float pos = this.mCurPos.y + this.mHeight / 2;
            return pos;
        }

        public float getBottom()
        {
            float pos = this.mCurPos.y - this.mHeight / 2;
            return pos;
        }

        public bool isPosVisible(UnityEngine.Vector3 pos)
        {
            bool visible = true;

            if(pos.x < this.mCenter.x - this.mWidth / 2 ||
               pos.x > this.mCenter.x + this.mWidth / 2 ||
               pos.y < this.mCenter.y - this.mHeight / 2 ||
               pos.x > this.mCenter.x + this.mHeight / 2)
            {
                visible = false;
            }

            return visible;
        }

        public void updateClipRect(UnityEngine.Vector3 pos)
        {

        }
    }
}
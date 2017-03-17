namespace SDK.Lib
{
    public class TDClipRect
    {
        protected UnityEngine.Vector2 mCenter;
        protected float mWidth;    // ���
        protected float mHeight;    // �߶�

        protected UnityEngine.Vector2 mPrePos;  // ֮ǰ��λ��
        protected UnityEngine.Vector2 mCurPos;  // ��ǰλ��
        protected float mTolerance;     // �����ƶ��ķ�Χ
        protected bool mIsRectDirty;    // �ü������Ƿ��� Dirty

        protected int mExtendRange;          // ��չ��Χ
        protected bool mIsFirst;        // �Ƿ��ǵ�һ��

        public TDClipRect()
        {
            this.init();
        }

        public void init()
        {
            this.mCenter = UtilMath.ZeroVec2;
            this.mWidth = 2;    // ����һ����С���
            this.mHeight = 2;   // ����һ����С�߶�
            this.mExtendRange = 0;

            this.mCurPos = UtilMath.ZeroVec2;
            this.mTolerance = 1.0f;
            this.mIsRectDirty = false;
            this.mIsFirst = true;

            Ctx.mInstance.mGlobalDelegate.mMainChildChangedDispatch.addEventHandle(null, this.onPosChanged);
        }

        public void dispose()
        {
            Ctx.mInstance.mGlobalDelegate.mMainChildChangedDispatch.removeEventHandle(null, this.onPosChanged);
        }

        public void setIsRectDirty(bool value)
        {
            this.mIsRectDirty = value;
        }

        public void setCam(UnityEngine.Camera cam)
        {
            UnityEngine.Vector2 size = UtilApi.getOrthoCamSize(cam);

            this.mWidth = size.x;
            this.mHeight = size.y;
        }

        public UnityEngine.Vector2 getCurPos()
        {
            return this.mCurPos;
        }

        public float getLeft()
        {
            float pos = this.mCurPos.x - this.mWidth / 2 - this.mExtendRange;
            return pos;
        }

        public float getRight()
        {
            float pos = this.mCurPos.x + this.mWidth / 2 + this.mExtendRange;
            return pos;
        }

        public float getTop()
        {
            float pos = this.mCurPos.y + this.mHeight / 2 + this.mExtendRange;
            return pos;
        }

        public float getBottom()
        {
            float pos = this.mCurPos.y - this.mHeight / 2 - this.mExtendRange;
            return pos;
        }

        public bool isPosVisible(UnityEngine.Vector3 pos)
        {
            bool visible = true;

            if(pos.x < this.mCenter.x - this.mWidth / 2 - this.mExtendRange ||
               pos.x > this.mCenter.x + this.mWidth / 2 + this.mExtendRange ||
               pos.y < this.mCenter.y - this.mHeight / 2 - this.mExtendRange ||
               pos.y > this.mCenter.y + this.mHeight / 2 + this.mExtendRange)
            {
                visible = false;
            }

            return visible;
        }

        public void onPosChanged(IDispatchObject dispObj)
        {
            if (null != Ctx.mInstance.mPlayerMgr.getHero())
            {
                UnityEngine.Vector3 pos = Ctx.mInstance.mPlayerMgr.getHero().getPos();
                this.updateClipRect(pos);
            }
        }

        public void updateClipRect(UnityEngine.Vector3 pos)
        {
            this.mCurPos.x = pos.x;
            this.mCurPos.y = pos.y;

            this.mCenter = this.mCurPos;    // ���ڲ���������ֱ���ƶ�����������

            if (!UtilMath.isEqualVec2(this.mPrePos, this.mCurPos, mTolerance))
            {
                this.mIsRectDirty = true;
                this.mPrePos = this.mCurPos;
            }
        }

        // ���³���ͼ
        public void updateSceneGraph()
        {
            if(this.mIsRectDirty || this.mIsFirst)
            {
                this.mIsRectDirty = false;
                this.mIsFirst = false;

                // ���³���������
                Ctx.mInstance.mTileMgr.updateClipRect(this);

                // ���µ��ι�����
                this.mExtendRange = 4;  // District �����һ��
                Ctx.mInstance.mTwoDTerrain.updateClipRect(this);
                this.mExtendRange = 0;
            }
            else
            {
                Ctx.mInstance.mTileMgr.updateDirtyEntity();
            }
        }
    }
}
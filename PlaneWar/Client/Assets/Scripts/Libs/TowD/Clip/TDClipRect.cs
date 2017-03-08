namespace SDK.Lib
{
    public class TDClipRect
    {
        protected UnityEngine.Vector2 mCenter;
        protected float mWidth;    // 宽度
        protected float mHeight;    // 高度

        protected UnityEngine.Vector2 mPrePos;  // 之前的位置
        protected UnityEngine.Vector2 mCurPos;  // 当前位置
        protected float mTolerance;     // 允许移动的范围
        protected bool mIsRectDirty;    // 裁剪矩形是否是 Dirty

        protected int mExtendRange;          // 扩展范围

        public TDClipRect()
        {
            this.init();
        }

        public void init()
        {
            this.mCenter = UnityEngine.Vector2.zero;
            this.mWidth = 2;    // 设置一个最小宽度
            this.mHeight = 2;   // 设置一个最小高度
            this.mExtendRange = 0;

            this.mCurPos = UnityEngine.Vector2.zero;
            this.mTolerance = 1.0f;
            this.mIsRectDirty = false;

            Ctx.mInstance.mGlobalDelegate.mMainChildChangedDispatch.addEventHandle(null, this.onPosChanged);
        }

        public void dispose()
        {
            Ctx.mInstance.mGlobalDelegate.mMainChildChangedDispatch.removeEventHandle(null, this.onPosChanged);
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
            UnityEngine.Vector3 pos = Ctx.mInstance.mPlayerMgr.getHero().getPos();
            this.updateClipRect(pos);
        }

        public void updateClipRect(UnityEngine.Vector3 pos)
        {
            this.mCurPos.x = pos.x;
            this.mCurPos.y = pos.y;

            this.mCenter = this.mCurPos;    // 现在不做动画，直接移动过来就行了

            if (!UtilMath.isEqualVec2(this.mPrePos, this.mCurPos, mTolerance))
            {
                this.mIsRectDirty = true;
                this.mPrePos = this.mCurPos;
            }
        }

        // 更新场景图
        public void updateSceneGraph()
        {
            if(this.mIsRectDirty)
            {
                this.mIsRectDirty = false;

                Ctx.mInstance.mTileMgr.updateClipRect(this);
            }
            else
            {
                Ctx.mInstance.mTileMgr.updateDirtyEntity();
            }
        }
    }
}
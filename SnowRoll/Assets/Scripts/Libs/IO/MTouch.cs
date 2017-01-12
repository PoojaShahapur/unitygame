using UnityEngine;

namespace SDK.Lib
{
    public class MTouch : MMouseOrTouch, IDispatchObject
    {
        static MDictionary<int, MTouch> mTouches = new MDictionary<int, MTouch>();

        protected int mTouchIndex;  // 触碰索引
        protected Touch mNativeTouch;   // Unity Touch

        static public MTouch GetTouch(int id)
        {
            MTouch touch = null;
            
            if (!mTouches.TryGetValue(id, out touch))
            {
                touch = new MTouch(id);
                touch.mPressTime = RealTime.time;
                touch.mTouchBegan = true;
                mTouches.Add(id, touch);
            }
            return touch;
        }

        public MTouch(int touchIndex)
        {
            this.mTouchIndex = touchIndex;
        }

        public void setNativeTouch(Touch nativeTouch)
        {
            this.mNativeTouch = nativeTouch;
        }

        public void onTick(float delta)
        {
            if (mNativeTouch.phase == TouchPhase.Began)
            {
                // 按下的时候，设置位置相同
                this.mPos = this.mNativeTouch.position;
                this.mLastPos = this.mPos;

                this.handleTouchBegan();
            }
            else if (mNativeTouch.phase == TouchPhase.Moved)
            {
                // Up 的时候，先设置之前的位置，然后设置当前位置
                this.mLastPos = this.mPos;
                this.mPos = this.mNativeTouch.position;

                this.handleTouchMoved();
            }
            else if (mNativeTouch.phase == TouchPhase.Stationary)
            {
                // Press 的时候，先设置之前的位置，然后设置当前位置
                this.mLastPos = this.mPos;
                this.mPos = this.mNativeTouch.position;

                this.handleTouchStationary();
            }
            else if (mNativeTouch.phase == TouchPhase.Ended)
            {
                this.mLastPos = this.mPos;
                this.mPos = this.mNativeTouch.position;

                this.handleTouchEnded();
            }
            else if (mNativeTouch.phase == TouchPhase.Canceled)
            {
                this.mLastPos = this.mPos;
                this.mPos = UnityEngine.Input.mousePosition;

                this.handleTouchCanceled();
            }
        }

        public void handleTouchBegan()
        {
            Ctx.mInstance.mTouchDispatchSystem.handleTouchBegan(this);
        }

        public void handleTouchMoved()
        {
            if (this.isPosChanged())
            {
                Ctx.mInstance.mTouchDispatchSystem.handleTouchMoved(this);
            }
        }

        public void handleTouchStationary()
        {
            Ctx.mInstance.mTouchDispatchSystem.handleTouchStationary(this);
        }

        public void handleTouchEnded()
        {
            Ctx.mInstance.mTouchDispatchSystem.handleTouchEnded(this);
        }

        public void handleTouchCanceled()
        {
            Ctx.mInstance.mTouchDispatchSystem.handleTouchCanceled(this);
        }
    }
}
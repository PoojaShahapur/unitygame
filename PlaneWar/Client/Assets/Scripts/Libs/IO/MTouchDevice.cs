using UnityEngine;

namespace SDK.Lib
{
    public class MTouchDevice : MMouseOrTouch, IDispatchObject
    {
        static MDictionary<int, MTouchDevice> mTouches = new MDictionary<int, MTouchDevice>();

        protected Touch mNativeTouch;   // Unity Touch

        static public MTouchDevice GetTouch(int id)
        {
            MTouchDevice touch = null;
            
            if (!mTouches.TryGetValue(id, out touch))
            {
                touch = new MTouchDevice(id);
                mTouches.Add(id, touch);
            }
            return touch;
        }

        public MTouchDevice(int touchIndex)
        {
            this.mTouchIndex = touchIndex;
        }

        public void setNativeTouch(Touch nativeTouch, int touchIndex)
        {
            this.mNativeTouch = nativeTouch;
            this.mTouchIndex = touchIndex;
        }

        public void onTick(float delta)
        {
            if (mNativeTouch.phase == TouchPhase.Began)
            {
                this.mPressTime = RealTime.time;
                this.mTouchBegan = true;
                this.mTouchEnd = false;

                // 按下的时候，设置位置相同
                this.mPos = this.mNativeTouch.position;
                this.mLastPos = this.mPos;

                this.handleTouchBegan();
            }
            else if (mNativeTouch.phase == TouchPhase.Moved)
            {
                this.mTouchBegan = false;
                this.mTouchEnd = false;

                // Up 的时候，先设置之前的位置，然后设置当前位置
                this.mLastPos = this.mPos;
                this.mPos = this.mNativeTouch.position;

                this.handleTouchMoved();
            }
            else if (mNativeTouch.phase == TouchPhase.Stationary)
            {
                this.mTouchBegan = false;
                this.mTouchEnd = false;

                // Press 的时候，先设置之前的位置，然后设置当前位置
                this.mLastPos = this.mPos;
                this.mPos = this.mNativeTouch.position;

                this.handleTouchStationary();
            }
            else if (mNativeTouch.phase == TouchPhase.Ended)
            {
                this.mTouchBegan = false;
                this.mTouchEnd = true;

                this.mLastPos = this.mPos;
                this.mPos = this.mNativeTouch.position;

                this.handleTouchEnded();
            }
            else if (mNativeTouch.phase == TouchPhase.Canceled)
            {
                this.mTouchBegan = false;
                this.mTouchEnd = true;

                this.mLastPos = this.mPos;
                this.mPos = UnityEngine.Input.mousePosition;

                this.handleTouchCanceled();
            }
        }

        public void handleTouchBegan()
        {
            Ctx.mInstance.mInputMgr.handleTouchBegan(this);
        }

        public void handleTouchMoved()
        {
            if (this.isPosChanged())
            {
                Ctx.mInstance.mInputMgr.handleTouchMoved(this);
            }
        }

        public void handleTouchStationary()
        {
            Ctx.mInstance.mInputMgr.handleTouchStationary(this);
        }

        public void handleTouchEnded()
        {
            Ctx.mInstance.mInputMgr.handleTouchEnded(this);
        }

        public void handleTouchCanceled()
        {
            Ctx.mInstance.mInputMgr.handleTouchCanceled(this);
        }
    }
}
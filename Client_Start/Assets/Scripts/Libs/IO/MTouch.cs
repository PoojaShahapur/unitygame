using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    public class MTouch : MMouseOrTouch, IDispatchObject
    {
        // 最多支持 5 个同时触碰
        public static Dictionary<int, MTouch> mTouches = new Dictionary<int, MTouch>();

        protected int mTouchIndex;  // 触碰索引
        protected Touch mNativeTouch;   // Unity Touch

        private AddOnceEventDispatch mOnTouchBeganDispatch;         // 触碰开始
        private AddOnceEventDispatch mOnTouchMovedDispatch;         // 触碰状态，但是移动
        private AddOnceEventDispatch mOnTouchStationaryDispatch;    // 触碰状态但是不移动
        private AddOnceEventDispatch mOnTouchEndedDispatch;         // 触碰结束
        private AddOnceEventDispatch mOnTouchCanceledDispatch;      // 触碰取消

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

            this.mOnTouchBeganDispatch = new AddOnceEventDispatch();
            this.mOnTouchMovedDispatch = new AddOnceEventDispatch();
            this.mOnTouchStationaryDispatch = new AddOnceEventDispatch();
            this.mOnTouchEndedDispatch = new AddOnceEventDispatch();
            this.mOnTouchCanceledDispatch = new AddOnceEventDispatch();
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

        public void addTouchListener(EventId evtID, MAction<IDispatchObject> handle)
        {
            if (EventId.TOUCHBEGIN_EVENT == evtID)
            {
                this.mOnTouchBeganDispatch.addEventHandle(null, handle);
            }
            else if (EventId.TOUCHMOVED_EVENT == evtID)
            {
                this.mOnTouchMovedDispatch.addEventHandle(null, handle);
            }
            else if (EventId.TOUCHSTATIONARY_EVENT == evtID)
            {
                this.mOnTouchStationaryDispatch.addEventHandle(null, handle);
            }
            else if (EventId.TOUCHENDED_EVENT == evtID)
            {
                this.mOnTouchEndedDispatch.addEventHandle(null, handle);
            }
            else if (EventId.TOUCHCANCELED_EVENT == evtID)
            {
                this.mOnTouchCanceledDispatch.addEventHandle(null, handle);
            }
        }

        public void removeTouchListener(EventId evtID, MAction<IDispatchObject> handle)
        {
            if (EventId.TOUCHBEGIN_EVENT == evtID)
            {
                this.mOnTouchBeganDispatch.removeEventHandle(null, handle);
            }
            else if (EventId.TOUCHMOVED_EVENT == evtID)
            {
                this.mOnTouchMovedDispatch.removeEventHandle(null, handle);
            }
            else if (EventId.TOUCHSTATIONARY_EVENT == evtID)
            {
                this.mOnTouchStationaryDispatch.removeEventHandle(null, handle);
            }
            else if (EventId.TOUCHENDED_EVENT == evtID)
            {
                this.mOnTouchEndedDispatch.removeEventHandle(null, handle);
            }
            else if (EventId.TOUCHCANCELED_EVENT == evtID)
            {
                this.mOnTouchCanceledDispatch.removeEventHandle(null, handle);
            }
        }

        // 是否还有需要处理的事件
        public bool hasEventHandle()
        {
            if (this.mOnTouchBeganDispatch.hasEventHandle())
            {
                return true;
            }
            if (this.mOnTouchMovedDispatch.hasEventHandle())
            {
                return true;
            }
            if (this.mOnTouchStationaryDispatch.hasEventHandle())
            {
                return true;
            }
            if (this.mOnTouchEndedDispatch.hasEventHandle())
            {
                return true;
            }
            if (this.mOnTouchCanceledDispatch.hasEventHandle())
            {
                return true;
            }

            return false;
        }

        public void handleTouchBegan()
        {
            if (null != this.mOnTouchBeganDispatch)
            {
                this.mOnTouchBeganDispatch.dispatchEvent(this);
            }
        }

        public void handleTouchMoved()
        {
            if (this.isPosChanged())
            {
                if (null != this.mOnTouchMovedDispatch)
                {
                    this.mOnTouchMovedDispatch.dispatchEvent(this);
                }
            }
        }

        public void handleTouchStationary()
        {
            if (null != this.mOnTouchStationaryDispatch)
            {
                this.mOnTouchStationaryDispatch.dispatchEvent(this);
            }
        }

        public void handleTouchEnded()
        {
            if (null != this.mOnTouchEndedDispatch)
            {
                this.mOnTouchEndedDispatch.dispatchEvent(this);
            }
        }

        public void handleTouchCanceled()
        {
            if (null != this.mOnTouchCanceledDispatch)
            {
                this.mOnTouchCanceledDispatch.dispatchEvent(this);
            }
        }
    }
}
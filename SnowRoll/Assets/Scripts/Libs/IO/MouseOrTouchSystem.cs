namespace SDK.Lib
{
    /**
     * @brief 触碰后事件分发系统，这里的事件可以通过逻辑直接触发，不用必须判断真正的硬件
     */
    public class MouseOrTouchDispatchSystem
    {
        public MControlScheme mCurrentScheme = MControlScheme.Mouse;
        public int mCurrentTouchId = -1;
        public MTouchDevice mCurrentTouch = null;

        private bool mHasTouch;
        private bool mHasMultiTouch;

        // 单触碰
        private AddOnceEventDispatch mOnTouchBeganDispatch;         // 触碰开始
        private AddOnceEventDispatch mOnTouchMovedDispatch;         // 触碰状态，但是移动
        private AddOnceEventDispatch mOnTouchStationaryDispatch;    // 触碰状态但是不移动
        private AddOnceEventDispatch mOnTouchEndedDispatch;         // 触碰结束
        private AddOnceEventDispatch mOnTouchCanceledDispatch;      // 触碰取消
        // 多触碰
        private AddOnceEventDispatch mOnMultiTouchBeganDispatch;         // 触碰开始
        private AddOnceEventDispatch mOnMultiTouchMovedDispatch;         // 触碰状态，但是移动
        private AddOnceEventDispatch mOnMultiTouchStationaryDispatch;    // 触碰状态但是不移动
        private AddOnceEventDispatch mOnMultiTouchEndedDispatch;         // 触碰结束
        private AddOnceEventDispatch mOnMultiTouchCanceledDispatch;      // 触碰取消

        // 多触碰集合
        protected MultiTouchSet mMultiTouchSet;
        // 鼠标模拟触碰
        public bool mMultiTouchEnabled;
        public bool mSimulateMouseWithTouches;
        public bool mTouchSupported;

        // 鼠标
        private MMouseDispatch[] mMouseDispatchArray;

        public MouseOrTouchDispatchSystem()
        {
            this.mOnTouchBeganDispatch = new AddOnceEventDispatch();
            this.mOnTouchMovedDispatch = new AddOnceEventDispatch();
            this.mOnTouchStationaryDispatch = new AddOnceEventDispatch();
            this.mOnTouchEndedDispatch = new AddOnceEventDispatch();
            this.mOnTouchCanceledDispatch = new AddOnceEventDispatch();

            this.mOnMultiTouchBeganDispatch = new AddOnceEventDispatch();
            this.mOnMultiTouchMovedDispatch = new AddOnceEventDispatch();
            this.mOnMultiTouchStationaryDispatch = new AddOnceEventDispatch();
            this.mOnMultiTouchEndedDispatch = new AddOnceEventDispatch();
            this.mOnMultiTouchCanceledDispatch = new AddOnceEventDispatch();

            this.mMouseDispatchArray = new MMouseDispatch[3];
            this.mMouseDispatchArray[0].init();
            this.mMouseDispatchArray[1].init();
            this.mMouseDispatchArray[2].init();

            this.mMultiTouchSet = new MultiTouchSet();

            this.mMultiTouchEnabled = UnityEngine.Input.multiTouchEnabled;
            this.mSimulateMouseWithTouches = UnityEngine.Input.simulateMouseWithTouches;
            this.mTouchSupported = UnityEngine.Input.touchSupported;

            // Test
            this.mSimulateMouseWithTouches = true;
            this.mTouchSupported = true;
            this.mMultiTouchEnabled = true;
        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        public void ProcessTouches(float delta)
        {
            if (Ctx.mInstance.mMouseOrTouchDispatchSystem.hasTouch() || Ctx.mInstance.mMouseOrTouchDispatchSystem.hasMultiTouch())
            {
                this.mMultiTouchSet.reset();

                this.mCurrentScheme = MControlScheme.Touch;

                int idx = 0;
                while (idx < UnityEngine.Input.touchCount)
                {
                    UnityEngine.Touch touch = UnityEngine.Input.GetTouch(idx);

                    this.mCurrentTouchId = this.mMultiTouchEnabled ? touch.fingerId : 0;
                    this.mCurrentTouch = MTouch.GetTouch(this.mCurrentTouchId);

                    this.mCurrentTouch.setNativeTouch(touch, this.mCurrentTouchId);
                    this.mCurrentTouch.onTick(delta);

                    if (Ctx.mInstance.mMouseOrTouchDispatchSystem.hasMultiTouch())
                    {
                        this.mMultiTouchSet.addTouch(this.mCurrentTouch);
                    }

                    ++idx;
                }

                if (Ctx.mInstance.mMouseOrTouchDispatchSystem.hasMultiTouch())
                {
                    this.mMultiTouchSet.onTick(delta);
                }
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

            this.mHasTouch = true;
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

            this.mHasTouch = this.hasEventHandle();
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

        public bool hasTouch()
        {
            return this.mHasTouch;
        }

        public void handleTouchBegan(MMouseOrTouch touch)
        {
            if (null != this.mOnTouchBeganDispatch)
            {
                this.mOnTouchBeganDispatch.dispatchEvent(touch);
            }
        }

        public void handleTouchMoved(MMouseOrTouch touch)
        {
            if (null != this.mOnTouchMovedDispatch)
            {
                this.mOnTouchMovedDispatch.dispatchEvent(touch);
            }
        }

        public void handleTouchStationary(MMouseOrTouch touch)
        {
            if (null != this.mOnTouchStationaryDispatch)
            {
                this.mOnTouchStationaryDispatch.dispatchEvent(touch);
            }
        }

        public void handleTouchEnded(MMouseOrTouch touch)
        {
            if (null != this.mOnTouchEndedDispatch)
            {
                this.mOnTouchEndedDispatch.dispatchEvent(touch);
            }
        }

        public void handleTouchCanceled(MMouseOrTouch touch)
        {
            if (null != this.mOnTouchCanceledDispatch)
            {
                this.mOnTouchCanceledDispatch.dispatchEvent(touch);
            }
        }

        /********************************** Multi Touch *********************************/
        public void addMultiTouchListener(EventId evtID, MAction<IDispatchObject> handle)
        {
            if (EventId.MULTI_TOUCHBEGIN_EVENT == evtID)
            {
                this.mOnMultiTouchBeganDispatch.addEventHandle(null, handle);
            }
            else if (EventId.MULTI_TOUCHMOVED_EVENT == evtID)
            {
                this.mOnMultiTouchMovedDispatch.addEventHandle(null, handle);
            }
            else if (EventId.MULTI_TOUCHSTATIONARY_EVENT == evtID)
            {
                this.mOnMultiTouchStationaryDispatch.addEventHandle(null, handle);
            }
            else if (EventId.MULTI_TOUCHENDED_EVENT == evtID)
            {
                this.mOnMultiTouchEndedDispatch.addEventHandle(null, handle);
            }
            else if (EventId.MULTI_TOUCHCANCELED_EVENT == evtID)
            {
                this.mOnMultiTouchCanceledDispatch.addEventHandle(null, handle);
            }

            this.mHasMultiTouch = true;
        }

        public void removeMultiTouchListener(EventId evtID, MAction<IDispatchObject> handle)
        {
            if (EventId.MULTI_TOUCHBEGIN_EVENT == evtID)
            {
                this.mOnMultiTouchBeganDispatch.removeEventHandle(null, handle);
            }
            else if (EventId.MULTI_TOUCHMOVED_EVENT == evtID)
            {
                this.mOnMultiTouchMovedDispatch.removeEventHandle(null, handle);
            }
            else if (EventId.MULTI_TOUCHSTATIONARY_EVENT == evtID)
            {
                this.mOnMultiTouchStationaryDispatch.removeEventHandle(null, handle);
            }
            else if (EventId.MULTI_TOUCHENDED_EVENT == evtID)
            {
                this.mOnMultiTouchEndedDispatch.removeEventHandle(null, handle);
            }
            else if (EventId.MULTI_TOUCHCANCELED_EVENT == evtID)
            {
                this.mOnMultiTouchCanceledDispatch.removeEventHandle(null, handle);
            }

            this.mHasMultiTouch = this.hasMultiEventHandle();
        }

        // 是否还有需要处理的事件
        public bool hasMultiEventHandle()
        {
            if (this.mOnMultiTouchBeganDispatch.hasEventHandle())
            {
                return true;
            }
            if (this.mOnMultiTouchMovedDispatch.hasEventHandle())
            {
                return true;
            }
            if (this.mOnMultiTouchStationaryDispatch.hasEventHandle())
            {
                return true;
            }
            if (this.mOnMultiTouchEndedDispatch.hasEventHandle())
            {
                return true;
            }
            if (this.mOnMultiTouchCanceledDispatch.hasEventHandle())
            {
                return true;
            }

            return false;
        }

        public bool hasMultiTouch()
        {
            return this.mHasMultiTouch;
        }

        public void handleMultiTouchBegan(IDispatchObject touch)
        {
            if (null != this.mOnMultiTouchBeganDispatch)
            {
                this.mOnMultiTouchBeganDispatch.dispatchEvent(touch);
            }
        }

        public void handleMultiTouchMoved(IDispatchObject touch)
        {
            if (null != this.mOnMultiTouchMovedDispatch)
            {
                this.mOnMultiTouchMovedDispatch.dispatchEvent(touch);
            }
        }

        public void handleMultiTouchStationary(IDispatchObject touch)
        {
            if (null != this.mOnMultiTouchStationaryDispatch)
            {
                this.mOnMultiTouchStationaryDispatch.dispatchEvent(touch);
            }
        }

        public void handleMultiTouchEnded(IDispatchObject touch)
        {
            if (null != this.mOnMultiTouchEndedDispatch)
            {
                this.mOnMultiTouchEndedDispatch.dispatchEvent(touch);
            }
        }

        public void handleMultiTouchCanceled(IDispatchObject touch)
        {
            if (null != this.mOnMultiTouchCanceledDispatch)
            {
                this.mOnMultiTouchCanceledDispatch.dispatchEvent(touch);
            }
        }

        /******************* Mouse Dispatch *********************/
        public void addMouseListener(MMouseDevice mouse, EventId evtID, MAction<IDispatchObject> handle)
        {
            this.mMouseDispatchArray[mouse.mTouchIndex].addMouseListener(evtID, handle);
        }

        public void removeMouseListener(MMouseDevice mouse, EventId evtID, MAction<IDispatchObject> handle)
        {
            this.mMouseDispatchArray[mouse.mTouchIndex].removeMouseListener(evtID, handle);
        }

        // 是否还有需要处理的事件
        public bool hasEventHandle(MMouseDevice mouse)
        {
            return this.mMouseDispatchArray[mouse.mTouchIndex].hasEventHandle();
        }

        public void handleMouseDown(MMouseOrTouch mouse)
        {
            this.mMouseDispatchArray[mouse.mTouchIndex].handleMouseDown(mouse);
        }

        public void handleMouseUp(MMouseOrTouch mouse)
        {
            this.mMouseDispatchArray[mouse.mTouchIndex].handleMouseUp(mouse);
        }

        public void handleMousePress(MMouseOrTouch mouse)
        {
            this.mMouseDispatchArray[mouse.mTouchIndex].handleMousePress(mouse);
        }

        public void handleMousePressOrMove(MMouseOrTouch mouse)
        {
            this.mMouseDispatchArray[mouse.mTouchIndex].handleMousePressOrMove(mouse);
        }

        public void handleMousePressMove(MMouseOrTouch mouse)
        {
            this.mMouseDispatchArray[mouse.mTouchIndex].handleMousePressMove(mouse);
        }
    }
}
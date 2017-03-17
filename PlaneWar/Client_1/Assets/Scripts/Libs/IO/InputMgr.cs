namespace SDK.Lib
{
    /**
     * @brief 主要是场景消息处理， UI 消息单独走，这个主要是从硬件判断
     * @error InputMgr 的 Tick 一定不能放在 FixedUpdate 里面，会经常检测不到
     */
    public class InputMgr : ITickedObject, IDelayHandleItem, INoOrPriorityObject
    {
        // 有监听事件的键盘 InputKey
        protected MList<InputKey> mEventInputKeyList;
        // 有监听事件的鼠标 MMouse
        protected MList<MMouseDevice> mEventMouseList;
        // 是否有重力感应事件
        protected bool mHasAccelerationHandle;

        //private Action mOnAxisDown = null;

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

        public InputMgr()
        {
            this.mEventInputKeyList = new MList<InputKey>();
            this.mEventMouseList = new MList<MMouseDevice>();
            this.mHasAccelerationHandle = false;

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
            this.mMouseDispatchArray[0] = new MMouseDispatch();
            this.mMouseDispatchArray[1] = new MMouseDispatch();
            this.mMouseDispatchArray[2] = new MMouseDispatch();

            this.mMultiTouchSet = new MultiTouchSet();

            this.mMultiTouchEnabled = UnityEngine.Input.multiTouchEnabled;
            //this.mSimulateMouseWithTouches = UnityEngine.Input.simulateMouseWithTouches;
            this.mTouchSupported = UnityEngine.Input.touchSupported;
            this.mSimulateMouseWithTouches = true;
            //this.mMultiTouchEnabled = true;
        }

        public void init()
        {
            InputKey.getInputKeyArray();
            this.mMouseDispatchArray[0].init();
            this.mMouseDispatchArray[1].init();
            this.mMouseDispatchArray[2].init();
        }

        public void dispose()
        {
            this.mMouseDispatchArray[0].dispose();
            this.mMouseDispatchArray[1].dispose();
            this.mMouseDispatchArray[2].dispose();
        }

        public void setClientDispose(bool isDispose)
        {

        }

        public bool isClientDispose()
        {
            return false;
        }

        /**
         * @inheritDoc
         */
        public void onTick(float delta, TickMode tickMode)
        {
            //handleAxis();

            // This function tracks which keys were just pressed (or released) within the last tick.
            // It should be called at the beginning of the tick to give the most accurate responses possible.
            int idx = 0;
            
            for (idx = 0; idx < this.mEventInputKeyList.Count(); idx++)
            {
                this.mEventInputKeyList[idx].onTick(delta, tickMode);
            }

            for (idx = 0; idx < this.mEventMouseList.Count(); idx++)
            {
                this.mEventMouseList[idx].onTick(delta, tickMode);
            }

            this.ProcessTouches(delta, tickMode);

            if(this.mHasAccelerationHandle)
            {
                MAcceleration.mAccelerationOne.onTick(delta, tickMode);
            }
        }

        //protected void handleAxis()
        //{
        //    float horizontal = Input.GetAxis("Horizontal");
        //    float vertical = Input.GetAxis("Vertical");
        //    if (horizontal != 0.0f || vertical != 0.0f)
        //    {
        //        if (mOnAxisDown != null)
        //        {
        //            mOnAxisDown();
        //        }
        //    }
        //}

        /**
         * Returns whether or not a key was pressed since the last tick.
         */
        public bool keyJustPressed(InputKey inputKey)
        {
            return inputKey.keyJustPressed();
        }

        /**
         * Returns whether or not a key was released since the last tick.
         */
        public bool keyJustReleased(InputKey inputKey)
        {
            return inputKey.keyJustReleased();
        }

        /**
         * Returns whether or not a specific key is down.
         */
        public bool isKeyDown(InputKey inputKey)
        {
            return inputKey.isKeyDown();
        }

        /**
         * Returns true if any key is down.
         */
        public bool isAnyKeyDown()
        {
            foreach (InputKey inputKey in InputKey.mInputKeyArray)
            {
                if (null != inputKey)
                {
                    return inputKey.mKeyState;
                }
            }
            return false;
        }

        // 添加 KeyInput 输入事件
        public void addKeyListener(InputKey inputKey, EventId evtID, MAction<IDispatchObject> handle)
        {
            inputKey.addKeyListener(evtID, handle);

            this.addEventInputKey(inputKey);
        }

        // 移除键盘 KeyInput 输入事件
        public void removeKeyListener(InputKey inputKey, EventId evtID, MAction<IDispatchObject> handle)
        {
            inputKey.removeKeyListener(evtID, handle);

            if(!inputKey.hasEventHandle())
            {
                this.removeEventInputKey(inputKey);
            }
        }

        // 添加鼠标监听器
        public void addMouseListener(MMouseDevice mouse, EventId evtID, MAction<IDispatchObject> handle)
        {
            this.mMouseDispatchArray[mouse.mTouchIndex].addMouseListener(evtID, handle);
            this.addEventMouse(mouse);
        }

        // 移除鼠标监听器
        public void removeMouseListener(MMouseDevice mouse, EventId evtID, MAction<IDispatchObject> handle)
        {
            this.mMouseDispatchArray[mouse.mTouchIndex].removeMouseListener(evtID, handle);

            if (!this.hasEventHandle(mouse))
            {
                this.removeEventMouse(mouse);
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

            if(!this.mTouchSupported)
            {
                if (this.mSimulateMouseWithTouches)
                {
                    this.addEventMouse(MMouseDevice.MouseLeftButton);
                }
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

            this.mHasTouch = this.hasEventHandle();

            if (!this.mTouchSupported && !this.mHasTouch)
            {
                if (this.mSimulateMouseWithTouches)
                {
                    this.mHasMultiTouch = this.hasMultiEventHandle();

                    if (!this.mHasMultiTouch)
                    {
                        this.removeEventMouse(MMouseDevice.MouseLeftButton);
                    }
                }
            }
        }

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

            if (!this.mTouchSupported)
            {
                if (this.mSimulateMouseWithTouches)
                {
                    this.addEventMouse(MMouseDevice.MouseLeftButton);
                }
            }
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

            if (!this.mTouchSupported && !this.mHasMultiTouch)
            {
                if (this.mSimulateMouseWithTouches)
                {
                    this.mHasTouch = this.hasEventHandle();

                    if (!this.mHasTouch)
                    {
                        this.removeEventMouse(MMouseDevice.MouseLeftButton);
                    }
                }
            }
        }

        public void addAccelerationListener(EventId evtID, MAction<IDispatchObject> handle)
        {
            MAcceleration.mAccelerationOne.addAccelerationListener(evtID, handle);
            this.mHasAccelerationHandle = true;
        }

        public void removeAccelerationListener(EventId evtID, MAction<IDispatchObject> handle)
        {
            MAcceleration.mAccelerationOne.removeAccelerationListener(evtID, handle);
            if(!MAcceleration.mAccelerationOne.hasEventHandle())
            {
                this.mHasAccelerationHandle = false;
            }
        }

        //public void addAxisListener(EventId evtID, Action cb)
        //{
        //    mOnAxisDown += cb;
        //}

        //public void removeAxisListener(EventId evtID, Action cb)
        //{
        //    mOnAxisDown -= cb;
        //}

        protected void addEventInputKey(InputKey inputKey)
        {
            if(-1 == this.mEventInputKeyList.IndexOf(inputKey))
            {
                this.mEventInputKeyList.Add(inputKey);
            }
        }

        protected void removeEventInputKey(InputKey inputKey)
        {
            if (-1 != this.mEventInputKeyList.IndexOf(inputKey))
            {
                this.mEventInputKeyList.Remove(inputKey);
            }
        }

        protected void addEventMouse(MMouseDevice mouse)
        {
            if (-1 == this.mEventMouseList.IndexOf(mouse))
            {
                this.mEventMouseList.Add(mouse);
            }
        }

        protected void removeEventMouse(MMouseDevice mouse)
        {
            if (-1 != this.mEventMouseList.IndexOf(mouse))
            {
                this.mEventMouseList.Remove(mouse);
            }
        }

        public void ProcessTouches(float delta, TickMode tickMode)
        {
            if (this.hasTouch() || this.hasMultiTouch())
            {
                this.mMultiTouchSet.reset();

                this.mCurrentScheme = MControlScheme.Touch;

                int idx = 0;
                while (idx < UnityEngine.Input.touchCount)
                {
                    UnityEngine.Touch touch = UnityEngine.Input.GetTouch(idx);

                    this.mCurrentTouchId = this.mMultiTouchEnabled ? touch.fingerId : 0;
                    this.mCurrentTouch = MTouchDevice.GetTouch(this.mCurrentTouchId);

                    this.mCurrentTouch.setNativeTouch(touch, this.mCurrentTouchId);
                    this.mCurrentTouch.onTick(delta, tickMode);

                    if (this.hasMultiTouch())
                    {
                        this.mMultiTouchSet.addTouch(this.mCurrentTouch);
                    }

                    ++idx;
                }

                if (this.hasMultiTouch())
                {
                    this.mMultiTouchSet.onTick(delta, tickMode);
                }
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
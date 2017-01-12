using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 主要是场景消息处理， UI 消息单独走
     */
    public class InputMgr : ITickedObject, IDelayHandleItem
    {
        public MControlScheme mCurrentScheme = MControlScheme.Mouse;
        public int mCurrentTouchId = -1;
        public MTouch mCurrentTouch = null;

        // 有监听事件的键盘 InputKey
        protected MList<InputKey> mEventInputKeyList;
        // 有监听事件的鼠标 MMouse
        protected MList<MMouse> mEventMouseList;
        // 是否有重力感应事件
        protected bool mHasAccelerationHandle;

        //private Action mOnAxisDown = null;
        // 单触碰事件分发
        protected AddOnceEventDispatch mOneTouchDispatch;
        // 多触碰事件分发
        protected AddOnceEventDispatch mMultiTouchDispatch;
        // 多触碰集合
        protected MultiTouchSet mMultiTouchSet;
        // 鼠标模拟触碰
        public bool mMultiTouchEnabled;
        public bool mSimulateMouseWithTouches;
        public bool mTouchSupported;

        public InputMgr()
        {
            this.mEventInputKeyList = new MList<InputKey>();
            this.mEventMouseList = new MList<MMouse>();
            this.mHasAccelerationHandle = false;

            this.mOneTouchDispatch = new AddOnceEventDispatch();
            this.mMultiTouchDispatch = new AddOnceEventDispatch();
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
            InputKey.getInputKeyArray();
        }

        public void dispose()
        {

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
        public void onTick(float delta)
        {
            //handleAxis();

            // This function tracks which keys were just pressed (or released) within the last tick.
            // It should be called at the beginning of the tick to give the most accurate responses possible.
            int idx = 0;
            
            for (idx = 0; idx < this.mEventInputKeyList.Count(); idx++)
            {
                this.mEventInputKeyList[idx].onTick(delta);
            }

            for (idx = 0; idx < this.mEventMouseList.Count(); idx++)
            {
                this.mEventMouseList[idx].onTick(delta);
            }

            this.ProcessTouches(delta);

            if(this.mHasAccelerationHandle)
            {
                MAcceleration.mAccelerationOne.onTick(delta);
            }
        }

        public void ProcessTouches(float delta)
        {
            if (Ctx.mInstance.mTouchDispatchSystem.hasTouch() || Ctx.mInstance.mTouchDispatchSystem.hasMultiTouch())
            {
                this.mMultiTouchSet.reset();

                this.mCurrentScheme = MControlScheme.Touch;

                int idx = 0;
                while (idx < Input.touchCount)
                {
                    Touch touch = Input.GetTouch(idx);

                    this.mCurrentTouchId = this.mMultiTouchEnabled ? touch.fingerId : 0;
                    this.mCurrentTouch = MTouch.GetTouch(this.mCurrentTouchId);

                    this.mCurrentTouch.setNativeTouch(touch, this.mCurrentTouchId);
                    this.mCurrentTouch.onTick(delta);

                    if(Ctx.mInstance.mTouchDispatchSystem.hasMultiTouch())
                    {
                        this.mMultiTouchSet.addTouch(this.mCurrentTouch);
                    }

                    ++idx;
                }

                if (Ctx.mInstance.mTouchDispatchSystem.hasMultiTouch())
                {
                    this.mMultiTouchSet.onTick(delta);
                }
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
        public void addMouseListener(MMouse mouse, EventId evtID, MAction<IDispatchObject> handle)
        {
            mouse.addMouseListener(evtID, handle);
            this.addEventMouse(mouse);
        }

        // 移除鼠标监听器
        public void removeMouseListener(MMouse mouse, EventId evtID, MAction<IDispatchObject> handle)
        {
            mouse.removeMouseListener(evtID, handle);

            if (!mouse.hasEventHandle())
            {
                this.removeEventMouse(mouse);
            }
        }

        public void addTouchListener(EventId evtID, MAction<IDispatchObject> handle)
        {
            //MTouch touch = MTouch.GetTouch(1);
            //MTouch touch2 = MTouch.GetTouch(2);

            //touch.addTouchListener(evtID, handle);
            //touch2.addTouchListener(evtID, handle);

            Ctx.mInstance.mTouchDispatchSystem.addTouchListener(evtID, handle);
        }

        public void removeTouchListener(EventId evtID, MAction<IDispatchObject> handle)
        {
            //MTouch touch = MTouch.GetTouch(1);
            //MTouch touch2 = MTouch.GetTouch(2);
            //touch.removeTouchListener(evtID, handle);
            //touch2.removeTouchListener(evtID, handle);
            Ctx.mInstance.mTouchDispatchSystem.removeTouchListener(evtID, handle);
        }

        public void addMultiTouchListener(EventId evtID, MAction<IDispatchObject> handle)
        {
            Ctx.mInstance.mTouchDispatchSystem.addMultiTouchListener(evtID, handle);
        }

        public void removeMultiTouchListener(EventId evtID, MAction<IDispatchObject> handle)
        {
            Ctx.mInstance.mTouchDispatchSystem.removeMultiTouchListener(evtID, handle);
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

        protected void addEventMouse(MMouse mouse)
        {
            if (-1 == this.mEventMouseList.IndexOf(mouse))
            {
                this.mEventMouseList.Add(mouse);
            }
        }

        protected void removeEventMouse(MMouse mouse)
        {
            if (-1 != this.mEventMouseList.IndexOf(mouse))
            {
                this.mEventMouseList.Remove(mouse);
            }
        }
    }
}
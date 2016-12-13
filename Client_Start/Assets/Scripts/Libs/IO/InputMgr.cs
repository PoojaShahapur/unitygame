using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 主要是场景消息处理， UI 消息单独走
     */
    public class InputMgr : ITickedObject, IDelayHandleItem
    {
        public MControlScheme mCurrentScheme;
        public bool mAllowMultiTouch;
        public int mCurrentTouchID;
        public MTouch mCurrentTouch;

        // 有监听事件的键盘 InputKey
        protected MList<InputKey> mEventInputKeyList;
        // 有监听事件的鼠标 MMouse
        protected MList<MMouse> mEventMouseList;
        // 是否有重力感应事件
        protected bool mHasAccelerationHandle;

        //private Action mOnAxisDown = null;
        protected bool mIsEnableIO;     // 是否开启输入

        public InputMgr()
        {
            this.mCurrentScheme = MControlScheme.Mouse;
            this.mAllowMultiTouch = false;
            this.mCurrentTouchID = -1;
            this.mCurrentTouch = null;

            this.mEventInputKeyList = new MList<InputKey>();
            this.mEventMouseList = new MList<MMouse>();
            this.mHasAccelerationHandle = false;

            this.mIsEnableIO = true;
        }

        public void init()
        {
            // 添加事件处理
            Ctx.mInstance.mCamSys.mUiCam = Ctx.mInstance.mLayerMgr.mPath2Go[NotDestroyPath.ND_CV_App].AddComponent<UICamera>();

            InputKey.getInputKeyArray();
        }

        public void dispose()
        {

        }

        public void setClientDispose()
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
            if (this.mIsEnableIO)
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

                if (this.mHasAccelerationHandle)
                {
                    MAcceleration.mAccelerationOne.onTick(delta);
                }
            }
        }

        public void ProcessTouches(float delta)
        {
            this.mCurrentScheme = MControlScheme.Touch;

            for (int i = 0; i < Input.touchCount && i < 1; ++i)
            {
                Touch touch = Input.GetTouch(i);

                this.mCurrentTouchID = this.mAllowMultiTouch ? touch.fingerId : 1;
                this.mCurrentTouch = MTouch.GetTouch(this.mCurrentTouchID);

                this.mCurrentTouch.setNativeTouch(touch);
                this.mCurrentTouch.onTick(delta);
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
            MTouch touch = MTouch.GetTouch(1);

            touch.addTouchListener(evtID, handle);
        }

        public void removeTouchListener(EventId evtID, MAction<IDispatchObject> handle)
        {
            MTouch touch = MTouch.GetTouch(1);
            touch.removeTouchListener(evtID, handle);
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
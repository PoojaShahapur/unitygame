using System;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 主要是场景消息处理， UI 消息单独走
     */
    public class InputMgr : ITickedObject, IDelayHandleItem
    {
        // 有监听事件的键盘 InputKey
        protected MList<InputKey> mEventInputKeyList;

        private Action mOnMouseUp = null;
        private Action mOnMouseDown = null;

        private Action mOnAxisDown = null;

        public InputMgr()
        {
            this.mEventInputKeyList = new MList<InputKey>();
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
            handleAxis();
            handleMouseUp();

            // This function tracks which keys were just pressed (or released) within the last tick.
            // It should be called at the beginning of the tick to give the most accurate responses possible.
            int idx = 0;
            
            for (idx = 0; idx < this.mEventInputKeyList.Count(); idx++)
            {
                this.mEventInputKeyList[idx].onTick(delta);
            }
        }

        protected void handleAxis()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            if (horizontal != 0.0f || vertical != 0.0f)
            {
                if (mOnAxisDown != null)
                {
                    mOnAxisDown();
                }
            }
        }

        public void handleMouseUp()
        {
            if(Input.GetMouseButtonUp(0))
            {
                onMouseUp();
            }
        }

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

        private void onMouseDown()
        {
            if (null != mOnMouseDown)
            {
                mOnMouseDown();
            }
        }

        private void onMouseUp()
        {
            if (null != mOnMouseUp)
            {
                mOnMouseUp();
            }
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
        public void addMouseListener(EventId evtID, Action cb)
        {
            if (EventId.MOUSEDOWN_EVENT == evtID)
            {
                mOnMouseDown += cb;
            }
            else if (EventId.MOUSEUP_EVENT == evtID)
            {
                mOnMouseUp += cb;
            }
        }

        // 移除鼠标监听器
        public void removeMouseListener(EventId evtID, Action cb)
        {
            if (EventId.MOUSEDOWN_EVENT == evtID)
            {
                mOnMouseDown -= cb;
            }
            else if (EventId.MOUSEUP_EVENT == evtID)
            {
                mOnMouseUp -= cb;
            }
        }

        public void addAxisListener(EventId evtID, Action cb)
        {
            mOnAxisDown += cb;
        }

        public void removeAxisListener(EventId evtID, Action cb)
        {
            mOnAxisDown -= cb;
        }

        public void addEventInputKey(InputKey inputKey)
        {
            if(-1 == this.mEventInputKeyList.IndexOf(inputKey))
            {
                this.mEventInputKeyList.Add(inputKey);
            }
        }

        public void removeEventInputKey(InputKey inputKey)
        {
            if (-1 != this.mEventInputKeyList.IndexOf(inputKey))
            {
                this.mEventInputKeyList.Remove(inputKey);
            }
        }
    }
}
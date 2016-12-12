using System;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 主要是场景消息处理， UI 消息单独走
     */
    public class InputMgr : ITickedObject, IDelayHandleItem
    {
        private AddOnceEventDispatch mOnKeyUp = null;
        private AddOnceEventDispatch mOnKeyDown = null;
        private AddOnceEventDispatch mOnKeyPress = null;

        private Action mOnMouseUp = null;
        private Action mOnMouseDown = null;

        private Action mOnAxisDown = null;

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
            handleKeyDown();
            handleKeyUp();
            handleKeyPress();
            handleAxis();
            handleMouseUp();

            // This function tracks which keys were just pressed (or released) within the last tick.
            // It should be called at the beginning of the tick to give the most accurate responses possible.
            int idx = 0;
            
            for (idx = 0; idx < InputKey.mInputKeyArray.Length; idx++)
            {
                InputKey.mInputKeyArray[idx].onTick(delta);
            }
        }

        // 按下和起一定要对称
        protected void handleKeyDown()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                onKeyDown(KeyCode.M);
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                onKeyDown(KeyCode.K);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                onKeyDown(KeyCode.W);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                onKeyDown(KeyCode.A);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                onKeyDown(KeyCode.S);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                onKeyDown(KeyCode.D);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                onKeyDown(KeyCode.UpArrow);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                onKeyDown(KeyCode.DownArrow);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                onKeyDown(KeyCode.RightArrow);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                onKeyDown(KeyCode.LeftArrow);
            }

            /*
            if(Input.anyKeyDown)
            {
                // Event.current 为什么一直是 null
                if (Event.current != null && Event.current.isKey)
                {
                    onKeyDown(Event.current.keyCode);
                }
            }
            */
        }

        protected void handleKeyUp()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                onKeyUp(KeyCode.Escape);
            }
            else if (Input.GetKeyUp(KeyCode.M))
            {
                onKeyUp(KeyCode.M);
            }
            else if (Input.GetKeyUp(KeyCode.K))
            {
                onKeyUp(KeyCode.K);
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                onKeyUp(KeyCode.W);
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                onKeyUp(KeyCode.A);
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                onKeyUp(KeyCode.S);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                onKeyUp(KeyCode.D);
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                onKeyUp(KeyCode.UpArrow);
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                onKeyUp(KeyCode.DownArrow);
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                onKeyUp(KeyCode.RightArrow);
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                onKeyUp(KeyCode.LeftArrow);
            }
        }

        public void handleKeyPress()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                onKeyPress(KeyCode.Escape);
            }
            else if (Input.GetKey(KeyCode.M))
            {
                onKeyPress(KeyCode.M);
            }
            else if (Input.GetKey(KeyCode.K))
            {
                onKeyPress(KeyCode.K);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                onKeyPress(KeyCode.W);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                onKeyPress(KeyCode.A);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                onKeyPress(KeyCode.S);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                onKeyPress(KeyCode.D);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                onKeyPress(KeyCode.UpArrow);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                onKeyPress(KeyCode.DownArrow);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                onKeyPress(KeyCode.RightArrow);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                onKeyPress(KeyCode.LeftArrow);
            }

            /*
            if(Input.anyKey)
            {
                if (Event.current != null && Event.current.isKey)
                {
                    onKeyPress(Event.current.keyCode);
                }
            }
            */
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
        public bool keyJustPressed(int keyCode)
        {
            return InputKey.mInputKeyArray[keyCode].mJustPressed;
        }
        
        /**
         * Returns whether or not a key was released since the last tick.
         */
        public bool keyJustReleased(int keyCode)
        {
            return InputKey.mInputKeyArray[keyCode].mJustReleased;
        }

        /**
         * Returns whether or not a specific key is down.
         */
        public bool isKeyDown(int keyCode)
        {
            return InputKey.mInputKeyArray[keyCode].mKeyState;
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

        private void onKeyDown(KeyCode keyCode)
        {
            if (InputKey.mInputKeyArray[(int)keyCode].mKeyState)
            {
                return;
            }

            InputKey.mInputKeyArray[(int)keyCode].mKeyState = true;
            if (null != mOnKeyDown)
            {
                mOnKeyDown.dispatchEvent(InputKey.mInputKeyArray[(int)keyCode]);
            }
        }

        private void onKeyUp(KeyCode keyCode)
        {
            InputKey.mInputKeyArray[(int)keyCode].mKeyState = false;
            if (null != mOnKeyUp)
            {
                mOnKeyUp.dispatchEvent(InputKey.mInputKeyArray[(int)keyCode]);
            }
        }

        private void onKeyPress(KeyCode keyCode)
        {
            if (null != mOnKeyPress)
            {
                mOnKeyPress.dispatchEvent(InputKey.mInputKeyArray[(int)keyCode]);
            }
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

        public void addKeyListener(EventID evtID, MAction<IDispatchObject> handle)
        {
            if (EventID.KEYUP_EVENT == evtID)
            {
                mOnKeyUp.addEventHandle(null, handle);
            }
            else if (EventID.KEYDOWN_EVENT == evtID)
            {
                mOnKeyDown.addEventHandle(null, handle);
            }
            else if (EventID.KEYPRESS_EVENT == evtID)
            {
                mOnKeyPress.addEventHandle(null, handle);
            }
        }

        public void removeKeyListener(EventID evtID, MAction<IDispatchObject> handle)
        {
            if (EventID.KEYUP_EVENT == evtID)
            {
                mOnKeyUp.removeEventHandle(null, handle);
            }
            else if (EventID.KEYDOWN_EVENT == evtID)
            {
                mOnKeyDown.removeEventHandle(null, handle);
            }
            else if (EventID.KEYPRESS_EVENT == evtID)
            {
                mOnKeyPress.removeEventHandle(null, handle);
            }
        }

        public void addMouseListener(EventID evtID, Action cb)
        {
            if (EventID.MOUSEDOWN_EVENT == evtID)
            {
                mOnMouseDown += cb;
            }
            else if (EventID.MOUSEUP_EVENT == evtID)
            {
                mOnMouseUp += cb;
            }
        }

        public void removeMouseListener(EventID evtID, Action cb)
        {
            if (EventID.MOUSEDOWN_EVENT == evtID)
            {
                mOnMouseDown -= cb;
            }
            else if (EventID.MOUSEUP_EVENT == evtID)
            {
                mOnMouseUp -= cb;
            }
        }

        public void addAxisListener(EventID evtID, Action cb)
        {
            mOnAxisDown += cb;
        }

        public void removeAxisListener(EventID evtID, Action cb)
        {
            mOnAxisDown -= cb;
        }
    }
}
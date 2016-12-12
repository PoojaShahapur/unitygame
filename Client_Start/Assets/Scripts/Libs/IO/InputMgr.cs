using System;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 主要是场景消息处理， UI 消息单独走
     */
    public class InputMgr : ITickedObject, IDelayHandleItem
    {
        Action<KeyCode> mOnKeyUp = null;
        Action<KeyCode> mOnKeyDown = null;
        Action<KeyCode> mOnKeyPress = null;
        
        Action mOnMouseUp = null;
        Action mOnMouseDown = null;

        Action mOnAxisDown = null;

        private bool[] mKeyState = new bool[(int)KeyCode.Joystick8Button19 + 1];     // The most recent information on key states
        private bool[] mKeyStateOld = new bool[(int)KeyCode.Joystick8Button19 + 1];  // The state of the keys on the previous tick
        private bool[] mJustPressed = new bool[(int)KeyCode.Joystick8Button19 + 1];  // An array of keys that were just pressed within the last tick.
        private bool[] mJustReleased = new bool[(int)KeyCode.Joystick8Button19 + 1]; // An array of keys that were just released within the last tick.

        public void init()
        {
            // 添加事件处理
            Ctx.mInstance.mCamSys.mUiCam = Ctx.mInstance.mLayerMgr.m_path2Go[NotDestroyPath.ND_CV_App].AddComponent<UICamera>();
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
        public void onTick(float deltaTime)
        {
            handleKeyDown();
            handleKeyUp();
            handleKeyPress();
            handleAxis();
            handleMouseUp();

            // This function tracks which keys were just pressed (or released) within the last tick.
            // It should be called at the beginning of the tick to give the most accurate responses possible.
            int cnt;
            
            for (cnt = 0; cnt < mKeyState.Length; cnt++)
            {
                if (mKeyState[cnt] && !mKeyStateOld[cnt])
                    mJustPressed[cnt] = true;
                else
                    mJustPressed[cnt] = false;
                
                if (!mKeyState[cnt] && mKeyStateOld[cnt])
                    mJustReleased[cnt] = true;
                else
                    mJustReleased[cnt] = false;
                
                mKeyStateOld[cnt] = mKeyState[cnt];
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
            return mJustPressed[keyCode];
        }
        
        /**
         * Returns whether or not a key was released since the last tick.
         */
        public bool keyJustReleased(int keyCode)
        {
            return mJustReleased[keyCode];
        }

        /**
         * Returns whether or not a specific key is down.
         */
        public bool isKeyDown(int keyCode)
        {
            return mKeyState[keyCode];
        }
        
        /**
         * Returns true if any key is down.
         */
        public bool isAnyKeyDown()
        {
            foreach (bool b in mKeyState)
            {
                if (b)
                    return true;
            }
            return false;
        }

        private void onKeyDown(KeyCode keyCode)
        {			
            if (mKeyState[(int)keyCode])
                return;

            mKeyState[(int)keyCode] = true;
            if (null != mOnKeyDown)
            {
                mOnKeyDown(keyCode);
            }
        }

        private void onKeyUp(KeyCode keyCode)
        {
		    mKeyState[(int)keyCode] = false;
            if (null != mOnKeyUp)
            {
                mOnKeyUp(keyCode);
            }
        }

        private void onKeyPress(KeyCode keyCode)
        {
            if (null != mOnKeyPress)
            {
                mOnKeyPress(keyCode);
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

        public void addKeyListener(EventID evtID, Action<KeyCode> cb)
        {
            if (EventID.KEYUP_EVENT == evtID)
            {
                mOnKeyUp += cb;
            }
            else if (EventID.KEYDOWN_EVENT == evtID)
            {
                mOnKeyDown += cb;
            }
            else if (EventID.KEYPRESS_EVENT == evtID)
            {
                mOnKeyPress += cb;
            }
        }

        public void removeKeyListener(EventID evtID, Action<KeyCode> cb)
        {
            if (EventID.KEYUP_EVENT == evtID)
            {
                mOnKeyUp -= cb;
            }
            else if (EventID.KEYDOWN_EVENT == evtID)
            {
                mOnKeyDown -= cb;
            }
            else if (EventID.KEYPRESS_EVENT == evtID)
            {
                mOnKeyPress -= cb;
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
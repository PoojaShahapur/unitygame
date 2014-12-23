using System;
using UnityEngine;
using System.Collections.Generic;
using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief 主要是场景消息处理， UI 消息单独走
     */
    class InputMgr : IInputMgr, ITickedObject
    {
        Action<KeyCode> m_onKeyUp = null;
        Action<KeyCode> m_onKeyDown = null;
        Action m_onMouseUp = null;
        Action m_onMouseDown = null;

        Action m_onAxisDown = null;

        private bool[] _keyState = new bool[256];     // The most recent information on key states
        private bool[] _keyStateOld = new bool[256];  // The state of the keys on the previous tick
        private bool[] _justPressed = new bool[256];  // An array of keys that were just pressed within the last tick.
        private bool[] _justReleased = new bool[256]; // An array of keys that were just released within the last tick.

        /**
         * @inheritDoc
         */
        public void OnTick(float deltaTime)
        {
            handleKeyDown();
            handleKeyUp();
            handleAxis();
            handleMouseUp();

            // This function tracks which keys were just pressed (or released) within the last tick.
            // It should be called at the beginning of the tick to give the most accurate responses possible.
            int cnt;
            
            for (cnt = 0; cnt < _keyState.Length; cnt++)
            {
                if (_keyState[cnt] && !_keyStateOld[cnt])
                    _justPressed[cnt] = true;
                else
                    _justPressed[cnt] = false;
                
                if (!_keyState[cnt] && _keyStateOld[cnt])
                    _justReleased[cnt] = true;
                else
                    _justReleased[cnt] = false;
                
                _keyStateOld[cnt] = _keyState[cnt];
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
        }

        protected void handleAxis()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            if (vertical != 0.0f || vertical != 0.0f)
            {
                if (m_onAxisDown != null)
                {
                    m_onAxisDown();
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
            return _justPressed[keyCode];
        }
        
        /**
         * Returns whether or not a key was released since the last tick.
         */
        public bool keyJustReleased(int keyCode)
        {
            return _justReleased[keyCode];
        }

        /**
         * Returns whether or not a specific key is down.
         */
        public bool isKeyDown(int keyCode)
        {
            return _keyState[keyCode];
        }
        
        /**
         * Returns true if any key is down.
         */
        public bool isAnyKeyDown()
        {
            foreach (bool b in _keyState)
            {
                if (b)
                    return true;
            }
            return false;
        }

        private void onKeyDown(KeyCode keyCode)
        {			
            if (_keyState[(int)keyCode])
                return;

            _keyState[(int)keyCode] = true;
            m_onKeyDown(keyCode);
        }

        private void onKeyUp(KeyCode keyCode)
        {
		    _keyState[(int)keyCode] = false;
            m_onKeyUp(keyCode);
        }

        private void onMouseDown()
        {
            m_onMouseDown();
        }

        private void onMouseUp()
        {
            m_onMouseUp();
        }

        public void addKeyListener(EventID evtID, Action<KeyCode> cb)
        {
            if (EventID.KEYUP_EVENT == evtID)
            {
                m_onKeyUp += cb;
            }
            else if (EventID.KEYDOWN_EVENT == evtID)
            {
                m_onKeyDown += cb;
            }
        }

        public void removeKeyListener(EventID evtID, Action<KeyCode> cb)
        {

        }

        public void addMouseListener(EventID evtID, Action cb)
        {
            if (EventID.MOUSEDOWN_EVENT == evtID)
            {
                m_onMouseDown += cb;
            }
            else if (EventID.MOUSEUP_EVENT == evtID)
            {
                m_onMouseUp += cb;
            }
        }

        public void removeMouseListener(EventID evtID, Action cb)
        {

        }

        public void addAxisListener(EventID evtID, Action cb)
        {
            m_onAxisDown += cb;
        }

        public void removeAxisListener(EventID evtID, Action cb)
        {

        }
    }
}
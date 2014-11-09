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
        public delegate bool onKeyUpCB(KeyCode value);
        onKeyUpCB m_onKeyUp = null;

        public delegate bool onKeyDownCB(KeyCode value);
        onKeyDownCB m_onKeyDown = null;

        public delegate bool onMouseUpCB();
        onMouseUpCB m_onMouseUp = null;

        public delegate bool onMouseDownCB();
        onMouseDownCB m_onMouseDown = null;

        private List<bool> _keyState = new List<bool>();     // The most recent information on key states
        private List<bool> _keyStateOld = new List<bool>();  // The state of the keys on the previous tick
        private List<bool> _justPressed = new List<bool>();  // An array of keys that were just pressed within the last tick.
        private List<bool> _justReleased = new List<bool>(); // An array of keys that were just released within the last tick.

        public onKeyUpCB getOnKeyUp()
        {
            return m_onKeyUp;
        }

        /**
         * @inheritDoc
         */
        public void OnTick(float deltaTime)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                onKeyUp(KeyCode.Escape);
            }

            // This function tracks which keys were just pressed (or released) within the last tick.
            // It should be called at the beginning of the tick to give the most accurate responses possible.
            int cnt;
            
            for (cnt = 0; cnt < _keyState.Count; cnt++)
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
    }
}
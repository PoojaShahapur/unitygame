using System;
using UnityEngine;
using System.Collections.Generic;
using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief 主要是场景消息处理， UI 消息单独走
     */
    class InputMgr : IInputMgr
    {
        public delegate bool onKeyUp(KeyCode value);
        onKeyUp m_onKeyUp = null;

        public void handleKeyBoard()
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                m_onKeyUp(KeyCode.Escape);
            }
        }

        public onKeyUp getOnKeyUp()
        {
            return m_onKeyUp;
        }
    }
}
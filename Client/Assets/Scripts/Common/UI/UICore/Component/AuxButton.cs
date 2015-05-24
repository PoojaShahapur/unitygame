using SDK.Lib;
using System;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 辅助 Button
     */
    public class AuxButton : AuxComponent
    {
        protected EventDispatch m_eventDisp = new EventDispatch();      // 分发

        // 点击回调
        protected void OnBtnClk()
        {
            m_eventDisp.dispatchEvent(this);
        }
    }
}
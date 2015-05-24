using SDK.Lib;
using System;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief ���� Button
     */
    public class AuxButton : AuxComponent
    {
        protected EventDispatch m_eventDisp = new EventDispatch();      // �ַ�

        // ����ص�
        protected void OnBtnClk()
        {
            m_eventDisp.dispatchEvent(this);
        }
    }
}
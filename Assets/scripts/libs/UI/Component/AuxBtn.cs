using System;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief ���� Button
     */
    public class AuxBtn : AuxWin
    {
        protected Action<GameObject> m_dispHandle;      // �ַ�

        // ����ص�
        protected void OnBtnClk(GameObject target)
        {
            if(m_dispHandle != null)
            {
                m_dispHandle(target);
            }
        }
    }
}
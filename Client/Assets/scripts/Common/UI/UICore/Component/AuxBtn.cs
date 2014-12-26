using System;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief ���� Button
     */
    public class AuxBtn : AuxComponent
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
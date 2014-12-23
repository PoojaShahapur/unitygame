using System;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 辅助 Button
     */
    public class AuxBtn : AuxWin
    {
        protected Action<GameObject> m_dispHandle;      // 分发

        // 点击回调
        protected void OnBtnClk(GameObject target)
        {
            if(m_dispHandle != null)
            {
                m_dispHandle(target);
            }
        }
    }
}
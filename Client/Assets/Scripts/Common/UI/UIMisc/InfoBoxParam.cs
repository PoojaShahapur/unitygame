using System;

namespace SDK.Common
{
    // 点击信息返回
    public enum InfoBoxBtnType
    {
        eBTN_OK,                // OK 按钮
        eBTN_CANCEL,            // cancel 按钮
    }

    /**
     * @brief 信息提示框参数
     */
    public class InfoBoxParam : IRecycle
    {
        public UIFormID m_formID = UIFormID.eUIInfo;       // form 的 id
        public string m_midDesc;        // 中间提示信息
        public string m_btnOkCap;       // OK 按钮标题
        public Action<InfoBoxBtnType> m_btnClkDisp;     // 按钮点击回调

        public void resetDefault()
        {
            m_formID = UIFormID.eUIInfo;
            m_midDesc = "";
            m_btnOkCap = "";
            m_btnClkDisp = null;
        }
    }
}
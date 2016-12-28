using System;

namespace SDK.Lib
{
    // 信息模式类型
    public enum InfoBoxModeType
    {
        eMode_1,
        eMode_2,
    }

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
        public UIFormId m_formID = UIFormId.eUITest;       // form 的 id
        public InfoBoxModeType m_infoBoxModeType = InfoBoxModeType.eMode_2;
        // 模式一
        public string m_inputTips;        // 输入框默认信息
        // 模式二
        public string m_midDesc;        // 中间提示信息

        public Action<InfoBoxBtnType> m_btnClkDisp;     // 按钮点击回调

        public void resetDefault()
        {
            m_infoBoxModeType = InfoBoxModeType.eMode_2;
            //m_formID = UIFormId.eUIInfo;
            m_midDesc = "";
            m_btnClkDisp = null;
        }
    }
}
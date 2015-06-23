using Game.Msg;
using SDK.Common;
using SDK.Lib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIGM : Form, IUIGM
    {
        public AuxInputField m_inputField;     // 输入

        public override void onReady()
        {
            base.onReady();
            findWidget();
            addEventHandle();
        }

        public override void onShow()
        {
            base.onShow();
        }

        protected void findWidget()
        {
            m_inputField = new AuxInputField(m_GUIWin.m_uiRoot, "InputField");
        }

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, "BtnSend", onBtnSendClk);
        }

        protected void onBtnSendClk()
        {
            stKokChatUserCmd cmd = new stKokChatUserCmd();
            cmd.dwType = 2;
            cmd.pstrName = Ctx.m_instance.m_dataPlayer.m_dataMain.m_name;
            cmd.pstrChat = m_inputField.text;
            UtilMsg.sendMsg(cmd);
            m_inputField.text = "";              // 清空内容
        }
    }
}
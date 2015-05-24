using Game.Msg;
using SDK.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /**
     * @brief 聊天
     */
    public class UIChat : Form
    {
        protected ChatData m_chatData = new ChatData();

        public override void onInit()
        {
            exitMode = false;         // 直接隐藏
            base.onInit();
        }

        override public void onShow()
        {
            base.onShow();
            if(Ctx.m_instance.m_dataPlayer.m_chatData.getStr().Length > 0)
            {
                outMsg(Ctx.m_instance.m_dataPlayer.m_chatData.getStr());
            }
        }
        
        // 初始化控件
        override public void onReady()
        {
            base.onReady();
            findWidget();
            addEventHandle();
        }

        // 每一次隐藏都会调用一次
        override public void onHide()
		{
            base.onHide();
		}

        // 每一次关闭都会调用一次
        override public void onExit()
        {
            base.onExit();
        }

        protected void findWidget()
        {
            m_chatData.m_logText = new AuxLabel(m_GUIWin.m_uiRoot, CVChat.TextOutput);
            m_chatData.m_inputField = UtilApi.getComByP<InputField>(m_GUIWin.m_uiRoot, CVChat.Input);
            m_chatData.m_scrollbar = UtilApi.getComByP<Scrollbar>(m_GUIWin.m_uiRoot, CVChat.Scrollbar);
        }

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, CVChat.SendBtn, onSendBtnClk);
        }

        protected void onSendBtnClk()
        {
            stKokChatUserCmd cmd = new stKokChatUserCmd();
            cmd.dwType = 2;
            cmd.pstrName = Ctx.m_instance.m_dataPlayer.m_dataMain.m_name;
            cmd.pstrChat = m_chatData.m_inputField.text;
            UtilMsg.sendMsg(cmd);

            outMsg(m_chatData.m_inputField.text);
            m_chatData.m_inputField.text = "";              // 清空内容
        }

        public void outMsg(string str)
        {
            m_chatData.m_logText.text += str;
            m_chatData.m_logText.text += "\n";

            // 改变大小
            //m_chatData.m_logText.rectTransform.sizeDelta = new Vector2(m_chatData.m_logText.rectTransform.sizeDelta.x, m_chatData.m_logText.preferredHeight);
            m_chatData.m_logText.changeSize();

            // 滚动条滚动到底部
            m_chatData.m_scrollbar.value = 0;
        }
    }
}
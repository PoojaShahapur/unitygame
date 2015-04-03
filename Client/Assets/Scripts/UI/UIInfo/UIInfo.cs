using SDK.Common;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIInfo : Form
    {
        protected Text m_textDesc;

        override public void onShow()
        {
            exitMode = false;
        }

        // 初始化控件
        override public void onReady()
        {
            getWidget();
            addEventHandle();
        }

        // 关联窗口
        protected void getWidget()
        {
            m_textDesc = UtilApi.getComByP<Text>(m_GUIWin.m_uiRoot, InfoComPath.PathTextDesc);
        }

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, InfoComPath.PathBtnOk, onBtnClkOk);
        }

        // 点击登陆处理
        protected void onBtnClkOk()
        {
            exit();
        }

        public Text textDesc
        {
            get
            {
                return m_textDesc;
            }
            set
            {
                m_textDesc = value;
            }
        }

        public static void showMsg(string str)
        {
            Ctx.m_instance.m_uiMgr.loadAndShow<UIInfo>(UIFormID.UIInfo);
            Ctx.m_instance.m_uiMgr.getForm<UIInfo>(UIFormID.UIInfo).m_textDesc.text = str;
        }
    }
}
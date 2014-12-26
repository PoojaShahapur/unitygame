using Game.Msg;
using SDK.Common;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIHeroSelect : Form, IUIHeroSelect
    {
        protected bool m_bLogined = false;      // 是否登陆过

        override public void onShow()
        {

        }

        // 初始化控件
        override public void onReady()
        {
            addEventHandle();
        }

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, LoginComPath.PathBtnLogin, onBtnClkLogin);
        }

        // 点击登陆处理
        protected void onBtnClkLogin()
        {
            if (!m_bLogined)
            {
                m_bLogined = true;
                InputField lblName = UtilApi.getComByP<InputField>(m_GUIWin.m_uiRoot, LoginComPath.PathLblName);

                stCreateSelectUserCmd cmd = new stCreateSelectUserCmd();
                cmd.strUserName = lblName.text;
                cmd.country = 1;
                UtilMsg.sendMsg(cmd);
            }
        }
    }
}
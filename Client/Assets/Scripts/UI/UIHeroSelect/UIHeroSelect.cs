using Game.Msg;
using SDK.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIHeroSelect : Form
    {
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
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, HeroSelectComPath.PathBtnLogin, onBtnClkLogin);
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, HeroSelectComPath.PathBtnRanName, onBtnClkRan);
        }

        // 点击登陆处理
        protected void onBtnClkLogin()
        {
            if (Ctx.m_instance.m_loginSys.get_LoginState() == LoginState.eLoginSuccessGateServer || Ctx.m_instance.m_loginSys.get_LoginState() == LoginState.eLoginNewCharError)    // 网关登陆成功或者建立角色错误
            {
                InputField lblName = UtilApi.getComByP<InputField>(m_GUIWin.m_uiRoot, LoginComPath.PathLblName);

                stCreateSelectUserCmd cmd = new stCreateSelectUserCmd();
                cmd.strUserName = lblName.text;
                cmd.country = 1;
                UtilMsg.sendMsg(cmd);
            }
        }

        // 点击随机
        protected void onBtnClkRan()
        {
            int aaa = UtilApi.Range(1, 200);
        }
    }
}
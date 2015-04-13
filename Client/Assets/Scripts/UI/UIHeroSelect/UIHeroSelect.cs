using Game.Msg;
using SDK.Common;
using SDK.Lib;
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

                if (lblName.text.Length == 0)       // 如果没有输入名字
                {
                    // 给出一个提示
                    Ctx.m_instance.m_langMgr.getText(LangTypeId.eSelectHero, (int)LangLogID.eItem1);
                    InfoBoxParam param = Ctx.m_instance.m_poolSys.newObject<InfoBoxParam>();
                    param.m_midDesc = Ctx.m_instance.m_shareData.m_retLangStr;
                    Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog, (int)LangLogID.eItem22);
                    param.m_btnOkCap = Ctx.m_instance.m_shareData.m_retLangStr;
                    UIInfo.showMsg(param);
                }
                else if (Ctx.m_instance.m_wordFilterManager.IsMatch(lblName.text))       // 如果包含非法字符
                {
                    // 给出一个提示
                    Ctx.m_instance.m_langMgr.getText(LangTypeId.eSelectHero, (int)LangLogID.eItem2);
                    InfoBoxParam param = Ctx.m_instance.m_poolSys.newObject<InfoBoxParam>();
                    param.m_midDesc = Ctx.m_instance.m_shareData.m_retLangStr;
                    UIInfo.showMsg(param);
                }
                else
                {
                    // 判断名字长度
                    if (UtilApi.CalcCharCount(lblName.text) <= CVMsg.MAX_NAMESIZE)
                    {
                        stCreateSelectUserCmd cmd = new stCreateSelectUserCmd();
                        cmd.strUserName = lblName.text;
                        cmd.country = 1;
                        UtilMsg.sendMsg(cmd);
                    }
                    else
                    {
                        // 给出一个提示
                        Ctx.m_instance.m_langMgr.getText(LangTypeId.eSelectHero, (int)LangLogID.eItem0);
                        InfoBoxParam param = Ctx.m_instance.m_poolSys.newObject<InfoBoxParam>();
                        param.m_midDesc = Ctx.m_instance.m_shareData.m_retLangStr;
                        Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog, (int)LangLogID.eItem22);
                        param.m_btnOkCap = Ctx.m_instance.m_shareData.m_retLangStr;
                        UIInfo.showMsg(param);
                    }
                }
            }
        }

        // 点击随机
        protected void onBtnClkRan()
        {
            string name = Ctx.m_instance.m_pRandName.getRandName();
            InputField lblName = UtilApi.getComByP<InputField>(m_GUIWin.m_uiRoot, LoginComPath.PathLblName);
            lblName.text = name;
        }
    }
}
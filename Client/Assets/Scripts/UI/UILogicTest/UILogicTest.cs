using SDK.Common;
using SDK.Lib;
using System.IO;
using UnityEngine.UI;
namespace Game.UI
{
    public class UILogicTest : Form
    {
        override public void onShow()
        {

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
            
        }

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, LogicTestComPath.PathButton, onBtnClkOk);
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, LogicTestComPath.PathButton2, onBtnClkOk2);
        }

        // 点击登陆处理
        protected void onBtnClkOk()
        {
            //testUIInfo();
            testAudio();
        }

        protected void onBtnClkOk2()
        {
            testClostAudio();
        }

        protected void testUIInfo()
        {
            InfoBoxParam param = Ctx.m_instance.m_poolSys.newObject<InfoBoxParam>();
            param.m_midDesc = "aaaaaa";
            Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog0, (int)LangItemID.eItem22);
            param.m_btnOkCap = Ctx.m_instance.m_shareData.m_retLangStr;
            UIInfo.showMsg(param);
        }

        // 测试音效
        protected void testAudio()
        {
            SoundParam param = Ctx.m_instance.m_poolSys.newObject<SoundParam>();
            param.m_path = "TestSound.prefab";
            //param.m_path = "ZuiZhenDeMeng.mp3";
            Ctx.m_instance.m_soundMgr.play(param);
            Ctx.m_instance.m_poolSys.deleteObj(param);
        }

        protected void testClostAudio()
        {
            Ctx.m_instance.m_soundMgr.stop("TestSound.prefab");
            //Ctx.m_instance.m_soundMgr.stop("ZuiZhenDeMeng.mp3");
        }
    }
}
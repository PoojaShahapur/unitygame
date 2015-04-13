using SDK.Common;
using SDK.Lib;
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
        }

        // 点击登陆处理
        protected void onBtnClkOk()
        {
            testUIInfo();
        }

        protected void testUIInfo()
        {
            InfoBoxParam param = Ctx.m_instance.m_poolSys.newObject<InfoBoxParam>();
            param.m_midDesc = "aaaaaa";
            Ctx.m_instance.m_langMgr.getText(LangTypeId.eLTLog, (int)LangLogID.eItem22);
            param.m_btnOkCap = Ctx.m_instance.m_shareData.m_retLangStr;
            UIInfo.showMsg(param);
        }
    }
}
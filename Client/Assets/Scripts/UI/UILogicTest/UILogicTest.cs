using SDK.Common;
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
            UIInfo.showMsg("aaaaaa");
        }
    }
}
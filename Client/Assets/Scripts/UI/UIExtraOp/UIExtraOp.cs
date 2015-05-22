using Game.Msg;
using SDK.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /**
     * @brief 额外操作
     */
    public class UIExtraOp : Form
    {
        protected ExtraOpData m_dzData = new ExtraOpData();

        override public void onShow()
        {
            base.onShow();
        }

        override public void onReady()
        {
            base.onReady();
            findWidget();
            addEventHandle();
        }

        protected void findWidget()
        {
        }

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, ExtraOpComPath.BtnQuitDZ, onQuitDZBtnClk);
        }

        protected void onQuitDZBtnClk()
        {
            stReqGiveUpOneBattleUserCmd cmd = new stReqGiveUpOneBattleUserCmd();
            UtilMsg.sendMsg(cmd);

//#if UNITY_EDITOR
//            UnityEditor.EditorApplication.isPlaying = false;
//#else
//            Application.Quit();
//#endif
        }
    }
}
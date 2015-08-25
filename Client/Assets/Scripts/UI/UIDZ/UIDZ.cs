﻿using Game.Msg;
using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 对战的时候显示的一些基本信息
     */
    public class UIDZ : Form, IUIDZ
    {
        protected DZData m_dzData = new DZData();

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
            m_dzData.m_lblArr[(int)EnDZLbl.eLblSelfName] = new AuxLabel(m_GUIWin.m_uiRoot, DZComPath.SelfName);
            m_dzData.m_lblArr[(int)EnDZLbl.eLblEnemyName] = new AuxLabel(m_GUIWin.m_uiRoot, DZComPath.EnemyName);
        }

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, DZComPath.BtnOp, onOpBtnClk);
        }

        protected void onOpBtnClk()
        {
            Ctx.m_instance.m_uiMgr.loadAndShow(UIFormID.eUIExtraOp);
        }

        // self 和 enemy hero 的名字显示
        public void psstNotifyFightEnemyInfoUserCmd()
        {
            m_dzData.m_lblArr[(int)EnDZLbl.eLblSelfName].text = Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_heroName;
            m_dzData.m_lblArr[(int)EnDZLbl.eLblEnemyName].text = Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerEnemy].m_heroName;
        }
    }
}
﻿using SDK.Common;
using SDK.Lib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public enum LeftBtnPnl_BtnIndex
    {
        eBtnJob0f,
        eBtnJob1f,
        eBtnJob2f,
        eBtnJob3f,

        eBtnJobTotal,
    }

    /**
     * @brief 左边按钮面板
     */
    public class LeftBtnPnl : TuJianPnlBase
    {
        protected AuxDynImageStaticGOImage m_jobBtnImage;

        public LeftBtnPnl(TuJianData data) :
            base(data)
        {
            m_jobBtnImage = new AuxDynImageStaticGOImage();
        }

        public new void findWidget()
        {
            m_jobBtnImage.selfGo = UtilApi.TransFindChildByPObjAndPath(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.BtnJob);
        }

        public new void addEventHandle()
        {
            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.BtnJob, onJobBtnClk);
            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.BtnFilter, onFilterBtnClk);
            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.BtnShouCang, onShouCangBtnClk);
        }

        public void onJobBtnClk()
        {
            Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu = ETuJianMenu.eJobSel;
            Ctx.m_instance.m_uiMgr.loadAndShow<UITuJianTop>(UIFormID.eUITuJianTop);
        }

        public void onFilterBtnClk()
        {
            Ctx.m_instance.m_auxUIHelp.m_auxTuJian.m_eTuJianMenu = ETuJianMenu.eFilter;
            Ctx.m_instance.m_uiMgr.loadAndShow<UITuJianTop>(UIFormID.eUITuJianTop);
        }

        public void onShouCangBtnClk()
        {

        }

        // 修改图标
        public void toggleJobBtn(int idx)
        {
            TableJobItemBody jobBody = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_JOB, (uint)idx).m_itemBody as TableJobItemBody;
            m_jobBtnImage.setImageInfo(CVAtlasName.TuJianDyn, jobBody.m_jobBtnRes);
            m_jobBtnImage.syncUpdateCom();
        }

        public void updateByCareer(int idx)
        {
            if (!m_tuJianData.m_pClassFilterPnl.bCurUpBtn(idx))            // 如果已经点击按钮，不要再点击了
            {
                toggleJobBtn(idx);
                if ((EnPlayerCareer)idx < EnPlayerCareer.ePCTotal)
                {
                    m_tuJianData.m_pClassFilterPnl.updateByCareer((EnPlayerCareer)idx, false);
                }
            }
        }
    }
}
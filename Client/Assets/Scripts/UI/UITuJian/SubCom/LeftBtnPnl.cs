using SDK.Common;
using System;
using System.Collections.Generic;
using System.Linq;
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
        protected GameObject m_jobPnlGo;
        protected GameObject m_filterPnlGo;

        protected AuxStaticImageStaticGoImage m_jobBtnImage;
        protected AuxBasicButton[] m_btnArr = new AuxBasicButton[(int)LeftBtnPnl_BtnIndex.eBtnJobTotal];

        public LeftBtnPnl(TuJianData data) :
            base(data)
        {
            
        }

        public new void findWidget()
        {
            m_jobPnlGo = UtilApi.TransFindChildByPObjAndPath(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.PnlJob);
            m_filterPnlGo = UtilApi.TransFindChildByPObjAndPath(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.PnlFilter);

            m_jobBtnImage = new AuxStaticImageStaticGoImage(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.BtnJob0f);

            m_btnArr[(int)LeftBtnPnl_BtnIndex.eBtnJob0f] = new AuxBasicButton(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.BtnJob0f);
            m_btnArr[(int)LeftBtnPnl_BtnIndex.eBtnJob1f] = new AuxBasicButton(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.BtnJob1f);
            m_btnArr[(int)LeftBtnPnl_BtnIndex.eBtnJob2f] = new AuxBasicButton(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.BtnJob2f);
            m_btnArr[(int)LeftBtnPnl_BtnIndex.eBtnJob3f] = new AuxBasicButton(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.BtnJob3f);

            toggleJobPnl(false);
            toggleFilterPnl(false);
        }

        public new void addEventHandle()
        {
            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.BtnJob0f, onJobBtnClk);
            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.BtnJob1f, onJobBtnClk);
            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.BtnJob2f, onJobBtnClk);
            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.BtnJob3f, onJobBtnClk);

            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.BtnFilter, onFilterBtnClk);
            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.BtnShouCang, onShouCangBtnClk);

            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.Job0f, onJobTypeBtnClk);
            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.Job1f, onJobTypeBtnClk);
            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.Job2f, onJobTypeBtnClk);
            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.Job3f, onJobTypeBtnClk);

            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.Filter0f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.Filter1f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.Filter2f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.Filter3f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.Filter4f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.Filter5f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.Filter6f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.Filter7f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_tuJianData.m_form.m_GUIWin.m_uiRoot, TuJianPath.Filter8f, onFilterTypeBtnClk);
        }

        public void hideAllJobBtn()
        {
            for(int idx = 0; idx < (int)LeftBtnPnl_BtnIndex.eBtnJobTotal; ++idx)
            {
                m_btnArr[idx].hide();
            }
        }

        public void onJobBtnClk()
        {
            toggleJobPnl(!m_jobPnlGo.activeSelf);
        }

        public void onFilterBtnClk()
        {
            toggleFilterPnl(!m_filterPnlGo.activeSelf);
        }

        public void onShouCangBtnClk()
        {

        }

        public void onJobTypeBtnClk()
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                int idx = UtilApi.findIdxByUnderline(EventSystem.current.currentSelectedGameObject.name);
                updateByCareer(idx);
            }
        }

        public void updateByCareer(int idx)
        {
            toggleJobPnl(false);

            if (!m_tuJianData.m_pClassFilterPnl.bCurUpBtn(idx))            // 如果已经点击按钮，不要再点击了
            {
                toggleJobBtn(idx);
                if ((EnPlayerCareer)idx < EnPlayerCareer.ePCTotal)
                {
                    m_tuJianData.m_pClassFilterPnl.updateByCareer((EnPlayerCareer)idx, false);
                }
            }
        }

        public void toggleJobBtn(int idx)
        {
            if (m_tuJianData.m_pClassFilterPnl.m_tabBtnIdx >= 0)
            {
                if (m_tuJianData.m_pClassFilterPnl.m_tabBtnIdx < (int)LeftBtnPnl_BtnIndex.eBtnJobTotal)
                {
                    m_btnArr[m_tuJianData.m_pClassFilterPnl.m_tabBtnIdx].hide();
                }
                else
                {
                    Debug.Log("数组越界");
                }
            }
            m_btnArr[idx].show();
        }

        protected void onFilterTypeBtnClk()
        {
            toggleFilterPnl(false);
            int idx = UtilApi.findIdxByUnderline(EventSystem.current.currentSelectedGameObject.name);
            m_tuJianData.m_wdscCardPnl.filterMp = idx;
        }

        protected void toggleJobPnl(bool bShow)
        {
            m_jobPnlGo.SetActive(bShow);
        }

        protected void toggleFilterPnl(bool bShow)
        {
            m_filterPnlGo.SetActive(bShow);
        }
    }
}
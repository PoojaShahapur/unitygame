using SDK.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    public enum LeftBtnPnl_BtnIndex
    {
        eBtnJob0f,
        eBtnJob1f,
        eBtnJob2f,
        eBtnJob3f,
        eBtnJob4f,

        eBtnJobTotal,
    }

    /**
     * @brief 左边按钮面板
     */
    public class LeftBtnPnl
    {
        public SceneWDSCData m_sceneWDSCData;

        protected GameObject m_jobPnlGo;
        protected GameObject m_filterPnlGo;

        protected Image m_jobBtnImage;
        protected Button[] m_btnArr = new Button[(int)LeftBtnPnl_BtnIndex.eBtnJobTotal];

        public void findWidget()
        {
            m_jobPnlGo = UtilApi.TransFindChildByPObjAndPath(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.PnlJob);
            m_filterPnlGo = UtilApi.TransFindChildByPObjAndPath(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.PnlFilter);

            m_jobBtnImage = UtilApi.getComByP<Image>(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.BtnJob0f);

            m_btnArr[(int)LeftBtnPnl_BtnIndex.eBtnJob0f] = UtilApi.getComByP<Button>(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.BtnJob0f);
            m_btnArr[(int)LeftBtnPnl_BtnIndex.eBtnJob1f] = UtilApi.getComByP<Button>(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.BtnJob1f);
            m_btnArr[(int)LeftBtnPnl_BtnIndex.eBtnJob2f] = UtilApi.getComByP<Button>(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.BtnJob2f);
            m_btnArr[(int)LeftBtnPnl_BtnIndex.eBtnJob3f] = UtilApi.getComByP<Button>(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.BtnJob3f);
            m_btnArr[(int)LeftBtnPnl_BtnIndex.eBtnJob4f] = UtilApi.getComByP<Button>(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.BtnJob4f);

            //UtilApi.TransFindChildByPObjAndPath(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Job1f).name = "1";
            //UtilApi.TransFindChildByPObjAndPath(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Job2f).name = "2";
            //UtilApi.TransFindChildByPObjAndPath(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Job3f).name = "3";
            //UtilApi.TransFindChildByPObjAndPath(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Job4f).name = "4";

            //UtilApi.TransFindChildByPObjAndPath(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Filter0f).name = "0";
            //UtilApi.TransFindChildByPObjAndPath(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Filter1f).name = "1";
            //UtilApi.TransFindChildByPObjAndPath(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Filter2f).name = "2";
            //UtilApi.TransFindChildByPObjAndPath(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Filter3f).name = "3";
            //UtilApi.TransFindChildByPObjAndPath(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Filter4f).name = "4";
            //UtilApi.TransFindChildByPObjAndPath(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Filter5f).name = "5";
            //UtilApi.TransFindChildByPObjAndPath(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Filter6f).name = "6";
            //UtilApi.TransFindChildByPObjAndPath(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Filter7f).name = "7";
            //UtilApi.TransFindChildByPObjAndPath(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Filter8f).name = "8";

            toggleJobPnl(false);
            toggleFilterPnl(false);
        }

        public void addEventHandle()
        {
            UtilApi.addEventHandle(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.BtnJob0f, onJobBtnClk);
            UtilApi.addEventHandle(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.BtnJob1f, onJobBtnClk);
            UtilApi.addEventHandle(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.BtnJob2f, onJobBtnClk);
            UtilApi.addEventHandle(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.BtnJob3f, onJobBtnClk);
            UtilApi.addEventHandle(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.BtnJob4f, onJobBtnClk);

            UtilApi.addEventHandle(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.BtnFilter, onFilterBtnClk);
            UtilApi.addEventHandle(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.BtnShouCang, onShouCangBtnClk);

            UtilApi.addEventHandle(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Job1f, onJobTypeBtnClk);
            UtilApi.addEventHandle(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Job2f, onJobTypeBtnClk);
            UtilApi.addEventHandle(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Job3f, onJobTypeBtnClk);
            UtilApi.addEventHandle(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Job4f, onJobTypeBtnClk);

            UtilApi.addEventHandle(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Filter0f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Filter1f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Filter2f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Filter3f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Filter4f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Filter5f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Filter6f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Filter7f, onFilterTypeBtnClk);
            UtilApi.addEventHandle(m_sceneWDSCData.m_sceneUIGo, SceneSCPath.Filter8f, onFilterTypeBtnClk);
        }

        public void hideAllJobBtn()
        {
            for(int idx = 0; idx < (int)LeftBtnPnl_BtnIndex.eBtnJobTotal; ++idx)
            {
                m_btnArr[idx].gameObject.SetActive(false);
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

            if (!m_sceneWDSCData.m_pClassFilterPnl.bCurUpBtn(idx))            // 如果已经点击按钮，不要再点击了
            {
                toggleJobBtn(idx);
                if ((EnPlayerCareer)idx < EnPlayerCareer.ePCTotal)
                {
                    m_sceneWDSCData.m_pClassFilterPnl.updateByCareer((EnPlayerCareer)idx, false);
                }
            }
        }

        public void toggleJobBtn(int idx)
        {
            if (m_sceneWDSCData.m_pClassFilterPnl.m_tabBtnIdx >= 0)
            {
                if (m_sceneWDSCData.m_pClassFilterPnl.m_tabBtnIdx < (int)LeftBtnPnl_BtnIndex.eBtnJobTotal)
                {
                    m_btnArr[m_sceneWDSCData.m_pClassFilterPnl.m_tabBtnIdx].gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("数组越界");
                }
            }
            m_btnArr[idx].gameObject.SetActive(true);
        }

        protected void onFilterTypeBtnClk()
        {
            toggleFilterPnl(false);
            int idx = UtilApi.findIdxByUnderline(EventSystem.current.currentSelectedGameObject.name);
            m_sceneWDSCData.m_wdscCardPnl.filterMp = idx;
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
using SDK.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class InfoMode_1
    {
        public InfoData m_infoData;
        public GameObject m_root;

        public InputField m_input;            // 输入

        public InfoMode_1(InfoData data)
        {
            m_infoData = data;
        }

        public void findWidget()
        {
            m_root = UtilApi.TransFindChildByPObjAndPath(m_infoData.m_form.m_GUIWin.m_uiRoot, InfoComPath.ModeGo_1);
            m_input = UtilApi.getComByP<InputField>(m_infoData.m_form.m_GUIWin.m_uiRoot, InfoComPath.InputField);
        }

        public void updateParam(InfoBoxParam infoParam)
        {
            m_input.text = infoParam.m_inputTips;
        }
    }

    public class InfoMode_2
    {
        public InfoData m_infoData;
        public GameObject m_root;

        public Text m_textDesc;            // 描述

        public InfoMode_2(InfoData data)
        {
            m_infoData = data;
        }

        public void findWidget()
        {
            m_root = UtilApi.TransFindChildByPObjAndPath(m_infoData.m_form.m_GUIWin.m_uiRoot, InfoComPath.ModeGo_2);
            m_textDesc = UtilApi.getComByP<Text>(m_infoData.m_form.m_GUIWin.m_uiRoot, InfoComPath.PathTextDesc);
        }

        public void updateParam(InfoBoxParam infoParam)
        {
            m_textDesc.text = infoParam.m_midDesc;
        }
    }

    public class UIInfo : Form
    {
        protected InfoData m_infoData;

        protected InfoMode_1 m_infoMode_1;
        protected InfoMode_2 m_infoMode_2;

        protected InfoBoxParam m_infoParam;

        public override void onInit()
        {
            exitMode = false;

            base.onInit();
        }

        // 初始化控件
        override public void onReady()
        {
            base.onReady();
            m_infoData = new InfoData(this);
            m_infoMode_1 = new InfoMode_1(m_infoData);
            m_infoMode_2 = new InfoMode_2(m_infoData);

            findWidget();
            addEventHandle();
        }

        override public void onShow()
        {
            base.onShow();
        }

        override public void onExit()
        {
            if (m_infoParam != null)
            {
                Ctx.m_instance.m_poolSys.deleteObj(m_infoParam);
            }
            base.onExit();
        }

        // 关联窗口
        protected void findWidget()
        {
            m_infoMode_1.findWidget();
            m_infoMode_2.findWidget();
        }

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, InfoComPath.PathBtnOk, onBtnClkOk);
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, InfoComPath.PathBtnCancel, onBtnClkCancel);
        }

        public void setParam(InfoBoxParam infoParam)
        {
            if (m_infoParam != null)
            {
                Ctx.m_instance.m_poolSys.deleteObj(m_infoParam);
            }
            m_infoParam = infoParam;

            updateParam();
        }

        protected void updateParam()
        {
            if(InfoBoxModeType.eMode_1 == m_infoParam.m_infoBoxModeType)
            {
                UtilApi.SetActive(m_infoMode_1.m_root, true);
                UtilApi.SetActive(m_infoMode_2.m_root, false);

                m_infoMode_1.updateParam(m_infoParam);
            }
            else if (InfoBoxModeType.eMode_2 == m_infoParam.m_infoBoxModeType)
            {
                UtilApi.SetActive(m_infoMode_1.m_root, false);
                UtilApi.SetActive(m_infoMode_2.m_root, true);

                m_infoMode_2.updateParam(m_infoParam);
            }
        }

        // 点击登陆处理
        protected void onBtnClkOk()
        {
            if (m_infoParam.m_btnClkDisp != null)
            {
                m_infoParam.m_btnClkDisp(InfoBoxBtnType.eBTN_OK);
            }
            exit();
        }

        public void onBtnClkCancel()
        {
            if (m_infoParam.m_btnClkDisp != null)
            {
                m_infoParam.m_btnClkDisp(InfoBoxBtnType.eBTN_CANCEL);
            }
            exit();
        }

        public static void showMsg(InfoBoxParam param)
        {
            Ctx.m_instance.m_uiMgr.loadAndShow<UIInfo>(param.m_formID);
            Ctx.m_instance.m_uiMgr.getForm<UIInfo>(param.m_formID).setParam(param);
        }
    }
}
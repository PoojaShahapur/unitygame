using SDK.Common;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIInfo : Form
    {
        protected Text m_textDesc;
        protected InfoBoxParam m_infoParam;

        override public void onShow()
        {
            exitMode = false;
        }

        // 初始化控件
        override public void onReady()
        {
            findWidget();
            addEventHandle();
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
            m_textDesc = UtilApi.getComByP<Text>(m_GUIWin.m_uiRoot, InfoComPath.PathTextDesc);
        }

        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, InfoComPath.PathBtnOk, onBtnClkOk);
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
            m_textDesc.text = m_infoParam.m_midDesc;
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

        public Text textDesc
        {
            get
            {
                return m_textDesc;
            }
            set
            {
                m_textDesc = value;
            }
        }

        public static void showMsg(InfoBoxParam param)
        {
            Ctx.m_instance.m_uiMgr.loadAndShow<UIInfo>(param.m_formID);
            Ctx.m_instance.m_uiMgr.getForm<UIInfo>(param.m_formID).setParam(param);
        }
    }
}
using SDK.Lib;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    public class AuxBasicButton : AuxComponent
    {
        protected EventDispatch m_eventDisp;      // 分发
        protected Button m_btn;

        public AuxBasicButton(GameObject pntNode = null, string path = "", BtnStyleID styleId = BtnStyleID.eBSID_None)
        {
            m_eventDisp = new EventDispatch();
            if (pntNode != null)
            {
                m_selfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
                updateBtnCom(null);
            }
        }

        override public void dispose()
        {
            if (m_eventDisp != null)
            {
                UtilApi.RemoveListener(m_btn, onBtnClk);
            }
            base.dispose();
        }

        virtual protected void updateBtnCom(IDispatchObject dispObj)
        {
            m_btn = UtilApi.getComByP<Button>(m_selfGo);
            UtilApi.addEventHandle(m_btn, onBtnClk);
        }

        public void enable()
        {
            m_btn.interactable = true;
        }

        public void disable()
        {
            m_btn.interactable = false;
        }

        // 点击回调
        protected void onBtnClk()
        {
            m_eventDisp.dispatchEvent(this);
        }

        public void addEventHandle(Action<IDispatchObject> btnClk)
        {
            m_eventDisp.addEventHandle(btnClk);
        }

        virtual public void syncUpdateCom()
        {

        }
    }
}
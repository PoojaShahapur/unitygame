using SDK.Lib;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDK.Common
{
    /**
     * @brief ���� Button
     */
    public class AuxButton : AuxComponent
    {
        protected EventDispatch m_eventDisp;      // �ַ�
        protected Button m_btn;

        public AuxButton(GameObject pntNode, string path, BtnStyleID styleId = BtnStyleID.eBSID_None)
        {
            m_eventDisp = new EventDispatch();
            m_selfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
            m_btn = UtilApi.getComByP<Button>(pntNode, path);
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

        // ����ص�
        protected void onBtnClk()
        {
            m_eventDisp.dispatchEvent(this);
        }

        public void addEventHandle(Action<IDispatchObject> btnClk)
        {
            m_eventDisp.addEventHandle(btnClk);
        }
    }
}
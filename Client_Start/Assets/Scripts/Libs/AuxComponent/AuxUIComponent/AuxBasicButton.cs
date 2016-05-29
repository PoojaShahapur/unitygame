using LuaInterface;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    public class AuxBasicButton : AuxWindow
    {
        protected EventDispatch m_eventDisp;      // 分发
        protected Button m_btn;

        public AuxBasicButton(GameObject go_)
        {
            m_eventDisp = new EventDispatch();
            m_selfGo = go_;
            updateBtnCom(null);
        }

        public AuxBasicButton(GameObject pntNode, string path, BtnStyleID styleId = BtnStyleID.eBSID_None)
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

        public void addEventHandle(ICalleeObject pThis, MAction<IDispatchObject> btnClk, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            m_eventDisp.addEventHandle(pThis, btnClk, luaTable, luaFunction);
        }

        public void removeEventHandle(ICalleeObject pThis, MAction<IDispatchObject> btnClk, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            m_eventDisp.removeEventHandle(pThis, btnClk, luaTable, luaFunction);
        }

        virtual public void syncUpdateCom()
        {

        }
    }
}
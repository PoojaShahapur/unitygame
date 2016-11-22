using LuaInterface;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    public class AuxBasicButton : AuxWindow
    {
        protected EventDispatch mEventDisp;      // 分发
        protected Button mBtn;

        public AuxBasicButton(GameObject go_)
        {
            this.mEventDisp = new EventDispatch();
            this.mSelfGo = go_;
            updateBtnCom(null);
        }

        public AuxBasicButton(GameObject pntNode, string path, BtnStyleID styleId = BtnStyleID.eBSID_None)
        {
            this.mEventDisp = new EventDispatch();
            if (pntNode != null)
            {
                this.mSelfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
                updateBtnCom(null);
            }
        }

        override public void dispose()
        {
            if (this.mEventDisp != null)
            {
                UtilApi.RemoveListener(this.mBtn, onBtnClk);
            }
            base.dispose();
        }

        virtual protected void updateBtnCom(IDispatchObject dispObj)
        {
            this.mBtn = UtilApi.getComByP<Button>(this.mSelfGo);
            UtilApi.addEventHandle(this.mBtn, onBtnClk);
        }

        public void enable()
        {
            this.mBtn.interactable = true;
        }

        public void disable()
        {
            this.mBtn.interactable = false;
        }

        // 点击回调
        protected void onBtnClk()
        {
            this.mEventDisp.dispatchEvent(this);
        }

        public void addEventHandle(ICalleeObject pThis, MAction<IDispatchObject> btnClk, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            this.mEventDisp.addEventHandle(pThis, btnClk, luaTable, luaFunction);
        }

        public void removeEventHandle(ICalleeObject pThis, MAction<IDispatchObject> btnClk, LuaTable luaTable = null, LuaFunction luaFunction = null)
        {
            this.mEventDisp.removeEventHandle(pThis, btnClk, luaTable, luaFunction);
        }

        virtual public void syncUpdateCom()
        {

        }
    }
}
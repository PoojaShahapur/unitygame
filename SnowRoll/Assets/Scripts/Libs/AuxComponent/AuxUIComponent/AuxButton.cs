using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    public class AuxButton : AuxWindow
    {
        protected EventDispatch mEventDisp;      // 分发
        protected Button mBtn;
        protected Text mText;

        public AuxButton(GameObject go_ = null)
        {
            this.mEventDisp = new EventDispatch();
            this.mSelfGo = go_;
            updateBtnCom(null);
        }

        public AuxButton(GameObject pntNode, string path, BtnStyleID styleId = BtnStyleID.eBSID_None)
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

        public void setSelfGo(GameObject pntNode, string path)
        {
            this.mSelfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
            updateBtnCom(null);
        }

        public void setText(string value)
        {
            if (this.mText != null)
            {
                this.mText.text = value;
            }
        }

        public string getText()
        {
            if (this.mText != null)
            {
                return this.mText.text;
            }
            return "";
        }

        public void setColor(Color color)
        {
            this.mText.color = color;
        }

        public Color getColor()
        {
            return this.mText.color;
        }

        virtual protected void updateBtnCom(IDispatchObject dispObj)
        {
            if (null != this.mSelfGo)
            {
                this.mBtn = UtilApi.getComByP<Button>(this.mSelfGo);
                UtilApi.addEventHandle(this.mBtn, onBtnClk);
                this.mText = UtilApi.getComByP<Text>(this.mSelfGo, UtilApi.TEXT_IN_BTN);
            }
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
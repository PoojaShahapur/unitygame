using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    public class AuxButton : AuxWindow
    {
        protected EventDispatch mClickEventDispatch;      // 点击事件分发
        protected EventDispatch mDownEventDispatch;       // Down 事件分发
        protected EventDispatch mUpEventDispatch;         // Up 事件分发
        protected EventDispatch mExitEventDispatch;       // Exit 事件分发

        protected Button mBtn;
        protected Text mText;

        public AuxButton(GameObject go_ = null)
        {
            this.mClickEventDispatch = new AddOnceEventDispatch();
            this.mDownEventDispatch = new AddOnceEventDispatch();
            this.mUpEventDispatch = new AddOnceEventDispatch();
            this.mExitEventDispatch = new AddOnceEventDispatch();

            this.mSelfGo = go_;
            updateBtnCom(null);
        }

        public AuxButton(GameObject pntNode, string path, BtnStyleID styleId = BtnStyleID.eBSID_None)
        {
            this.mClickEventDispatch = new AddOnceEventDispatch();
            this.mDownEventDispatch = new AddOnceEventDispatch();
            this.mUpEventDispatch = new AddOnceEventDispatch();
            this.mExitEventDispatch = new AddOnceEventDispatch();

            if (pntNode != null)
            {
                this.mSelfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
                updateBtnCom(null);
            }
        }

        override public void dispose()
        {
            if (this.mClickEventDispatch != null)
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

        public void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
        {
            this.mDownEventDispatch.dispatchEvent(this);
        }

        public void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
        {
            this.mUpEventDispatch.dispatchEvent(this);
        }

        public void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
        {
            this.mExitEventDispatch.dispatchEvent(this);
        }

        // 点击回调
        protected void onBtnClk()
        {
            this.mClickEventDispatch.dispatchEvent(this);
        }

        // 添加点击事件处理器
        public void addEventHandle(ICalleeObject pThis, MAction<IDispatchObject> btnClk, uint eventId = 0, LuaTable luaTable = null, LuaFunction luaFunction = null, uint luaEventId = 0)
        {
            this.mClickEventDispatch.addEventHandle(pThis, btnClk, eventId, luaTable, luaFunction, luaEventId);
        }

        public void removeEventHandle(ICalleeObject pThis, MAction<IDispatchObject> btnClk, uint eventId = 0, LuaTable luaTable = null, LuaFunction luaFunction = null, uint luaEventId = 0)
        {
            this.mClickEventDispatch.removeEventHandle(pThis, btnClk, eventId, luaTable, luaFunction, luaEventId);
        }

        // Down 事件处理
        public void addDownEventHandle(ICalleeObject pThis, MAction<IDispatchObject> btnClk, uint eventId = 0, LuaTable luaTable = null, LuaFunction luaFunction = null, uint luaEventId = 0)
        {
            this.mDownEventDispatch.addEventHandle(pThis, btnClk, eventId, luaTable, luaFunction, luaEventId);
        }

        public void removeDownEventHandle(ICalleeObject pThis, MAction<IDispatchObject> btnClk, uint eventId = 0, LuaTable luaTable = null, LuaFunction luaFunction = null, uint luaEventId = 0)
        {
            this.mDownEventDispatch.removeEventHandle(pThis, btnClk, eventId, luaTable, luaFunction, luaEventId);
        }

        // Up 事件处理
        public void addUpEventHandle(ICalleeObject pThis, MAction<IDispatchObject> btnClk, uint eventId = 0, LuaTable luaTable = null, LuaFunction luaFunction = null, uint luaEventId = 0)
        {
            this.mUpEventDispatch.addEventHandle(pThis, btnClk, eventId, luaTable, luaFunction, luaEventId);
        }

        public void removeUpEventHandle(ICalleeObject pThis, MAction<IDispatchObject> btnClk, uint eventId = 0, LuaTable luaTable = null, LuaFunction luaFunction = null, uint luaEventId = 0)
        {
            this.mUpEventDispatch.removeEventHandle(pThis, btnClk, eventId, luaTable, luaFunction, luaEventId);
        }

        // Exit 事件处理
        public void addExitEventHandle(ICalleeObject pThis, MAction<IDispatchObject> btnClk, uint eventId = 0, LuaTable luaTable = null, LuaFunction luaFunction = null, uint luaEventId = 0)
        {
            this.mExitEventDispatch.addEventHandle(pThis, btnClk, eventId, luaTable, luaFunction, luaEventId);
        }

        public void removeExitEventHandle(ICalleeObject pThis, MAction<IDispatchObject> btnClk, uint eventId = 0, LuaTable luaTable = null, LuaFunction luaFunction = null, uint luaEventId = 0)
        {
            this.mExitEventDispatch.removeEventHandle(pThis, btnClk, eventId, luaTable, luaFunction, luaEventId);
        }

        virtual public void syncUpdateCom()
        {

        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
	/**
	 * @brief 支持拖动，支持深度排序
	 */
	public class Form : Window
	{
        protected bool mExitMode;               // 关闭退出模式
		protected bool mIsHideOnCreate;         // 创建后是否隐藏
        //protected bool m_bResLoaded;            // 资源加载进来
        protected UIFormId mId;
        protected bool mIsLoadWidgetRes;                // 是否应该加载窗口资源
        protected bool mIsReady;            // 是否准备就绪

        protected bool mIsBlurBg;       // 是否模糊背景
        protected bool mIsHandleExitBtn;       // 是否关联关闭按钮

        protected LuaCSBridgeForm mLuaCSBridgeForm;
        protected string mFormName;            // 这个是 Lua 中传的标识符，会传给 Lua 使用，客户端自己不用
        protected MDictionary<GameObject, GOExtraInfo> mGo2Path;

		public Form()
            : base()
		{
            mExitMode = true;
            mIsHideOnCreate = false;
            mIsLoadWidgetRes = false;
            mIsReady = false;
            mIsBlurBg = false;
            mIsHandleExitBtn = false;
            mAlignVertial = (int)WindowAnchor.CENTER;
			mAlignHorizontal = (int)WindowAnchor.CENTER;

            mGo2Path = new MDictionary<GameObject, GOExtraInfo>();
		}

        public UIFormId id
        {
            get
            {
                return mId;
            }
            set
            {
                mId = value;
            }
        }

        public UIFormId getFormID()
        {
            return mId;
        }
		
		public bool hideOnCreate
		{
            get
            {
                return mIsHideOnCreate;
            }
            set
            {
			    mIsHideOnCreate = value;
            }
		}

        public bool exitMode
        {
            get
            {
                return mExitMode;
            }
            set
            {
                mExitMode = value;
            }
        }

        public bool isLoadWidgetRes
        {
            get
            {
                return mIsLoadWidgetRes;
            }
            set
            {
                mIsLoadWidgetRes = true;
            }
        }

        public bool bReady
        {
            get
            {
                return mIsReady;
            }
        }

        public string formName
        {
            get
            {
                return mFormName;
            }
            set
            {
                mFormName = value;
            }
        }

        public LuaCSBridgeForm luaCSBridgeForm
        {
            get
            {
                return mLuaCSBridgeForm;
            }
            set
            {
                mLuaCSBridgeForm = value;
            }
        }

        public void init()
        {
            onInit();
        }

        public void show()
        {
            Ctx.mInstance.mUiMgr.showForm(mId);
        }

        //private void hide()
        //{
        //    Ctx.mInstance.mUiMgr.hideForm(mId);
        //}

        public void exit()
        {
            Ctx.mInstance.mUiMgr.exitForm(mId);
        }

        // 界面代码创建后就调用
        virtual public void onInit()
        {
            if(mLuaCSBridgeForm != null)
            {
                mLuaCSBridgeForm.callClassMethod("", LuaCSBridgeForm.ON_INIT);
            }
            //if (mIsLoadWidgetRes)
            //{
                // 默认会继续加载资源
                //Ctx.mInstance.mUiMgr.loadWidgetRes(this.id);
            //}
        }
        
        // 第一次显示之前会调用一次
        virtual public void onReady()
        {
            if (mLuaCSBridgeForm != null)
            {
                mLuaCSBridgeForm.callClassMethod("", LuaCSBridgeForm.ON_READY);
            }

            mIsReady = true;
            if (mIsHandleExitBtn)
            {
                UtilApi.addEventHandle(mGuiWin.m_uiRoot, "BtnClose", onExitBtnClick); // 关闭事件
            }
        }

        // 每一次显示都会调用一次
        virtual public void onShow()
		{
            if (mLuaCSBridgeForm != null)
            {
                mLuaCSBridgeForm.callClassMethod("", LuaCSBridgeForm.ON_SHOW);
            }

            if (mIsBlurBg)
            {
                //Ctx.mInstance.mUiMgr.showForm(UIFormId.eUIBlurBg);        // 显示模糊背景界面
            }
		    //adjustPosWithAlign();
		}

        // 每一次隐藏都会调用一次
        virtual public void onHide()
		{
            if (mLuaCSBridgeForm != null)
            {
                mLuaCSBridgeForm.callClassMethod("", LuaCSBridgeForm.ON_HIDE);
            }

            if (mIsBlurBg)
            {
                //Ctx.mInstance.mUiMgr.exitForm(UIFormId.eUIBlurBg);
            }
		}

        // 每一次关闭都会调用一次
        virtual public void onExit()
		{
            if (mLuaCSBridgeForm != null)
            {
                mLuaCSBridgeForm.callClassMethod("", LuaCSBridgeForm.ON_EXIT);
            }

            if (mIsBlurBg)
            {
                //Ctx.mInstance.mUiMgr.exitForm(UIFormId.eUIBlurBg);
            }
		}

        public bool isVisible()
        {
            return mGuiWin.m_uiRoot.activeSelf;        // 仅仅是自己是否可见
        }

        /*
         * stage的大小发生变化后，这个函数会被调用。子类可重载这个函数
         */
        public void onStageReSize()
        {
            adjustPosWithAlign();
        }
		
		public void adjustPosWithAlign()
		{
			MPointF pos = computeAdjustPosWithAlign();
			this.x = pos.x;
			this.y = pos.y;
		}
		
		protected MPointF computeAdjustPosWithAlign()
		{
			MPointF ret = new MPointF(0, 0);
			int widthStage = 0;
			int heightStage = 0;
            if (mAlignVertial == (int)WindowAnchor.CENTER)
			{
                ret.y = (heightStage - this.mHeight) / 2;
			}
            else if (mAlignVertial == (int)WindowAnchor.TOP)
			{
				ret.y = this.mMarginTop;
			}
			else
			{
				ret.y = heightStage - this.mHeight - this.mMarginBottom;
			}
			
			if (mAlignHorizontal == (int)WindowAnchor.CENTER)
			{
                ret.x = (widthStage - this.mWidth) / 2;
			}
			else if (mAlignHorizontal == (int)WindowAnchor.LEFT)
			{
				ret.x = mMarginLeft;
			}
			else
			{
                ret.x = widthStage - this.mWidth - mMarginRight;
			}
			return ret;
		}
		
        // 按钮点击关闭
		protected void onExitBtnClick()
		{
            exit();
		}

        public void registerBtnClickEventByList(string[] btnList)
        {
            foreach(var path in btnList)
            {
                addClick(mGuiWin.m_uiRoot, path);
            }
        }

        public void registerImageClickEventByList(string[] imageList)
        {

        }

        public void registerWidgetEvent()
        {
            string[] pathArr = Ctx.mInstance.mLuaSystem.getTable2StrArray("BtnClickTable");
            foreach(var path in pathArr)
            {
                addClick(mGuiWin.m_uiRoot, path);
            }
        }

        protected void onBtnClk(GameObject go_)
        {
            if(mLuaCSBridgeForm != null)
            {
                if(mGo2Path.ContainsKey(go_))
                {
                    mLuaCSBridgeForm.handleUIEvent("onBtnClk", mFormName, mGo2Path[go_].mPath);
                }
            }

            // 测试全局分发事件
            // Ctx.mInstance.m_globalEventMgr.eventDispatchGroup.dispatchEvent((int)eGlobalEventType.eGlobalTest, null);
        }

        public void addClick(GameObject go, string path)
        {
            UtilApi.addEventHandle(go, path, onBtnClk);
            GameObject btnGo = UtilApi.TransFindChildByPObjAndPath(go, path);
            if(btnGo = null)
            {
                if(!mGo2Path.ContainsKey(btnGo))
                {
                    mGo2Path[btnGo] = new GOExtraInfo();
                    mGo2Path[btnGo].mPath = path;
                }
            }
        }

        public void removeClick(GameObject go, string path)
        {
            UtilApi.removeEventHandle(go, path);
            GameObject btnGo = UtilApi.TransFindChildByPObjAndPath(go, path);
            if(btnGo != null)
            {
                if(mGo2Path.ContainsKey(btnGo))
                {
                    mGo2Path.Remove(btnGo);
                }
            }
        }
	}
}
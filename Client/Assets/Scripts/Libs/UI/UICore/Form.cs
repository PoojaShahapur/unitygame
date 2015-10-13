using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
	/**
	 * @brief 支持拖动，支持深度排序
	 */
	public class Form : Window
	{
        protected bool m_exitMode;               // 关闭退出模式
		protected bool m_bHideOnCreate;         // 创建后是否隐藏
        //protected bool m_bResLoaded;            // 资源加载进来
        protected UIFormID m_id;
        protected bool m_bLoadWidgetRes;                // 是否应该加载窗口资源
        protected bool m_bReady;            // 是否准备就绪

        protected bool m_bBlurBg;       // 是否模糊背景
        protected bool m_bHandleExitBtn;       // 是否关联关闭按钮

        protected LuaCSBridgeForm m_luaCSBridgeForm;
        protected string m_formName;            // 这个是 Lua 中传的标识符，会传给 Lua 使用，客户端自己不用
        protected Dictionary<GameObject, GOExtraInfo> m_go2Path;

		public Form()
            : base()
		{
            m_exitMode = true;
            m_bHideOnCreate = false;
            m_bLoadWidgetRes = false;
            m_bReady = false;
            m_bBlurBg = false;
            m_bHandleExitBtn = false;
            m_alignVertial = (int)WindowAnchor.CENTER;
			m_alignHorizontal = (int)WindowAnchor.CENTER;

            m_go2Path = new Dictionary<GameObject, GOExtraInfo>();
		}

        public UIFormID id
        {
            get
            {
                return m_id;
            }
            set
            {
                m_id = value;
            }
        }

        public UIFormID getFormID()
        {
            return m_id;
        }
		
		public bool hideOnCreate
		{
            get
            {
                return m_bHideOnCreate;
            }
            set
            {
			    m_bHideOnCreate = value;
            }
		}

        public bool exitMode
        {
            get
            {
                return m_exitMode;
            }
            set
            {
                m_exitMode = value;
            }
        }

        public bool bLoadWidgetRes
        {
            get
            {
                return m_bLoadWidgetRes;
            }
            set
            {
                m_bLoadWidgetRes = true;
            }
        }

        public bool bReady
        {
            get
            {
                return m_bReady;
            }
        }

        public string formName
        {
            get
            {
                return m_formName;
            }
            set
            {
                m_formName = value;
            }
        }

        public LuaCSBridgeForm luaCSBridgeForm
        {
            get
            {
                return m_luaCSBridgeForm;
            }
            set
            {
                m_luaCSBridgeForm = value;
            }
        }

        public void init()
        {
            onInit();
        }

        public void show()
        {
            Ctx.m_instance.m_uiMgr.showForm(m_id);
        }

        //private void hide()
        //{
        //    Ctx.m_instance.m_uiMgr.hideForm(m_id);
        //}

        public void exit()
        {
            Ctx.m_instance.m_uiMgr.exitForm(m_id);
        }

        // 界面代码创建后就调用
        virtual public void onInit()
        {
            if(m_luaCSBridgeForm != null)
            {
                m_luaCSBridgeForm.CallMethod(LuaCSBridgeForm.ON_INIT);
            }
            //if (m_bLoadWidgetRes)
            //{
                // 默认会继续加载资源
                Ctx.m_instance.m_uiMgr.loadWidgetRes(this.id);
            //}
        }
        
        // 第一次显示之前会调用一次
        virtual public void onReady()
        {
            if (m_luaCSBridgeForm != null)
            {
                m_luaCSBridgeForm.CallMethod(LuaCSBridgeForm.ON_READY);
            }

            m_bReady = true;
            if (m_bHandleExitBtn)
            {
                UtilApi.addEventHandle(m_GUIWin.m_uiRoot, "BtnClose", onExitBtnClick); // 关闭事件
            }
        }

        // 每一次显示都会调用一次
        virtual public void onShow()
		{
            if (m_luaCSBridgeForm != null)
            {
                m_luaCSBridgeForm.CallMethod(LuaCSBridgeForm.ON_SHOW);
            }

            if (m_bBlurBg)
            {
                Ctx.m_instance.m_uiMgr.showForm(UIFormID.eUIBlurBg);        // 显示模糊背景界面
            }
		    //adjustPosWithAlign();
		}

        // 每一次隐藏都会调用一次
        virtual public void onHide()
		{
            if (m_luaCSBridgeForm != null)
            {
                m_luaCSBridgeForm.CallMethod(LuaCSBridgeForm.ON_HIDE);
            }

            if (m_bBlurBg)
            {
                Ctx.m_instance.m_uiMgr.exitForm(UIFormID.eUIBlurBg);
            }
		}

        // 每一次关闭都会调用一次
        virtual public void onExit()
		{
            if (m_luaCSBridgeForm != null)
            {
                m_luaCSBridgeForm.CallMethod(LuaCSBridgeForm.ON_EXIT);
            }

            if (m_bBlurBg)
            {
                Ctx.m_instance.m_uiMgr.exitForm(UIFormID.eUIBlurBg);
            }
		}

        public bool isVisible()
        {
            return m_GUIWin.m_uiRoot.activeSelf;        // 仅仅是自己是否可见
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
			PointF pos = computeAdjustPosWithAlign();
			this.x = pos.x;
			this.y = pos.y;
		}
		
		protected PointF computeAdjustPosWithAlign()
		{
			PointF ret = new PointF(0, 0);
			int widthStage = 0;
			int heightStage = 0;
            if (m_alignVertial == (int)WindowAnchor.CENTER)
			{
                ret.y = (heightStage - this.m_height) / 2;
			}
            else if (m_alignVertial == (int)WindowAnchor.TOP)
			{
				ret.y = this.m_marginTop;
			}
			else
			{
				ret.y = heightStage - this.m_height - this.m_marginBottom;
			}
			
			if (m_alignHorizontal == (int)WindowAnchor.CENTER)
			{
                ret.x = (widthStage - this.m_width) / 2;
			}
			else if (m_alignHorizontal == (int)WindowAnchor.LEFT)
			{
				ret.x = m_marginLeft;
			}
			else
			{
                ret.x = widthStage - this.m_width - m_marginRight;
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
                addClick(m_GUIWin.m_uiRoot, path);
            }
        }

        public void registerImageClickEventByList(string[] imageList)
        {

        }

        public void registerWidgetEvent()
        {
            string[] pathArr = m_luaCSBridgeForm.getTable2StrArray("BtnClickTable");
            foreach(var path in pathArr)
            {
                addClick(m_GUIWin.m_uiRoot, path);
            }
        }

        protected void onBtnClk(GameObject go_)
        {
            if(m_luaCSBridgeForm != null)
            {
                if(m_go2Path.ContainsKey(go_))
                {
                    m_luaCSBridgeForm.handleUIEvent("onBtnClk", m_formName, m_go2Path[go_].m_path);
                }
            }

            // 测试全局分发事件
            // Ctx.m_instance.m_globalEventMgr.eventDispatchGroup.dispatchEvent((int)eGlobalEventType.eGlobalTest, null);
        }

        public void addClick(GameObject go, string path)
        {
            UtilApi.addEventHandle(go, path, onBtnClk);
            GameObject btnGo = UtilApi.TransFindChildByPObjAndPath(go, path);
            if(btnGo = null)
            {
                if(!m_go2Path.ContainsKey(btnGo))
                {
                    m_go2Path[btnGo] = new GOExtraInfo();
                    m_go2Path[btnGo].m_path = path;
                }
            }
        }

        public void removeClick(GameObject go, string path)
        {
            UtilApi.removeEventHandle(go, path);
            GameObject btnGo = UtilApi.TransFindChildByPObjAndPath(go, path);
            if(btnGo != null)
            {
                if(m_go2Path.ContainsKey(btnGo))
                {
                    m_go2Path.Remove(btnGo);
                }
            }
        }
	}
}
using UnityEngine;

namespace SDK.Common
{
	/**
	 * @brief 支持拖动，支持深度排序
	 */
	public class Form : Window
	{
        protected bool m_exitMode = true;               // 关闭退出模式
		protected bool m_bHideOnCreate = false;         // 创建后是否隐藏
        protected bool m_bResLoaded = false;            // 资源加载进来
        protected UIFormID m_id;
        protected bool m_bLoadWidgetRes = false;                // 是否应该加载窗口资源

		public Form()
            : base()
		{
			m_alignVertial = Window.CENTER;
			m_alignHorizontal = Window.CENTER;	
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
            //if (m_bLoadWidgetRes)
            //{
                // 默认会继续加载资源
                Ctx.m_instance.m_uiMgr.loadWidgetRes(this.id);
            //}
        }
        
        // 第一次显示之前会调用一次
        virtual public void onReady()
        {
            UtilApi.addEventHandle(m_GUIWin.m_uiRoot, "BtnClose", onExitBtnClick); // 关闭事件
        }

        // 每一次显示都会调用一次
        virtual public void onShow()
		{
            Ctx.m_instance.m_uiMgr.showForm(UIFormID.UIBlurBg);        // 显示模糊背景界面
		    //adjustPosWithAlign();
		}

        // 每一次隐藏都会调用一次
        virtual public void onHide()
		{
            Ctx.m_instance.m_uiMgr.exitForm(UIFormID.UIBlurBg);
		}

        // 每一次关闭都会调用一次
        virtual public void onExit()
		{
            Ctx.m_instance.m_uiMgr.exitForm(UIFormID.UIBlurBg);
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
            if (m_alignVertial == CENTER)
			{
                ret.y = (heightStage - this.m_height) / 2;
			}
            else if (m_alignVertial == TOP)
			{
				ret.y = this.m_marginTop;
			}
			else
			{
				ret.y = heightStage - this.m_height - this.m_marginBottom;
			}
			
			if (m_alignHorizontal == CENTER)
			{
                ret.x = (widthStage - this.m_width) / 2;
			}
			else if (m_alignHorizontal == LEFT)
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

        public bool bReady
		{
            get
            {
                return m_bReady;
            } 
		}	
	}
}
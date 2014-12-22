using UnityEngine;

namespace SDK.Common
{
	/**
	 * @brief 支持拖动，支持深度排序
	 */
	public class Form : Window, IForm
	{
		protected bool m_bHideOnCreate = false;         // 创建后是否隐藏
        protected bool m_bResLoaded = false;            // 资源加载进来
		
		public Form()
            : base()
		{
			m_alignVertial = Window.CENTER;
			m_alignHorizontal = Window.CENTER;	
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

        public void init()
        {
            onInit();
        }

        public void show()
        {
            Ctx.m_instance.m_uiMgr.showForm(m_id);
        }

        public void hide()
        {
            Ctx.m_instance.m_uiMgr.hideForm(m_id);
        }

        public void exit()
        {
            Ctx.m_instance.m_uiMgr.exitForm(m_id);
        }

        // 资源加载完成就调用
        virtual public void onInit()
        {
            // 默认会继续加载资源
            Ctx.m_instance.m_uiMgr.loadWidgetRes(this.id);
        }
        
        // 第一次显示之前会调用一次
        virtual public void onReady()
        {

        }

        // 每一次显示都会调用一次
        virtual public void onShow()
		{
		    adjustPosWithAlign();
		}

        // 每一次隐藏都会调用一次
        virtual public void onHide()
		{
			
		}

        // 每一次关闭都会调用一次
        virtual public void onExit()
		{
			
		}

        public bool isVisible()
        {
            return m_GUIWin.m_uiRoot.activeSelf;
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
		
		protected void onExitBtnClick(GameObject curTarget)
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
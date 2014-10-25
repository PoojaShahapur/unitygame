using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
	/**
	 * @brief 支持拖动，支持深度排序
	 */
	public class Form : Window, IForm
	{
		protected bool m_bHideOnCreate;
		
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

		public bool isVisible()
		{
            return m_GUIWin.m_uiRoot.activeInHierarchy;
		}
		
		/*
		 * stage的大小发生变化后，这个函数会被调用。子类可重载这个函数
		 */
		public void onStageReSize()
		{
			adjustPosWithAlign();
		}

        public void onReady()
        {

        }
		
		public void onShow()
		{
		    adjustPosWithAlign();
		}
		
		public void onHide()
		{
			
		}
		
		public void onDestroy()
		{
			
		}
		
		virtual public void dispose()
		{

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

		public void exit()
		{
			if (m_exitMode == EXITMODE_DESTORY)
			{
                GameObject.DestroyImmediate(m_GUIWin.m_uiRoot);
                Ctx.m_instance.m_UIMgr.destroyForm(m_id);
			}
			else
			{
                m_GUIWin.m_uiRoot.SetActive(false);
			}
		}	
			
		public void show()
		{
            m_GUIWin.m_uiRoot.SetActive(true);
		}
		
		public void hide()
		{
            m_GUIWin.m_uiRoot.SetActive(false);
		}
		
		protected void onExitBtnClick(GameObject curTarget)
		{
			exit();
		}
		
		public bool isInitiated
		{
            get
            {
			    return m_bInitiated;
            } 
		}	
	}
}
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 基本的窗口内容，包括添加一些基本控件
     */
    public class Window
    {
        public GUIWin m_guiWin;      // 控件数据
		protected bool m_draggable;

		protected int m_hitYMax;	// 可点击范围 Y 的最大值
		protected int m_alignVertial;
		protected int m_alignHorizontal;
	
		protected int m_marginLeft;
		protected int m_marginTop;
		protected int m_marginRight;
		protected int m_marginBottom;

        public int m_width;
        public int m_height;
        public UILayer m_uiLayer;
        protected bool m_isResReady;            // 资源是否已经加载并初始化

        public Window()
        {
            m_guiWin = new GUIWin();
            m_draggable = true;
            m_hitYMax = 30;
            m_alignVertial = 0;
            m_alignHorizontal = 0;
            m_isResReady = false;
        }

        public float x
        {
            get
            {
                return m_guiWin.m_uiRoot.transform.localPosition.x;
            }
            set
            {
                UtilApi.setPos(m_guiWin.m_uiRoot.transform, new Vector3(value, m_guiWin.m_uiRoot.transform.localPosition.y, m_guiWin.m_uiRoot.transform.localPosition.z));
            }
        }

        public float y
        {
            get
            {
                return m_guiWin.m_uiRoot.transform.localPosition.y;
            }
            set
            {
                UtilApi.setPos(m_guiWin.m_uiRoot.transform, new Vector3(m_guiWin.m_uiRoot.transform.localPosition.x, value, m_guiWin.m_uiRoot.transform.localPosition.z));
            }
        }

        public UILayer uiLayer
        {
            get
            {
                return m_uiLayer;
            }
            set
            {
                m_uiLayer = value;
            }
        }

        public bool IsVisible()
        {
            return m_guiWin.m_uiRoot.activeSelf;
        }

        public bool IsResReady
        {
            get
            {
                return m_isResReady;
            }
            set 
            {
                m_isResReady = value;
            }
        }

        public GUIWin GUIWin()
        {
            return m_guiWin;
        }
    }
}
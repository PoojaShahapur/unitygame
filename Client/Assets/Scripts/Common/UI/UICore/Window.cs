using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 基本的窗口内容，包括添加一些基本控件
     */
    public class Window
    {
        public static int LEFT = 0; //居左
		public static int CENTER = 1; //居中(横向和纵向都用此值)
		public static int RIGHT = 2; //居右
		
		public static int TOP = 0; //居上
		public static int BOTTOM = 2; //居下

        public GUIWin m_GUIWin = new GUIWin();      // 控件数据
		protected bool m_draggable = true;

		protected int m_hitYMax = 30;	// 可点击范围 Y 的最大值
		protected int m_alignVertial = 0;
		protected int m_alignHorizontal = 0;
	
		protected int m_marginLeft;
		protected int m_marginTop;
		protected int m_marginRight;
		protected int m_marginBottom;

        public int m_width;
        public int m_height;
        public UILayer m_uiLayer;
        protected bool m_isResReady = false;            // 资源是否已经加载并初始化

        public float x
        {
            get
            {
                return m_GUIWin.m_uiRoot.transform.localPosition.x;
            }
            set
            {
                m_GUIWin.m_uiRoot.transform.localPosition = new Vector3(value, m_GUIWin.m_uiRoot.transform.localPosition.y, m_GUIWin.m_uiRoot.transform.localPosition.z);
            }
        }

        public float y
        {
            get
            {
                return m_GUIWin.m_uiRoot.transform.localPosition.y;
            }
            set
            {
                m_GUIWin.m_uiRoot.transform.localPosition = new Vector3(m_GUIWin.m_uiRoot.transform.localPosition.x, value, m_GUIWin.m_uiRoot.transform.localPosition.z);
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
            return m_GUIWin.m_uiRoot.activeSelf;
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
    }
}
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 基本的窗口内容，包括添加一些基本控件
     */
    public class Window
    {
        public GuiWin mGuiWin;      // 控件数据
		protected bool mDraggable;

		protected int mHitYMax;	// 可点击范围 Y 的最大值
		protected int mAlignVertial;
		protected int mAlignHorizontal;
	
		protected int mMarginLeft;
		protected int mMarginTop;
		protected int mMarginRight;
		protected int mMarginBottom;

        public int mWidth;
        public int mHeight;
        public UILayer mUiLayer;
        protected bool mIsResReady;            // 资源是否已经加载并初始化

        public Window()
        {
            mGuiWin = new GuiWin();
            mDraggable = true;
            mHitYMax = 30;
            mAlignVertial = 0;
            mAlignHorizontal = 0;
            mIsResReady = false;
        }

        public float x
        {
            get
            {
                return mGuiWin.mUiRoot.transform.localPosition.x;
            }
            set
            {
                UtilApi.setPos(mGuiWin.mUiRoot.transform, new Vector3(value, mGuiWin.mUiRoot.transform.localPosition.y, mGuiWin.mUiRoot.transform.localPosition.z));
            }
        }

        public float y
        {
            get
            {
                return mGuiWin.mUiRoot.transform.localPosition.y;
            }
            set
            {
                UtilApi.setPos(mGuiWin.mUiRoot.transform, new Vector3(mGuiWin.mUiRoot.transform.localPosition.x, value, mGuiWin.mUiRoot.transform.localPosition.z));
            }
        }

        public UILayer uiLayer
        {
            get
            {
                return mUiLayer;
            }
            set
            {
                mUiLayer = value;
            }
        }

        public bool IsVisible()
        {
            return mGuiWin.mUiRoot.activeSelf;
        }

        public bool IsResReady
        {
            get
            {
                return mIsResReady;
            }
            set 
            {
                mIsResReady = value;
            }
        }

        public GuiWin guiWin()
        {
            return mGuiWin;
        }
    }
}
namespace SDK.Lib
{
    public class MMouse : MMouseOrTouch
    {
        public static MMouse MouseLeftButton = new MMouse(0);
        public static MMouse MouseRightButton = new MMouse(1);
        public static MMouse MouseMiddleButton = new MMouse(2);

        static MMouse[] mMouse = new MMouse[] { MouseLeftButton, MouseRightButton, MouseMiddleButton };

        protected int mButton;  // 0 左键 1 右键 2 中键

        private AddOnceEventDispatch mOnMouseDownDispatch;
        private AddOnceEventDispatch mOnMouseUpDispatch;
        private AddOnceEventDispatch mOnMousePressedDispatch;
        private AddOnceEventDispatch mOnMouseMovedDispatch;
        private AddOnceEventDispatch mOnMouseCanceledDispatch;

        static public MMouse GetMouse(int button)
        {
            return mMouse[button];
        }

        public MMouse(int button)
        {
            this.mButton = button;

            this.mOnMouseDownDispatch = new AddOnceEventDispatch();
            this.mOnMouseUpDispatch = new AddOnceEventDispatch();
            this.mOnMousePressedDispatch = new AddOnceEventDispatch();
            this.mOnMouseMovedDispatch = new AddOnceEventDispatch();
            this.mOnMouseCanceledDispatch = new AddOnceEventDispatch();
        }

        public void onTick(float delta)
        {
            if (UnityEngine.Input.GetMouseButtonDown(this.mButton))
            {
                // 按下的时候，设置位置相同
                this.mPos = UnityEngine.Input.mousePosition;
                this.mLastPos = this.mPos;

                this.handleMouseDown();
            }
            else if (UnityEngine.Input.GetMouseButtonUp(this.mButton))
            {
                // Up 的时候，先设置之前的位置，然后设置当前位置
                this.mLastPos = this.mPos;
                this.mPos = UnityEngine.Input.mousePosition;

                this.handleMouseUp();
            }
            else if (UnityEngine.Input.GetMouseButton(this.mButton))
            {
                // Press 的时候，先设置之前的位置，然后设置当前位置
                this.mLastPos = this.mPos;
                this.mPos = UnityEngine.Input.mousePosition;

                this.handleMousePress();
            }
            else
            {
                // 鼠标移动
                this.mLastPos = this.mPos;
                this.mPos = UnityEngine.Input.mousePosition;

                this.handleMouseMove();
            }
        }

        public void addMouseListener(EventId evtID, MAction<IDispatchObject> handle)
        {
            if (EventId.MOUSEDOWN_EVENT == evtID)
            {
                this.mOnMouseDownDispatch.addEventHandle(null, handle);
            }
            else if (EventId.MOUSEUP_EVENT == evtID)
            {
                this.mOnMouseUpDispatch.addEventHandle(null, handle);
            }
            else if (EventId.MOUSEPRESS_EVENT == evtID)
            {
                this.mOnMousePressedDispatch.addEventHandle(null, handle);
            }
            else if (EventId.MOUSEMove_EVENT == evtID)
            {
                this.mOnMouseMovedDispatch.addEventHandle(null, handle);
            }
        }

        public void removeMouseListener(EventId evtID, MAction<IDispatchObject> handle)
        {
            if (EventId.KEYUP_EVENT == evtID)
            {
                this.mOnMouseDownDispatch.removeEventHandle(null, handle);
            }
            else if (EventId.KEYDOWN_EVENT == evtID)
            {
                this.mOnMouseUpDispatch.removeEventHandle(null, handle);
            }
            else if (EventId.KEYPRESS_EVENT == evtID)
            {
                this.mOnMousePressedDispatch.removeEventHandle(null, handle);
            }
            else if (EventId.MOUSEMove_EVENT == evtID)
            {
                this.mOnMouseMovedDispatch.removeEventHandle(null, handle);
            }
        }

        // 是否还有需要处理的事件
        public bool hasEventHandle()
        {
            if (this.mOnMouseDownDispatch.hasEventHandle())
            {
                return true;
            }
            if (this.mOnMouseUpDispatch.hasEventHandle())
            {
                return true;
            }
            if (this.mOnMousePressedDispatch.hasEventHandle())
            {
                return true;
            }

            return false;
        }

        public void handleMouseDown()
        {
            if (null != this.mOnMouseDownDispatch)
            {
                this.mOnMouseDownDispatch.dispatchEvent(this);
            }
        }

        public void handleMouseUp()
        {
            if (null != this.mOnMouseUpDispatch)
            {
                this.mOnMouseUpDispatch.dispatchEvent(this);
            }
        }

        public void handleMousePress()
        {
            if (null != this.mOnMousePressedDispatch)
            {
                this.mOnMousePressedDispatch.dispatchEvent(this);
            }
        }

        public void handleMouseMove()
        {
            if(this.isPosChanged())
            {
                if (null != this.mOnMouseMovedDispatch)
                {
                    this.mOnMouseMovedDispatch.dispatchEvent(this);
                }
            }
        }
    }
}
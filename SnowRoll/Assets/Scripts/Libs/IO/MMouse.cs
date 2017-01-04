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
        private AddOnceEventDispatch mOnMousePressDispatch;
        private AddOnceEventDispatch mOnMouseMoveDispatch;
        private AddOnceEventDispatch mOnMousePressMoveDispatch;
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
            this.mOnMousePressDispatch = new AddOnceEventDispatch();
            this.mOnMouseMoveDispatch = new AddOnceEventDispatch();
            this.mOnMousePressMoveDispatch = new AddOnceEventDispatch();
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

                if (this.isPosChanged())
                {
                    this.handleMousePressMove();
                }
                else
                {
                    this.handleMousePress();
                }
            }
            else if(this.isPosChanged())     // 位置不相等的时候，就是移动
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
                this.mOnMousePressDispatch.addEventHandle(null, handle);
            }
            else if (EventId.MOUSEMOVE_EVENT == evtID)
            {
                this.mOnMouseMoveDispatch.addEventHandle(null, handle);
            }
            else if (EventId.MOUSEPRESS_MOVE_EVENT == evtID)
            {
                this.mOnMousePressMoveDispatch.addEventHandle(null, handle);
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
                this.mOnMousePressDispatch.removeEventHandle(null, handle);
            }
            else if (EventId.MOUSEMOVE_EVENT == evtID)
            {
                this.mOnMouseMoveDispatch.removeEventHandle(null, handle);
            }
            else if (EventId.MOUSEPRESS_MOVE_EVENT == evtID)
            {
                this.mOnMousePressMoveDispatch.removeEventHandle(null, handle);
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
            if (this.mOnMousePressDispatch.hasEventHandle())
            {
                return true;
            }
            if (this.mOnMousePressMoveDispatch.hasEventHandle())
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
            if (null != this.mOnMousePressDispatch)
            {
                this.mOnMousePressDispatch.dispatchEvent(this);
            }
        }

        public void handleMouseMove()
        {
            if(this.isPosChanged())
            {
                if (null != this.mOnMouseMoveDispatch)
                {
                    this.mOnMouseMoveDispatch.dispatchEvent(this);
                }
            }
        }

        public void handleMousePressMove()
        {
            if (this.isPosChanged())
            {
                if (null != this.mOnMousePressMoveDispatch)
                {
                    this.mOnMousePressMoveDispatch.dispatchEvent(this);
                }
            }
        }
    }
}
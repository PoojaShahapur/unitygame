namespace SDK.Lib
{
    public class MMouseDevice : MMouseOrTouch
    {
        public static MMouseDevice MouseLeftButton = new MMouseDevice(0);
        public static MMouseDevice MouseRightButton = new MMouseDevice(1);
        public static MMouseDevice MouseMiddleButton = new MMouseDevice(2);

        static MMouseDevice[] mMouse = new MMouseDevice[] { MouseLeftButton, MouseRightButton, MouseMiddleButton };

        static public MMouseDevice GetMouse(int button)
        {
            return mMouse[button];
        }

        public MMouseDevice(int button)
        {
            this.mTouchIndex = button;
        }

        public void onTick(float delta)
        {
            if (UnityEngine.Input.GetMouseButtonDown(this.mTouchIndex))
            {
                this.mPressTime = RealTime.time;
                this.mTouchBegan = true;
                this.mTouchEnd = false;

                // 按下的时候，设置位置相同
                this.mPos = UnityEngine.Input.mousePosition;
                this.mLastPos = this.mPos;

                this.handleMouseDown();
            }
            else if (UnityEngine.Input.GetMouseButtonUp(this.mTouchIndex))
            {
                this.mTouchBegan = false;
                this.mTouchEnd = true;

                // Up 的时候，先设置之前的位置，然后设置当前位置
                this.mLastPos = this.mPos;
                this.mPos = UnityEngine.Input.mousePosition;

                this.handleMouseUp();
            }
            else if (UnityEngine.Input.GetMouseButton(this.mTouchIndex))
            {
                this.mTouchBegan = false;
                this.mTouchEnd = false;

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
                this.mTouchBegan = false;
                this.mTouchEnd = false;

                // 鼠标移动
                this.mLastPos = this.mPos;
                this.mPos = UnityEngine.Input.mousePosition;

                this.handleMousePressOrMove();
            }
        }

        public void handleMouseDown()
        {
            if (!Ctx.mInstance.mInputMgr.mSimulateMouseWithTouches)
            {
                if (null != this.mOnMouseDownDispatch)
                {
                    this.mOnMouseDownDispatch.dispatchEvent(this);
                }
            }
            else
            {
                Ctx.mInstance.mMouseOrTouchDispatchSystem.handleTouchBegan(this);
            }
        }

        public void handleMouseUp()
        {
            if (!Ctx.mInstance.mInputMgr.mSimulateMouseWithTouches)
            {
                if (null != this.mOnMouseUpDispatch)
                {
                    this.mOnMouseUpDispatch.dispatchEvent(this);
                }
            }
            else
            {
                Ctx.mInstance.mMouseOrTouchDispatchSystem.handleTouchEnded(this);
            }
        }

        public void handleMousePress()
        {
            if (!Ctx.mInstance.mInputMgr.mSimulateMouseWithTouches)
            {
                if (null != this.mOnMousePressDispatch)
                {
                    this.mOnMousePressDispatch.dispatchEvent(this);
                }
            }
            else
            {
                Ctx.mInstance.mMouseOrTouchDispatchSystem.handleTouchStationary(this);
            }
        }

        public void handleMousePressOrMove()
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
                if (!Ctx.mInstance.mInputMgr.mSimulateMouseWithTouches)
                {
                    if (null != this.mOnMousePressMoveDispatch)
                    {
                        this.mOnMousePressMoveDispatch.dispatchEvent(this);
                    }
                }
                else
                {
                    Ctx.mInstance.mMouseOrTouchDispatchSystem.handleTouchMoved(this);
                }
            }
        }
    }
}
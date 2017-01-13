using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief JoyStick 输入信息
     */
    public class JoyStickTouchInfo
    {
        protected bool mIsValid;
        protected int mTouchId;     // 触碰 Id

        public JoyStickTouchInfo()
        {
            this.reset();
        }

        public void reset()
        {
            this.mIsValid = false;
            this.mTouchId = -1;
        }

        public bool onTouchBegin(MMouseOrTouch touch)
        {
            bool ret = false;

            if (!this.mIsValid)
            {
                if (touch.mPos.x < Ctx.mInstance.mResizeMgr.getHalfWidth())
                {
                    this.mIsValid = true;
                    this.mTouchId = touch.getTouchIndex();
                    ret = true;
                }
            }

            return ret;
        }

        public bool onTouchMove(MMouseOrTouch touch)
        {
            bool ret = false;

            if (this.mIsValid)
            {
                if (this.mTouchId == touch.getTouchIndex())
                {
                    if (touch.mPos.x >= Ctx.mInstance.mResizeMgr.getHalfWidth())
                    {
                        this.mIsValid = false;
                        ret = true;
                    }
                    else
                    {
                        ret = true;
                    }
                }
            }
            else
            {
                if (touch.mPos.x < Ctx.mInstance.mResizeMgr.getHalfWidth())
                {
                    this.mIsValid = true;
                    this.mTouchId = touch.getTouchIndex();
                    ret = true;
                }
            }

            return ret;
        }

        public bool onTouchEnd(MMouseOrTouch touch)
        {
            bool ret = false;

            if (this.mIsValid)
            {
                if (this.mTouchId == touch.getTouchIndex())
                {
                    this.mIsValid = false;
                    ret = true;
                }
            }

            return ret;
        }
    }
}
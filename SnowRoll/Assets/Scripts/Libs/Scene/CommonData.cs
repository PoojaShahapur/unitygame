using System;
using System.Collections.Generic;
namespace SDK.Lib
{
    /**
     * @brief 保存一些常用数据
     */
    public class CommonData
    {
        protected bool mIsClickSplit;        // 是否点击分裂

        protected bool mIsSplitSuccess;      // 是否分裂成功
        protected bool mIsEmitSuccess;       // 是否吞吐成功

        private float mEulerAngles_x;         // 相机转向角度
        private float mEulerAngles_y;

        public CommonData()
        {
            this.mIsClickSplit = false;
            this.mIsSplitSuccess = true;
            this.mIsEmitSuccess = true;

            this.mEulerAngles_x = 10.0f;
            this.mEulerAngles_y = 0.0f;
        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        public void setClickSplit(bool value)
        {
            this.mIsClickSplit = value;
        }

        public bool isClickSplit()
        {
            return this.mIsClickSplit;
        }

        public void setSplitSuccess(bool value)
        {
            this.mIsSplitSuccess = value;
        }

        public bool isSplitSuccess()
        {
            return this.mIsSplitSuccess;
        }

        public void setEmitSuccess(bool value)
        {
            this.mIsEmitSuccess = value;
        }

        public bool isEmitSuccess()
        {
            return this.mIsEmitSuccess;
        }

        public void setEulerAngles_x(float value)
        {
            this.mEulerAngles_x = value;
        }

        public float getEulerAngles_x()
        {
            return this.mEulerAngles_x;
        }

        public void setEulerAngles_y(float value)
        {
            this.mEulerAngles_y = value;
        }

        public float getEulerAngles_y()
        {
            return this.mEulerAngles_y;
        }

        public void resetEulerAngles_xy()
        {
            this.mEulerAngles_x = 10.0f;
            this.mEulerAngles_y = 0.0f;
        }
    }
}
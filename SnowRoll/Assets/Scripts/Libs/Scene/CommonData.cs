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

        public CommonData()
        {
            this.mIsClickSplit = false;
            this.mIsSplitSuccess = true;
            this.mIsEmitSuccess = true;
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
    }
}
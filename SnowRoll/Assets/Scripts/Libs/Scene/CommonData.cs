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

        public CommonData()
        {
            this.mIsClickSplit = false;
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
    }
}
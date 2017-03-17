namespace SDK.Lib
{
    public class LoopDepth
    {
        private int mLoopDepth;         // 是否在循环中，支持多层嵌套，就是循环中再次调用循环
        private CallFuncObjectNoParam mIncHandle;     // 增加处理器
        private CallFuncObjectNoParam mDecHandle;     // 减少处理器
        private CallFuncObjectNoParam mZeroHandle;    // 减少到 0 处理器

        public LoopDepth()
        {
            this.mLoopDepth = 0;
            this.mIncHandle = null;
            this.mDecHandle = null;
            this.mZeroHandle = null;
        }

        public void setIncHandle(MAction value)
        {
            if(null == this.mIncHandle)
            {
                this.mIncHandle = new CallFuncObjectNoParam();
            }

            this.mIncHandle.setThisAndHandleNoParam(null, value);
        }

        public void setDecHandle(MAction value)
        {
            if (null == this.mDecHandle)
            {
                this.mDecHandle = new CallFuncObjectNoParam();
            }

            this.mDecHandle.setThisAndHandleNoParam(null, value);
        }

        public void setZeroHandle(MAction value)
        {
            if (null == this.mZeroHandle)
            {
                this.mZeroHandle = new CallFuncObjectNoParam();
            }

            this.mZeroHandle.setThisAndHandleNoParam(null, value);
        }

        public void incDepth()
        {
            ++this.mLoopDepth;

            if(null != this.mIncHandle)
            {
                this.mIncHandle.call();
            }
        }

        public void decDepth()
        {
            --this.mLoopDepth;

            if (null != this.mDecHandle)
            {
                this.mDecHandle.call();
            }

            if(0 == this.mLoopDepth)
            {
                if (null != this.mZeroHandle)
                {
                    this.mZeroHandle.call();
                }
            }

            if(this.mLoopDepth < 0)
            {
                this.mLoopDepth = 0;
                // 错误，不对称
                UnityEngine.Debug.LogError("LoopDepth::decDepth, Error !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            }
        }

        public bool isInDepth()
        {
            return this.mLoopDepth > 0;
        }
    }
}
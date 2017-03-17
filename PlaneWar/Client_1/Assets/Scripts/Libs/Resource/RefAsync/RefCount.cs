namespace SDK.Lib
{
    /**
     * @brief 脚本引用计数
     */
    public class RefCount
    {
        protected uint mRefNum;                // 引用计数

        public RefCount()
        {
            this.mRefNum = 0;       // 引用计数从 1 改成 0 
        }

        public uint refNum
        {
            get
            {
                return this.mRefNum;
            }
            set
            {
                this.mRefNum = value;
            }
        }

        public void reset()
        {
            this.mRefNum = 0;
        }

        public void incRef()
        {
            ++this.mRefNum;
        }

        public void decRef()
        {
            --this.mRefNum;
        }

        public bool isNoRef()
        {
            return this.mRefNum == 0;
        }

        public void copyFrom(RefCount rhv)
        {
            this.mRefNum = rhv.refNum;
        }
    }
}
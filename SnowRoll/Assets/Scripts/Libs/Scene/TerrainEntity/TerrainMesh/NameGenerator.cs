namespace SDK.Lib
{
    /**
     * @brief 非线程安全的名字生成
     */
    public class MNameGenerator
    {
        protected string mPrefix;
        protected ulong mNext;

        public MNameGenerator(MNameGenerator rhs)
        {
            mPrefix = rhs.mPrefix;
            mNext = rhs.mNext;
        }

        public MNameGenerator(string prefix)
        {
            mPrefix = prefix;
            mNext = 1;
        }

        public string generate()
        {
            string ret = "";
            ret = string.Format("{0}{1}", mPrefix , mNext);
            mNext++;
            return ret;
        }

        public void reset()
        {
            mNext = 1;
        }

        public void setNext(ulong val)
        {
            mNext = val;
        }

        public ulong getNext()
        {
            return mNext;
        }
    }
}
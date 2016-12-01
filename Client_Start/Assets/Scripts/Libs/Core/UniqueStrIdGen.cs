namespace SDK.Lib
{
    /**
     * @brief 唯一字符串生成器
     */
    public class UniqueStrIdGen : UniqueNumIdGen
    {
        protected string mPrefix;
        protected string mRetId;

        public UniqueStrIdGen(string prefix, uint baseUniqueId)
            : base(baseUniqueId)
        {
            mPrefix = prefix;
        }

        public string genNewStrId()
        {
            mRetId = string.Format("{0}_{1}", mPrefix, this.genNewId());
            return mRetId;
        }
    }
}
namespace SDK.Lib
{
    /**
     * @brief 二进制资源
     */
    public class BytesRes : InsResBase
    {
        protected byte[] mBytes;

        public BytesRes()
        {

        }

        override protected void initImpl(ResItem res)
        {
            // 获取资源单独保存
            mBytes = res.getBytes(res.getPrefabName());
            base.initImpl(res);
        }

        override public void unload()
        {
            mBytes = null;

            base.unload();
        }

        public byte[] getBytes(string name)
        {
            return mBytes;
        }
    }
}
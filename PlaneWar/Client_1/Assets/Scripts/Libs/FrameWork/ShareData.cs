namespace SDK.Lib
{
    /**
     * @brief 共享内容，主要是数据
     */
    public class ShareData
    {
        public string mTmpStr = "";
        //public string m_retLangStr = "";     // 返回的语言描述，多线程访问会有问题，因此不用了
        public ByteBuffer mTmpBA;
        //public string m_resInPakPath = null;            // 返回的资源所在的包的目录
        public int noticeTimes = 0;
        public int noticeId = 0;
        public string noticeMsg = "";
    }
}
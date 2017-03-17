namespace SDK.Lib
{
    /**
     * @breif 聊天数据
     */
    public class ChatData
    {
        protected string mMsgStr = "";          // 临时存放聊天消息内容

        public ChatData()
        {

        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        public void appendStr(string str)
        {
            mMsgStr += str;
        }

        public string getStr()
        {
            return mMsgStr;
        }
    }
}
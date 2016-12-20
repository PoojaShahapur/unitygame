namespace SDK.Lib
{
    /**
     * @breif 聊天数据
     */
    public class ChatData
    {
        protected string m_msgStr = "";          // 临时存放聊天消息内容

        public void appendStr(string str)
        {
            m_msgStr += str;
        }

        public string getStr()
        {
            return m_msgStr;
        }
    }
}
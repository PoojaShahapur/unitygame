namespace SDK.Lib
{
    public class WebSocketMgr
    {
        public MWebSocketClient m_pWebSocketClient;

        public bool openSocket(string ip, int port)
        {
            //if (m_pWebSocketClient == null)
            //{
            //    m_pWebSocketClient = new MWebSocketClient();
            //}

            //m_pWebSocketClient.CreateClient(ip, port);

            return true;
        }

        public void closeSocket(string ip, int port)
        {
            //m_pWebSocketClient.Close();
        }

        public void send(string msg)
        {
            //m_pWebSocketClient.SendData(msg);
        }
    }
}
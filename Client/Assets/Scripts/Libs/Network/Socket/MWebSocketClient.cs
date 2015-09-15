//using WebSocket4Net;

namespace SDK.Lib
{
    public class MWebSocketClient
    {
        /*
        private WebSocketVersion m_Version;
        protected WebSocket m_pWebSocket;

        public string m_host = "localhost";
        public int m_port = 50000;

        protected AutoResetEvent MessageReceiveEvent = new AutoResetEvent(false);
        protected AutoResetEvent DataReceiveEvent = new AutoResetEvent(false);
        protected AutoResetEvent OpenedEvent = new AutoResetEvent(false);
        protected AutoResetEvent CloseEvent = new AutoResetEvent(false);
        protected string CurrentMessage { get; private set; }
        protected byte[] CurrentData { get; private set; }

        public MWebSocketClient()
        {

        }

        public WebSocket CreateClient(string ip, int port, WebSocketVersion version = WebSocketVersion.Rfc6455, bool autoConnect = true)
        {
            m_host = ip;
            m_port = port;
            m_Version = version;

            string endPoint = string.Format("ws://{0}:{1}", ip, port);
            //var webSocketClient = new WebSocket(endPoint, "basic", version);
            var webSocketClient = new WebSocket(endPoint);
            webSocketClient.Opened += new EventHandler(webSocketClient_Opened);
            webSocketClient.Closed += new EventHandler(webSocketClient_Closed);
            webSocketClient.DataReceived += new EventHandler<DataReceivedEventArgs>(webSocketClient_DataReceived);
            webSocketClient.MessageReceived += new EventHandler<MessageReceivedEventArgs>(webSocketClient_MessageReceived);

            if (autoConnect)
            {
                webSocketClient.Open();

                if (!OpenedEvent.WaitOne(1000))
                {
                    //Assert.Fail("Failed to open");
                }
            }

            m_pWebSocket = webSocketClient;
            return webSocketClient;
        }

        void webSocketClient_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            CurrentMessage = e.Message;
            MessageReceiveEvent.Set();
        }

        void webSocketClient_DataReceived(object sender, DataReceivedEventArgs e)
        {
            CurrentData = e.Data;
            DataReceiveEvent.Set();
        }

        void webSocketClient_Closed(object sender, EventArgs e)
        {
            CloseEvent.Set();
        }

        void webSocketClient_Opened(object sender, EventArgs e)
        {
            OpenedEvent.Set();
        }

        public void SendData(string msg)
        {
            var data = Encoding.UTF8.GetBytes(msg);
            m_pWebSocket.Send(data, 0, data.Length);

            if (!this.DataReceiveEvent.WaitOne(1000))
            {
                //Assert.Fail("Cannot get response in time!");
            }

            //Assert.AreEqual(msg, Encoding.UTF8.GetString(CurrentData));
        }

        public void Close()
        {
            m_pWebSocket.Close();

            if (!CloseEvent.WaitOne(1000))
            {
                //Assert.Fail("Failed to close session ontime");
            }
        }
        */
    }
}
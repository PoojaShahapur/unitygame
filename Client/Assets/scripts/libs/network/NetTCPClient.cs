using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using SDK.Common;

namespace SDK.Lib
{
    public class NetTCPClient
    {
        // 发送和接收的超时时间
        public int m_sendTimeout = 3;
        public int m_revTimeout = 3;

        public string m_host = "localhost";
        public int m_port = 50000;

        protected Socket m_socket = null;
        protected DataBuffer m_dataBuffer;
        protected bool m_canSend = true;                // 是否可以发送数据
        protected bool m_brecvThreadStart = false;      // 接收线程是否启动
        protected bool m_isConnected = false;

        public NetTCPClient(string ip, int port)
        {
            m_host = ip;
            m_port = port;

            m_dataBuffer = new DataBuffer();
        }

        public DataBuffer dataBuffer
        {
            get
            {
                return m_dataBuffer;
            }
        }

        public bool brecvThreadStart
        {
            get
            {
                return m_brecvThreadStart;
            }
            set
            {
                m_brecvThreadStart = value;
            }
        }

        public bool isConnected
        {
            get
            {
                return m_isConnected;
            }
        }

        // 连接服务器
        public bool Connect(string address, int remotePort)
        {
            if (m_socket != null && m_socket.Connected)
            {
                return true;
            }
            try
            {
                //获得远程服务器的地址
                IPAddress remoteAdd = IPAddress.Parse(address);
                IPEndPoint ipe = new IPEndPoint(remoteAdd, remotePort);
                // 创建socket
                m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // 开始连接
                m_socket.BeginConnect(ipe, new System.AsyncCallback(ConnectionCallback), m_socket);
                //break;
            }
            catch (System.Exception e)
            {
                // 连接失败
                Ctx.m_instance.m_log.log(e.Message);
                return false;
            }

            return true;
        }

        // 异步连接回调
        void ConnectionCallback(System.IAsyncResult ar)
        {
            try
            {
                // 与服务器取得连接
                m_socket.EndConnect(ar);
                m_isConnected = true;
                // 设置timeout
                //m_socket.SendTimeout = m_sendTimeout;
                //m_socket.ReceiveTimeout = m_revTimeout;

                #if !NETMULTHREAD
                Receive();
                #endif

                // 连接成功，通知
                if (Ctx.m_instance.m_sysMsgRoute.m_socketOpenedCB != null)
                {
                    Ctx.m_instance.m_sysMsgRoute.m_socketOpenedCB();
                }
            }
            catch (System.Exception e)
            {
                // 输出日志
                Ctx.m_instance.m_log.log(e.Message);
                // 错误处理
                //if ( e.GetType() == typeof(SocketException))
                //{
                //    if (((SocketException)e).SocketErrorCode == SocketError.ConnectionRefused)
                //    {
                //        // 输出日志
                //        Ctx.m_instance.m_log.log(e.Message);
                //    }
                //    else
                //    {
                //        // 输出日志
                //        Ctx.m_instance.m_log.log(e.Message);
                //    }
                //}

                //Disconnect(0);
            }
        }

        // 接受数据
        public void Receive()
        {
            // 只有 socket 连接的时候才继续接收数据
            if (m_socket.Connected)
            {
                // 接收从服务器返回的头信息
                m_socket.BeginReceive(m_dataBuffer.dynBuff.buff, 0, (int)m_dataBuffer.dynBuff.capacity, SocketFlags.None, new System.AsyncCallback(ReceiveData), 0);
            }
        }

        // 接收头消息
        void ReceiveData(System.IAsyncResult ar)
        {
            if (!checkAndUpdateConnect())        // 如果连接完成后直接断开，这个时候如果再使用 m_socket.EndReceive 这个函数就会抛出异常
            {
                return;
            }

            int read = 0;
            try
            {
                read = m_socket.EndReceive(ar);          // 获取读取的长度

                if (read > 0)
                {
                    Ctx.m_instance.m_log.synclog("接收到数据 " + read.ToString());

                    m_dataBuffer.dynBuff.size = (uint)read; // 设置读取大小
                    m_dataBuffer.moveDyn2Raw();             // 将接收到的数据放到原始数据队列
                    #if !NETMULTHREAD
                    m_dataBuffer.moveRaw2Msg();
                    #endif

                    Receive();                  // 继续接收
                }
            }
            catch (System.Exception e)
            {
                // 输出日志
                Ctx.m_instance.m_log.synclog(e.Message);
                Disconnect(0);
            }
        }

        // 发送消息
        public void Send()
        {
            if (!checkAndUpdateConnect())
            {
                return;
            }

            if (!m_canSend)
            {
                return;
            }

            if (0 == m_dataBuffer.sendTmpBuffer.size)
            {
                return;
            }

            m_canSend = false;

            try
            {
                m_dataBuffer.getSendData();
                m_socket.BeginSend(m_dataBuffer.sendBuffer.buff, 0, (int)m_dataBuffer.sendBuffer.size, 0, new System.AsyncCallback(SendCallback), 0);
            }
            catch (System.Exception e)
            {
                // 输出日志
                Ctx.m_instance.m_log.synclog(e.Message);
                Disconnect(0);
            }
        }

        //发送回调
        private void SendCallback(System.IAsyncResult ar)
        {
            if (!checkAndUpdateConnect())
            {
                return;
            }

            try
            {
                m_canSend = true;
                int bytesSent = m_socket.EndSend(ar);
                Ctx.m_instance.m_log.synclog("发送数据 " + bytesSent.ToString());
                Send();                 // 继续发送数据
            }
            catch (System.Exception e)
            {
                // 输出日志
                Ctx.m_instance.m_log.log(e.Message);
                Disconnect(0);
            }
        }

        // 关闭连接
        public void Disconnect(int timeout)
        {
            if (m_socket.Connected)
            {
                m_socket.Shutdown(SocketShutdown.Both);
                m_socket.Close(timeout);
            }
            else
            {
                m_socket.Close();
            }
        }
        
        // 检查并且更新连接状态
        protected bool checkAndUpdateConnect()
        {
            if (!m_socket.Connected)
            {
                m_isConnected = false;

                if (null != Ctx.m_instance.m_sysMsgRoute.m_socketClosedCB)
                {    
                    Ctx.m_instance.m_sysMsgRoute.m_socketClosedCB();
                }
            }

            return m_isConnected;
        }
    }
}
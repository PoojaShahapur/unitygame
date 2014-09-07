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

        // 连接服务器
        public bool Connect(string address, int remotePort)
        {
            if (m_socket != null && m_socket.Connected)
                return true;

            IPHostEntry hostEntry = Dns.GetHostEntry(address);
            foreach (IPAddress ip in hostEntry.AddressList)
            {
                try
                {
                    //获得远程服务器的地址
                    IPEndPoint ipe = new IPEndPoint(ip, remotePort);
                    // 创建socket
                    m_socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    // 开始连接
                    m_socket.BeginConnect(ipe, new System.AsyncCallback(ConnectionCallback), m_socket);
                    break;
                }
                catch (System.Exception e)
                {
                    // 连接失败
                    Ctx.m_instance.m_log.log(e.Message);
                    return false;
                }
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

                // 设置timeout
                m_socket.SendTimeout = m_sendTimeout;
                m_socket.ReceiveTimeout = m_revTimeout;

                // 接收从服务器返回的头信息
                m_socket.BeginReceive(m_dataBuffer.dynBuff.buff, 0, (int)m_dataBuffer.dynBuff.capacity, SocketFlags.None, new System.AsyncCallback(ReceiveData), 0);
            }
            catch (System.Exception e)
            {
                // 错误处理
                if ( e.GetType() == typeof(SocketException))
                {
                    if (((SocketException)e).SocketErrorCode == SocketError.ConnectionRefused)
                    {
                        // 输出日志
                        Ctx.m_instance.m_log.log(e.Message);
                    }
                    else
                    {
                        // 输出日志
                        Ctx.m_instance.m_log.log(e.Message);
                    }
                }

                Disconnect(0);
            }
        }


        // 接收头消息
        void ReceiveData(System.IAsyncResult ar)
        {
            try
            {
                int read = m_socket.EndReceive(ar);          // 获取读取的长度

                // 服务器断开连接
                if (read < 1)
                {
                    Disconnect(0);
                    return;
                }
                m_dataBuffer.dynBuff.size = (uint)read; // 设置读取大小
                m_dataBuffer.moveDyn2Raw();             // 将接收到的数据放到原始数据队列

                // 下一个读取
                m_socket.BeginReceive(m_dataBuffer.dynBuff.buff, 0, (int)m_dataBuffer.dynBuff.capacity, SocketFlags.None, new System.AsyncCallback(ReceiveData), 0);  
            }
            catch (System.Exception e)
            {
                // 输出日志
                Ctx.m_instance.m_log.log(e.Message);
                Disconnect(0);
            }
        }

        // 发送消息
        public void Send()
        {
            if (!m_socket.Connected)
                return;

            if(m_dataBuffer.sendBuffer.size == 0)
            {
                return;
            }

            NetworkStream ns;
            lock (m_socket)
            {
                ns = new NetworkStream(m_socket);
            }

            if (ns.CanWrite)
            {
                try
                {
                    //ns.BeginWrite(m_dataBuffer.sendBuffer.buff, 0, (int)m_dataBuffer.sendBuffer.size, new System.AsyncCallback(SendCallback), ns);
                    m_dataBuffer.sendBuffer.getByte2Stream(ns, new System.AsyncCallback(SendCallback));
                }
                catch (System.Exception e)
                {
                    // 输出日志
                    Ctx.m_instance.m_log.log(e.Message);
                    Disconnect(0);
                }
            }
        }

        //发送回调
        private void SendCallback(System.IAsyncResult ar)
        {
            NetworkStream ns = (NetworkStream)ar.AsyncState;
            try
            {
                ns.EndWrite(ar);
                ns.Flush();
                ns.Close();
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
                m_socket.Shutdown(SocketShutdown.Receive);
                m_socket.Close(timeout);
            }
            else
            {
                m_socket.Close();
            }
        }
    }
}
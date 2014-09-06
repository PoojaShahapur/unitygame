using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace San.Guo
{
    public class NetTCPClient
    {
        // 发送和接收的超时时间
        public int _sendTimeout = 3;
        public int _revTimeout = 3;

        public string m_host = "localhost";
        public int m_port = 50000;

        protected Socket _socket = null;
        protected DataBuffer m_dataBuffer;

        public NetTCPClient(string ip, int port)
        {
            m_host = ip;
            m_port = port;
        }

        // 连接服务器
        public bool Connect(string address, int remotePort)
        {
            if (_socket != null && _socket.Connected)
                return true;

            IPHostEntry hostEntry = Dns.GetHostEntry(address);
            foreach (IPAddress ip in hostEntry.AddressList)
            {
                try
                {
                    //获得远程服务器的地址
                    IPEndPoint ipe = new IPEndPoint(ip, remotePort);
                    // 创建socket
                    _socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    // 开始连接
                    _socket.BeginConnect(ipe, new System.AsyncCallback(ConnectionCallback), _socket);
                    break;
                }
                catch (System.Exception e)
                {
                    // 连接失败
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
                _socket.EndConnect(ar);

                // 设置timeout
                _socket.SendTimeout = _sendTimeout;
                _socket.ReceiveTimeout = _revTimeout;

                // 接收从服务器返回的头信息
                _socket.BeginReceive(m_dataBuffer.dynBuff.buff, 0, (int)m_dataBuffer.dynBuff.capacity, SocketFlags.None, new System.AsyncCallback(ReceiveData), 0);
            }
            catch (System.Exception e)
            {
                // 错误处理
                if ( e.GetType() == typeof(SocketException))
                {
                    if (((SocketException)e).SocketErrorCode == SocketError.ConnectionRefused)
                    {
                        // 输出日志
                    }
                    else
                    {
                        // 输出日志
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
                int read = _socket.EndReceive(ar);          // 获取读取的长度

                // 服务器断开连接
                if (read < 1)
                {
                    Disconnect(0);
                    return;
                }
                m_dataBuffer.dynBuff.size = (uint)read; // 设置读取大小
                m_dataBuffer.moveDyn2Raw();             // 将接收到的数据放到原始数据队列

                // 下一个读取
                _socket.BeginReceive(m_dataBuffer.dynBuff.buff, 0, (int)m_dataBuffer.dynBuff.capacity, SocketFlags.None, new System.AsyncCallback(ReceiveData), 0);  
            }
            catch (System.Exception e)
            {
                // 输出日志
                Disconnect(0);
            }
        }

        // 发送消息
        public void Send()
        {
            if (!_socket.Connected)
                return;

            NetworkStream ns;
            lock (_socket)
            {
                ns = new NetworkStream(_socket);
            }

            if (ns.CanWrite)
            {
                try
                {
                    ns.BeginWrite(m_dataBuffer.sendBuffer.buff, 0, (int)m_dataBuffer.sendBuffer.size, new System.AsyncCallback(SendCallback), ns);
                }
                catch (System.Exception )
                {
                    // 输出日志
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
            catch (System.Exception)
            {
                // 输出日志
                Disconnect(0);
            }

        }

        // 关闭连接
        public void Disconnect(int timeout)
        {
            if (_socket.Connected)
            {
                _socket.Shutdown(SocketShutdown.Receive);
                _socket.Close(timeout);
            }
            else
            {
                _socket.Close();
            }
        }
    }
}

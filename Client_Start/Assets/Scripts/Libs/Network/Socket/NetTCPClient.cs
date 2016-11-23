using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;

namespace SDK.Lib
{
    public class NetTCPClient
    {
        // 发送和接收的超时时间
        public int mConnectTimeout = 5000;
        // 超时值（以毫秒为单位）。如果将该属性设置为 1 到 499 之间的值，该值将被更改为 500。默认值为 0，指示超时期限无限大。指定 -1 还会指示超时期限无限大。
        //public int m_sendTimeout = 5000;
        //public int m_revTimeout = 0;

        public string mIp;
        public int mPort;

        protected Socket mSocket = null;
        protected ClientBuffer mClientBuffer;
        protected bool mIsRecvThreadStart;      // 接收线程是否启动
        protected bool mIsConnected;

        protected MEvent mMsgSendEndEvent;       // 当前所有的消息都发送出去了，通知等待线程
        protected MMutex mSendMutex;   // 读互斥

        public NetTCPClient(string ip = "localhost", int port = 5000)
        {
            mIsRecvThreadStart = false;
            mIsConnected = false;
            mMsgSendEndEvent = new MEvent(false);
            mSendMutex = new MMutex(false, "NetTCPClient_SendMutex");

            mIp = ip;
            mPort = port;

            mClientBuffer = new ClientBuffer();
            mClientBuffer.setEndian(SystemEndian.msServerEndian);     // 设置服务器字节序
        }

        public ClientBuffer clientBuffer
        {
            get
            {
                return mClientBuffer;
            }
        }

        public bool brecvThreadStart
        {
            get
            {
                return mIsRecvThreadStart;
            }
            set
            {
                mIsRecvThreadStart = value;
            }
        }

        public bool isConnected
        {
            get
            {
                return mIsConnected;
            }
        }

        public MEvent msgSendEndEvent
        {
            get
            {
                return mMsgSendEndEvent;
            }
            set
            {
                mMsgSendEndEvent = value;
            }
        }

        // 是否可以发送新的数据，上一次发送的数据是否发送完成，只有上次发送的数据全部发送完成，才能发送新的数据
        public bool canSendNewData()
        {
            return (mClientBuffer.sendBuffer.bytesAvailable == 0);
        }

        // 设置接收缓冲区大小，和征途服务器对接，这个一定要和服务器大小一致，并且一定要是 8 的整数倍，否则在消息比较多，并且一个包发送过来的时候，会出错
        public void SetRevBufferSize(int size)
        {
            mSocket.ReceiveBufferSize = size;      // ReceiveBufferSize 默认 8096 字节
            mClientBuffer.SetRevBufferSize(size);
        }

        public void SetSendBufferSize(int size)
        {
            mSocket.SendBufferSize = size;      // SendBufferSize 默认 8096 字节
        }

        // 连接服务器
        public bool Connect(string address, int remotePort)
        {
            if (mSocket != null && mSocket.Connected)
            {
                return true;
            }
            try
            {
                //获得远程服务器的地址
                IPAddress remoteAdd = IPAddress.Parse(address);
                IPEndPoint ipe = new IPEndPoint(remoteAdd, remotePort);
                // 创建socket
                mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // 开始连接
                IAsyncResult result = mSocket.BeginConnect(ipe, new System.AsyncCallback(ConnectionCallback), mSocket);
                // 这里做一个超时的监测，当连接超过5秒还没成功表示超时
                bool success = result.AsyncWaitHandle.WaitOne(mConnectTimeout, true);
                if (!success)
                {
                    //超时
                    //Disconnect(0);
                    Ctx.mInstance.mLogSys.log("socket connect Time Out");
                }
                else
                {
                    // 设置建立链接标识
                    mIsConnected = true;
                    // 打印端口信息
                    string ipPortStr;

                    ipPortStr = string.Format("local IP: {0}, Port: {1}", ((IPEndPoint)mSocket.LocalEndPoint).Address.ToString(), ((IPEndPoint)mSocket.LocalEndPoint).Port.ToString());
                    Ctx.mInstance.mLogSys.log(ipPortStr);

                    ipPortStr = string.Format("Remote IP: {0}, Port: {1}", ((IPEndPoint)mSocket.RemoteEndPoint).Address.ToString(), ((IPEndPoint)mSocket.RemoteEndPoint).Port.ToString());
                    Ctx.mInstance.mLogSys.log(ipPortStr);
                }
            }
            catch (System.Exception e)
            {
                // 连接失败
                Ctx.mInstance.mLogSys.error(e.Message);
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
                mSocket.EndConnect(ar);
                mIsConnected = true;
                // 设置选项
                mSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
                SetRevBufferSize(8096);
                // 设置 timeout
                //mSocket.SendTimeout = m_sendTimeout;
                //mSocket.ReceiveTimeout = m_revTimeout;

                if (!MacroDef.NET_MULTHREAD)
                {
                    Receive();
                }

                // 连接成功，通知
                // 这个在主线程中调用
                Ctx.mInstance.mSysMsgRoute.push(new SocketOpenedMR());
            }
            catch (System.Exception e)
            {
                // 错误处理
                if (e.GetType() == typeof(SocketException))
                {
                    if (((SocketException)e).SocketErrorCode == SocketError.ConnectionRefused)
                    {
                        // 输出日志
                        Ctx.mInstance.mLogSys.log(e.Message);
                    }
                    else
                    {
                        // 输出日志
                        Ctx.mInstance.mLogSys.log(e.Message);
                    }
                }
                else
                {
                    // 输出日志
                    Ctx.mInstance.mLogSys.error(e.Message);
                }

                // 一旦建立失败
                //Disconnect();
            }
        }

        // 接受数据
        public void Receive()
        {
            // 只有 socket 连接的时候才继续接收数据
            if (mSocket.Connected)
            {
                // 接收从服务器返回的信息
                IAsyncResult asyncSend = mSocket.BeginReceive(mClientBuffer.dynBuffer.buffer, 0, (int)mClientBuffer.dynBuffer.capacity, SocketFlags.None, new System.AsyncCallback(ReceiveData), 0);

                //checkThread();

                //bool success = asyncSend.AsyncWaitHandle.WaitOne(m_revTimeout, true);
                //if (!success)
                //{
                //    Ctx.mInstance.mLogSys.asyncLog(string.Format("RecvMsg Timeout {0} ", m_revTimeout));
                //}
            }
        }

        // 接收头消息
        void ReceiveData(System.IAsyncResult ar)
        {
            if (!checkAndUpdateConnect())        // 如果连接完成后直接断开，这个时候如果再使用 mSocket.EndReceive 这个函数就会抛出异常
            {
                return;
            }

            //checkThread();

            if (mSocket == null)        // SocketShutdown.Both 这样关闭，只有还会收到数据，因此判断一下
            {
                return;
            }

            int read = 0;
            try
            {
                read = mSocket.EndReceive(ar);          // 获取读取的长度

                if (read > 0)
                {
                    Ctx.mInstance.mLogSys.log("接收到数据 " + read.ToString());
                    mClientBuffer.dynBuffer.size = (uint)read; // 设置读取大小
                    //mClientBuffer.moveDyn2Raw();             // 将接收到的数据放到原始数据队列
                    //mClientBuffer.moveRaw2Msg();             // 将完整的消息移动到消息缓冲区
                    mClientBuffer.moveDyn2Raw_KBE();
                    mClientBuffer.moveRaw2Msg_KBE();
                    Receive();                  // 继续接收
                }
                else
                {
                    // Socket 已经断开或者异常，需要断开链接
                    Disconnect(0);
                }
            }
            catch (System.Exception e)
            {
                // 输出日志
                Ctx.mInstance.mLogSys.error(e.Message);
                Ctx.mInstance.mLogSys.error("接收数据出错");
                // 断开链接
                Disconnect(0);
            }
        }

        // 发送消息
        public void Send()
        {
            using (MLock mlock = new MLock(mSendMutex))
            {
                if (!checkAndUpdateConnect())
                {
                    return;
                }

                //checkThread();

                if (mSocket == null)
                {
                    return;
                }

                if (mClientBuffer.sendBuffer.bytesAvailable == 0)     // 如果发送缓冲区没有要发送的数据
                {
                    if (mClientBuffer.sendTmpBuffer.circularBuffer.size > 0)      // 如果发送临时缓冲区有数据要发
                    {
                        mClientBuffer.getSocketSendData();
                    }

                    if (mClientBuffer.sendBuffer.bytesAvailable == 0)        // 如果发送缓冲区中确实没有数据
                    {
                        if (MacroDef.NET_MULTHREAD)
                        {
                            mMsgSendEndEvent.Set();        // 通知等待线程，所有数据都发送完成
                        }
                        return;
                    }
                }

                try
                {
                    Ctx.mInstance.mLogSys.log(string.Format("开始发送字节数 {0} ", mClientBuffer.sendBuffer.bytesAvailable));

                    IAsyncResult asyncSend = mSocket.BeginSend(mClientBuffer.sendBuffer.dynBuffer.buffer, (int)mClientBuffer.sendBuffer.position, (int)mClientBuffer.sendBuffer.bytesAvailable, 0, new System.AsyncCallback(SendCallback), 0);
                    //bool success = asyncSend.AsyncWaitHandle.WaitOne(m_sendTimeout, true);
                    //if (!success)
                    //{
                    //    Ctx.mInstance.mLogSys.asyncLog(string.Format("SendMsg Timeout {0} ", m_sendTimeout));
                    //}
                }
                catch (System.Exception e)
                {
                    if (MacroDef.NET_MULTHREAD)
                    {
                        mMsgSendEndEvent.Set();        // 发生异常，通知等待线程，所有数据都发送完成，防止等待线程不能解锁
                    }
                    // 输出日志
                    Ctx.mInstance.mLogSys.error(e.Message);
                    // 断开链接
                    Disconnect(0);
                }
            }
        }

        //发送回调
        private void SendCallback(System.IAsyncResult ar)
        {
            using (MLock mlock = new MLock(mSendMutex))
            {
                if (!checkAndUpdateConnect())
                {
                    return;
                }

                //checkThread();

                try
                {
                    int bytesSent = mSocket.EndSend(ar);
                    Ctx.mInstance.mLogSys.log(string.Format("结束发送字节数 {0} ", bytesSent));

                    if (mClientBuffer.sendBuffer.length < mClientBuffer.sendBuffer.position + (uint)bytesSent)
                    {
                        Ctx.mInstance.mLogSys.log(string.Format("结束发送字节数错误 {0}", bytesSent));
                        mClientBuffer.sendBuffer.setPos(mClientBuffer.sendBuffer.length);
                    }
                    else
                    {
                        mClientBuffer.sendBuffer.setPos(mClientBuffer.sendBuffer.position + (uint)bytesSent);
                    }

                    if (mClientBuffer.sendBuffer.bytesAvailable > 0)     // 如果上一次发送的数据还没发送完成，继续发送
                    {
                        Send();                 // 继续发送数据
                    }
                }
                catch (System.Exception e)
                {
                    // 输出日志
                    Ctx.mInstance.mLogSys.error(e.Message);
                    Disconnect(0);
                }
            }
        }

        // 关闭连接
        public void Disconnect(int timeout = 0)
        {
            // 关闭之后 mSocket.Connected 设置成 false
            if (mSocket != null)
            {
                if (mSocket.Connected)
                {
                    mSocket.Shutdown(SocketShutdown.Both);
                    //mSocket.Close(timeout);  // timeout 不能是 0 ，是 0 含义未定义
                    if (timeout > 0)
                    {
                        mSocket.Close(timeout);
                    }
                    else
                    {
                        mSocket.Close();
                    }
                }
                else
                {
                    mSocket.Close();
                }

                mSocket = null;
            }
        }
        
        // 检查并且更新连接状态
        protected bool checkAndUpdateConnect()
        {
            if (mSocket != null && !mSocket.Connected)
            {
                if (mIsConnected)
                {
                    Ctx.mInstance.mSysMsgRoute.push(new SocketCloseedMR());
                }
                mIsConnected = false;
            }

            return mIsConnected;
        }

        protected bool checkThread()
        {
            if(Ctx.mInstance.mNetMgr.isNetThread(Thread.CurrentThread.ManagedThreadId))
            {
                return true;
            }

            return false;
        }
    }
}
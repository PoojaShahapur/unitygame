using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;

namespace San.Guo
{
    public class NetworkSocket
    {
        public String m_host = "localhost";
        public Int32 m_port = 50000;

        internal Boolean m_socketReady = false;
        TcpClient m_tcpSocket;
        NetworkStream m_netStream;

        StreamWriter m_socketWriter;
        StreamReader m_socketReader;

        CirculeBuffer m_rawBuffer;      // ֱ�Ӵӷ��������յ���ԭʼ�����ݣ�����ѹ���ͼ��ܹ�
        CirculeBuffer m_msgBuffer;      // ����ʹ�õĻ�����
        CirculeBuffer m_sendBuffer;     // ���ͻ�����

        protected byte[] m_buff;
        protected uint m_len;

        public NetworkSocket(string host, Int32 port)
        {
            m_host = host;
            m_port = port;

            m_rawBuffer = new CirculeBuffer();
            m_msgBuffer = new CirculeBuffer();
            m_sendBuffer = new CirculeBuffer();

            m_buff = new byte[64 * 1024];
        }

        void Update()
        {
            // ��������
            readSocket();    // ��ȡ����
            if (m_len > 0)                // ���������
            {
                m_rawBuffer.pushBack(m_buff, 0, m_len);      // �Ž�ԭʼ��������
                // Debug.Log(received_data);
            }

            // �������ݴ���
        }

        public void connect()
        {
            try
            {
                m_tcpSocket = new TcpClient(m_host, m_port);

                m_netStream = m_tcpSocket.GetStream();
                m_socketWriter = new StreamWriter(m_netStream);
                m_socketReader = new StreamReader(m_netStream);

                m_socketReady = true;
            }
            catch (Exception e)
            {
                // Something went wrong
                Debug.Log("Socket error: " + e);
            }
        }

        public void writeSocket(string line)
        {
            if (!m_socketReady)
                return;

            line = line + "\r\n";
            m_socketWriter.Write(line);
            m_socketWriter.Flush();
        }

        public void readSocket()
        {
            if (!m_socketReady)
            {
                m_len = 0;
            }

            if (m_netStream.DataAvailable)
            {
                m_len = (uint)m_netStream.Length;
                m_socketReader.BaseStream.Read(m_buff, 0, (int)m_len);
            }
        }

        public void closeSocket()
        {
            if (!m_socketReady)
                return;

            m_socketWriter.Close();
            m_socketReader.Close();
            m_tcpSocket.Close();
            m_socketReady = false;
        }
    }
}
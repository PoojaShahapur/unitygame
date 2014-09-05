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

        public NetworkSocket(string host, Int32 port)
        {
            m_host = host;
            m_port = port;

            m_rawBuffer = new CirculeBuffer();
            m_msgBuffer = new CirculeBuffer();
            m_sendBuffer = new CirculeBuffer();
        }

        void Update()
        {
            // ��������
            string received_data = readSocket();    // ��ȡ����
            if (received_data != "")                // ���������
            {
                m_rawBuffer.pushBack(received_data.ToCharArray());      // �Ž�ԭʼ��������
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

        public String readSocket()
        {
            if (!m_socketReady)
                return "";

            if (m_netStream.DataAvailable)
                return m_socketReader.ReadLine();

            return "";
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
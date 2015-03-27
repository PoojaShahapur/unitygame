﻿using System.Threading;

namespace SDK.Lib
{
    /**
     * @brief 互斥
     */
    public class MMutex
    {
        private Mutex m_mutex;   // 读互斥

        public MMutex(bool initiallyOwned, string name)
        {
            #if NET_MULTHREAD
            m_mutex = new Mutex(initiallyOwned, name);
            #endif
        }

        public void WaitOne()
        {
            #if NETMULTHREAD
            m_mutex.WaitOne();
            #endif
        }

        public void ReleaseMutex()
        {
            #if NETMULTHREAD
            m_mutex.ReleaseMutex();
            #endif
        }

        public void close()
        {
            #if NETMULTHREAD
            m_mutex.Close();
            #endif
        }
    }
}
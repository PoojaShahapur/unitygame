using System.Threading;

namespace SDK.Lib
{
    /**
     * @brief 互斥
     */
    public class MMutex
    {
        private Mutex m_mutex; 	// 读互斥
		private string mName;	// name

        public MMutex(bool initiallyOwned, string name)
        {
            if (MacroDef.NET_MULTHREAD)
            {
                // IOS 下不支持，错误提示如下： "Named mutexes are not supported"
                //m_mutex = new Mutex(initiallyOwned, name);
				m_mutex = new Mutex(initiallyOwned);
				mName = name;
            }
        }

        public void WaitOne()
        {
            if (MacroDef.NET_MULTHREAD)
            {
                m_mutex.WaitOne();
            }
        }

        public void ReleaseMutex()
        {
            if (MacroDef.NET_MULTHREAD)
            {
                m_mutex.ReleaseMutex();
            }
        }

        public void close()
        {
            if (MacroDef.NET_MULTHREAD)
            {
                m_mutex.Close();
            }
        }
    }
}
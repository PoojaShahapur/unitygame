using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace San.Guo
{
    public class Context
    {
        public INetworkMgr m_netMgr;
        public Config m_cfg;
        static public Context m_instance;
        public ILogger m_log;

        public Context()
        {

        }

        static public Context instance
        {
            get
            {
                if(m_instance == null)
                {
                    m_instance = new Context();
                }

                return m_instance;
            }
        }

        public void Awake()
        {
            
        }

        // Use this for initialization
        public void Start()
        {
            m_netMgr.openSocket(m_cfg.m_ip, m_cfg.m_port);
        }

        // Update is called once per frame
        public void Update() 
        {
            ByteArray ret = m_netMgr.getMsg();
	    }

        public void OnApplicationQuit()
        {

        }
    }
}

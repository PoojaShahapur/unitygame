using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace San.Guo
{
    public class DB
    {
        public INetworkMgr m_netMgr;
        public Config m_cfg;
        static public DB m_instance;

        static public DB instance
        {
            get
            {
                if(m_instance == null)
                {
                    m_instance = new DB();
                }

                return m_instance;
            }
        }

        public void Awark()
        {
            m_netMgr = new NetworkMgr();
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

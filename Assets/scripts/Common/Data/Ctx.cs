﻿using UnityEngine;
using System;
using System.Collections.Generic;

namespace SDK.Common
{
    public class Ctx
    {
        public INetworkMgr m_netMgr;
        public Config m_cfg;
        static public Ctx m_instance;
        public ILogger m_log;
        public IResMgr m_resMgr;
        public IInputMgr m_inputMgr;
        public Transform m_dataTrans;

        public Ctx()
        {

        }

        //static public Ctx instance
        //{
        //    get
        //    {
        //        if(m_instance == null)
        //        {
        //            m_instance = new Ctx();
        //        }

        //        return m_instance;
        //    }
        //}

        public void Awake()
        {
            
        }

        // Use this for initialization
        public void Start()
        {
            //m_netMgr.openSocket(m_cfg.m_ip, m_cfg.m_port);
        }

        // Update is called once per frame
        public void Update() 
        {
            // 处理网络
            //ByteArray ret = m_netMgr.getMsg();

            // 处理 input
            m_inputMgr.handleKeyBoard();
	    }

        public void OnApplicationQuit()
        {

        }
    }
}

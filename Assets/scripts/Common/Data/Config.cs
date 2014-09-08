using System;
using System.Collections.Generic;

namespace SDK.Common
{
    /**
     * @brief 基本的配置
     */
    public class Config
    {
        public string m_ip = "127.0.0.1";
        public int m_port = 50000;
        public string m_webIP = "http://127.0.0.1/";

        public string[] m_pathLst;

        public Config()
        {
            m_pathLst = new string[(int)ResPathType.eTotal];
            m_pathLst[(int)ResPathType.ePathScene] = "Game/Scene/";
            m_pathLst[(int)ResPathType.ePathUI] = "Game/UI/";
        }
    }
}

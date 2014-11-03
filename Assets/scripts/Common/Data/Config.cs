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
        //public string m_webIP = "http://127.0.0.1/";
        public string m_webIP = "";

        public string[] m_pathLst;
        public ResLoadType m_resLoadType;   // 资源加载类型

        public Config()
        {
            m_resLoadType = ResLoadType.eLoadDisc;
            m_pathLst = new string[(int)ResPathType.eTotal];
            m_pathLst[(int)ResPathType.ePathScene] = "StreamingAssets/Scene/";
            m_pathLst[(int)ResPathType.ePathSceneXml] = "StreamingAssets/Scene/Xml/";
            m_pathLst[(int)ResPathType.ePathModule] = "StreamingAssets/Module/";
            m_pathLst[(int)ResPathType.ePathComUI] = "StreamingAssets/UI/";
        }
    }
}

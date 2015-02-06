using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 宏定义说明区域
     * @def DEBUG_NOTNET 没有网络处理
     */

    /**
     * @brief 基本的配置
     */
    public class Config
    {
        public const string StreamingAssets = "StreamingAssets/";

        public string m_ip = "192.168.125.254";
        public int m_port = 10002;
        //public string m_webIP = "http://127.0.0.1/";
        public string m_webIP = "";

        public string[] m_pathLst;
        public ResLoadType m_resLoadType;   // 资源加载类型
        public string m_dataPath;
        //public bool m_bNeedNet = false;                     // 是否需要网络

        public Config()
        {
            m_resLoadType = ResLoadType.eLoadDisc;
            m_pathLst = new string[(int)ResPathType.eTotal];
            m_pathLst[(int)ResPathType.ePathScene] = "Scene/";
            m_pathLst[(int)ResPathType.ePathSceneXml] = "Scene/Xml/";
            m_pathLst[(int)ResPathType.ePathModule] = "Module/";
            m_pathLst[(int)ResPathType.ePathComUI] = "UI/";
            m_pathLst[(int)ResPathType.ePathBeingPath] = "Being/";
            m_pathLst[(int)ResPathType.ePathAIPath] = "AI/";
            m_pathLst[(int)ResPathType.ePathTablePath] = "Table/";
            m_pathLst[(int)ResPathType.ePathLangXml] = "Languages/";
            m_pathLst[(int)ResPathType.ePathXmlCfg] = "XmlConfig/";
            m_pathLst[(int)ResPathType.ePathModel] = "Model/";
            m_pathLst[(int)ResPathType.ePathMaterial] = "Model/Materials/";

            m_dataPath = Application.dataPath;
        }
    }
}
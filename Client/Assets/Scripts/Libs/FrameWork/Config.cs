using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 宏定义说明区域
     */

    /**
     * @brief 基本的配置
     */
    public class Config
    {
        // Release 自己使用的定义
        //NET_MULTHREAD;MSG_ENCRIPT;MSG_COMPRESS

        // Debug 自己使用的定义
        //NET_MULTHREAD;MSG_ENCRIPT;MSG_COMPRESS;THREAD_CALLCHECK;ENABLE_WINLOG;ENABLE_NETLOG;UNIT_TEST;ENABLE_FILELOG

        // 宏定义开始
        // 调试不需要网络
        static public bool DEBUG_NOTNET = false;

        // 网络处理多线程，主要是调试的时候使用单线程，方便调试，运行的时候使用多线程
        static public bool NET_MULTHREAD = true;

        // 是否检查函数接口调用线程
        static public bool THREAD_CALLCHECK = true;

        // 消息加密
        static public bool MSG_ENCRIPT = true;

        // 消息压缩
        static public bool MSG_COMPRESS = true;

        // 开启窗口日志
        static public bool ENABLE_WINLOG = true;

        // 开启网络日志
        static public bool ENABLE_NETLOG = true;

        // 开启文件日志
        static public bool ENABLE_FILELOG = true;

        // 单元测试
        static public bool UNIT_TEST = true;

        // 外网测试
        static public bool KOKSERVER_TEST = false;

        // 不使用的代码
        static public bool DEPRECATE_CODE = false;

        // 使用打包模式加载资源
        static public bool PKG_RES_LOAD = false;

        // 非打包文件系统资源加载
        static public bool UnPKG_RES_LOAD = false;

        // 是否开启 EnableProtoBuf
        static public bool EnableProtoBuf = false;

        // 是否开启 SharpZipLib
        static public bool ENABLE_SHARP_ZIP_LIB = false;

        // 宏定义结束

        public static string StreamingAssets;
        public static string UIModelLayer;

#if KOKSERVER_TEST
        public string m_ip = "222.73.30.21";
        public int m_port = 7000;
#else
        public string m_ip;
        public int m_port;
        public ushort m_zone;
#endif

        public string m_webIP;               // web 服务器
        public int m_webPort;

        public string[] m_pathLst;
        public ResLoadType m_resLoadType;   // 资源加载类型
        public string m_dataPath;
        //public bool m_bNeedNet = false;                       // 是否需要网络
        public string m_netLogPhp;       // Php 处理文件
        public List<string> m_pakExtNameList;       // 打包的扩展名字列表

        public Config()
        {
            StreamingAssets = "StreamingAssets/";
            UIModelLayer = "UIModel";

            m_ip = "192.168.122.253";
            m_port = 10002;
            m_zone = 30;

            m_webIP = "http://127.0.0.1/UnityServer/";
            m_webPort = 80;
            m_netLogPhp = "/netlog/NetLog.php";
            m_pakExtNameList = new List<string>();

            m_resLoadType = ResLoadType.eLoadDisc;
            m_pathLst = new string[(int)ResPathType.eTotal];
            m_pathLst[(int)ResPathType.ePathScene] = "Scenes/";
            m_pathLst[(int)ResPathType.ePathSceneXml] = "Scenes/Xml/";
            m_pathLst[(int)ResPathType.ePathModule] = "Module/";
            m_pathLst[(int)ResPathType.ePathComUI] = "UI/";
            m_pathLst[(int)ResPathType.ePathComUIScene] = "UIScene/";
            m_pathLst[(int)ResPathType.ePathBeingPath] = "Being/";
            m_pathLst[(int)ResPathType.ePathAIPath] = "AI/";
            m_pathLst[(int)ResPathType.ePathTablePath] = "Table/";
            m_pathLst[(int)ResPathType.ePathLangXml] = "Languages/";
            m_pathLst[(int)ResPathType.ePathXmlCfg] = "XmlConfig/";
            m_pathLst[(int)ResPathType.ePathModel] = "Model/";
            m_pathLst[(int)ResPathType.ePathMaterial] = "Model/Materials/";
            m_pathLst[(int)ResPathType.ePathBuildImage] = "Image/Build/";
            m_pathLst[(int)ResPathType.ePathCardImage] = "Image/Card/";
            m_pathLst[(int)ResPathType.ePathWord] = "Word/";
            m_pathLst[(int)ResPathType.ePathAudio] = "Sound/";
            m_pathLst[(int)ResPathType.ePathAtlas] = "Atlas/";
            m_pathLst[(int)ResPathType.ePathSpriteAni] = "Effect/SpriteEffect/";
            m_pathLst[(int)ResPathType.ePathSceneAnimatorController] = "Animation/Scene/";
            m_pathLst[(int)ResPathType.ePathULua] = "LuaScript/";
            m_pathLst[(int)ResPathType.ePathLuaScript] = "LuaScript/";
            m_pathLst[(int)ResPathType.ePathSkillAction] = "SkillAction/";

            m_dataPath = Application.dataPath;

            m_pakExtNameList.Add("prefab");
            m_pakExtNameList.Add("png");
            m_pakExtNameList.Add("shader");
            m_pakExtNameList.Add("unity");
        }
    }
}
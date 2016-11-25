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
        public static string StreamingAssets;

        public string mIp;
        public int mPort;
        public ushort mZone;

        public string mWebIP;               // web 服务器
        public int mWebPort;

        public string[] mPathLst;
        public ResLoadType mResLoadType;   // 资源加载类型
        public string mDataPath;
        //public bool m_bNeedNet = false;                       // 是否需要网络
        public string mNetLogPhp;       // Php 处理文件
        public List<string> mPakExtNameList;       // 打包的扩展名字列表

        public Config()
        {
            StreamingAssets = "StreamingAssets/";

            this.mIp = "192.168.96.14";
            this.mPort = 20013;
            this.mZone = 30;

            this.mWebIP = "http://127.0.0.1/UnityServer/";
            this.mWebPort = 80;
            this.mNetLogPhp = "/netlog/NetLog.php";
            this.mPakExtNameList = new List<string>();

            this.mResLoadType = ResLoadType.eLoadResource;
            this.mPathLst = new string[(int)ResPathType.eTotal];
            this.mPathLst[(int)ResPathType.ePathScene] = "Scenes/";
            this.mPathLst[(int)ResPathType.ePathSceneXml] = "Scenes/Xml/";
            this.mPathLst[(int)ResPathType.ePathModule] = "Module/";
            this.mPathLst[(int)ResPathType.ePathComUI] = "UI/";
            this.mPathLst[(int)ResPathType.ePathComUIScene] = "UIScene/";
            this.mPathLst[(int)ResPathType.ePathBeingPath] = "Being/";
            this.mPathLst[(int)ResPathType.ePathAIPath] = "AI/";
            this.mPathLst[(int)ResPathType.ePathTablePath] = "Table/";
            this.mPathLst[(int)ResPathType.ePathLangXml] = "Languages/";
            this.mPathLst[(int)ResPathType.ePathXmlCfg] = "XmlConfig/";
            this.mPathLst[(int)ResPathType.ePathModel] = "Model/";
            this.mPathLst[(int)ResPathType.ePathMaterial] = "Model/Materials/";
            this.mPathLst[(int)ResPathType.ePathBuildImage] = "Image/Build/";
            this.mPathLst[(int)ResPathType.ePathCardImage] = "Image/Card/";
            this.mPathLst[(int)ResPathType.ePathWord] = "Word/";
            this.mPathLst[(int)ResPathType.ePathAudio] = "Sound/";
            this.mPathLst[(int)ResPathType.ePathAtlas] = "Atlas/";
            this.mPathLst[(int)ResPathType.ePathSpriteAni] = "Effect/SpriteEffect/";
            this.mPathLst[(int)ResPathType.ePathSceneAnimatorController] = "Animation/Scene/";
            this.mPathLst[(int)ResPathType.ePathULua] = "LuaScript/";
            this.mPathLst[(int)ResPathType.ePathLuaScript] = "LuaScript/";
            this.mPathLst[(int)ResPathType.ePathSkillAction] = "SkillAction/";

            this.mDataPath = Application.dataPath;

            this.mPakExtNameList.Add("prefab");
            this.mPakExtNameList.Add("png");
            this.mPakExtNameList.Add("shader");
            this.mPakExtNameList.Add("unity");
        }
    }
}
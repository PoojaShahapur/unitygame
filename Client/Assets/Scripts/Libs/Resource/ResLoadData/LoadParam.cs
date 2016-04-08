using System;

namespace SDK.Lib
{
    public class LoadParam : IRecycle
    {
        public ResPackType m_resPackType;           // 加载资源的类型
        public ResLoadType m_resLoadType;           // 资源加载类型

        public string m_path = "";                  // 资源路径，传递进来都是完成的路径，都是相对 Resources 开始的，例如 Table\CardBase_client.txt，然后内部解析后
        public string m_subPath = "";               // 子目录，可能一个包中有多个资源
        public string m_pathNoExt = "";             // 这个数据变成了从 Resources 目录开始，没有扩展名字，打包的包名字在包加载的时候判断
        protected string m_prefabName = "";         // 预设的名字，就是在 AssetBundle 里面完整的资源目录和名字
        protected string m_extName = "prefab";      // 加载的资源的扩展名字

        public string m_version = "";               // 加载的资源的版本号
        protected string m_lvlName = "";               // 关卡名字
        public Action<IDispatchObject> m_loadEventHandle;    // 加载事件回调函数

        public bool m_resNeedCoroutine = true;      // 资源是否需要协同程序
        public bool m_loadNeedCoroutine = true;     // 加载是否需要协同程序

        public string m_origPath = "";                   // 原始资源加载目录，主要是打包的时候使用
        public string m_pakPath = "";                   // 打包的资源目录，如果打包， m_pakPath 应该就是 m_path

        public ResItem m_loadRes = null;
        public InsResBase m_loadInsRes = null;

        public LoadParam()
        {
            m_path = "";
        }

        public string prefabName
        {
            get
            {
                return m_prefabName;
            }
        }

        public string extName
        {
            get
            {
                return m_extName;
            }
        }

        public string lvlName
        {
            get
            {
                return m_lvlName;
            }
        }

        public void resetDefault()          // 将数据清空，有时候上一次调用的时候的参数 m_loaded 还在，结果被认为是这一次的回调了
        {
            m_loadEventHandle = null;
            m_version = "";
            m_extName = "prefab";
            m_origPath = "";

            m_loadRes = null;
            m_loadInsRes = null;
        }

        // 解析目录
        public void resolvePath()
        {
            int dotIdx = m_path.IndexOf(".");

            if (-1 == dotIdx)
            {
                m_extName = "";
                m_pathNoExt = m_path;
            }
            else
            {
                m_extName = m_path.Substring(dotIdx + 1);
                m_pathNoExt = m_path.Substring(0, dotIdx);
            }

            if(m_extName.Length == 0)
            {
                m_prefabName = m_pathNoExt;
            }
            else
            {
                m_prefabName = m_pathNoExt + "." + m_extName;
            }
        }

        public void resolveLevel()
        {
            resolvePath();

            int slashIdx = 0;
            if (string.IsNullOrEmpty(m_origPath))
            {
                slashIdx = m_pathNoExt.LastIndexOf("/");
                if (slashIdx != -1)
                {
                    m_lvlName = m_pathNoExt.Substring(slashIdx + 1);
                }
                else
                {
                    m_lvlName = m_pathNoExt;
                }
            }
            else        // 如果是打包，需要从原始加载目录获取关卡名字
            {
                m_lvlName = UtilLogic.convScenePath2LevelName(m_origPath);
            }
        }

        public void copyFrom(LoadParam rhs)
        {
            this.m_resPackType = rhs.m_resPackType;
            this.m_resLoadType = rhs.m_resLoadType;

            this.m_path = rhs.m_path;
            this.m_subPath = rhs.m_subPath;
            this.m_pathNoExt = rhs.m_pathNoExt;
            this.m_prefabName = rhs.m_prefabName;
            this.m_extName = rhs.m_extName;

            this.m_version = rhs.m_version;
            this.m_lvlName = rhs.m_lvlName;
            this.m_loadEventHandle = rhs.m_loadEventHandle;

            this.m_resNeedCoroutine = rhs.m_resNeedCoroutine;
            this.m_loadNeedCoroutine = rhs.m_loadNeedCoroutine;

            this.m_origPath = rhs.m_origPath;
            this.m_pakPath = rhs.m_pakPath;
        }
    }
}
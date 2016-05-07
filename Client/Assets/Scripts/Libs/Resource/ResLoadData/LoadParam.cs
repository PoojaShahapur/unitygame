﻿using LuaInterface;
using System;

namespace SDK.Lib
{
    public class LoadParam : IRecycle
    {
        public ResPackType m_resPackType;           // 加载资源的类型
        public ResLoadType m_resLoadType;           // 资源加载类型

        public string mLoadPath = "";               // 真正的资源加载目录
        public string m_origPath = "";              // 原始资源加载目录，就是直接传递进来的目录
        protected string m_prefabName = "";         // 预设的名字，就是在 AssetBundle 里面完整的资源目录和名字
        protected string m_extName = "prefab";      // 加载的原始资源的扩展名字，不是 AssetBundles 的扩展名字
        public string m_pathNoExt = "";             // mLoadPath 的没有扩展名字的路径
        public string mLogicPath;                   // 逻辑传递进来的目录，这个目录可能是没有扩展名字的，而 m_origPath 就是有扩展名字的，如果 mLogicPath 有扩展名字，就是和 m_origPath 完全一样了

        public string m_subPath = "";               // 子目录，可能一个包中有多个资源
        public string m_pakPath = "";               // 打包的资源目录，如果打包， m_pakPath 应该就是 m_path
        public string mResUniqueId;                 // 资源唯一 Id，查找资源的索引

        public string m_version = "";               // 加载的资源的版本号
        protected string m_lvlName = "";            // 关卡名字
        public Action<IDispatchObject> m_loadEventHandle;    // 加载事件回调函数

        public bool m_resNeedCoroutine = true;      // 资源是否需要协同程序
        public bool m_loadNeedCoroutine = true;     // 加载是否需要协同程序

        public ResItem m_loadRes = null;
        public InsResBase m_loadInsRes = null;
        public LuaTable mLuaTable;
        public LuaFunction mLuaFunction;
        public bool mIsLoadAll;                 // 是否一次性加载所有的内容

        public LoadParam()
        {
            mLoadPath = "";
            mIsLoadAll = false;
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
            int dotIdx = mLoadPath.IndexOf(".");

            if (-1 == dotIdx)
            {
                m_extName = "";
                m_pathNoExt = mLoadPath;
            }
            else
            {
                m_extName = mLoadPath.Substring(dotIdx + 1);
                m_pathNoExt = mLoadPath.Substring(0, dotIdx);
            }

            /*
            if(m_extName.Length == 0)
            {
                m_prefabName = m_pathNoExt;
            }
            else
            {
                m_prefabName = m_pathNoExt + "." + m_extName;
            }
            */
            m_prefabName = mLoadPath;
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

            this.mLoadPath = rhs.mLoadPath;
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

        public void setPath(string path)
        {
            string fullPath = "";
            m_origPath = path;

            int dotIdx = m_origPath.IndexOf(".");
            if (-1 == dotIdx)
            {
                m_extName = "";
                mLogicPath = m_origPath;
            }
            else
            {
                m_extName = m_origPath.Substring(dotIdx + 1);
                //mLogicPath = m_origPath.Substring(0, dotIdx);     // mLogicPath 没有扩展名字
                mLogicPath = m_origPath;        // mLogicPath 有扩展名字
            }

            // 如果直接加载的就是 AssetBundles 资源
            if(m_extName != UtilApi.UNITY3d)
            {
                fullPath = ResPathResolve.convResourcesPath2AssetBundlesPath(m_origPath);
            }
            else
            {
                fullPath = m_origPath;
            }

            if (-1 == dotIdx)
            {
                m_pathNoExt = fullPath;
            }
            else
            {
                m_pathNoExt = fullPath.Substring(0, dotIdx);
            }

            if (m_extName != UtilApi.UNITY3d)
            {
                m_prefabName = m_pathNoExt + "." + m_extName;
            }
            else
            {
                // 如果直接加载一个 .unity3d 文件，必然是一个被依赖的 AssetBundles ，这个时候不会从 AssetBundles 里面获取任何东西，因此 m_prefabName 设置为空， AssetBundleManifest 这个 .unity3d 除外
                m_prefabName = "";
            }

            if (MacroDef.ASSETBUNDLES_LOAD)
            {
                mLoadPath = m_pathNoExt + UtilApi.DOTUNITY3d;
            }
            else
            {
                mLoadPath = m_pathNoExt;
            }

            mResUniqueId = m_pathNoExt;
        }
    }
}
 
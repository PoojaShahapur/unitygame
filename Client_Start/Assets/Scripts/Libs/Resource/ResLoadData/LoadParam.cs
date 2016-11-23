using LuaInterface;

namespace SDK.Lib
{
    public class LoadParam : IRecycle
    {
        public ResPackType mResPackType;           // 加载资源的类型
        public ResLoadType mResLoadType;           // 资源加载类型

        public string mLoadPath = "";               // 真正的资源加载目录
        public string mOrigPath = "";              // 原始资源加载目录，就是直接传递进来的目录
        protected string mPrefabName = "";         // 预设的名字，就是在 AssetBundle 里面完整的资源目录和名字
        protected string mExtName = "prefab";      // 加载的原始资源的扩展名字，不是 AssetBundles 的扩展名字
        public string mPathNoExt = "";             // mLoadPath 的没有扩展名字的路径
        public string mLogicPath;                   // 逻辑传递进来的目录，这个目录可能是没有扩展名字的，而 mOrigPath 就是有扩展名字的，如果 mLogicPath 有扩展名字，就是和 mOrigPath 完全一样了

        public string mSubPath = "";               // 子目录，可能一个包中有多个资源
        public string mPakPath = "";               // 打包的资源目录，如果打包， mPakPath 应该就是 mPath
        public string mResUniqueId;                 // 资源唯一 Id，查找资源的索引

        public string mVersion = "";               // 加载的资源的版本号
        protected string mLvlName = "";            // 关卡名字
        public MAction<IDispatchObject> mLoadEventHandle;    // 加载事件回调函数

        public bool mResNeedCoroutine = true;      // 资源是否需要协同程序
        public bool mLoadNeedCoroutine = true;     // 加载是否需要协同程序

        public ResItem mLoadRes = null;
        public InsResBase mLoadInsRes = null;
        public LuaTable mLuaTable;
        public LuaFunction mLuaFunction;
        public bool mIsLoadAll;                 // 是否一次性加载所有的内容
        public bool mIsCheckDep;                // 是否检查依赖

        public LoadParam()
        {
            mLoadPath = "";
            mIsLoadAll = false;
        }

        public string prefabName
        {
            get
            {
                return mPrefabName;
            }
        }

        public string extName
        {
            get
            {
                return mExtName;
            }
        }

        public string lvlName
        {
            get
            {
                return mLvlName;
            }
        }

        public void resetDefault()          // 将数据清空，有时候上一次调用的时候的参数 m_loaded 还在，结果被认为是这一次的回调了
        {
            mLoadEventHandle = null;
            mVersion = "";
            mExtName = "prefab";
            mOrigPath = "";

            mLoadRes = null;
            mLoadInsRes = null;
        }

        // 解析目录
        public void resolvePath()
        {
            int dotIdx = mLoadPath.IndexOf(".");

            if (-1 == dotIdx)
            {
                mExtName = "";
                mPathNoExt = mLoadPath;
            }
            else
            {
                mExtName = mLoadPath.Substring(dotIdx + 1);
                mPathNoExt = mLoadPath.Substring(0, dotIdx);
            }

            mPrefabName = mLoadPath;
        }

        public void resolveLevel()
        {
            int slashIdx = 0;
            if (string.IsNullOrEmpty(mOrigPath))
            {
                slashIdx = mPathNoExt.LastIndexOf("/");
                if (slashIdx != -1)
                {
                    mLvlName = mPathNoExt.Substring(slashIdx + 1);
                }
                else
                {
                    mLvlName = mPathNoExt;
                }
            }
            else        // 如果是打包，需要从原始加载目录获取关卡名字
            {
                mLvlName = UtilLogic.convScenePath2LevelName(mOrigPath);
            }
        }

        public void copyFrom(LoadParam rhs)
        {
            this.mResPackType = rhs.mResPackType;
            this.mResLoadType = rhs.mResLoadType;

            this.mLoadPath = rhs.mLoadPath;
            this.mSubPath = rhs.mSubPath;
            this.mPathNoExt = rhs.mPathNoExt;
            this.mPrefabName = rhs.mPrefabName;
            this.mExtName = rhs.mExtName;
            this.mLogicPath = rhs.mLogicPath;
            this.mLogicPath = rhs.mLogicPath;
            this.mResUniqueId = rhs.mResUniqueId;

            this.mVersion = rhs.mVersion;
            this.mLvlName = rhs.mLvlName;
            this.mLoadEventHandle = rhs.mLoadEventHandle;

            this.mResNeedCoroutine = rhs.mResNeedCoroutine;
            this.mLoadNeedCoroutine = rhs.mLoadNeedCoroutine;

            this.mOrigPath = rhs.mOrigPath;
            this.mPakPath = rhs.mPakPath;
        }

        public void setPath(string path)
        {
            string fullPath = "";
            mOrigPath = path;

            int dotIdx = mOrigPath.IndexOf(".");
            if (-1 == dotIdx)
            {
                mExtName = "";
                mLogicPath = mOrigPath;
            }
            else
            {
                mExtName = mOrigPath.Substring(dotIdx + 1);
                //mLogicPath = mOrigPath.Substring(0, dotIdx);     // mLogicPath 没有扩展名字
                mLogicPath = mOrigPath;        // mLogicPath 有扩展名字
            }

            ResRedirectItem redirectItem = Ctx.mInstance.mResRedirect.getResRedirectItem(mOrigPath);
            if(redirectItem != null && redirectItem.mFileVerInfo != null)
            {
                mResUniqueId = redirectItem.mFileVerInfo.mResUniqueId;
                mLoadPath = redirectItem.mFileVerInfo.mLoadPath;
                mResLoadType = redirectItem.mResLoadType;
                // 解析加载方式
                setPackAndLoadType(redirectItem);
            }
            else
            {
                // 如果没有就从 Resources 文件夹下加载
                mResLoadType = ResLoadType.eLoadResource;
                mResPackType = ResPackType.eResourcesType;
                mResUniqueId = UtilPath.getFilePathNoExt(mOrigPath);
                mLoadPath = mResUniqueId;
            }

            fullPath = mLoadPath;

            dotIdx = fullPath.IndexOf(".");
            if (-1 == dotIdx)
            {
                mPathNoExt = fullPath;
            }
            else
            {
                mPathNoExt = fullPath.Substring(0, dotIdx);
            }

            if (mExtName != UtilApi.UNITY3D)
            {
                mPrefabName = mPathNoExt + "." + mExtName;
            }
            else
            {
                // 如果直接加载一个 .unity3d 文件，可能是一个仅仅被依赖的 AssetBundles ，也可能是一个其它被引用的 AssetBundles ，这个时候可能从 AssetBundles 里面获取任何东西，也可能不获取，因此 m_PrefabName 也需要设置对应的在 AssetBundles 中的路径。 所有依赖的 unity3d 这个文件不太一样，它在 AssetBundles 中的名字是  AssetBundleManifest ，不是 unity3d 的名字，这个需要注意
                mPrefabName = mPathNoExt + "." + mExtName;
            }
        }

        // 设置资源加载和打包类型
        protected void setPackAndLoadType(ResRedirectItem redirectItem)
        {
            if (isLevelType(mExtName))
            {
                mResPackType = ResPackType.eLevelType;
            }
            else if (isPrefabType(mExtName))
            {
                if (mResLoadType == ResLoadType.eLoadResource)
                {
                    mResPackType = ResPackType.eResourcesType;
                }
                else
                {
                    mResPackType = ResPackType.eBundleType;
                }
            }
            else if (isAssetBundleType(mExtName))
            {
                mResPackType = ResPackType.eBundleType;
            }
            else
            {
                mResPackType = ResPackType.eDataType;
            }
        }

        // 是否是从 Resources 目录下加载的资源
        static public bool isPrefabType(string extName)
        {
            return extName == UtilApi.PREFAB ||
                   extName == UtilApi.MAT ||
                   extName == UtilApi.PNG ||
                   extName == UtilApi.JPG ||
                   extName == UtilApi.TGA ||
                   extName == UtilApi.SHADER ||
                   extName == UtilApi.TXT ||
                   extName == UtilApi.BYTES;
        }

        static public bool isAssetBundleType(string extName)
        {
            return extName == UtilApi.UNITY3D;
        }

        static public bool isLevelType(string extName)
        {
            return extName == UtilApi.UNITY;
        }

        static public bool isDataType(string extName)
        {
            return !isLevelType(extName) && !isPrefabType(extName) && !isAssetBundleType(extName);
        }

        static public string convLoadPathToUniqueId(string loadPath)
        {
            string resUniqueId = UtilPath.getFileNameNoExt(loadPath);
            return resUniqueId;
        }

        static public string convOrigPathToUniqueId(string origPath)
        {
            string resUniqueId = "";
            ResRedirectItem redirectItem = Ctx.mInstance.mResRedirect.getResRedirectItem(origPath);
            if (redirectItem != null && redirectItem.mFileVerInfo != null)
            {
                resUniqueId = redirectItem.mFileVerInfo.mResUniqueId;
            }
            else
            {
                resUniqueId = UtilPath.getFileNameNoExt(origPath);
            }

            return resUniqueId;
        }
    }
}
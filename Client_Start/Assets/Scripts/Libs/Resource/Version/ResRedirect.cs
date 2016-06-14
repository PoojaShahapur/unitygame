﻿using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     *@brief 资源重定向 Item
     */
    public class ResRedirectItem
    {
        public string mOrigPath;             // 资源原始目录，就是逻辑加载资源的目录
        public ResLoadType mResLoadType;        // 资源目录
        public FileVerInfo mFileVerInfo;        // 文件的基本信息

        public ResRedirectItem(string origPath = "", int redirect = (int)ResLoadType.eLoadResource)
        {
            mOrigPath = origPath;
            mResLoadType = (ResLoadType)redirect;
        }

        public bool isRedirectR()
        {
            return mResLoadType == ResLoadType.eLoadResource;
        }

        public bool isRedirectS()
        {
            return mResLoadType == ResLoadType.eLoadStreamingAssets;
        }

        public bool isRedirectP()
        {
            return mResLoadType == ResLoadType.eLoadLocalPersistentData;
        }

        public bool isRedirectW()
        {
            return mResLoadType == ResLoadType.eLoadWeb;
        }
    }

    /**
     * @brief 资源重定向，确定资源最终位置
     */
    public class ResRedirect
    {
        protected Dictionary<string, ResRedirectItem> mOrigPath2ItemDic;

        public ResRedirect()
        {
            mOrigPath2ItemDic = new Dictionary<string, ResRedirectItem>();
        }

        public void postInit()
        {
            
        }

        public ResRedirectItem getResRedirectItem(string origPath)
        {
            ResRedirectItem item = null;
            if (mOrigPath2ItemDic.ContainsKey(origPath))
            {
                item = mOrigPath2ItemDic[origPath];
            }
            else
            {
                // 从版本系统中获取
                item = new ResRedirectItem(origPath, (int)ResLoadType.eLoadResource);
                mOrigPath2ItemDic[origPath] = item;
                FileVerInfo fileVerInfo = null;
                item.mResLoadType = (ResLoadType)Ctx.m_instance.m_versionSys.m_localVer.getFileVerInfo(origPath, ref fileVerInfo);
                item.mFileVerInfo = fileVerInfo;
            }

            return item;
        }
    }
}
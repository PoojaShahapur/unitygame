using System.Collections.Generic;
using UnityEngine;

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
            Ctx.mInstance.mLogSys.log(string.Format("ResRedirectItem::getResRedirectItem, origPath is {0}", origPath), LogTypeId.eLogResLoader);

            ResRedirectItem item = null;
            FileVerInfo fileVerInfo = null;

            if (mOrigPath2ItemDic.ContainsKey(origPath))
            {
                Ctx.mInstance.mLogSys.log("ResRedirectItem::getResRedirectItem, Cached", LogTypeId.eLogResLoader);

                item = mOrigPath2ItemDic[origPath];
            }
            else
            {
                Ctx.mInstance.mLogSys.log("ResRedirectItem::getResRedirectItem, Not Cached", LogTypeId.eLogResLoader);

                // 从版本系统中获取
                item = new ResRedirectItem(origPath, (int)ResLoadType.eLoadResource);
                mOrigPath2ItemDic[origPath] = item;

                item.mResLoadType = (ResLoadType)Ctx.mInstance.mVersionSys.mLocalVer.getFileVerInfo(origPath, ref fileVerInfo);
                item.mFileVerInfo = fileVerInfo;
            }

            if (item.mFileVerInfo == null)
            {
                Ctx.mInstance.mLogSys.log(string.Format("ResRedirectItem::getResRedirectItem, Path is {0}, Can not Find Version Info Item", origPath), LogTypeId.eLogResLoader);

                fileVerInfo = new FileVerInfo();
                fileVerInfo.mOrigPath = origPath;
                fileVerInfo.mResUniqueId = UtilPath.getFilePathNoExt(origPath);
                fileVerInfo.mLoadPath = UtilPath.getFilePathNoExt(origPath);
                fileVerInfo.mFileMd5 = "error";
                fileVerInfo.mFileSize = 0;

                item.mFileVerInfo = fileVerInfo;
            }
            else
            {
                Ctx.mInstance.mLogSys.log(string.Format("ResRedirectItem::getResRedirectItem, Path is {0}, ResLoadType is {1}", item.mFileVerInfo.mOrigPath, item.mResLoadType), LogTypeId.eLogResLoader);
            }

            return item;
        }
    }
}
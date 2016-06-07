using System;
using System.Collections.Generic;
using System.IO;

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
        protected string mRedirectFileName;

        public ResRedirect()
        {
            mOrigPath2ItemDic = new Dictionary<string, ResRedirectItem>();
            mRedirectFileName = UtilPath.combine(MFileSys.msPersistentDataPath, "Redirect.txt");
        }

        public void postInit()
        {
            checkOrWriteRedirectFile();
            loadRedirectFile();
        }

        // 写入 persistentDataPath 一个固定的版本文件，以后可能放到配置文件
        protected void checkOrWriteRedirectFile()
        {
            if (UtilPath.existFile(mRedirectFileName))
            {
                UtilPath.deleteFile(mRedirectFileName);
            }
            if (!UtilPath.existFile(mRedirectFileName))
            {
                MDataStream dataStream = new MDataStream(mRedirectFileName);
                string content = "Version_R.txt=0" + UtilApi.CR_LF + "Version_S.txt=1" + UtilApi.CR_LF  + "Version_P.txt=2";
                dataStream.writeText(content);
                dataStream.dispose();
                dataStream = null;
            }
        }

        public void loadRedirectFile()
        {
            // 这个文件必须使用文件系统去读取
            if (UtilPath.existFile(mRedirectFileName))
            {
                MDataStream dataStream = new MDataStream(mRedirectFileName, FileMode.Open);
                dataStream.checkAndOpen();

                if (dataStream.isValid())
                {
                    string text = dataStream.readText();

                    string[] lineSplitStr = { UtilApi.CR_LF };
                    string[] equalSplitStr = { UtilApi.SEPARATOR };
                    string[] lineList = text.Split(lineSplitStr, StringSplitOptions.RemoveEmptyEntries);
                    int lineIdx = 0;
                    string[] equalList = null;

                    ResRedirectItem item;
                    while (lineIdx < lineList.Length)
                    {
                        equalList = lineList[lineIdx].Split(equalSplitStr, StringSplitOptions.RemoveEmptyEntries);
                        item = new ResRedirectItem();
                        item.mFileVerInfo = new FileVerInfo();
                        item.mOrigPath = equalList[0];
                        item.mResLoadType = (ResLoadType)MBitConverter.ToInt32(equalList[1]);

                        item.mFileVerInfo.mOrigPath = equalList[0];
                        if (item.mResLoadType == ResLoadType.eLoadResource)
                        {
                            item.mFileVerInfo.mResUniqueId = UtilPath.getFileNameNoExt(equalList[0]);
                            item.mFileVerInfo.mLoadPath = item.mFileVerInfo.mResUniqueId;
                        }
                        else
                        {
                            item.mFileVerInfo.mResUniqueId = equalList[0];
                            item.mFileVerInfo.mLoadPath = equalList[0];
                        }

                        mOrigPath2ItemDic[item.mOrigPath] = item;
                        ++lineIdx;
                    }
                }

                dataStream.dispose();
                dataStream = null;
            }
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
                FileVerInfo fileVerInfo = null;
                item.mResLoadType = (ResLoadType)Ctx.m_instance.m_versionSys.m_localVersion.getFileVerInfo(origPath, ref fileVerInfo);
                item.mFileVerInfo = fileVerInfo;
                mOrigPath2ItemDic[origPath] = item;
            }

            return item;
        }
    }
}
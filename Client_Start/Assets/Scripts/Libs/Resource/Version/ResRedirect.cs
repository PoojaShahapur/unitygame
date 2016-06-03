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
        public string mResUniqueId;             // 资源唯一 Id
        public ResLoadType mResLoadType;        // 资源目录
        public FileVerInfo mFileVerInfo;        // 文件的基本信息

        public ResRedirectItem(string resUniqueId = "", int redirect = (int)ResLoadType.eLoadResource)
        {
            mResUniqueId = resUniqueId;
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
        protected Dictionary<string, ResRedirectItem> mUniqueId2ItemDic;
        protected string mRedirectFileName;

        public ResRedirect()
        {
            mUniqueId2ItemDic = new Dictionary<string, ResRedirectItem>();
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
                string content = "Version_R=0" + UtilApi.CR_LF + "Version_S=1" + UtilApi.CR_LF  + "Version_P=2";
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
                        item.mResUniqueId = equalList[0];
                        item.mResLoadType = (ResLoadType)MBitConverter.ToInt32(equalList[1]);
                        mUniqueId2ItemDic[item.mResUniqueId] = item;
                        ++lineIdx;
                    }
                }

                dataStream.dispose();
                dataStream = null;
            }
        }

        public ResRedirectItem getResRedirectItem(string resUniqueId)
        {
            ResRedirectItem item = null;
            if (mUniqueId2ItemDic.ContainsKey(resUniqueId))
            {
                item = mUniqueId2ItemDic[resUniqueId];
            }
            else
            {
                // 自己暂时模拟代码
                item = new ResRedirectItem(resUniqueId, (int)ResLoadType.eLoadStreamingAssets);
            }

            return item;
        }

        public ResRedirectItem getResRedirectItemByOrigPath(string origPath)
        {
            ResRedirectItem item = null;
            foreach(KeyValuePair<string, ResRedirectItem> kv in mUniqueId2ItemDic)
            {
                if(kv.Value.mFileVerInfo.mOrigPath == origPath)
                {
                    item = kv.Value;
                    break;
                }
            }

            return item;
        }
    }
}
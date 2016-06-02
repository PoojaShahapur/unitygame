using System;
using System.Collections.Generic;
using System.IO;

namespace SDK.Lib
{
    /**
     * @brief 资源目录
     */
    public enum eResRedirectDir
    {
        eResources = 0,     // Resources 目录下
        eStreaming = 1,     // StreamingAssets 目录下
        ePersistent = 2,    // persistentDataPath 目录下
    }

    /**
     *@brief 资源重定向 Item
     */
    public class ResRedirectItem
    {
        public string mResUniqueId;                     // 资源唯一 Id
        public eResRedirectDir mResRedirectDir;         // 资源目录

        public ResRedirectItem(string resUniqueId = "", int redirect = (int)eResRedirectDir.eResources)
        {
            mResUniqueId = resUniqueId;
            mResRedirectDir = (eResRedirectDir)redirect;
        }

        public bool isRedirectR()
        {
            return mResRedirectDir == eResRedirectDir.eResources;
        }

        public bool isRedirectS()
        {
            return mResRedirectDir == eResRedirectDir.eStreaming;
        }

        public bool isRedirectP()
        {
            return mResRedirectDir == eResRedirectDir.ePersistent;
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
                        item.mResUniqueId = equalList[0];
                        item.mResRedirectDir = (eResRedirectDir)MBitConverter.ToInt32(equalList[1]);
                        mUniqueId2ItemDic[item.mResUniqueId] = item;
                        ++lineIdx;
                    }
                }

                dataStream.dispose();
                dataStream = null;
            }
        }

        // 根据 reqUniqueId 返回，为了绑定到 Lua，尽量返回类型不使用 Enum
        public int getResRedirectDir(string resUniqueId)
        {
            int dir = 0;
            if(mUniqueId2ItemDic.ContainsKey(resUniqueId))
            {
                dir = (int)mUniqueId2ItemDic[resUniqueId].mResRedirectDir;
            }
            else
            {
                // 自己暂时模拟代码
                dir = (int)eResRedirectDir.eResources;
            }

            return dir;
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
                item = new ResRedirectItem(resUniqueId, (int)eResRedirectDir.eResources);
            }

            return item;
        }
    }
}
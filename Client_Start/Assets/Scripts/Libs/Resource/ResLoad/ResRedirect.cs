using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 资源目录
     */
    public enum eResDir
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
        public string mResUniqueId;     // 资源唯一 Id
        public eResDir mResDir;         // 资源目录
    }

    /**
     * @brief 资源重定向，确定资源最终位置
     */
    public class ResRedirect
    {
        protected Dictionary<string, ResRedirectItem> mUniqueId2ItemDic;

        public ResRedirect()
        {
            mUniqueId2ItemDic = new Dictionary<string, ResRedirectItem>();
        }

        public void postInit()
        {
            
        }

        // 写入 persistentDataPath 一个固定的版本文件
        protected void writeRedirectFile()
        {
            string fileName = UtilPath.combine(MFileSys.msPersistentDataPath, "Redirect.txt");
            if(!UtilPath.existFile(fileName))
            {
                MDataStream dataStream = new MDataStream(fileName);
                string content = "Version_R.txt=0\r\nVersion_S.txt=0\r\nVersion_P.txt=0\r\n";
                dataStream.writeText(content);
                dataStream.dispose();
                dataStream = null;
            }
        }

        // 根据 reqUniqueId 返回，为了绑定到 Lua，尽量返回类型不使用 Enum
        public int getResDir(string resUniqueId)
        {
            int dir = 0;
            if(mUniqueId2ItemDic.ContainsKey(resUniqueId))
            {
                dir = (int)mUniqueId2ItemDic[resUniqueId].mResDir;
            }
            else
            {
                // 自己暂时模拟代码
                dir = (int)eResDir.eResources;
            }

            return dir;
        }
    }
}
using LuaInterface;

namespace SDK.Lib
{
    public enum DownloadType
    {
        eWWW,
        eHttpWeb,
        eTotal
    }

    /**
     * @brief 下载参数
     */
    public class DownloadParam
    {
        public string mLoadPath;
        public string mOrigPath;
        public string mLogicPath;
        public string mResUniqueId;
        public string mExtName;
        public string mVersion;

        public MAction<IDispatchObject> mLoadEventHandle;
        public DownloadType mDownloadType;
        public ResLoadType mResLoadType;
        public ResPackType mResPackType;

        public LuaTable mLuaTable;
        public LuaFunction mLuaFunction;
        public bool mIsWriteFile;       // 下载完成是否写入文件
        public long mFileLen;           // 文件长度，如果使用 HttpWeb 下载，使用这个字段判断文件长度

        public DownloadParam()
        {
            reset();
        }

        public void reset()
        {
            mResLoadType = ResLoadType.eLoadWeb;
            mDownloadType = DownloadType.eHttpWeb;
            mIsWriteFile = true;
            mFileLen = 0;
            mVersion = "";
        }

        public void setPath(string origPath)
        {
            mOrigPath = origPath;
            mLoadPath = mOrigPath;
            mLogicPath = mOrigPath;
            mResUniqueId = mOrigPath;
            mVersion = "4";

            mExtName = UtilPath.getFileExt(mOrigPath);

            if(mExtName == UtilApi.UNITY3D)
            {
                mResPackType = ResPackType.eBundleType;
            }
            else
            {
                mResPackType = ResPackType.eDataType;
            }
        }
    }
}
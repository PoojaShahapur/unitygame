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
        public string m_extName;
        public string mVersion = "";

        public MAction<IDispatchObject> m_loadEventHandle;
        public DownloadType mDownloadType;
        public ResLoadType mResLoadType;
        public ResPackType mResPackType;

        public LuaTable mLuaTable;
        public LuaFunction mLuaFunction;

        public DownloadParam()
        {
            reset();
        }

        public void reset()
        {
            mResLoadType = ResLoadType.eLoadWeb;
            mDownloadType = DownloadType.eWWW;
        }

        public void setPath(string origPath)
        {
            mOrigPath = origPath;
            mLoadPath = mOrigPath;
            mLogicPath = mOrigPath;
            mResUniqueId = mOrigPath;
            mVersion = "4";

            m_extName = UtilPath.getFileExt(mOrigPath);

            if(m_extName == UtilApi.UNITY3D)
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
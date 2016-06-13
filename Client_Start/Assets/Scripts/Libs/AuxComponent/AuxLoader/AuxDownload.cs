namespace SDK.Lib
{
    /**
     * @brief 下载
     */
    public class AuxDownload
    {
        // 下载一个资源
        public void load(string origPath, MAction<IDispatchObject> dispObj = null, bool isWriteFile = true, int downloadType = 1)
        {
            DownloadParam param = new DownloadParam();

            param.setPath(origPath);
            param.m_loadEventHandle = dispObj;
            param.mIsWriteFile = isWriteFile;
            param.mDownloadType = (DownloadType)downloadType;

            Ctx.m_instance.mDownloadMgr.load(param);
        }
    }
}
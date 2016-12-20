namespace SDK.Lib
{
    /**
     * @brief 下载
     */
    public class AuxDownloader : AuxLoaderBase
    {
        protected DownloadItem mDownloadItem;

        public byte[] getBytes()
        {
            if (mDownloadItem != null)
            {
                return mDownloadItem.getBytes();
            }

            return null;
        }

        // 下载一个资源
        public void download(string origPath, MAction<IDispatchObject> dispObj = null, long fileLen = 0, bool isWriteFile = true, int downloadType = (int)DownloadType.eHttpWeb)
        {
            if (needUnload(origPath))
            {
                unload();
            }

            this.setPath(origPath);

            if (this.isInvalid())
            {
                mEvtHandle = new ResEventDispatch();
                mEvtHandle.addEventHandle(null, dispObj);

                DownloadParam param = new DownloadParam();

                param.setPath(origPath);
                param.mLoadEventHandle = onDownloaded;
                param.mFileLen = fileLen;
                param.mIsWriteFile = isWriteFile;
                param.mDownloadType = (DownloadType)downloadType;

                Ctx.mInstance.mDownloadMgr.load(param);
            }
        }

        // 下载完成
        public void onDownloaded(IDispatchObject dispObj)
        {
            mDownloadItem = dispObj as DownloadItem;
            if (mDownloadItem.hasSuccessLoaded())
            {
                mIsSuccess = true;
            }
            else if (mDownloadItem.hasFailed())
            {
                mIsSuccess = false;
                mDownloadItem = null;
            }

            if (mEvtHandle != null)
            {
                mEvtHandle.dispatchEvent(this);
            }
        }
    }
}
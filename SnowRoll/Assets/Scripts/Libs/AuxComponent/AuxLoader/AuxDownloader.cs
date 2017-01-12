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
            if (this.mDownloadItem != null)
            {
                return this.mDownloadItem.getBytes();
            }

            return null;
        }

        // 下载一个资源
        override public void download(string origPath, MAction<IDispatchObject> dispObj = null, long fileLen = 0, bool isWriteFile = true, int downloadType = (int)DownloadType.eHttpWeb)
        {
            base.download(origPath, dispObj, fileLen, isWriteFile, downloadType);

            if (this.isInvalid())
            {
                DownloadParam param = new DownloadParam();

                param.setPath(origPath);
                param.mLoadEventHandle = this.onDownloaded;
                param.mFileLen = fileLen;
                param.mIsWriteFile = isWriteFile;
                param.mDownloadType = (DownloadType)downloadType;

                Ctx.mInstance.mDownloadMgr.download(param);
            }
            else if (this.hasLoadEnd())
            {
                this.onDownloaded(this.mDownloadItem);
            }
        }

        // 下载完成
        public void onDownloaded(IDispatchObject dispObj)
        {
            if (null != dispObj)
            {
                this.mDownloadItem = dispObj as DownloadItem;

                if (this.mDownloadItem.hasSuccessLoaded())
                {
                    this.mResLoadState.setSuccessLoaded();
                }
                else if (this.mDownloadItem.hasFailed())
                {
                    this.mResLoadState.setFailed();
                    this.mDownloadItem = null;
                }
            }

            if (this.mEvtHandle != null)
            {
                this.mEvtHandle.dispatchEvent(this);
            }
        }
    }
}
using System;
using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    /**
    * @brief 使用 Unity 的 WWW 从网络下载数据
    */
    public class WWWDownloadItem : DownloadItem
    {
        public WWWDownloadItem()
        {

        }

        override public void load()
        {
            base.load();

            Ctx.mInstance.mCoroutineMgr.StartCoroutine(downloadAsset());
        }

        // mPath 是这个格式 http://127.0.0.1/UnityServer/Version.txt?ver=100
        override protected IEnumerator downloadAsset()
        {
            deleteFromCache(mDownloadVerPath);
            if (mResPackType == ResPackType.eBundleType)
            {
                mW3File = WWW.LoadFromCacheOrDownload(mDownloadNoVerPath, Convert.ToInt32(mVersion), 0);
            }
            else
            {
                mW3File = new WWW(mDownloadVerPath);
            }
            yield return mW3File;

            onWWWEnd();
        }

        // 加载完成回调处理
        override protected void onWWWEnd()
        {
            if (isLoadedSuccess(mW3File))
            {
                if(mW3File.size > 0)
                {
                    if (mW3File.bytes != null)
                    {
                        mBytes = mW3File.bytes;
                    }
                    else if(mW3File.text != null)
                    {
                        mText = mW3File.text;
                    }
                }

                if (mIsWriteFile)
                {
                    writeFile();
                }

                mRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                mRefCountResLoadResultNotify.resLoadState.setFailed();
            }
            mRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }
    }
}
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

            Ctx.m_instance.m_coroutineMgr.StartCoroutine(downloadAsset());
        }

        // m_path 是这个格式 http://127.0.0.1/UnityServer/Version.txt?ver=100
        override protected IEnumerator downloadAsset()
        {
            deleteFromCache(mDownloadVerPath);
            if (mResPackType == ResPackType.eBundleType)
            {
                m_w3File = WWW.LoadFromCacheOrDownload(mDownloadNoVerPath, Convert.ToInt32(mVersion), 0);
            }
            else
            {
                m_w3File = new WWW(mDownloadVerPath);
            }
            yield return m_w3File;

            onWWWEnd();
        }

        // 加载完成回调处理
        override protected void onWWWEnd()
        {
            if (isLoadedSuccess(m_w3File))
            {
                if(m_w3File.size > 0)
                {
                    if (m_w3File.bytes != null)
                    {
                        mBytes = m_w3File.bytes;
                    }
                    else if(m_w3File.text != null)
                    {
                        mText = m_w3File.text;
                    }
                }

                writeFile();
                m_refCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                m_refCountResLoadResultNotify.resLoadState.setFailed();
            }
            m_refCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }
    }
}
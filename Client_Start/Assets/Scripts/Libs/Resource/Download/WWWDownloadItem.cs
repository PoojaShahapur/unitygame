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
            deleteFromCache(m_loadPath);
            m_w3File = WWW.LoadFromCacheOrDownload(m_loadPath, Int32.Parse(m_version));
            yield return m_w3File;

            onWWWEnd();
        }

        // 加载完成回调处理
        override protected void onWWWEnd()
        {
            if (isLoadedSuccess(m_w3File))
            {
                m_bytes = m_w3File.bytes;

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
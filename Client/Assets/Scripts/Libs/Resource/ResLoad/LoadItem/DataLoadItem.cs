using SDK.Common;
using System;
using System.Collections;
using UnityEngine;
namespace SDK.Lib
{
    /**
     * @brief 从本地磁盘或者网络加载纯数据
     */
    public class DataLoadItem : LoadItem
    {
        public byte[] m_bytes;

        override public void load()
        {
            base.load();
            if (ResLoadType.eLoadDisc == m_resLoadType)
            {
                loadFromDisc();
            }
            else if (ResLoadType.eLoadWeb == m_resLoadType)
            {
                Ctx.m_instance.m_coroutineMgr.StartCoroutine(downloadAsset());
            }
        }

        protected void loadFromDisc()
        {
            if (Ctx.m_instance.m_localFileSys.isFileExist(string.Format("{0}/{1}", Ctx.m_instance.m_localFileSys.getLocalWriteDir(), m_path)))
            {
                m_bytes = Ctx.m_instance.m_localFileSys.LoadFileByte(Ctx.m_instance.m_localFileSys.getLocalWriteDir(), m_path);
            }
            else if (Ctx.m_instance.m_localFileSys.isFileExist(string.Format("{0}/{1}", Ctx.m_instance.m_localFileSys.getLocalReadDir(), m_path)))
            {
                m_bytes = Ctx.m_instance.m_localFileSys.LoadFileByte(Ctx.m_instance.m_localFileSys.getLocalReadDir(), m_path);
            }

            if (m_bytes != null)
            {
                if (onLoaded != null)
                {
                    onLoaded(this);
                }
            }
            else
            {
                if (onFailed != null)
                {
                    onFailed(this);
                }
            }
        }

        // m_path 是这个格式 http://127.0.0.1/UnityServer/Version.txt?ver=100
        override protected IEnumerator downloadAsset()
        {
            string path = "";
            string[] pathSplit = { "?" };
            string[] pathList = m_path.Split(pathSplit, StringSplitOptions.RemoveEmptyEntries);

            string[] equalSplit = { "=" };
            string[] equalList = pathList[1].Split(equalSplit, StringSplitOptions.RemoveEmptyEntries);

            path = Ctx.m_instance.m_cfg.m_webIP + pathList[0];
            deleteFromCache(path);
            m_w3File = WWW.LoadFromCacheOrDownload(path, Int32.Parse(equalList[1]));
            yield return m_w3File;

            onWWWEnd();
        }

        // 加载完成回调处理
        override protected void onWWWEnd()
        {
            if (isLoadedSuccess(m_w3File))
            {
                m_bytes = m_w3File.bytes;

                if (onLoaded != null)
                {
                    onLoaded(this);
                }
            }
            else
            {
                if (onFailed != null)
                {
                    onFailed(this);
                }
            }
        }
    }
}
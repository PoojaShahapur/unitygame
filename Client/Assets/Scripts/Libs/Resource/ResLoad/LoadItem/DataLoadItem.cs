using SDK.Common;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
namespace SDK.Lib
{
    /**
     * @brief 从本地磁盘或者网络加载纯数据
     */
    public class DataLoadItem : LoadItem
    {
        public byte[] m_bytes;

        override public void reset()
        {
            base.reset();
            m_bytes = null;
        }

        override public void load()
        {
            base.load();
            if (ResLoadType.eStreamingAssets == m_resLoadType)
            {
                loadFromStreamingAssets();
            }
            else if (ResLoadType.ePersistentData == m_resLoadType)
            {
                loadFromPersistentData();
            }
            else if (ResLoadType.eLoadWeb == m_resLoadType)
            {
                //Ctx.m_instance.m_coroutineMgr.StartCoroutine(downloadAsset());
                Ctx.m_instance.m_coroutineMgr.StartCoroutine(webDown());
            }
        }

        protected void loadFromStreamingAssets()
        {
            if (Ctx.m_instance.m_localFileSys.isFileExist(string.Format("{0}/{1}", Ctx.m_instance.m_localFileSys.getLocalReadDir(), m_path)))
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

        protected void loadFromPersistentData()
        {
            if (Ctx.m_instance.m_localFileSys.isFileExist(string.Format("{0}/{1}", Ctx.m_instance.m_localFileSys.getLocalWriteDir(), m_path)))
            {
                m_bytes = Ctx.m_instance.m_localFileSys.LoadFileByte(Ctx.m_instance.m_localFileSys.getLocalWriteDir(), m_path);
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

        // 下载
        protected IEnumerator webDown()
        {
            string uri = Ctx.m_instance.m_cfg.m_webIP + m_path;

            try
            {
                //打开网络连接 
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(uri);
                System.Net.HttpWebRequest requestGetCount = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(uri);
                long countLength = requestGetCount.GetResponse().ContentLength;

                //向服务器请求，获得服务器回应数据流 
                System.IO.Stream ns = request.GetResponse().GetResponseStream();

                m_bytes = new byte[countLength];
                int nReadSize = 0;
                nReadSize = ns.Read(m_bytes, 0, (int)countLength);
                ns.Close();
                if (nReadSize != request.GetResponse().ContentLength)
                {
                    if (onFailed != null)
                    {
                        onFailed(this);
                    }
                }
                else
                {
                    if (onLoaded != null)
                    {
                        onLoaded(this);
                    }
                }
            }
            catch (System.Exception e)
            {
                if (onFailed != null)
                {
                    onFailed(this);
                }
            }

            yield return null;
        }
    }
}
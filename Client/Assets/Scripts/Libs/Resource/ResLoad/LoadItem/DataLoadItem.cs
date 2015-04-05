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
    public class DataLoadItem : LoadItem, ITask
    {
        public byte[] m_bytes;
        public string m_version;
        public bool m_isRunSuccess = true;

        override public void reset()
        {
            base.reset();
            m_bytes = null;
        }

        protected void parsePath()
        {
            string[] pathSplit = { "?" };
            string[] pathList = m_path.Split(pathSplit, StringSplitOptions.RemoveEmptyEntries);
            m_path = pathList[0];

            if (pathList.Length > 1)
            {
                string[] equalSplit = { "=" };
                string[] equalList = pathList[1].Split(equalSplit, StringSplitOptions.RemoveEmptyEntries);
                m_version = equalList[1];
            }
        }

        override public void load()
        {
            parsePath();
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
                //Ctx.m_instance.m_coroutineMgr.StartCoroutine(coroutWebDown());
                Ctx.m_instance.m_TaskQueue.push(this);
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
            if (Ctx.m_instance.m_localFileSys.isFileExist(Path.Combine(Ctx.m_instance.m_localFileSys.getLocalWriteDir(), m_path)))
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
            path = Ctx.m_instance.m_cfg.m_webIP + m_path;
            deleteFromCache(path);
            m_w3File = WWW.LoadFromCacheOrDownload(path, Int32.Parse(m_version));
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

        // 协程下载
        protected IEnumerator coroutWebDown()
        {
            string uri = Ctx.m_instance.m_cfg.m_webIP + m_path;
            string saveFile = Path.Combine(Ctx.m_instance.m_localFileSys.getLocalWriteDir(), m_path);

            //try
            {
                //打开网络连接 
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                long contentLength = response.ContentLength;
                long readedLength = 0;

                long lStartPos = 0;
                FileStream fs;
                if (File.Exists(saveFile))
                {
                    fs = System.IO.File.OpenWrite(saveFile);
                    lStartPos = fs.Length;
                    if (contentLength - lStartPos <= 0)     // 文件已经完成
                    {
                        fs.Close();
                        yield break;
                    }
                    fs.Seek(lStartPos, SeekOrigin.Current); //移动文件流中的当前指针 
                }
                else
                {
                    fs = new FileStream(saveFile, System.IO.FileMode.Create);
                }

                if (lStartPos > 0)
                {
                    request.AddRange((int)lStartPos); //设置Range值
                    contentLength -= lStartPos;
                }

                //向服务器请求，获得服务器回应数据流 
                System.IO.Stream ns = response.GetResponseStream();
                int len = 1024 * 8;
                m_bytes = new byte[len];
                int nReadSize = 0;
                string logStr;
                while(readedLength != contentLength)
                {
                    nReadSize = ns.Read(m_bytes, 0, len);
                    fs.Write(m_bytes, 0, nReadSize);
                    readedLength += nReadSize;
                    logStr = "已下载:" + fs.Length / 1024 + "kb /" + contentLength / 1024 + "kb";
                    Ctx.m_instance.m_log.log(logStr);
                    yield return false;
                }
                ns.Close();
                fs.Close();
                if (readedLength == contentLength)
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
            //catch (Exception)
            //{
            //    if (onFailed != null)
            //    {
            //        onFailed(this);
            //    }
            //}
        }

        // 线程下载
        public void runTask()
        {
            string uri = Ctx.m_instance.m_cfg.m_webIP + m_path;
            string saveFile = Path.Combine(Ctx.m_instance.m_localFileSys.getLocalWriteDir(), m_path);

            try
            {
                //打开网络连接 
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                long contentLength = response.ContentLength;
                long readedLength = 0;

                long lStartPos = 0;
                FileStream fs;
                if (File.Exists(saveFile))
                {
                    fs = System.IO.File.OpenWrite(saveFile);
                    lStartPos = fs.Length;
                    if (contentLength - lStartPos <= 0)     // 文件已经完成
                    {
                        fs.Close();
                        return;
                    }
                    fs.Seek(lStartPos, SeekOrigin.Current); //移动文件流中的当前指针 
                }
                else
                {
                    fs = new FileStream(saveFile, System.IO.FileMode.Create);
                }

                if (lStartPos > 0)
                {
                    request.AddRange((int)lStartPos); //设置Range值
                    contentLength -= lStartPos;
                }

                //向服务器请求，获得服务器回应数据流 
                System.IO.Stream ns = response.GetResponseStream();
                int len = 1024 * 8;
                m_bytes = new byte[len];
                int nReadSize = 0;
                string logStr;
                while (readedLength != contentLength)
                {
                    nReadSize = ns.Read(m_bytes, 0, len);
                    fs.Write(m_bytes, 0, nReadSize);
                    readedLength += nReadSize;
                    logStr = "已下载:" + fs.Length / 1024 + "kb /" + contentLength / 1024 + "kb";
                    Ctx.m_instance.m_log.asynclog(logStr);
                }
                ns.Close();
                fs.Close();
                if (readedLength == contentLength)
                {
                    m_isRunSuccess = true;
                }
                else
                {
                    m_isRunSuccess = false;
                }
            }
            catch (Exception)
            {
                m_isRunSuccess = false;
            }
        }

        public void handleResult()
        {
            if (m_isRunSuccess)
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
    }
}
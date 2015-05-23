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
        public string m_version = "";
        public bool m_isRunSuccess = true;
        public string m_localPath;

        override public void reset()
        {
            base.reset();
            m_bytes = null;
        }

        //protected void parsePath()
        //{
        //    string[] pathSplit = { "?" };
        //    string[] pathList = m_path.Split(pathSplit, StringSplitOptions.RemoveEmptyEntries);
        //    m_path = pathList[0];

        //    if (pathList.Length > 1)
        //    {
        //        string[] equalSplit = { "=" };
        //        string[] equalList = pathList[1].Split(equalSplit, StringSplitOptions.RemoveEmptyEntries);
        //        m_version = equalList[1];
        //    }
        //}

        override public void load()
        {
            //parsePath();
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
                m_localPath = Path.Combine(Ctx.m_instance.m_localFileSys.getLocalWriteDir(), UtilApi.getRelPath(m_path));
                if (!string.IsNullOrEmpty(m_version))
                {
                    m_localPath = UtilApi.combineVerPath(m_localPath, m_version);
                }

                Ctx.m_instance.m_logSys.log(string.Format("添加下载任务 {0}", m_path));
                Ctx.m_instance.m_TaskQueue.push(this);
            }
        }

        protected void loadFromStreamingAssets()
        {
            if (Ctx.m_instance.m_localFileSys.isFileExist(m_path))
            {
                m_bytes = Ctx.m_instance.m_localFileSys.LoadFileByte(m_path);
            }

            if (m_bytes != null)
            {
                nonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                nonRefCountResLoadResultNotify.resLoadState.setFailed();
            }
        }

        protected void loadFromPersistentData()
        {
            string verPath = m_path;
            if(!string.IsNullOrEmpty(m_version))
            {
                verPath = UtilApi.combineVerPath(m_path, m_version);
            }
            if (Ctx.m_instance.m_localFileSys.isFileExist(verPath))
            {
                m_bytes = Ctx.m_instance.m_localFileSys.LoadFileByte(verPath);
            }

            if (m_bytes != null)
            {
                nonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                nonRefCountResLoadResultNotify.resLoadState.setFailed();
            }
            nonRefCountResLoadResultNotify.loadEventDispatch.dispatchEvent(this);
        }

        // m_path 是这个格式 http://127.0.0.1/UnityServer/Version.txt?ver=100
        override protected IEnumerator downloadAsset()
        {
            deleteFromCache(m_path);
            m_w3File = WWW.LoadFromCacheOrDownload(m_path, Int32.Parse(m_version));
            yield return m_w3File;

            onWWWEnd();
        }

        // 加载完成回调处理
        override protected void onWWWEnd()
        {
            if (isLoadedSuccess(m_w3File))
            {
                m_bytes = m_w3File.bytes;

                nonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                nonRefCountResLoadResultNotify.resLoadState.setFailed();
            }
            nonRefCountResLoadResultNotify.loadEventDispatch.dispatchEvent(this);
        }

        // 协程下载
        protected IEnumerator coroutWebDown()
        {
            string uri = UtilApi.webFullPath(m_path);
            string saveFile = Path.Combine(Ctx.m_instance.m_localFileSys.getLocalWriteDir(), m_path);

            //try
            {
                //打开网络连接 
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                long contentLength = response.ContentLength;
                long readedLength = 0;

                long lStartPos = 0;
                FileStream fileStream;
                if (File.Exists(saveFile))
                {
                    fileStream = System.IO.File.OpenWrite(saveFile);
                    lStartPos = fileStream.Length;
                    if (contentLength - lStartPos <= 0)     // 文件已经完成
                    {
                        fileStream.Close();
                        yield break;
                    }
                    fileStream.Seek(lStartPos, SeekOrigin.Current); //移动文件流中的当前指针 
                }
                else
                {
                    fileStream = new FileStream(saveFile, System.IO.FileMode.Create);
                }

                if (lStartPos > 0)
                {
                    request.AddRange((int)lStartPos); //设置Range值
                    contentLength -= lStartPos;
                }

                //向服务器请求，获得服务器回应数据流 
                System.IO.Stream retStream = response.GetResponseStream();
                int len = 1024 * 8;
                m_bytes = new byte[len];
                int nReadSize = 0;
                string logStr;
                while(readedLength != contentLength)
                {
                    nReadSize = retStream.Read(m_bytes, 0, len);
                    fileStream.Write(m_bytes, 0, nReadSize);
                    readedLength += nReadSize;
                    logStr = "已下载:" + fileStream.Length / 1024 + "kb /" + contentLength / 1024 + "kb";
                    Ctx.m_instance.m_logSys.log(logStr);
                    yield return false;
                }
                retStream.Close();
                fileStream.Close();
                if (readedLength == contentLength)
                {
                    nonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
                }
                else
                {
                    nonRefCountResLoadResultNotify.resLoadState.setFailed();
                }
                nonRefCountResLoadResultNotify.loadEventDispatch.dispatchEvent(this);
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
            Ctx.m_instance.m_logSys.log(string.Format("线程开始下载下载任务 {0}", m_path));

            string saveFile = Path.Combine(Ctx.m_instance.m_localFileSys.getLocalWriteDir(), UtilApi.getRelPath(m_path));
            string origFile = saveFile;     // 没有版本号的文件名字，如果本地没有这个文件，需要先建立这个文件，等下载完成后，然后再改名字，保证下载的文件除了网络传输因素外，肯定正确
            bool bNeedReName = false;
            if (!string.IsNullOrEmpty(m_version))
            {
                saveFile = UtilApi.combineVerPath(saveFile, m_version);
            }

            try
            {
                //打开网络连接
                string webPath;
                if (!string.IsNullOrEmpty(m_version))
                {
                    webPath = string.Format("{0}?v={1}", m_path, m_version);
                }
                else
                {
                    webPath = m_path;
                }

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(webPath);
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                request.KeepAlive = false;
                request.Proxy = null;
                request.Timeout = 5000;

                ServicePointManager.DefaultConnectionLimit = 50;

                // GetRequestStream 总是出错，因此只能使用 GET 方式
                //StreamWriter requestWriter = null;
                //Stream webStream = request.GetRequestStream();
                //requestWriter = new StreamWriter(webStream);
                //try
                //{
                //    string postString = string.Format("v={0}", m_version);
                //    requestWriter.Write(postString);
                //}
                //catch (Exception ex2)
                //{
                //    Ctx.m_instance.m_logSys.asynclog("error");
                //}
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                long contentLength = response.ContentLength;
                long readedLength = 0;

                long lStartPos = 0;
                FileStream fileStream = null;
                if (File.Exists(saveFile))
                {
                    fileStream = File.OpenWrite(saveFile);
                    lStartPos = fileStream.Length;
                    if (contentLength - lStartPos <= 0)     // 文件已经完成
                    {
                        fileStream.Close();
                        onRunTaskEnd();
                        Ctx.m_instance.m_logSys.log("之前文件已经下载完成，不用重新下载");
                        return;
                    }
                    fileStream.Seek(lStartPos, SeekOrigin.Current); //移动文件流中的当前指针 
                }
                else
                {
                    bNeedReName = true;
                    try
                    {
                        fileStream = new FileStream(origFile, System.IO.FileMode.Create);
                    }
                    catch (Exception /*ex2*/)
                    {
                        Ctx.m_instance.m_logSys.error(string.Format("{0} 文件创建失败", saveFile));
                    }
                }

                if (lStartPos > 0)
                {
                    request.AddRange((int)lStartPos); //设置Range值
                    contentLength -= lStartPos;
                }

                //向服务器请求，获得服务器回应数据流 
                System.IO.Stream retStream = response.GetResponseStream();
                int len = 1024 * 8;
                m_bytes = new byte[len];
                int nReadSize = 0;
                string logStr;
                bool isBytesValid = true;        // m_bytes 中数据是否有效
                while (readedLength != contentLength)
                {
                    nReadSize = retStream.Read(m_bytes, 0, len);
                    fileStream.Write(m_bytes, 0, nReadSize);
                    readedLength += nReadSize;
                    //logStr = "已下载:" + fs.Length / 1024 + "kb /" + contentLength / 1024 + "kb";
                    logStr = string.Format("文件 {0} 已下载: {1} b / {2} b", m_path, fileStream.Length, contentLength);
                    Ctx.m_instance.m_logSys.log(logStr);

                    if (isBytesValid)
                    {
                        if(readedLength != contentLength)
                        {
                            isBytesValid = false;
                        }
                    }
                }

                // 释放资源
                request.Abort();
                request = null;
                response.Close();
                response = null;

                retStream.Close();
                fileStream.Close();

                // 修改文件名字
                if (bNeedReName)
                {
                    UtilApi.renameFile(origFile, saveFile);
                }

                if (!isBytesValid)
                {
                    m_bytes = null;
                }

                if (readedLength == contentLength)
                {
                    m_isRunSuccess = true;
                }
                else
                {
                    m_isRunSuccess = false;
                }
                onRunTaskEnd();
            }
            catch (Exception /*err*/)
            {
                m_isRunSuccess = false;
                onRunTaskEnd();
            }
        }

        protected void onRunTaskEnd()
        {
            Ctx.m_instance.m_logSys.log(string.Format("线程结束下载下载任务 {0}", m_path));

            LoadedWebResMR pRoute = Ctx.m_instance.m_poolSys.newObject<LoadedWebResMR>();
            pRoute.m_task = this;

            Ctx.m_instance.m_logSys.log(string.Format("线程下载结果推动给主线程 {0}", m_path));

            Ctx.m_instance.m_sysMsgRoute.push(pRoute);
        }

        public void handleResult()
        {
            if (m_isRunSuccess)
            {
                nonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                nonRefCountResLoadResultNotify.resLoadState.setFailed();
            }
            nonRefCountResLoadResultNotify.loadEventDispatch.dispatchEvent(this);
        }
    }
}
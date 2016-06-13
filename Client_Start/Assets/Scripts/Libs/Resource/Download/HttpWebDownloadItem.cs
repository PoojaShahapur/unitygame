using System;
using System.Collections;
using System.IO;
using System.Net;

namespace SDK.Lib
{
    /**
    * @brief 使用 HttpWeb 从网络下载数据
    */
    public class HttpWebDownloadItem : DownloadItem
    {
        public HttpWebDownloadItem()
        {

        }

        override public void load()
        {
            base.load();

            //Ctx.m_instance.m_coroutineMgr.StartCoroutine(coroutWebDown());
            Ctx.m_instance.m_TaskQueue.push(this);
        }

        // 协程下载
        protected IEnumerator coroutWebDown()
        {
            string uri = UtilLogic.webFullPath(m_loadPath);
            string saveFile = Path.Combine(MFileSys.getLocalWriteDir(), m_loadPath);

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
                while (readedLength != contentLength)
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
                    m_refCountResLoadResultNotify.resLoadState.setSuccessLoaded();
                }
                else
                {
                    m_refCountResLoadResultNotify.resLoadState.setFailed();
                }
                m_refCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
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
        override public void runTask()
        {
            Ctx.m_instance.m_logSys.log(string.Format("线程开始下载下载任务 {0}", m_loadPath));

            string saveFile = Path.Combine(MFileSys.getLocalWriteDir(), UtilLogic.getRelPath(m_loadPath));
            string origFile = saveFile;     // 没有版本号的文件名字，如果本地没有这个文件，需要先建立这个文件，等下载完成后，然后再改名字，保证下载的文件除了网络传输因素外，肯定正确
            bool bNeedReName = false;
            if (!string.IsNullOrEmpty(m_version))
            {
                saveFile = UtilLogic.combineVerPath(saveFile, m_version);
            }

            try
            {
                //打开网络连接
                string webPath;
                if (!string.IsNullOrEmpty(m_version))
                {
                    webPath = string.Format("{0}?v={1}", m_loadPath, m_version);
                }
                else
                {
                    webPath = m_loadPath;
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
                    logStr = string.Format("文件 {0} 已下载: {1} b / {2} b", m_loadPath, fileStream.Length, contentLength);
                    Ctx.m_instance.m_logSys.log(logStr);

                    if (isBytesValid)
                    {
                        if (readedLength != contentLength)
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
                    UtilPath.renameFile(origFile, saveFile);
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
            Ctx.m_instance.m_logSys.log(string.Format("线程结束下载下载任务 {0}", m_loadPath));

            LoadedWebResMR pRoute = Ctx.m_instance.m_poolSys.newObject<LoadedWebResMR>();
            pRoute.m_task = this;

            Ctx.m_instance.m_logSys.log(string.Format("线程下载结果推动给主线程 {0}", m_loadPath));

            Ctx.m_instance.m_sysMsgRoute.push(pRoute);
        }

        // 处理结果在这回调，然后分发给资源处理器，如果资源提前释放，就自动断开资源和加载器的事件分发就行了，不用在线程中处理了
        override public void handleResult()
        {
            if (m_isRunSuccess)
            {
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
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

            Ctx.mInstance.mCoroutineMgr.StartCoroutine(coroutWebDown());
            //Ctx.mInstance.m_TaskQueue.push(this);
        }

        // 协程下载
        protected IEnumerator coroutWebDown()
        {
            runTask();

            yield return null;
        }

        // 线程下载
        override public void runTask()
        {
            if(this.mFileLen == 0)
            {
                downWithNoFileLen();
            }
            else
            {
                downWithFileLen();
            }
        }

        // 没有文件大小直接下载
        protected void downWithNoFileLen()
        {
            string saveFile = mLocalPath;
            string origFile = saveFile;     // 没有版本号的文件名字，如果本地没有这个文件，需要先建立这个文件，等下载完成后，然后再改名字，保证下载的文件除了网络传输因素外，肯定正确
            bool bNeedReName = false;
            if (!string.IsNullOrEmpty(mVersion))
            {
                saveFile = UtilLogic.combineVerPath(saveFile, mVersion);
            }

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            System.IO.Stream retStream = null;
            MDataStream fileStream = null;

            try
            {
                //打开网络连接
                request = (HttpWebRequest)HttpWebRequest.Create(mDownloadVerPath);
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
                //    string postString = string.Format("v={0}", mVersion);
                //    requestWriter.Write(postString);
                //}
                //catch (Exception ex2)
                //{
                //    Ctx.mInstance.mLogSys.asynclog("error");
                //}

                long readedLength = 0;
                long startPos = 0;

                if (UtilPath.existFile(saveFile))
                {
                    fileStream = new MDataStream(saveFile, null, FileMode.Append, FileAccess.Write);
                    startPos = fileStream.getLength();

                    fileStream.seek(startPos, SeekOrigin.Current); //移动文件流中的当前指针 
                }
                else
                {
                    bNeedReName = true;

                    try
                    {
                        string path = UtilPath.getFilePathNoName(origFile);

                        if (!UtilPath.existDirectory(path))
                        {
                            UtilPath.createDirectory(path);
                        }

                        fileStream = new MDataStream(origFile, null, FileMode.CreateNew, FileAccess.Write);
                    }
                    catch (Exception exp)
                    {
                        Ctx.mInstance.mLogSys.error(string.Format("HttpWebDownloadItem::downWithNoFileLen, Fail, Path is{0}, Exception Message is {1}", saveFile, exp.Message));
                    }
                }

                if (startPos > 0)
                {
                    request.AddRange((int)startPos); //设置Range值
                }

                // 获取响应
                response = (HttpWebResponse)request.GetResponse();
                // 向服务器请求，获得服务器回应数据流 
                retStream = response.GetResponseStream();

                int len = 1024 * 8;
                mBytes = new byte[len];
                int readSize = 0;
                string logStr = "";

                readSize = retStream.Read(mBytes, 0, len);

                while (readSize > 0)
                {
                    fileStream.writeByte(mBytes, 0, readSize);
                    readedLength += readSize;

                    logStr = string.Format("HttpWebDownloadItem::downWithNoFileLen, Path is {0}, Already Downloaded {1} b", mLoadPath, fileStream.getLength());
                    
                    readSize = retStream.Read(mBytes, 0, len);
                }

                // 释放资源
                request.Abort();
                request = null;

                response.Close();
                response = null;

                retStream.Close();
                fileStream.dispose();
                fileStream = null;

                // 修改文件名字
                if (bNeedReName)
                {
                    UtilPath.renameFile(origFile, saveFile);
                }

                mRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();

                onRunTaskEnd();
            }
            catch (Exception exp)
            {
                // 释放资源
                if (request != null)
                {
                    request.Abort();
                    request = null;
                }

                if (response != null)
                {
                    response.Close();
                    response = null;
                }

                if (retStream != null)
                {
                    retStream.Close();
                    retStream = null;
                }

                if (fileStream != null)
                {
                    fileStream.dispose();
                    fileStream = null;
                }

                mRefCountResLoadResultNotify.resLoadState.setFailed();

                onRunTaskEnd();
            }
        }

        // 根据文件大小下载
        protected void downWithFileLen()
        {
            string saveFile = mLocalPath;
            string origFile = saveFile;     // 没有版本号的文件名字，如果本地没有这个文件，需要先建立这个文件，等下载完成后，然后再改名字，保证下载的文件除了网络传输因素外，肯定正确
            bool bNeedReName = false;
            if (!string.IsNullOrEmpty(mVersion))
            {
                saveFile = UtilLogic.combineVerPath(saveFile, mVersion);
            }

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            System.IO.Stream retStream = null;
            MDataStream fileStream = null;

            try
            {
                //打开网络连接
                request = (HttpWebRequest)HttpWebRequest.Create(mDownloadVerPath);
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                request.KeepAlive = false;
                request.Proxy = null;
                request.Timeout = 5000; // 在服务器上查找文件的最大等待时间

                ServicePointManager.DefaultConnectionLimit = 50;

                // GetRequestStream 总是出错，因此只能使用 GET 方式
                //StreamWriter requestWriter = null;
                //Stream webStream = request.GetRequestStream();
                //requestWriter = new StreamWriter(webStream);
                //try
                //{
                //    string postString = string.Format("v={0}", mVersion);
                //    requestWriter.Write(postString);
                //}
                //catch (Exception ex2)
                //{
                //    Ctx.mInstance.mLogSys.asynclog("error");
                //}
                
                long contentLength = mFileLen;
                long readedLength = 0;
                long startPos = 0;

                if (UtilPath.existFile(origFile))
                {
                    fileStream = new MDataStream(origFile, null, FileMode.Append, FileAccess.Write);
                    startPos = fileStream.getLength();

                    if (contentLength - startPos <= 0)     // 文件已经完成
                    {
                        fileStream.dispose();
                        fileStream = null;

                        onRunTaskEnd();

                        return;
                    }

                    fileStream.seek(startPos, SeekOrigin.Current); //移动文件流中的当前指针 
                }
                else
                {
                    bNeedReName = true;

                    try
                    {
                        string path = UtilPath.getFilePathNoName(origFile);

                        if (!UtilPath.existDirectory(path))
                        {
                            UtilPath.createDirectory(path);
                        }

                        fileStream = new MDataStream(origFile, null, FileMode.CreateNew, FileAccess.Write);
                    }
                    catch (Exception exp)
                    {
                        
                    }
                }

                if (startPos > 0)
                {
                    request.AddRange((int)startPos); //设置Range值
                    contentLength -= startPos;
                }

                // 获取响应，如果在服务器上不能查找到，返回会等待 request.Timeout 时间
                response = (HttpWebResponse)request.GetResponse();
                // 向服务器请求，获得服务器回应数据流 
                retStream = response.GetResponseStream();

                int len = 1024 * 8;
                mBytes = new byte[len];
                int readSize = 0;
                string logStr = "";
                bool isBytesValid = false;

                readSize = retStream.Read(mBytes, 0, len);

                while (readSize > 0)
                {
                    fileStream.writeByte(mBytes, 0, readSize);
                    readedLength += readSize;

                    logStr = string.Format("HttpWebDownloadItem::downWithFileLen, Path is {0}, Already Downloaded [ {1} b / {2} b ]", mLoadPath, fileStream.getLength(), contentLength);
                    
                    if (readedLength == contentLength)
                    {
                        isBytesValid = true;
                    }

                    readSize = retStream.Read(mBytes, 0, len);
                }

                // 释放资源
                request.Abort();
                request = null;

                response.Close();
                response = null;

                retStream.Close();
                fileStream.dispose();
                fileStream = null;

                // 修改文件名字
                if (bNeedReName)
                {
                    UtilPath.renameFile(origFile, saveFile);
                }

                if (!isBytesValid)
                {
                    mBytes = null;
                }

                if (readedLength == contentLength)
                {
                    mRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
                }
                else
                {
                    mRefCountResLoadResultNotify.resLoadState.setFailed();
                }

                onRunTaskEnd();
            }
            catch (Exception exp)
            {
                // 释放资源
                if (request != null)
                {
                    request.Abort();
                    request = null;
                }

                if (response != null)
                {
                    response.Close();
                    response = null;
                }

                if (retStream != null)
                {
                    retStream.Close();
                    retStream = null;
                }

                if (fileStream != null)
                {
                    fileStream.dispose();
                    fileStream = null;
                }

                mRefCountResLoadResultNotify.resLoadState.setFailed();

                onRunTaskEnd();
            }
        }

        protected void onRunTaskEnd()
        {
            LoadedWebResMR pRoute = Ctx.mInstance.mPoolSys.newObject<LoadedWebResMR>();
            pRoute.mTask = this;

            Ctx.mInstance.mSysMsgRoute.addMsg(pRoute);
        }

        // 处理结果在这回调，然后分发给资源处理器，如果资源提前释放，就自动断开资源和加载器的事件分发就行了，不用在线程中处理了
        override public void handleResult()
        {
            mRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }
    }
}
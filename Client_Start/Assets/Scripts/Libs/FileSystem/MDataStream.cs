using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 仅支持本地文件操作，仅支持同步操作
     */
    public class MDataStream : GObject, IDispatchObject
    {
        public enum eFilePlatformAndPath
        {
            eResourcesPath = 0,                 // Resources 文件夹下的文件操作
            eAndroidStreamingAssetsPath = 1,    // Android 平台下 StreamingAssetsPath 目录下，只有 Android 的 StreamingAssets 这个目录必须需要 WWW 才能读取，其它的平台所有的目录都可以使用 File 系统读取操作
            eOther,                             // 其它
        }

        public enum eFileOpState
        {
            eNoOp = 0,      // 无操作
            eOpening = 1,   // 打开中
            eOpenSuccess = 2,   // 打开成功
            eOpenFail = 3,      // 打开失败
            eOpenClose = 4,     // 关闭
        }

        public FileStream mFileStream;
        protected WWW mWWW;

        protected string mFilePath;
        protected FileMode mMode;
        protected FileAccess mAccess;
        protected eFileOpState mFileOpState;

        protected eFilePlatformAndPath mFilePlatformAndPath;

        protected bool mIsSyncMode;
        protected AddOnceAndCallOnceEventDispatch mOpenedEvtDisp;   // 文件打开结束分发，主要是 WWW 是异步读取本地文件的，因此需要确保文件被打开成功

        protected string mText;
        protected byte[] mBytes;

        /**
         * @brief 仅支持同步操作，目前无视参数 isSyncMode 和 evtDisp。FileMode.CreateNew 如果文件已经存在就抛出异常，FileMode.Append 和 FileAccess.Write 要同时使用
         */
        public MDataStream(string filePath, MAction<IDispatchObject> openedDisp = null, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read, bool isSyncMode = true)
        {
            this.mTypeId = "MDataStream";

            mFilePath = filePath;
            mMode = mode;
            mAccess = access;
            mFileOpState = eFileOpState.eNoOp;
            mIsSyncMode = isSyncMode;

            checkPlatformAndPath(mFilePath);

            checkAndOpen(openedDisp);
        }

        public void seek(long offset, SeekOrigin origin)
        {
            if(mFileOpState == eFileOpState.eOpenSuccess)
            {
                if(isResourcesFile())
                {

                }
                else if (isWWWStream())
                {

                }
                else
                {
                    mFileStream.Seek(offset, origin);
                }
            }
        }

        public void addOpenedHandle(MAction<IDispatchObject> openedDisp = null)
        {
            if (mOpenedEvtDisp == null)
            {
                mOpenedEvtDisp = new AddOnceAndCallOnceEventDispatch();
            }

            mOpenedEvtDisp.addEventHandle(null, openedDisp);
        }

        public void dispose()
        {
            close();
        }

        public void checkPlatformAndPath(string path)
        {
            if(checkResourcesFile())
            {
                Ctx.mInstance.mLogSys.log(string.Format("MDataStream::checkPlatformAndPath, Resources Load, Path is {0}", path), LogTypeId.eLogLocalFile);

                mFilePlatformAndPath = eFilePlatformAndPath.eResourcesPath;
            }
            else if (UtilPath.isAndroidRuntime() && UtilPath.isStreamingAssetsPath(path))
            {
                Ctx.mInstance.mLogSys.log(string.Format("MDataStream::checkPlatformAndPath, WWWStreamingAssets Load, Path is {0}", path), LogTypeId.eLogLocalFile);

                mFilePlatformAndPath = eFilePlatformAndPath.eAndroidStreamingAssetsPath;
            }
            else
            {
                Ctx.mInstance.mLogSys.log(string.Format("MDataStream::checkPlatformAndPath, FileStream Load, Path is {0}", path), LogTypeId.eLogLocalFile);

                mFilePlatformAndPath = eFilePlatformAndPath.eOther;
            }
        }

        protected bool checkResourcesFile()
        {
            if (mFilePath.Substring(0, MFileSys.msDataStreamResourcesPath.Length) == MFileSys.msDataStreamResourcesPath)
            {
                mFilePath = mFilePath.Substring(MFileSys.msDataStreamResourcesPath.Length + 1);
                return true;
            }

            return false;
        }

        public bool isResourcesFile()
        {
            if(mFilePlatformAndPath == eFilePlatformAndPath.eResourcesPath)
            {
                return true;
            }

            return false;
        }

        public bool isWWWStream()
        {
            //return mFilePlatformAndPath == eFilePlatformAndPath.eAndroidStreamingAssetsPath ||
            //       mFilePlatformAndPath == eFilePlatformAndPath.eOther;

            return mFilePlatformAndPath == eFilePlatformAndPath.eAndroidStreamingAssetsPath;
        }

        protected void syncOpenFileStream()
        {
            if (mFileOpState == eFileOpState.eNoOp)
            {
                mFileOpState = eFileOpState.eOpening;
                if(!isWWWStream())
                {
                    try
                    {
                        mFileStream = new FileStream(mFilePath, mMode, mAccess);
                        mFileOpState = eFileOpState.eOpenSuccess;
                    }
                    catch(Exception exp)
                    {
                        mFileOpState = eFileOpState.eOpenFail;

                        Ctx.mInstance.mLogSys.log(string.Format("MDataStream::syncOpenFileStream, Open File Fail, FileName is {0}, Exception is {1}", mFilePath, exp.Message), LogTypeId.eLogLocalFile);
                    }
                }

                onAsyncOpened();
            }
        }

        // 异步打开
        public IEnumerator asyncWWWStreamingAssetOpen()
        {
            if (mFileOpState == eFileOpState.eNoOp)
            {
                mFileOpState = eFileOpState.eOpening;

                if (isWWWStream())
                {
                    // Android 平台
                    mWWW = new WWW(mFilePath);   // 同步加载资源
                    yield return mWWW;

                    if(UtilApi.isWWWNoError(mWWW))
                    {
                        Ctx.mInstance.mLogSys.log(string.Format("MDataStream::asyncWWWStreamingAssetOpen, Success, Path is {0}", mFilePath), LogTypeId.eLogLocalFile);

                        mFileOpState = eFileOpState.eOpenSuccess;
                    }
                    else
                    {
                        Ctx.mInstance.mLogSys.log(string.Format("MDataStream::asyncWWWStreamingAssetOpen, Fail, Path is {0}", mFilePath), LogTypeId.eLogLocalFile);

                        mFileOpState = eFileOpState.eOpenFail;
                    }

                    onAsyncOpened();
                }
                else
                {
                    yield break;
                }
            }

            yield break;
        }

        public void syncOpenResourcesFile()
        {
            if (mFileOpState == eFileOpState.eNoOp)
            {
                mFileOpState = eFileOpState.eOpening;

                TextAsset textAsset = null;
                try
                {
                    string fileNoExt = UtilPath.getFilePathNoExt(mFilePath);
                    textAsset = Resources.Load<TextAsset>(fileNoExt);
                    if (textAsset != null)
                    {
                        Ctx.mInstance.mLogSys.log(string.Format("MDataStream::syncOpenResourcesFile, Success, OrigPath is {0}, LoadPath is {1}", mFilePath, fileNoExt), LogTypeId.eLogLocalFile);

                        mFileOpState = eFileOpState.eOpenSuccess;

                        mText = textAsset.text;
                        mBytes = textAsset.bytes;
                        Resources.UnloadAsset(textAsset);
                    }
                    else
                    {
                        Ctx.mInstance.mLogSys.log(string.Format("MDataStream::syncOpenResourcesFile, Fail, OrigPath is {0}, LoadPath is {1}", mFilePath, fileNoExt), LogTypeId.eLogLocalFile);

                        mFileOpState = eFileOpState.eOpenFail;
                    }
                }
                catch (Exception exp)
                {
                    mFileOpState = eFileOpState.eOpenFail;

                    Ctx.mInstance.mLogSys.log(string.Format("MDataStream Load Failed, OrigPath is {0}, Exception Message is {1}", mFilePath, exp.Message), LogTypeId.eLogLocalFile);
                }

                onAsyncOpened();
            }
        }

        public IEnumerator asyncOpenResourcesFile()
        {
            if (mFileOpState == eFileOpState.eNoOp)
            {
                mFileOpState = eFileOpState.eOpening;

                TextAsset textAsset = null;
                //try
                //{
                string fileNoExt = UtilPath.getFilePathNoExt(mFilePath);
                ResourceRequest req = null;
                req = Resources.LoadAsync<TextAsset>(fileNoExt);
                yield return req;

                if (req != null && req.isDone)
                {
                    textAsset = req.asset as TextAsset;
                    if (textAsset != null)
                    {
                        Ctx.mInstance.mLogSys.log(string.Format("MDataStream::asyncOpenResourcesFile, Success, FileName is {0}", mFilePath), LogTypeId.eLogLocalFile);

                        mFileOpState = eFileOpState.eOpenSuccess;

                        mText = textAsset.text;
                        mBytes = textAsset.bytes;
                        Resources.UnloadAsset(textAsset);
                    }
                    else
                    {
                        Ctx.mInstance.mLogSys.log(string.Format("MDataStream::asyncOpenResourcesFile, Fail, FileName is {0}", mFilePath), LogTypeId.eLogLocalFile);

                        mFileOpState = eFileOpState.eOpenFail;
                    }
                }
                //}
                //catch (Exception exp)
                //{
                //    mFileOpState = eFileOpState.eOpenFail;

                //    Ctx.mInstance.mLogSys.log("MDataStream Load Failed, FileName is " + mFilePath + " Exception is" + exp.Message, LogTypeId.eLogLocalFile);
                //}

                onAsyncOpened();
            }
        }

        // 异步打开结束
        public void onAsyncOpened()
        {
            if (mOpenedEvtDisp != null)
            {
                mOpenedEvtDisp.dispatchEvent(this);
            }
        }

        protected void checkAndOpen(MAction<IDispatchObject> openedDisp = null)
        {
            if (openedDisp != null)
            {
                this.addOpenedHandle(openedDisp);
            }

            if (mFileOpState == eFileOpState.eNoOp)
            {
                if(isResourcesFile())
                {
                    if(mIsSyncMode)
                    {
                        // 同步模式
                        syncOpenResourcesFile();
                    }
                    else
                    {
                        // 异步模式
                        Ctx.mInstance.mCoroutineMgr.StartCoroutine(asyncOpenResourcesFile());
                    }
                }
                else if (isWWWStream())
                {
                    Ctx.mInstance.mCoroutineMgr.StartCoroutine(asyncWWWStreamingAssetOpen());
                }
                else
                {
                    syncOpenFileStream();
                }
            }
        }

        public bool isValid()
        {
            return mFileOpState == eFileOpState.eOpenSuccess;
        }

        // 获取总共长度
        public int getLength()
        {
            int len = 0;
            if (mFileOpState == eFileOpState.eOpenSuccess)
            {
                if(isResourcesFile())
                {
                    if (mText != null)
                    {
                        len = mText.Length;
                    }
                    else if(mBytes != null)
                    {
                        len = mBytes.Length;
                    }
                }
                else if (isWWWStream())
                {
                    if (mWWW != null)
                    {
                        len = mWWW.size;
                    }
                }
                else
                {
                    if (mFileStream != null)
                    {
                        len = (int)mFileStream.Length;
                    }
                    /*
                    if (mFileStream != null && mFileStream.CanSeek)
                    {
                        try
                        {
                            len = (int)mFileStream.Seek(0, SeekOrigin.End);     // 移动到文件结束，返回长度
                            len = (int)mFileStream.Position;                    // Position 移动到 Seek 位置
                        }
                        catch(Exception exp)
                        {
                            Ctx.mInstance.mLogSys.log("FileSeek Failed" + exp.Message, LogTypeId.eLogCommon);
                        }
                    }
                    */
                }
            }

            return len;
        }

        protected void close()
        {
            if (mFileOpState == eFileOpState.eOpenSuccess)
            {
                if (isResourcesFile())
                {

                }
                else if (isWWWStream())
                {
                    if(mWWW != null)
                    {
                        mWWW.Dispose();
                        mWWW = null;
                    }
                }
                else
                {
                    if (mFileStream != null)
                    {
                        mFileStream.Close();
                        mFileStream.Dispose();
                        mFileStream = null;
                    }
                }

                mFileOpState = eFileOpState.eOpenClose;
                mFileOpState = eFileOpState.eNoOp;
            }
        }

        public string readText(int offset = 0, int count = 0, Encoding encode = null)
        {
            checkAndOpen();

            string retStr = "";
            byte[] bytes = null;

            if (encode == null)
            {
                encode = Encoding.UTF8;
            }

            if (count == 0)
            {
                count = getLength();
            }

            if (mFileOpState == eFileOpState.eOpenSuccess)
            {
                if (isResourcesFile())
                {
                    retStr = mText;
                }
                else if (isWWWStream())
                {
                    if (UtilApi.isWWWNoError(mWWW))
                    {
                        if (mWWW.text != null)
                        {
                            retStr = mWWW.text;
                        }
                        else if (mWWW.bytes != null)
                        {
                            retStr = encode.GetString(bytes);
                        }
                    }
                }
                else
                {
                    if (mFileStream.CanRead)
                    {
                        try
                        {
                            bytes = new byte[count];
                            mFileStream.Read(bytes, 0, count);

                            retStr = encode.GetString(bytes);
                        }
                        catch (Exception err)
                        {
                            Ctx.mInstance.mLogSys.log(string.Format("MDataStream::readText, Exception Message is {0}", err.Message), LogTypeId.eLogLocalFile);
                        }
                    }
                }
            }

            return retStr;
        }

        public byte[] readByte(int offset = 0, int count = 0)
        {
            checkAndOpen();

            if (count == 0)
            {
                count = getLength();
            }

            byte[] bytes = null;

            if (isResourcesFile())
            {
                bytes = mBytes;
            }
            else if (isWWWStream())
            {
                if (UtilApi.isWWWNoError(mWWW))
                {
                    if (mWWW.bytes != null)
                    {
                        bytes = mWWW.bytes;
                    }
                }
            }
            else
            {
                if (mFileStream.CanRead)
                {
                    try
                    {
                        bytes = new byte[count];
                        mFileStream.Read(bytes, 0, count);
                    }
                    catch (Exception err)
                    {
                        Ctx.mInstance.mLogSys.log(string.Format("MDataStream::readByte, Exception Message is {0}", err.Message), LogTypeId.eLogLocalFile);
                    }
                }
            }

            return bytes;
        }

        //public void writeText(string text, Encoding encode = null)
        public void writeText(string text, GkEncode gkEncode = GkEncode.eUTF8)
        {
            Encoding encode = UtilApi.convGkEncode2EncodingEncoding(gkEncode);

            checkAndOpen();

            if (isResourcesFile())
            {

            }
            else if (isWWWStream())
            {
                Ctx.mInstance.mLogSys.log("MDataStream::writeText, Current Path Cannot Write Content", LogTypeId.eLogLocalFile);
            }
            else
            {
                if (mFileStream.CanWrite)
                {
                    //if (encode == null)
                    //{
                    //    encode = GkEncode.UTF8;
                    //}

                    byte[] bytes = encode.GetBytes(text);
                    if (bytes != null)
                    {
                        try
                        {
                            mFileStream.Write(bytes, 0, bytes.Length);
                        }
                        catch (Exception err)
                        {
                            Ctx.mInstance.mLogSys.log(string.Format("MDataStream::writeText, Exception Message {0}", err.Message), LogTypeId.eLogLocalFile);
                        }
                    }
                }
            }
        }

        public void writeByte(byte[] bytes, int offset = 0, int count = 0)
        {
            checkAndOpen();

            if (isResourcesFile())
            {

            }
            else if (isWWWStream())
            {
                Ctx.mInstance.mLogSys.log("MDataStream::writeByte, Current Path Cannot Write Content", LogTypeId.eLogLocalFile);
            }
            else
            {
                if (mFileStream.CanWrite)
                {
                    if (bytes != null)
                    {
                        if (count == 0)
                        {
                            count = bytes.Length;
                        }

                        if (count != 0)
                        {
                            try
                            {
                                mFileStream.Write(bytes, offset, count);
                            }
                            catch (Exception err)
                            {
                                Ctx.mInstance.mLogSys.log(string.Format("MDataStream::writeByte, Exception Message is {0}", err.Message), LogTypeId.eLogLocalFile);
                            }
                        }
                    }
                }
            }
        }

        //public void writeLine(string text, Encoding encode = null)
        public void writeLine(string text, GkEncode gkEncode = GkEncode.eUTF8)
        {
            text = text + UtilApi.CR_LF;
            //writeText(text, encode);
            writeText(text, gkEncode);
        }
    }
}
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
            eAndroidStreamingAssetsPath = 1,    // Android 平台下 StreamingAssetsPath 目录下
            eOther,                             // 其它
        }

        public FileStream mFileStream;
        protected WWW mWWW;

        protected string mFilePath;
        protected FileMode mMode;
        protected FileAccess mAccess;
        protected bool mIsValid;

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
            mIsValid = false;
            mIsSyncMode = isSyncMode;

            checkPlatformAndPath(mFilePath);

            checkAndOpen(openedDisp);
        }

        public void seek(long offset, SeekOrigin origin)
        {
            if(mIsValid)
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
            if(isResourcesFile())
            {
                mFilePlatformAndPath = eFilePlatformAndPath.eResourcesPath;
            }
            else if (UtilPath.isAndroidRuntime() && UtilPath.isStreamingAssetsPath(path))
            {
                mFilePlatformAndPath = eFilePlatformAndPath.eAndroidStreamingAssetsPath;
            }
            else
            {
                mFilePlatformAndPath = eFilePlatformAndPath.eOther;
            }
        }

        public bool isResourcesFile()
        {
            if(mFilePath.IndexOf(MFileSys.msPersistentDataPath) != 0 && 
               mFilePath.IndexOf(MFileSys.msStreamingAssetsPath) != 0)
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

        protected void syncOpen()
        {
            if (!mIsValid)
            {
                mIsValid = true;
                if(!isWWWStream())
                {
                    mFileStream = new FileStream(mFilePath, mMode, mAccess);
                }

                onAsyncOpened();
            }
        }

        // 异步打开
        public IEnumerator asyncOpen()
        {
            if (!mIsValid)
            {
                mIsValid = true;
                if (isWWWStream())
                {
                    // Android 平台
                    string path = UtilPath.getRuntimeWWWStreamingAssetsPath(mFilePath);
                    mWWW = new WWW(path);   // 同步加载资源
                    yield return mWWW;

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
            if (!mIsValid)
            {
                mIsValid = true;

                TextAsset textAsset = null;
                try
                {
                    string fileNoExt = UtilPath.getFileNameNoExt(mFilePath);
                    textAsset = Resources.Load<TextAsset>(fileNoExt);
                    if (textAsset != null)
                    {
                        mText = textAsset.text;
                        mBytes = textAsset.bytes;
                        Resources.UnloadAsset(textAsset);
                    }
                    else
                    {
                        mIsValid = false;
                    }
                }
                catch (Exception exp)
                {
                    mIsValid = false;
                    Ctx.m_instance.m_logSys.log("MDataStream Load Failed, FileName is " + mFilePath + " Exception is" + exp.Message);
                }

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

            if(!mIsValid)
            {
                if(isResourcesFile())
                {
                    syncOpenResourcesFile();
                }
                else if (isWWWStream())
                {
                    Ctx.m_instance.m_coroutineMgr.StartCoroutine(asyncOpen());
                }
                else
                {
                    syncOpen();
                }
            }
        }

        public bool isValid()
        {
            return mIsValid;
        }

        // 获取总共长度
        public int getLength()
        {
            int len = 0;
            if (mIsValid)
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
                            Ctx.m_instance.m_logSys.log("FileSeek Failed" + exp.Message, LogTypeId.eLogCommon);
                        }
                    }
                    */
                }
            }

            return len;
        }

        protected void close()
        {
            if (mIsValid)
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

                mIsValid = false;
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
                        Ctx.m_instance.m_logSys.log(err.Message);
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
                        Ctx.m_instance.m_logSys.log(err.Message);
                    }
                }
            }

            return bytes;
        }

        public void writeText(string text, Encoding encode = null)
        {
            checkAndOpen();

            if (isResourcesFile())
            {

            }
            else if (isWWWStream())
            {
                Ctx.m_instance.m_logSys.log("Current Path Cannot Write Content", LogTypeId.eLogCommon);
            }
            else
            {
                if (mFileStream.CanWrite)
                {
                    if (encode == null)
                    {
                        encode = GkEncode.UTF8;
                    }

                    byte[] bytes = encode.GetBytes(text);
                    if (bytes != null)
                    {
                        try
                        {
                            mFileStream.Write(bytes, 0, bytes.Length);
                        }
                        catch (Exception err)
                        {
                            Ctx.m_instance.m_logSys.log(err.Message);
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
                Ctx.m_instance.m_logSys.log("Current Path Cannot Write Content", LogTypeId.eLogCommon);
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
                                Ctx.m_instance.m_logSys.log(err.Message);
                            }
                        }
                    }
                }
            }
        }

        public void writeLine(string text, Encoding encode = null)
        {
            text = text + UtilApi.CR_LF;
            writeText(text, encode);
        }
    }
}
using System;
using System.IO;
using System.Text;

namespace SDK.Lib
{
    /**
     * @brief 仅支持本地文件操作，仅支持同步操作
     */
    public class MFileStream : GObject, IDispatchObject
    {
        public enum eFileOpState
        {
            eNoOp = 0,      // 无操作
            eOpening = 1,   // 打开中
            eOpenSuccess = 2,   // 打开成功
            eOpenFail = 3,      // 打开失败
            eOpenClose = 4,     // 关闭
        }

        public FileStream mFileStream;
        
        protected string mFilePath;
        protected FileMode mMode;
        protected FileAccess mAccess;
        protected eFileOpState mFileOpState;

        protected string mText;
        protected byte[] mBytes;
        protected AddOnceAndCallOnceEventDispatch mOpenedEventDispatch;

        /**
         * @brief 仅支持同步操作，目前无视参数 isSyncMode 和 evtDisp。FileMode.CreateNew 如果文件已经存在就抛出异常，FileMode.Append 和 FileAccess.Write 要同时使用
         */
        public MFileStream(string filePath, MAction<IDispatchObject> openedHandle = null, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read)
        {
            this.mTypeId = "MFileStream";

            this.mFilePath = filePath;
            this.mMode = mode;
            this.mAccess = access;
            this.mFileOpState = eFileOpState.eNoOp;

            this.checkAndOpen(openedHandle);
        }

        public void seek(long offset, SeekOrigin origin)
        {
            if(this.mFileOpState == eFileOpState.eOpenSuccess)
            {
                this.mFileStream.Seek(offset, origin);
            }
        }

        public void addOpenedHandle(MAction<IDispatchObject> openedDisp = null)
        {
            if (this.mOpenedEventDispatch == null)
            {
                this.mOpenedEventDispatch = new AddOnceAndCallOnceEventDispatch();
            }

            this.mOpenedEventDispatch.addEventHandle(null, openedDisp);
        }

        public void dispose()
        {
            this.close();
        }

        protected void syncOpenFileStream()
        {
            if (this.mFileOpState == eFileOpState.eNoOp)
            {
                this.mFileOpState = eFileOpState.eOpening;

                try
                {
                    this.mFileStream = new FileStream(mFilePath, mMode, mAccess);
                    this.mFileOpState = eFileOpState.eOpenSuccess;
                }
                catch(Exception exp)
                {
                    this.mFileOpState = eFileOpState.eOpenFail;
                }

                this.onAsyncOpened();
            }
        }

        // 异步打开结束
        public void onAsyncOpened()
        {
            if (this.mOpenedEventDispatch != null)
            {
                this.mOpenedEventDispatch.dispatchEvent(this);
            }
        }

        protected void checkAndOpen(MAction<IDispatchObject> openedHandle = null)
        {
            if (openedHandle != null)
            {
                this.addOpenedHandle(openedHandle);
            }

            if (this.mFileOpState == eFileOpState.eNoOp)
            {
                this.syncOpenFileStream();
            }
        }

        public bool isValid()
        {
            return this.mFileOpState == eFileOpState.eOpenSuccess;
        }

        // 获取总共长度
        public int getLength()
        {
            int len = 0;

            if (this.mFileOpState == eFileOpState.eOpenSuccess)
            {
                if (this.mFileStream != null)
                {
                    len = (int)this.mFileStream.Length;
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
                    }
                }
                */
            }

            return len;
        }

        protected void close()
        {
            if (this.mFileOpState == eFileOpState.eOpenSuccess)
            {
                if (this.mFileStream != null)
                {
                    this.mFileStream.Close();
                    this.mFileStream.Dispose();
                    this.mFileStream = null;
                }

                this.mFileOpState = eFileOpState.eOpenClose;
                this.mFileOpState = eFileOpState.eNoOp;
            }
        }

        public string readText(int offset = 0, int count = 0, Encoding encode = null)
        {
            this.checkAndOpen();

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

            if (this.mFileOpState == eFileOpState.eOpenSuccess)
            {
                if (this.mFileStream.CanRead)
                {
                    try
                    {
                        bytes = new byte[count];
                        this.mFileStream.Read(bytes, 0, count);

                        retStr = encode.GetString(bytes);
                    }
                    catch (Exception err)
                    {
                            
                    }
                }
            }

            return retStr;
        }

        public byte[] readByte(int offset = 0, int count = 0)
        {
            this.checkAndOpen();

            if (count == 0)
            {
                count = getLength();
            }

            byte[] bytes = null;

            if (this.mFileStream.CanRead)
            {
                try
                {
                    bytes = new byte[count];
                    this.mFileStream.Read(bytes, 0, count);
                }
                catch (Exception err)
                {
                        
                }
            }

            return bytes;
        }

        public void writeText(string text, GkEncode gkEncode = GkEncode.eUTF8)
        {
            Encoding encode = UtilApi.convGkEncode2EncodingEncoding(gkEncode);

            this.checkAndOpen();

            if (this.mFileStream.CanWrite)
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
                        this.mFileStream.Write(bytes, 0, bytes.Length);
                    }
                    catch (Exception err)
                    {
                            
                    }
                }
            }
        }

        public void writeByte(byte[] bytes, int offset = 0, int count = 0)
        {
            this.checkAndOpen();

            if (this.mFileStream.CanWrite)
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
                            this.mFileStream.Write(bytes, offset, count);
                        }
                        catch (Exception err)
                        {
                                
                        }
                    }
                }
            }
        }

        public void writeLine(string text, GkEncode gkEncode = GkEncode.eUTF8)
        {
            text = text + UtilApi.CR_LF;
            writeText(text, gkEncode);
        }
    }
}
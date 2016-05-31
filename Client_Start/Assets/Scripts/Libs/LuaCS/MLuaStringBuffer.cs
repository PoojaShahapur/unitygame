using System;
using System.Runtime.InteropServices;

namespace SDK.Lib
{
    public class MLuaStringBuffer
    {
        //从lua端读取协议数据
        public MLuaStringBuffer(IntPtr source, int len)
        {
            buffer = new byte[len];
            Marshal.Copy(source, buffer, 0, len);
        }

        //c#端创建协议数据
        public MLuaStringBuffer(byte[] buf, int len)
        {
            this.buffer = buf;
            this.mLen = len;
            if (this.mLen == 0)
            {
                this.mLen = this.buffer.Length;
            }
        }

        public int mLen;
        public byte[] buffer = null;
    }
}
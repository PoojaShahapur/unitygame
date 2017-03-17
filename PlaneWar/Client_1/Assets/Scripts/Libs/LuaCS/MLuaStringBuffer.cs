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
        public MLuaStringBuffer(byte[] buf)
        {
            this.buffer = buf;
        }

        public byte[] buffer = null;
    }
}